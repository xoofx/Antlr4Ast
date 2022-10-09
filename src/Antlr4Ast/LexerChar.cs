// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

/// <summary>
/// An element used in a lexer rule that defines a character range.
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
    /// Gets or sets the character range.
    /// </summary>
    public string Value { get; set; }

    /// <inheritdoc />
    protected override void ToTextImpl(StringBuilder builder, AntlrFormattingOptions options)
    {
        builder.Append(Value);
    }
}