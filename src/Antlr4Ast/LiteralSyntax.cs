// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

/// <summary>
/// An element used in a lexer/parser rule that defines a literal string enclosed by single quotes 'literal'.
/// </summary>
public sealed class LiteralSyntax : ElementSyntax
{
    /// <summary>
    /// Creates an instance of this object.
    /// </summary>
    /// <param name="text">The literal string without the single quotes.</param>
    public LiteralSyntax(string text)
    {
        Text = text;
    }

    /// <summary>
    /// Gets or sets the literal string without the single quotes.
    /// </summary>
    public string Text { get; set; }

    /// <inheritdoc />
    public override IEnumerable<SyntaxNode> Children()
    {
        if (ElementOptions is not null) yield return ElementOptions;
    }

    /// <inheritdoc />
    public override void Accept(Antlr4Visitor visitor)
    {
        visitor.Visit(this);
    }

    /// <inheritdoc />
    public override TResult? Accept<TResult>(Antlr4Visitor<TResult> transform) where TResult : default
    {
        return transform.Visit(this);
    }

    /// <inheritdoc />
    protected override void ToTextImpl(StringBuilder builder, AntlrFormattingOptions options)
    {
        SyntaxExtensions.ToLiteral(Text, builder);
    }
}