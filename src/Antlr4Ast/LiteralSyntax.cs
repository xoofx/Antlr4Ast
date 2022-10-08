// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

public sealed class LiteralSyntax : ElementSyntax
{
    public LiteralSyntax(string text)
    {
        Text = text;
    }

    public string Text { get; set; }

    public override void ToText(StringBuilder builder)
    {
        builder.Append(Text);
        builder.Append(Suffix.ToText());
    }
}