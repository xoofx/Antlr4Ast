// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

public sealed class LexerChar : ElementSyntax
{
    public LexerChar(string value)
    {
        Value = value;
    }

    public string Value { get; set; }

    protected override void ToTextImpl(StringBuilder builder, FormattingOptions options)
    {
        if (IsNot) builder.Append("~ ");
        builder.Append(Value);
        builder.Append(Suffix.ToText());
    }
}