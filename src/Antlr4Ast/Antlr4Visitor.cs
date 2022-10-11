// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

namespace Antlr4Ast;

/// <summary>
/// Base class for a visitor.
/// </summary>
public abstract class Antlr4Visitor
{
    /// <summary>
    /// Visits the specified list of alternative.
    /// </summary>
    /// <param name="alternativeList">The list of alternative to visit.</param>
    public virtual void Visit(AlternativeListSyntax alternativeList) => DefaultVisit(alternativeList);

    /// <summary>
    /// Visits the specified alternative.
    /// </summary>
    /// <param name="alternative">The alternative to visit.</param>
    public virtual void Visit(AlternativeSyntax alternative) => DefaultVisit(alternative);

    /// <summary>
    /// Visits the specified block.
    /// </summary>
    /// <param name="block">The block to visit.</param>
    public virtual void Visit(BlockSyntax block) => DefaultVisit(block);

    /// <summary>
    /// Visits the specified channels.
    /// </summary>
    /// <param name="channels">The channels to visit.</param>
    public virtual void Visit(ChannelsSyntax channels) => DefaultVisit(channels);

    /// <summary>
    /// Visits the specified character range.
    /// </summary>
    /// <param name="characterRange">The character range to visit.</param>
    public virtual void Visit(CharacterRange characterRange) => DefaultVisit(characterRange);

    /// <summary>
    /// Visits the specified dot element.
    /// </summary>
    /// <param name="dotElement">The dot element to visit.</param>
    public virtual void Visit(DotSyntax dotElement) => DefaultVisit(dotElement);
    
    /// <summary>
    /// Visits the specified element options.
    /// </summary>
    /// <param name="elementOptions">The element options to visit.</param>
    public virtual void Visit(ElementOptionsSyntax elementOptions) => DefaultVisit(elementOptions);
    
    /// <summary>
    /// Visits the specified element option.
    /// </summary>
    /// <param name="elementOption">The element option to visit.</param>
    public virtual void Visit(ElementOptionSyntax elementOption) => DefaultVisit(elementOption);

    /// <summary>
    /// Visits the specified element option.
    /// </summary>
    /// <param name="emptyElement">The element option to visit.</param>
    public virtual void Visit(EmptySyntax emptyElement) => DefaultVisit(emptyElement);

    /// <summary>
    /// Visits the specified grammar.
    /// </summary>
    /// <param name="grammar">The grammar to visit.</param>
    public virtual void Visit(GrammarSyntax grammar) => DefaultVisit(grammar);
    
    /// <summary>
    /// Visits the specified import name.
    /// </summary>
    /// <param name="importName">The import name to visit.</param>
    public virtual void Visit(ImportNameSyntax importName) => DefaultVisit(importName);
    
    /// <summary>
    /// Visits the specified import.
    /// </summary>
    /// <param name="import">The import to visit.</param>
    public virtual void Visit(ImportSyntax import) => DefaultVisit(import);
    
    /// <summary>
    /// Visits the specified lexer block.
    /// </summary>
    /// <param name="lexerBlock">The lexer block to visit.</param>
    public virtual void Visit(LexerBlockSyntax lexerBlock) => DefaultVisit(lexerBlock);

    /// <summary>
    /// Visits the specified lexer character set.
    /// </summary>
    /// <param name="lexerCharSet">The lexer character set to visit.</param>
    public virtual void Visit(LexerChar lexerCharSet) => DefaultVisit(lexerCharSet);

    /// <summary>
    /// Visits the specified lexer commands.
    /// </summary>
    /// <param name="lexerCommands">The lexer commands to visit.</param>
    public virtual void Visit(LexerCommandsSyntax lexerCommands) => DefaultVisit(lexerCommands);
    
    /// <summary>
    /// Visits the specified lexer command.
    /// </summary>
    /// <param name="lexerCommand">The lexer command to visit.</param>
    public virtual void Visit(LexerCommandSyntax lexerCommand) => DefaultVisit(lexerCommand);

    /// <summary>
    /// Visits the specified lexer mode.
    /// </summary>
    /// <param name="lexerMode">The lexer mode to visit.</param>
    public virtual void Visit(LexerModeSyntax lexerMode) => DefaultVisit(lexerMode);

    /// <summary>
    /// Visits the specified literal.
    /// </summary>
    /// <param name="literal">The literal to visit.</param>
    public virtual void Visit(LiteralSyntax literal) => DefaultVisit(literal);
    
    /// <summary>
    /// Visits the specified options.
    /// </summary>
    /// <param name="options">The options to visit.</param>
    public virtual void Visit(OptionsSyntax options) => DefaultVisit(options);

