// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

public class AlternativeListSyntax : ElementSyntax
{
    public AlternativeListSyntax()
    {
        Items = new List<AlternativeSyntax>();
    }
    
    public List<AlternativeSyntax> Items { get; }
    
    protected override void ToTextImpl(StringBuilder builder, FormattingOptions options)
    {
        for (var i = 0; i < Items.Count; i++)
        {
            var item = Items[i];
            if (i > 0)
            {
                if (options.ShouldDisplayRulesAsMultiLine && this.GetType() == typeof(AlternativeListSyntax))
                {
                    builder.AppendLine().Append(' ');
                }
                builder.Append(" | ");
            }
            item.ToText(builder, options);
        }
    }
}