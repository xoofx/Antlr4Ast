// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

public sealed class ElementOptionsSyntax : SyntaxNode
{
    public ElementOptionsSyntax()
    {
        Items = new List<ElementOptionSyntax>();
    }
    public List<ElementOptionSyntax> Items { get; }

    public override void ToText(StringBuilder builder)
    {
        if (Items.Count == 0) return;
        builder.Append('<');
        for (var i = 0; i < Items.Count; i++)
        {
            var elementOptionSyntax = Items[i];
            if (i > 0) builder.Append(", ");
            elementOptionSyntax.ToText(builder);
        }
        builder.Append('>');
    }
}