    /// <summary>
    /// Visits the specified options.
    /// </summary>
    /// <param name="option">The options to visit.</param>
    public virtual void Visit(OptionSyntax option) => DefaultVisit(option);

    /// <summary>
    /// Visits the specified rule reference.
    /// </summary>
    /// <param name="ruleRef">The rule reference to visit.</param>
    public virtual void Visit(RuleRef ruleRef) => DefaultVisit(ruleRef);

    /// <summary>
    /// Visits the specified rule.
    /// </summary>
    /// <param name="rule">The rule to visit.</param>
    public virtual void Visit(RuleSyntax rule) => DefaultVisit(rule);

    /// <summary>
    /// Visits the specified token reference.
    /// </summary>
    /// <param name="tokenRef">The token reference to visit.</param>
    public virtual void Visit(TokenRef tokenRef) => DefaultVisit(tokenRef);
    
    /// <summary>
    /// Visits the specified tokens.
    /// </summary>
    /// <param name="tokens">The tokens to visit.</param>
    public virtual void Visit(TokensSyntax tokens) => DefaultVisit(tokens);
    
    /// <summary>
    /// Visit the base <see cref="SyntaxNode"/>.
    /// </summary>
    /// <param name="node">The base syntax node.</param>
    public virtual void Visit(SyntaxNode node) => DefaultVisit(node);

    /// <summary>
    /// The default visit will visit first the children of the specified node before visiting the specified node.
    /// </summary>
    /// <param name="node">The node to visit.</param>
    public virtual void DefaultVisit(SyntaxNode node)
    {
        foreach (var syntaxNode in node.Children())
        {
            syntaxNode.Accept(this);
        }
    }
}

/// <summary>
/// Base class for a transform visitor.
/// </summary>
/// <typeparam name="TResult">The result of the transform.</typeparam>
public abstract class Antlr4Visitor<TResult>
{
    /// <summary>
    /// Transforms the specified list of alternative.
    /// </summary>
    /// <param name="alternativeList">The list of alternative to visit.</param>
    /// <returns>The transformed result.</returns>
    public virtual TResult? Visit(AlternativeListSyntax alternativeList) => DefaultVisit(alternativeList);

    /// <summary>
    /// Transforms the specified alternative.
    /// </summary>
    /// <param name="alternative">The alternative to visit.</param>
    /// <returns>The transformed result.</returns>
    public virtual TResult? Visit(AlternativeSyntax alternative) => DefaultVisit(alternative);

    /// <summary>
    /// Transforms the specified block.
    /// </summary>
    /// <param name="block">The block to visit.</param>
    /// <returns>The transformed result.</returns>
    public virtual TResult? Visit(BlockSyntax block) => DefaultVisit(block);

    /// <summary>
    /// Transforms the specified channels.
    /// </summary>
    /// <param name="channels">The channels to visit.</param>
    /// <returns>The transformed result.</returns>
    public virtual TResult? Visit(ChannelsSyntax channels) => DefaultVisit(channels);

    /// <summary>
    /// Transforms the specified character range.
    /// </summary>
    /// <param name="characterRange">The character range to visit.</param>
    /// <returns>The transformed result.</returns>
    public virtual TResult? Visit(CharacterRange characterRange) => DefaultVisit(characterRange);

    /// <summary>
    /// Transforms the specified dot element.
    /// </summary>
    /// <param name="dotElement">The dot element to visit.</param>
    /// <returns>The transformed result.</returns>
    public virtual TResult? Visit(DotSyntax dotElement) => DefaultVisit(dotElement);

    /// <summary>
    /// Transforms the specified element options.
    /// </summary>
    /// <param name="elementOptions">The element options to visit.</param>
    /// <returns>The transformed result.</returns>
    public virtual TResult? Visit(ElementOptionsSyntax elementOptions) => DefaultVisit(elementOptions);

    /// <summary>
    /// Transforms the specified element option.
    /// </summary>
    /// <param name="elementOption">The element option to visit.</param>
    /// <returns>The transformed result.</returns>
    public virtual TResult? Visit(ElementOptionSyntax elementOption) => DefaultVisit(elementOption);

    /// <summary>
    /// Transforms the specified element option.
    /// </summary>
    /// <param name="emptyElement">The element option to visit.</param>
    /// <returns>The transformed result.</returns>
    public virtual TResult? Visit(EmptySyntax emptyElement) => DefaultVisit(emptyElement);

    /// <summary>
    /// Transforms the specified grammar.
    /// </summary>
    /// <param name="grammar">The grammar to visit.</param>
    /// <returns>The transformed result.</returns>
    public virtual TResult? Visit(GrammarSyntax grammar) => DefaultVisit(grammar);

