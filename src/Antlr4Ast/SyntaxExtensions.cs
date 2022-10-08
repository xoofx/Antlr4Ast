// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

public static class SyntaxExtensions
{
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

    public static string ToText(this GrammarKind kind) =>
        kind switch
        {
            GrammarKind.Full => "",
            GrammarKind.Lexer => "lexer",
            GrammarKind.Parser => "parser",
            _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
        };
    
    public static void ToLiteral(string literal, StringBuilder builder)
    {
        // TODO: escape literal
        builder.Append('\'').Append(literal).Append('\'');
    }
}