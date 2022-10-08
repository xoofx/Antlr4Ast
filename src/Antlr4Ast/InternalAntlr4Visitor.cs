// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using Antlr4.Runtime;

namespace Antlr4Ast;

internal class InternalAntlr4Visitor : ANTLRv4ParserBaseVisitor<SyntaxNode?>
{
    private readonly CommonTokenStream _tokens;
    private readonly GrammarSyntax _grammar;

    public InternalAntlr4Visitor(CommonTokenStream tokens, GrammarSyntax grammarSyntax)
    {
        _tokens = tokens;
        _grammar = grammarSyntax;
    }

    public override SyntaxNode? VisitGrammarSpec(ANTLRv4Parser.GrammarSpecContext context)
    {
        // Parse GrammarDecl
        var grammarType = context.grammarDecl().grammarType();
        _grammar.Kind = GrammarKind.Full;
        if (grammarType.LEXER() != null)
        {
            _grammar.Kind = GrammarKind.Lexer;
        }
        else if (grammarType.PARSER() != null)
        {
            _grammar.Kind = GrammarKind.Parser;
        }
        _grammar.Name = GetIdentifier(context.grammarDecl().identifier());

        // Parse Options/Imports
        foreach (var prequel in context.prequelConstruct())
        {
            var node = VisitPrequelConstruct(prequel);
            switch (node)
            {
                case OptionsSyntax optionList:
                    _grammar.Options.Add(optionList);
                    break;
                case ImportSyntax importSyntax:
                    _grammar.Imports.Add(importSyntax);
                    break;
            }
        }

        // Parse Rules
        VisitRules(context.rules());

        return null;
    }

    public override SyntaxNode? VisitParserRuleSpec(ANTLRv4Parser.ParserRuleSpecContext context)
    {
        var ruleName = context.RULE_REF().GetText();

        var node = base.VisitRuleBlock(context.ruleBlock());

        if (node is AlternativeListSyntax ruleNode)
        {
            var parserRule = SpanAndComment(context, new ParserRuleSyntax(ruleName, ruleNode));
            _grammar.ParserRules.Add(parserRule);
        }

        return null;
    }

    public override SyntaxNode? VisitAlternative(ANTLRv4Parser.AlternativeContext context)
    {
        var alternative = new AlternativeSyntax();
        var elements = context.element();
        foreach (var element in elements)
        {
            if (VisitElement(element) is ElementSyntax node)
            {
                alternative.Elements.Add(node);
            }
        }

        return SpanAndComment(context, alternative);
    }


    public override SyntaxNode? VisitRuleAltList(ANTLRv4Parser.RuleAltListContext context)
    {
        var list = new AlternativeListSyntax();
        foreach (var labeledAltContext in context.labeledAlt())
        {
            if (VisitLabeledAlt(labeledAltContext) is AlternativeSyntax node)
            {
                list.Items.Add(node);
            }
        }

        return SpanAndComment(context, list);
    }

    public override SyntaxNode? VisitLabeledAlt(ANTLRv4Parser.LabeledAltContext context)
    {
        if (VisitAlternative(context.alternative()) is AlternativeSyntax node)
        {
            if (context.identifier() is { } identifier)
            {
                node.Label = GetIdentifier(identifier);
            }

            return node;
        }

        return null;
    }

    private static string GetIdentifier(ANTLRv4Parser.IdentifierContext identifier)
    {
        return identifier.TOKEN_REF()?.GetText() ?? identifier.RULE_REF().GetText();
    }

    public override SyntaxNode? VisitAltList(ANTLRv4Parser.AltListContext context)
    {
        var list = new AlternativeListSyntax();
        foreach (var alternative in context.alternative())
        {
            if (VisitAlternative(alternative) is AlternativeSyntax node)
            {
                list.Items.Add(node);
            }
        }

        return SpanAndComment(context, list);
    }

    public override SyntaxNode? VisitAtom(ANTLRv4Parser.AtomContext atom)
    {
        if (atom.terminal() is { } terminal)
        {
            if (terminal.TOKEN_REF() is { } tokenRef)
            {
                return SpanAndComment(terminal, new Token(tokenRef.GetText()));
            }
            else if (terminal.STRING_LITERAL() is { } stringLiteral)
            {
                return SpanAndComment(terminal, new LiteralSyntax(stringLiteral.GetText()));
            }
        }
        else if (atom.ruleref() is { } ruleRef)
        {
            return SpanAndComment(ruleRef, new RuleRefSyntax(ruleRef.RULE_REF().GetText()));
        }

        // Not supported
        return null;
    }

    public override SyntaxNode? VisitBlock(ANTLRv4Parser.BlockContext context)
    {
        var block = new BlockSyntax();
        foreach (var alternative in context.altList().alternative())
        {
            if (VisitAlternative(alternative) is AlternativeSyntax node)
            {
                block.Items.Add(node);
            }
        }
        return SpanAndComment(context, block);
    }

