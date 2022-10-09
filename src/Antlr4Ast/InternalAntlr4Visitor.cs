// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace Antlr4Ast;

internal class InternalAntlr4Visitor : ANTLRv4ParserBaseVisitor<SyntaxNode?>
{
    private readonly CommonTokenStream _tokens;
    private readonly GrammarSyntax _grammar;
    private readonly HashSet<int> _tokenIndicesUsed;

    public InternalAntlr4Visitor(CommonTokenStream tokens, GrammarSyntax grammarSyntax)
    {
        _tokens = tokens;
        _grammar = grammarSyntax;
        _tokenIndicesUsed = new HashSet<int>();
    }

    public override SyntaxNode? VisitGrammarSpec(ANTLRv4Parser.GrammarSpecContext context)
    {
        // Attach comments
        SpanAndComment(context, _grammar);

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
                case ChannelsSyntax channelsSyntax:
                    _grammar.Channels.Add(channelsSyntax);
                    break;
                case TokensSyntax tokensSyntax:
                    _grammar.Tokens.Add(tokensSyntax);
                    break;
            }
        }

        // Parse parser and lexer rules
        foreach (var ruleSpec in context.rules().ruleSpec())
        {
            var ruleSyntax = (RuleSyntax)VisitRuleSpec(ruleSpec)!;
            if (ruleSyntax.IsLexer)
            {
                _grammar.LexerRules.Add(ruleSyntax);
            }
            else
            {
                _grammar.ParserRules.Add(ruleSyntax);
            }
        }

        foreach (var mode in context.modeSpec())
        {
            _grammar.LexerModes.Add((LexerModeSyntax)VisitModeSpec(mode)!);
        }

        return null;
    }

    public override SyntaxNode? VisitLexerRuleSpec(ANTLRv4Parser.LexerRuleSpecContext context)
    {
        var ruleName = context.TOKEN_REF().GetText();

        var alternativeList = (AlternativeListSyntax)VisitLexerRuleBlock(context.lexerRuleBlock())!;

        var lexerRule = new RuleSyntax(ruleName, alternativeList)
        {
            IsFragment = context.FRAGMENT() != null
        };

        if (context.optionsSpec() != null)
        {
            lexerRule.Options.Add((OptionsSyntax)VisitOptionsSpec(context.optionsSpec())!);
        }

        return SpanAndComment(context, lexerRule);
    }

    public override SyntaxNode? VisitParserRuleSpec(ANTLRv4Parser.ParserRuleSpecContext context)
    {
        var ruleName = context.RULE_REF().GetText();

        var node = (AlternativeListSyntax)VisitRuleBlock(context.ruleBlock())!;
        var rule = new RuleSyntax(ruleName, node);

        foreach (var prequel in context.rulePrequel())
        {
            if (VisitRulePrequel(prequel) is OptionsSyntax options)
            {
                rule.Options.Add(options);
            }
        }

        return SpanAndComment(context, rule);
    }

    public override SyntaxNode? VisitLexerAltList(ANTLRv4Parser.LexerAltListContext context)
    {
        var alternativeList = new AlternativeListSyntax();
        foreach (var lexerAlt in context.lexerAlt())
        {
            if (VisitLexerAlt(lexerAlt) is AlternativeSyntax node)
            {
                alternativeList.Items.Add(node);
            }
        }

        return SpanAndComment(context, alternativeList);
    }

    public override SyntaxNode? VisitLexerAlt(ANTLRv4Parser.LexerAltContext context)
    {
        var alternative = new AlternativeSyntax();
        var elements = context.lexerElements().lexerElement();
        foreach (var element in elements)
        {
            if (VisitLexerElement(element) is ElementSyntax node)
            {
                alternative.Elements.Add(node);
            }
        }

        if (context.lexerElements().lexerElement().Length == 0)
        {
            alternative.Elements.Add(SpanAndComment(context.lexerElements(), new EmptySyntax()));
        }

        if (context.lexerCommands() is { } commands)
        {
            alternative.LexerCommands = (LexerCommandsSyntax)VisitLexerCommands(commands)!;
        }

        return SpanAndComment(context, alternative);
    }

    public override SyntaxNode? VisitLexerCommands(ANTLRv4Parser.LexerCommandsContext context)
    {
        var commands = new LexerCommandsSyntax();
        foreach (var lexerCommand in context.lexerCommand())
        {
            if (VisitLexerCommand(lexerCommand) is LexerCommandSyntax lexerCommandSyntax)
            {
                commands.Items.Add(lexerCommandSyntax);
            }
        }

        return SpanAndComment(context, commands);
    }

    public override SyntaxNode? VisitLexerCommand(ANTLRv4Parser.LexerCommandContext context)
    {
        string name;
        if (context.lexerCommandName().identifier() is { } identifier)
        {
            name = GetIdentifier(identifier);
        }
        else
        {
            name = context.lexerCommandName().MODE().GetText();
        }

        var lexerCommand = new LexerCommandSyntax(name);
        if (context.lexerCommandExpr() is { } commandExpr)
        {
            if (commandExpr.identifier() is { } identifierExpr)
            {
                lexerCommand.Expression = GetIdentifier(identifierExpr);
            }
            else if (commandExpr.INT() is { } intExpr)
            {
                lexerCommand.Expression = int.Parse(intExpr.GetText());
            }
        }

        return SpanAndComment(context, lexerCommand);
    }

    public override SyntaxNode? VisitAlternative(ANTLRv4Parser.AlternativeContext context)
    {
        var alternative = new AlternativeSyntax();
        if (context.elementOptions() is { } elementOptions)
        {
            alternative.Options = (ElementOptionsSyntax)VisitElementOptions(elementOptions)!;
        }

        var elements = context.element();
        foreach (var element in elements)
        {
            if (VisitElement(element) is ElementSyntax node)
            {
                alternative.Elements.Add(node);
            }
        }

        if (elements.Length == 0)
        {
            alternative.Elements.Add(SpanAndComment(context, new EmptySyntax()));
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
                node.ParserLabel = GetIdentifier(identifier);
            }

            return node;
        }

        return null;
    }

    private static string GetIdentifier(ANTLRv4Parser.IdentifierContext identifier)
    {
        return identifier.TOKEN_REF()?.GetText() ?? identifier.RULE_REF().GetText();
    }

    //public override SyntaxNode? VisitAltList(ANTLRv4Parser.AltListContext context)
    //{
    //    var list = new AlternativeListSyntax();
    //    foreach (var alternative in context.alternative())
    //    {
    //        if (VisitAlternative(alternative) is AlternativeSyntax node)
    //        {
    //            list.Items.Add(node);
    //        }
    //    }

    //    return SpanAndComment(context, list);
    //}

    public override SyntaxNode? VisitAtom(ANTLRv4Parser.AtomContext atom)
    {
        //atom
        //   : terminal
        //   | ruleref
        //   | notSet
        //   | DOT elementOptions?
        //   ;
        if (atom.terminal() is { } terminal)
        {
            if (terminal.TOKEN_REF() is { } tokenRef)
            {
                return SpanAndComment(terminal, new Token(tokenRef.GetText()));
            }
            else if (terminal.STRING_LITERAL() is { } stringLiteral)
            {
                return SpanAndComment(stringLiteral, new LiteralSyntax(GetStringLiteral(stringLiteral)));
            }
        }
        else if (atom.ruleref() is { } ruleRef)
        {
            return SpanAndComment(ruleRef, new RuleRefSyntax(ruleRef.RULE_REF().GetText()));
        }
        else if (atom.notSet() is { } notSet)
        {
            return VisitNotSet(notSet);
        }
        else if (atom.DOT() != null)
        {
            var elementOptions = atom.elementOptions() is { } eltOptions ? (ElementOptionsSyntax)VisitElementOptions(eltOptions)! : null;
            return SpanAndComment(atom, new DotSyntax() { ElementOptions = elementOptions });
        }

        // Not supported
        return null;
    }

    public override SyntaxNode? VisitModeSpec(ANTLRv4Parser.ModeSpecContext context)
    {
        var modeSpec = new LexerModeSyntax(GetIdentifier(context.identifier()));
        foreach (var lexerRuleSpec in context.lexerRuleSpec())
        {
            if (VisitLexerRuleSpec(lexerRuleSpec) is RuleSyntax rule)
            {
                modeSpec.LexerRules.Add(rule);
            }
        }

        return SpanAndComment(context, modeSpec);
    }

    public override SyntaxNode? VisitBlock(ANTLRv4Parser.BlockContext context)
    {
        var block = new BlockSyntax();
        if (context.optionsSpec() is { } optionsSpec)
        {
            block.Options = (OptionsSyntax)VisitOptionsSpec(optionsSpec)!;
        }

        foreach (var alternative in context.altList().alternative())
        {
            if (VisitAlternative(alternative) is AlternativeSyntax node)
            {
                block.Items.Add(node);
            }
        }
        return SpanAndComment(context, block);
    }

    public override SyntaxNode? VisitLexerElement(ANTLRv4Parser.LexerElementContext context)
    {
        //lexerElement
        //    : labeledLexerElement ebnfSuffix?
        //    | lexerAtom ebnfSuffix ?
        //    | lexerBlock ebnfSuffix ?
        //    | actionBlock QUESTION ?
        //    ;
        ElementSyntax? node;

        if (context.labeledLexerElement() is { } labeledElement)
        {
            var identifier = labeledElement.identifier();
            node = (ElementSyntax)(labeledElement.lexerAtom() is { } atom ? VisitLexerAtom(atom)! : VisitLexerBlock(labeledElement.lexerBlock())!);
            node.Label = GetIdentifier(identifier);
            node.LabelKind = labeledElement.ASSIGN() != null ? LabelKind.Assign : LabelKind.PlusAssign;

            if (context.ebnfSuffix() is { } suffix)
            {
                ApplySuffix(suffix, node);
            }
        }
        else if (context.lexerBlock() is { } lexerBlock)
        {
            node = (ElementSyntax)VisitLexerBlock(lexerBlock)!;
        }
        else if (context.lexerAtom() is { } atom)
        {
            node = (ElementSyntax)VisitLexerAtom(atom)!;
        }
        else
        {
            // We don't handle action block
            node = null;
        }

        if (node is not null && context.ebnfSuffix() != null)
        {
            ApplySuffix(context.ebnfSuffix(), node!);
        }

        return node;
    }

    public override SyntaxNode? VisitLexerBlock(ANTLRv4Parser.LexerBlockContext context)
    {
        var blockSyntax = new BlockSyntax();
        foreach (var lexerAlt in context.lexerAltList().lexerAlt())
        {
            if (VisitLexerAlt(lexerAlt) is AlternativeSyntax alt)
            {
                blockSyntax.Items.Add(alt);
            }
        }

        return SpanAndComment(context, blockSyntax);
    }

    public override SyntaxNode? VisitElementOptions(ANTLRv4Parser.ElementOptionsContext context)
    {
        //elementOptions
        //    : LT elementOption(COMMA elementOption)*GT
        //    ;
        var elementOptions = new ElementOptionsSyntax();
        foreach (var elementOptionContext in context.elementOption())
        {
            elementOptions.Items.Add((ElementOptionSyntax)VisitElementOption(elementOptionContext)!);
        }

        return SpanAndComment(context, elementOptions);
    }

    public override SyntaxNode? VisitElementOption(ANTLRv4Parser.ElementOptionContext context)
    {
        //elementOption
        //    : identifier
        //    | identifier ASSIGN(identifier | STRING_LITERAL)
        //    ;
        var elementOption = new ElementOptionSyntax(GetIdentifier(context.identifier()[0]))
        {
            Value = context.identifier().Length == 2 ? GetIdentifier(context.identifier()[1]) : SpanAndComment(context.STRING_LITERAL(), new LiteralSyntax(GetStringLiteral(context.STRING_LITERAL())))
        };
        return SpanAndComment(context, elementOption);
    }

    private string GetStringLiteral(ITerminalNode node)
    {
        var text = node.GetText();
        if (text.StartsWith('\'') && text.EndsWith('\''))
        {
            text = text.Substring(1, text.Length - 2);
        }
        return text;
    }

    public override SyntaxNode? VisitLexerAtom(ANTLRv4Parser.LexerAtomContext atom)
    {
        if (atom.terminal() is { } terminal)
        {
            return VisitTerminal(terminal);
        }

        if (atom.characterRange() is { } characterRange)
        {
            return VisitCharacterRange(characterRange);
        }

        if (atom.DOT() != null)
        {
            var elementOptions = atom.elementOptions() is { } eltOptions ? (ElementOptionsSyntax)VisitElementOptions(eltOptions)! : null;
            return SpanAndComment(atom, new DotSyntax() { ElementOptions = elementOptions });
        }

        if (atom.notSet() is { } notSet)
        {
            return VisitNotSet(notSet);
        }

        var lexerCharSet = atom.LEXER_CHAR_SET();
        return VisitLexerCharSet(atom, lexerCharSet);
    }

    private LexerChar VisitLexerCharSet(ParserRuleContext context, ITerminalNode node)
    {
        return SpanAndComment(context, new LexerChar(node.GetText()));
    }
    
    public override SyntaxNode? VisitNotSet(ANTLRv4Parser.NotSetContext context)
    {
        if (context.blockSet() is { } blockSet)
        {
            var element = (ElementSyntax)VisitBlockSet(blockSet)!;
            element.IsNot = true;
            return element;
        }

        if (context.setElement() is { } setElement)
        {
            var element = (ElementSyntax)VisitSetElement(setElement)!;
            element.IsNot = true;
            return element;
        }

        return null;
    }

    public override SyntaxNode? VisitBlockSet(ANTLRv4Parser.BlockSetContext context)
    {
        //blockSet
        //    : LPAREN setElement(OR setElement)*RPAREN
        //    ;
        var blockSyntax = new LexerBlockSyntax();
        foreach (var setElement in context.setElement())
        {
            if (VisitSetElement(setElement) is ElementSyntax elementSyntax)
            {
                blockSyntax.Items.Add(elementSyntax);
            }
        }

        return SpanAndComment(context, blockSyntax);
    }

    public override SyntaxNode? VisitSetElement(ANTLRv4Parser.SetElementContext context)
    {
        //setElement
        //   : TOKEN_REF elementOptions?
        //   | STRING_LITERAL elementOptions?
        //   | characterRange
        //   | LEXER_CHAR_SET
        //   ;
        var elementOptions = context.elementOptions() is { } eltOptions ? (ElementOptionsSyntax)VisitElementOptions(eltOptions)! : null;
        if (context.TOKEN_REF() is { } tokenRef)
        {
            return SpanAndComment(context, new Token(tokenRef.GetText()) { ElementOptions = elementOptions });

        }
        else if (context.STRING_LITERAL() is { } stringLiteral)
        {
            return SpanAndComment(stringLiteral, new LiteralSyntax(GetStringLiteral(stringLiteral)) { ElementOptions = elementOptions });
        }
        else if (context.characterRange() is { } characterRange)
        {
            return VisitCharacterRange(characterRange);

        }
        else if (context.LEXER_CHAR_SET() is {} lexerCharSet)
        {
            return VisitLexerCharSet(context, lexerCharSet);
        }

        return base.VisitSetElement(context);
    }

    public override SyntaxNode? VisitCharacterRange(ANTLRv4Parser.CharacterRangeContext context)
    {
        //characterRange
        //   : STRING_LITERAL RANGE STRING_LITERAL
        //   ;
        var from = GetStringLiteral(context.STRING_LITERAL()[0]);
        var to = GetStringLiteral(context.STRING_LITERAL()[1]);
        return SpanAndComment(context, new CharacterRange(from, to));
    }

    public override SyntaxNode? VisitTerminal(ANTLRv4Parser.TerminalContext terminal)
    {
        //terminal
        //   : TOKEN_REF elementOptions?
        //   | STRING_LITERAL elementOptions?
        //   ;
        var elementOptions = terminal.elementOptions() is { } eltOptions ? (ElementOptionsSyntax)VisitElementOptions(eltOptions)! : null;

        if (terminal.TOKEN_REF() is { } tokenRef)
        {
            return SpanAndComment(terminal, new Token(tokenRef.GetText()) { ElementOptions = elementOptions });
        }
        return SpanAndComment(terminal, new LiteralSyntax(GetStringLiteral(terminal.STRING_LITERAL())) { ElementOptions = elementOptions });
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
            var node = labeledElement.atom() is { } atom ? (ElementSyntax)VisitAtom(atom)! : (ElementSyntax)VisitBlock(labeledElement.block())!;
            node.Label = GetIdentifier(identifier);
            node.LabelKind = labeledElement.ASSIGN() != null ? LabelKind.Assign : LabelKind.PlusAssign;

            if (context.ebnfSuffix() is { } suffix)
            {
                ApplySuffix(suffix, node);
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
        var optionList = new OptionsSyntax();
        foreach (var option in context.option())
        {
            optionList.Items.Add((OptionSyntax)VisitOption(option)!);
        }

        return SpanAndComment(context, optionList);
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
            value = SpanAndComment(strLiteral, new LiteralSyntax(GetStringLiteral(strLiteral)));
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
        //delegateGrammars
        //   : IMPORT delegateGrammar (COMMA delegateGrammar)* SEMI
        //   ;
        var importSyntax = new ImportSyntax();

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

        return SpanAndComment(context, importSyntax);
    }

    public override SyntaxNode? VisitTokensSpec(ANTLRv4Parser.TokensSpecContext context)
    {
        //tokensSpec
        //   : TOKENS idList? RBRACE
        //   ;
        //idList
        //   : identifier (COMMA identifier)* COMMA?
        //   ;
        var tokens = new TokensSyntax();

        foreach (var id in context.idList().identifier())
        {
            tokens.Ids.Add(GetIdentifier(id));
        }

        return SpanAndComment(context, tokens);
    }

    public override SyntaxNode? VisitChannelsSpec(ANTLRv4Parser.ChannelsSpecContext context)
    {
        //channelsSpec
        //   : CHANNELS idList? RBRACE
        //   ;
        var tokens = new ChannelsSyntax();

        foreach (var id in context.idList().identifier())
        {
            tokens.Ids.Add(GetIdentifier(id));
        }

        return SpanAndComment(context, tokens);
    }
    
    private static void ApplySuffix(ANTLRv4Parser.EbnfSuffixContext suffix, ElementSyntax node)
    {
        var delta = suffix.QUESTION().Length == 2 ? 3 : 0;
        node.Suffix = (suffix.PLUS() != null ? SuffixKind.Plus : suffix.STAR() != null ? SuffixKind.Star : SuffixKind.Optional) + delta;
    }

    private static TextSpan CreateSpan(ParserRuleContext context) => CreateSpan(context.Start, context.Stop);

    private static TextSpan CreateSpan(ITerminalNode terminal) => CreateSpan(terminal.Symbol, terminal.Symbol);
    
    private static TextSpan CreateSpan(IToken start, IToken stop)
    {
        return new TextSpan(start.TokenSource.SourceName)
        {
            Begin = new TextLocation(start.StartIndex, start.Line, start.Column),
            End = new TextLocation(stop.StopIndex, stop.Line, stop.Column)
        };
    }

    private T SpanAndComment<T>(ITerminalNode terminal, T node) where T : SyntaxNode
    {
        node.Span = CreateSpan(terminal);
        AddComments(_tokens.GetHiddenTokensToLeft(terminal.Symbol.TokenIndex), node.CommentsBefore);
        AddComments(_tokens.GetHiddenTokensToRight(terminal.Symbol.TokenIndex), node.CommentsAfter, true);
        return node;
    }

    private T SpanAndComment<T>(ParserRuleContext context, T node) where T : SyntaxNode
    {
        node.Span = CreateSpan(context);
        AddComments(_tokens.GetHiddenTokensToLeft(context.Start.TokenIndex), node.CommentsBefore);
        AddComments(_tokens.GetHiddenTokensToRight(context.Stop.TokenIndex), node.CommentsAfter, true);
        return node;
    }

    private static readonly char[] EndOfLineChars = new[] { '\r', '\n' };

    void AddComments(IList<IToken>? tokens, List<CommentSyntax> comments, bool isAfter = false)
    {
        if (tokens is null) return;

        foreach (var token in tokens)
        {
            // Don't collect tokens after if they are not on the same line
            if (isAfter && token.Type == ANTLRv4Lexer.WS && token.Text.IndexOfAny(EndOfLineChars) >= 0)
            {
                break;
            }

            if (_tokenIndicesUsed.Contains(token.TokenIndex)) continue;

            if (token.Type == ANTLRv4Lexer.DOC_COMMENT)
            {
                comments.Add(new CommentSyntax(token.Text, CommentKind.Doc));
                _tokenIndicesUsed.Add(token.TokenIndex);
            }
            else if (token.Type == ANTLRv4Lexer.LINE_COMMENT)
            {
                comments.Add(new CommentSyntax(token.Text, CommentKind.Line));
                _tokenIndicesUsed.Add(token.TokenIndex);
            }
            else if (token.Type == ANTLRv4Lexer.BLOCK_COMMENT)
            {
                comments.Add(new CommentSyntax(token.Text, CommentKind.Block));
                _tokenIndicesUsed.Add(token.TokenIndex);
            }
        }
    }
}