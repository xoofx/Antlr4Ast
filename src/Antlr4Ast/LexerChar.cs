// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

/// <summary>
/// An element used in a lexer rule that defines a character set (e.g [a-zA-Z])
/// </summary>
public sealed class LexerChar : ElementSyntax
{
    /// <summary>
    /// Creates a new instance of this object.
    /// </summary>
    /// <param name="value">The character range.</param>
    public LexerChar(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Gets or sets the character set.
    /// </summary>
    public string Value { get; set; }

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
        builder.Append(Value);
    }
}