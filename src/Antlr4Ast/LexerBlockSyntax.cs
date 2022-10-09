// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

public sealed class LexerBlockSyntax : ElementSyntax
{
    public LexerBlockSyntax()
    {
        Items = new List<ElementSyntax>();
    }

    public List<ElementSyntax> Items { get; }


    protected override void ToTextImpl(StringBuilder builder, FormattingOptions options)
    {
        builder.Append("( ");
        for (var i = 0; i < Items.Count; i++)
        {
            var item = Items[i];
            if (i > 0) builder.Append(" | ");
            item.ToText(builder, options);
        }

        builder.Append(" )");
    }
}