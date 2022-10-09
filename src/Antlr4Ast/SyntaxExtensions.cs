// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

/// <summary>
/// Method extensions for various AST classes and enums.
/// </summary>
public static class SyntaxExtensions
{
    /// <summary>
    /// Converts the specified suffix into the equivalent string representation.
    /// </summary>
    /// <param name="suffix">The suffix enum.</param>
    /// <returns>A string representation of the suffix.</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static string ToText(this SuffixKind suffix) =>
        suffix switch
        {
            SuffixKind.None => "",
            SuffixKind.Star => "*",
            SuffixKind.Plus => "+",
            SuffixKind.Optional => "?",
            SuffixKind.StarGreedy => "*?",
            SuffixKind.PlusGreedy => "+?",
            SuffixKind.OptionalGreedy => "??",
            _ => throw new ArgumentOutOfRangeException(nameof(suffix), suffix, null)
        };

    /// <summary>
    /// Converts the specified label  into the equivalent string representation.
    /// </summary>
    /// <param name="labelKind">The label kind enum.</param>
    /// <returns>A string representation of the label kind.</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static string ToText(this LabelKind labelKind) =>
        labelKind switch
        {
            LabelKind.Assign => "=",
            LabelKind.PlusAssign => "+=",
            _ => throw new ArgumentOutOfRangeException(nameof(labelKind), labelKind, null)
        };

    /// <summary>
    /// Converts the specified grammar kind into the equivalent string representation.
    /// </summary>
    /// <param name="grammarKind">The grammar kind enum.</param>
    /// <returns>A string representation of the grammar kind.</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static string ToText(this GrammarKind grammarKind) =>
        grammarKind switch
        {
            GrammarKind.Full => "",
            GrammarKind.Lexer => "lexer",
            GrammarKind.Parser => "parser",
            _ => throw new ArgumentOutOfRangeException(nameof(grammarKind), grammarKind, null)
        };

    /// <summary>
    /// Appends the specified string literal to a <see cref="StringBuilder"/> with enclosing single quotes.
    /// </summary>
    /// <param name="literal">A literal string without enclosing single quotes.</param>
    /// <param name="builder">The string builder receiving the enclosed literal.</param>
    public static void ToLiteral(string literal, StringBuilder builder)
    {
        builder.Append('\'').Append(literal).Append('\'');
    }
}