    public override SyntaxNode? VisitElement(ANTLRv4Parser.ElementContext context)
    {
        //element
        //   : labeledElement (ebnfSuffix |)
        //   | atom (ebnfSuffix |)
        //   | ebnf
        //   | actionBlock QUESTION?
        //   ;

        if (context.labeledElement() is { } labeledElement)
        {
            var identifier = labeledElement.identifier();
            var node = labeledElement.atom() is { } atom ? VisitAtom(atom) : VisitBlock(labeledElement.block());
            if (node is ElementSyntax ruleNode)
            {
                ruleNode.Label = GetIdentifier(identifier);
            }

            return node;
        }
        else if (context.ebnf() is { } ebnf && VisitBlock(ebnf.block()) is BlockSyntax blockSyntax)
        {
            if (ebnf.blockSuffix()?.ebnfSuffix() is { } suffix)
            {
                ApplySuffix(suffix, blockSyntax);
            }

            return blockSyntax;
        }
        else if (context.atom() is { } atom && VisitAtom(atom) is ElementSyntax atomSyntax)
        {
            if (context.ebnfSuffix() is { } suffix)
            {
                ApplySuffix(suffix, atomSyntax);
            }
            return atomSyntax;
        }

        return base.VisitElement(context);
    }

    public override SyntaxNode? VisitOptionsSpec(ANTLRv4Parser.OptionsSpecContext context)
    {
        // optionsSpec
        // : OPTIONS(option SEMI) * RBRACE;
        var optionList = SpanAndComment(context, new OptionsSyntax());
        foreach (var option in context.option())
        {
            optionList.Items.Add((OptionSyntax)VisitOption(option)!);
        }

        return optionList;
    }

    public override SyntaxNode? VisitOption(ANTLRv4Parser.OptionContext option)
    {
        //option
        //   : identifier ASSIGN optionValue
        var name = GetIdentifier(option.identifier());
        object? value = null;
        //optionValue
        //   : identifier(DOT identifier)*
        //   | STRING_LITERAL
        //   | actionBlock
        //   | INT
        //   ;
        var optionValue = option.optionValue();

        var identifiers = optionValue.identifier();
        if (identifiers.Length > 0)
        {
            value = string.Join(".", identifiers.Select(GetIdentifier));
        }
        else if (optionValue.STRING_LITERAL() is { } strLiteral)
        {
            value = strLiteral.GetText();
        }
        else if (optionValue.INT() is { } intLiteral)
        {
            if (int.TryParse(intLiteral.GetText(), out var intValue))
            {
                value = intValue;
            }
        }

        return SpanAndComment(option, new OptionSyntax(name, value));
    }

    public override SyntaxNode? VisitDelegateGrammars(ANTLRv4Parser.DelegateGrammarsContext context)
    {
        var importSyntax = SpanAndComment(context, new ImportSyntax());

        foreach(var delegateGrammar in context.delegateGrammar())
        {
            var identifiers = delegateGrammar.identifier();
            var importName = SpanAndComment(delegateGrammar, new ImportNameSyntax(GetIdentifier(identifiers[0])));
            if (identifiers.Length == 2)
            {
                importName.Value = GetIdentifier(identifiers[1]);
            }
            importSyntax.Names.Add(importName);
        }

        return importSyntax;
    }

    public override SyntaxNode? VisitTokensSpec(ANTLRv4Parser.TokensSpecContext context)
    {
        var tokens = SpanAndComment(context, new TokensSyntax());

        foreach (var id in context.idList().identifier())
        {
            tokens.Ids.Add(GetIdentifier(id));
        }

        return tokens;
    }

    public override SyntaxNode? VisitChannelsSpec(ANTLRv4Parser.ChannelsSpecContext context)
    {
        var tokens = SpanAndComment(context, new ChannelsSyntax());

        foreach (var id in context.idList().identifier())
        {
            tokens.Ids.Add(GetIdentifier(id));
        }

        return tokens;
    }
    
    private static void ApplySuffix(ANTLRv4Parser.EbnfSuffixContext suffix, ElementSyntax node)
    {
        var delta = suffix.QUESTION().Length == 2 ? 3 : 0;
        node.Suffix = (suffix.PLUS() != null ? SuffixKind.Plus : suffix.STAR() != null ? SuffixKind.Star : SuffixKind.Optional) + delta;
    }

    private static TextSpan CreateSpan(ParserRuleContext context)
    {
        var start = context.Start;
        var stop  = context.Stop;
        return new TextSpan(start.TokenSource.SourceName)
        {
            Begin = new TextLocation(start.StartIndex, start.Line, start.Column),
            End = new TextLocation(stop.StopIndex, stop.Line, stop.Column)
        };
    }

    private T SpanAndComment<T>(ParserRuleContext context, T node) where T : SyntaxNode
    {
        node.Span = CreateSpan(context);
        AddTokens(_tokens.GetHiddenTokensToLeft(context.Start.TokenIndex), node.CommentsBefore);
        AddTokens(_tokens.GetHiddenTokensToRight(context.Stop.TokenIndex), node.CommentsAfter);

        void AddTokens(IList<IToken>? tokens, List<CommentSyntax> comments)
        {
            if (tokens is null) return;

            foreach (var token in tokens)
            {
                if (token.Type == ANTLRv4Lexer.DOC_COMMENT)
                {
                    comments.Add(new CommentSyntax(token.Text, CommentKind.Doc));
                }
                else if (token.Type == ANTLRv4Lexer.LINE_COMMENT)
                {
                    comments.Add(new CommentSyntax(token.Text, CommentKind.Line));
                }
                else if (token.Type == ANTLRv4Lexer.BLOCK_COMMENT)
                {
                    comments.Add(new CommentSyntax(token.Text, CommentKind.Block));
                }
            }
        }

        return node;
    }
}