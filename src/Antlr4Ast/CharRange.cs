// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

/// <summary>
/// An element used in a lexer/parser rule that defines a range of characters 'a' .. 'b'.
/// </summary>
public sealed class CharRange : SyntaxElement
{
    /// <summary>
    /// Creates a new instance of this object.
    /// </summary>
    /// <param name="from">The start character.</param>
    /// <param name="to">The end character.</param>
    public CharRange(string from, string to)
    {
        From = from;
        To = to;
    }

    /// <summary>
    /// Gets or sets the start character.
    /// </summary>
    public string From { get; set; }

    /// <summary>
    /// Gets or sets the end character.
    /// </summary>
    public string To { get; set; }

    /// <inheritdoc />
    public override IEnumerable<SyntaxNode> Children()
    {
        if (ElementOptions is not null) yield return ElementOptions;
    }

    /// <inheritdoc />
    public override void Accept(GrammarVisitor visitor)
    {
        visitor.Visit(this);
    }

    /// <inheritdoc />
    public override TResult? Accept<TResult>(GrammarVisitor<TResult> transform) where TResult : default
    {
        return transform.Visit(this);
    }

    /// <inheritdoc />
    protected override void ToTextImpl(StringBuilder builder, AntlrFormattingOptions options)
    {
        SyntaxExtensions.ToLiteral(From, builder);
        builder.Append(" .. ");
        SyntaxExtensions.ToLiteral(To, builder);
    }
}