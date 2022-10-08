// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

public class AlternativeSyntax : SyntaxNode
{
    public AlternativeSyntax()
    {
        Elements = new List<ElementSyntax>();
    }

    public List<ElementSyntax> Elements { get; }

    public string? Label { get; set; }

    public override void ToText(StringBuilder builder)
    {
        for (var i = 0; i < Elements.Count; i++)
        {
            var elementSyntax = Elements[i];
            if (i > 0) builder.Append(" ");
            builder.Append(elementSyntax);
        }

        if (Label is not null)
        {
            builder.Append(" # ");
            builder.Append(Label);
        }
    }
}