    /// <summary>
    /// Transforms the specified import name.
    /// </summary>
    /// <param name="importName">The import name to visit.</param>
    /// <returns>The transformed result.</returns>
    public virtual TResult? Visit(ImportNameSyntax importName) => DefaultVisit(importName);

    /// <summary>
    /// Transforms the specified import.
    /// </summary>
    /// <param name="import">The import to visit.</param>
    /// <returns>The transformed result.</returns>
    public virtual TResult? Visit(ImportSyntax import) => DefaultVisit(import);

    /// <summary>
    /// Transforms the specified lexer block.
    /// </summary>
    /// <param name="lexerBlock">The lexer block to visit.</param>
    /// <returns>The transformed result.</returns>
    public virtual TResult? Visit(LexerBlockSyntax lexerBlock) => DefaultVisit(lexerBlock);

    /// <summary>
    /// Transforms the specified lexer character set.
    /// </summary>
    /// <param name="lexerCharSet">The lexer character set to visit.</param>
    /// <returns>The transformed result.</returns>
    public virtual TResult? Visit(LexerChar lexerCharSet) => DefaultVisit(lexerCharSet);

    /// <summary>
    /// Transforms the specified lexer commands.
    /// </summary>
    /// <param name="lexerCommands">The lexer commands to visit.</param>
    /// <returns>The transformed result.</returns>
    public virtual TResult? Visit(LexerCommandsSyntax lexerCommands) => DefaultVisit(lexerCommands);

    /// <summary>
    /// Transforms the specified lexer command.
    /// </summary>
    /// <param name="lexerCommand">The lexer command to visit.</param>
    /// <returns>The transformed result.</returns>
    public virtual TResult? Visit(LexerCommandSyntax lexerCommand) => DefaultVisit(lexerCommand);

    /// <summary>
    /// Transforms the specified lexer mode.
    /// </summary>
    /// <param name="lexerMode">The lexer mode to visit.</param>
    /// <returns>The transformed result.</returns>
    public virtual TResult? Visit(LexerModeSyntax lexerMode) => DefaultVisit(lexerMode);

    /// <summary>
    /// Transforms the specified literal.
    /// </summary>
    /// <param name="literal">The literal to visit.</param>
    /// <returns>The transformed result.</returns>
    public virtual TResult? Visit(LiteralSyntax literal) => DefaultVisit(literal);

    /// <summary>
    /// Transforms the specified options.
    /// </summary>
    /// <param name="options">The options to visit.</param>
    /// <returns>The transformed result.</returns>
    public virtual TResult? Visit(OptionsSyntax options) => DefaultVisit(options);

    /// <summary>
    /// Transforms the specified options.
    /// </summary>
    /// <param name="option">The options to visit.</param>
    /// <returns>The transformed result.</returns>
    public virtual TResult? Visit(OptionSyntax option) => DefaultVisit(option);

    /// <summary>
    /// Transforms the specified rule reference.
    /// </summary>
    /// <param name="ruleRef">The rule reference to visit.</param>
    /// <returns>The transformed result.</returns>
    public virtual TResult? Visit(RuleRef ruleRef) => DefaultVisit(ruleRef);

    /// <summary>
    /// Transforms the specified rule.
    /// </summary>
    /// <param name="rule">The rule to visit.</param>
    /// <returns>The transformed result.</returns>
    public virtual TResult? Visit(RuleSyntax rule) => DefaultVisit(rule);

    /// <summary>
    /// Transforms the specified token reference.
    /// </summary>
    /// <param name="tokenRef">The token reference to visit.</param>
    /// <returns>The transformed result.</returns>
    public virtual TResult? Visit(TokenRef tokenRef) => DefaultVisit(tokenRef);

    /// <summary>
    /// Transforms the specified tokens.
    /// </summary>
    /// <param name="tokens">The tokens to visit.</param>
    /// <returns>The transformed result.</returns>
    public virtual TResult? Visit(TokensSyntax tokens) => DefaultVisit(tokens);

    /// <summary>
    /// Visit the base <see cref="SyntaxNode"/>.
    /// </summary>
    /// <param name="node">The base syntax node.</param>
    /// <returns>The transformed result.</returns>
    public virtual TResult? Visit(SyntaxNode node) => DefaultVisit(node);

    /// <summary>
    /// The default transform method that will visit first the children of the specified node before visiting the specified node.
    /// </summary>
    /// <param name="node">The node to visit.</param>
    /// <returns>The transformed result.</returns>
    public virtual TResult? DefaultVisit(SyntaxNode node)
    {
        foreach (var syntaxNode in node.Children())
        {
            syntaxNode.Accept(this);
        }

        return default;
    }
}