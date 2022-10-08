// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

public sealed class CharacterRange : ElementSyntax
{
    public CharacterRange(string from, string to)
    {
        From = from;
        To = to;
    }

    public string From { get; set; }

    public string To { get; set; }

    protected override void ToTextImpl(StringBuilder builder, FormattingOptions options)
    {
        if (IsNot) builder.Append("~ ");

        SyntaxExtensions.ToLiteral(From, builder);
        builder.Append(" .. ");
        SyntaxExtensions.ToLiteral(To, builder);
    }
}