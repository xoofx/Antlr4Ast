// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

public sealed class Token : ElementSyntax
{
    public Token(string name)
    {
        Name = name;
    }

    public string Name { get; set; }

    public override void ToText(StringBuilder builder)
    {
        if (Label != null) builder.Append(Label).Append('=');
        builder.Append(Name);
        builder.Append(Suffix.ToText());
    }
}