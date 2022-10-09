// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace Antlr4Ast;

/// <summary>
/// Internal visitor used for creating the AST from the raw ANTLRv4 visitor.
/// </summary>
internal sealed class InternalAntlr4Visitor : ANTLRv4ParserBaseVisitor<SyntaxNode?>
{
    private readonly CommonTokenStream _tokens;
    private readonly HashSet<int> _tokenIndicesUsed;

    public InternalAntlr4Visitor(CommonTokenStream tokens)
    {
        _tokens = tokens;
        _tokenIndicesUsed = new HashSet<int>();
    }

    public override SyntaxNode? VisitGrammarSpec(ANTLRv4Parser.GrammarSpecContext context)
    {
        //grammarSpec
        //   : grammarDecl prequelConstruct* rules modeSpec* EOF
        //   ;

        //grammarDecl
        //   : grammarType identifier SEMI
        //   ;

        //grammarType
        //   : (LEXER GRAMMAR | PARSER GRAMMAR | GRAMMAR)
        //   ;
        // Attach comments
        var grammar = new GrammarSyntax();
        SpanAndComment(context, grammar);

        // Parse GrammarDecl
        var grammarType = context.grammarDecl().grammarType();
        grammar.Kind = GrammarKind.Full;
        if (grammarType.LEXER() != null)
        {
            grammar.Kind = GrammarKind.Lexer;
        }
        else if (grammarType.PARSER() != null)
        {
            grammar.Kind = GrammarKind.Parser;
        }
        grammar.Name = GetIdentifier(context.grammarDecl().identifier());

        // Parse Options/Imports
        foreach (var prequel in context.prequelConstruct())
        {
            var node = VisitPrequelConstruct(prequel);
            switch (node)
            {
                case OptionsSyntax optionList:
                    grammar.Options.Add(optionList);
                    break;
                case ImportSyntax importSyntax:
                    grammar.Imports.Add(importSyntax);
                    break;
                case ChannelsSyntax channelsSyntax:
                    grammar.Channels.Add(channelsSyntax);
                    break;
                case TokensSyntax tokensSyntax:
                    grammar.Tokens.Add(tokensSyntax);
                    break;
            }
        }

        // Parse parser and lexer rules
        foreach (var ruleSpec in context.rules().ruleSpec())
        {
            var ruleSyntax = (RuleSyntax)VisitRuleSpec(ruleSpec)!;
            if (ruleSyntax.IsLexer)
            {
                grammar.LexerRules.Add(ruleSyntax);
            }
            else
            {
                grammar.ParserRules.Add(ruleSyntax);
            }
        }

        foreach (var mode in context.modeSpec())
        {
            grammar.LexerModes.Add((LexerModeSyntax)VisitModeSpec(mode)!);
        }

        return grammar;
    }

    public override SyntaxNode? VisitLexerRuleSpec(ANTLRv4Parser.LexerRuleSpecContext context)
    {
        //lexerRuleSpec
        //   : FRAGMENT? TOKEN_REF optionsSpec? COLON lexerRuleBlock SEMI
        //   ;
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
        //parserRuleSpec
        //   : ruleModifiers? RULE_REF argActionBlock? ruleReturns? throwsSpec? localsSpec? rulePrequel* COLON ruleBlock SEMI exceptionGroup
        //   ;
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
        //lexerAltList
        //   : lexerAlt (OR lexerAlt)*
        //   ;
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
        //lexerAlt
        //   : lexerElements lexerCommands?
        //   |
        //   // explicitly allow empty alts
        //   ;
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
        //lexerCommands
        //   : RARROW lexerCommand (COMMA lexerCommand)*
        //   ;
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
        //lexerCommand
        //   : lexerCommandName LPAREN lexerCommandExpr RPAREN
        //   | lexerCommandName
        //   ;
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
        //alternative
        //   : elementOptions? element+
        //   |
        //   // explicitly allow empty alts
        //   ;
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
        //ruleAltList
        //   : labeledAlt (OR labeledAlt)*
        //   ;
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
        //labeledAlt
        //   : alternative (POUND identifier)?
        //   ;
        var node = (AlternativeSyntax)VisitAlternative(context.alternative())!;
        if (context.identifier() is { } identifier)
        {
            node.ParserLabel = GetIdentifier(identifier);
        }
        return node;
    }

