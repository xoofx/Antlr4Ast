// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

/// <summary>
/// A list of parser and lexer alternative separated by |.
/// </summary>
public class AlternativeListSyntax : ElementSyntax
{
    /// <summary>
    /// Creates a new instance of this object
    /// </summary>
    public AlternativeListSyntax()
    {
        Items = new List<AlternativeSyntax>();
    }

    /// <summary>
    /// Gets the alternatives.
    /// </summary>
    public List<AlternativeSyntax> Items { get; }

    /// <inheritdoc />
    protected override void ToTextImpl(StringBuilder builder, AntlrFormattingOptions options)
    {
        for (var i = 0; i < Items.Count; i++)
        {
            var item = Items[i];
            if (i > 0)
            {
                if (options.MultiLineWithComments && this.GetType() == typeof(AlternativeListSyntax))
                {
                    // Don't output a new line if we have already a comment
                    if (builder[builder.Length - 1] != '\n')
                    {
                        builder.AppendLine();
                    }
                    builder.Append(' ');
                }
                builder.Append(" | ");
            }
            item.ToText(builder, options);
        }
    }
}