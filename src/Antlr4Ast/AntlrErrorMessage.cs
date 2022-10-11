// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

namespace Antlr4Ast;

/// <summary>
/// A class reporting a lexer/parser error used in <see cref="Grammar.ErrorMessages"/>.
/// </summary>
public sealed class AntlrErrorMessage
{
    /// <summary>
    /// Creates a new instance of this object.
    /// </summary>
    /// <param name="span">The span location the error is reported.</param>
    /// <param name="message">The error message.</param>
    public AntlrErrorMessage(TextSpan span, string message)
    {
        Span = span;
        Message = message;
    }
    /// <summary>
    /// Gets the span location the error is reported.
    /// </summary>
    public TextSpan Span { get; }

    /// <summary>
    /// The error message.
    /// </summary>
    public string Message { get; }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{Span}: error: {Message}";
    }
}