    private static string GetIdentifier(ANTLRv4Parser.IdentifierContext identifier)
    {
        return identifier.TOKEN_REF()?.GetText() ?? identifier.RULE_REF().GetText();
    }

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
            return VisitTerminal(terminal);
        }
        if (atom.ruleref() is { } ruleRef)
        {
            var elementOptions = ruleRef.elementOptions() is { } eltOptions ? (ElementOptionsSyntax)VisitElementOptions(eltOptions)! : null;
            return SpanAndComment(ruleRef, new RuleRef(ruleRef.RULE_REF().GetText()) { ElementOptions = elementOptions } );
        }
        if (atom.notSet() is { } notSet)
        {
            return VisitNotSet(notSet);
        }
        else
        {
            var elementOptions = atom.elementOptions() is { } eltOptions ? (ElementOptionsSyntax)VisitElementOptions(eltOptions)! : null;
            return SpanAndComment(atom, new DotSyntax() { ElementOptions = elementOptions });
        }
    }

    public override SyntaxNode? VisitModeSpec(ANTLRv4Parser.ModeSpecContext context)
    {
        //modeSpec
        //   : MODE identifier SEMI lexerRuleSpec*
        //   ;
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
        //block
        //   : LPAREN (optionsSpec? ruleAction* COLON)? altList RPAREN
        //   ;
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
        //lexerBlock
        //   : LPAREN lexerAltList RPAREN
        //   ;
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
        var identifiers = context.identifier();
        var elementOption = new ElementOptionSyntax(GetIdentifier(identifiers[0]));
        if (context.ASSIGN() is not null)
        {
            elementOption.Value = identifiers.Length == 2 ? GetIdentifier(identifiers[1]) : SpanAndComment(context.STRING_LITERAL(), new LiteralSyntax(GetStringLiteral(context.STRING_LITERAL())));
        }

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
        //lexerAtom
        //   : characterRange
        //   | terminal
        //   | notSet
        //   | LEXER_CHAR_SET
        //   | DOT elementOptions?
        //   ;
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
        //notSet
        //   : NOT setElement
        //   | NOT blockSet
        //   ;
        var element = context.blockSet() is { } blockSet ? (ElementSyntax)VisitBlockSet(blockSet)! : (ElementSyntax)VisitSetElement(context.setElement())!;
        element.IsNot = true;
        return element;
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
            return SpanAndComment(context, new TokenRef(tokenRef.GetText()) { ElementOptions = elementOptions });

        }
        else if (context.STRING_LITERAL() is { } stringLiteral)
        {
            return SpanAndComment(stringLiteral, new LiteralSyntax(GetStringLiteral(stringLiteral)) { ElementOptions = elementOptions });
        }
        else if (context.characterRange() is { } characterRange)
        {
            return VisitCharacterRange(characterRange);

        }
        return VisitLexerCharSet(context, context.LEXER_CHAR_SET());
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
            return SpanAndComment(terminal, new TokenRef(tokenRef.GetText()) { ElementOptions = elementOptions });
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
            importSyntax.Names.Add((ImportNameSyntax)VisitDelegateGrammar(delegateGrammar)!);
        }

        return SpanAndComment(context, importSyntax);
    }

    public override SyntaxNode? VisitDelegateGrammar(ANTLRv4Parser.DelegateGrammarContext context)
    {
        var identifiers = context.identifier();
        var importName = new ImportNameSyntax(GetIdentifier(identifiers[0]));
        if (identifiers.Length == 2)
        {
            importName.Value = GetIdentifier(identifiers[1]);
        }

        return SpanAndComment(context, importName);
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

    private static TextSpan CreateSpan(ParserRuleContext context) => Antlr4Parser.CreateSpan(context.Start, context.Stop);

    private static TextSpan CreateSpan(ITerminalNode terminal) => Antlr4Parser.CreateSpan(terminal.Symbol, terminal.Symbol);
    
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
                // /** ... */
                var text = token.Text;
                text = text.Substring(3, text.Length - (text.EndsWith("*/") ? 5 : 3 ));
                comments.Add(new CommentSyntax(text, CommentKind.Doc));
                _tokenIndicesUsed.Add(token.TokenIndex);
            }
            else if (token.Type == ANTLRv4Lexer.LINE_COMMENT)
            {
                // //
                var text = token.Text.Substring(2);
                comments.Add(new CommentSyntax(text, CommentKind.Line));
                _tokenIndicesUsed.Add(token.TokenIndex);
            }
            else if (token.Type == ANTLRv4Lexer.BLOCK_COMMENT)
            {
                // /* ... */
                var text = token.Text;
                text = text.Substring(2, text.Length - (text.EndsWith("*/") ? 4 : 2));
                comments.Add(new CommentSyntax(text, CommentKind.Block));
                _tokenIndicesUsed.Add(token.TokenIndex);
            }
        }
    }
}