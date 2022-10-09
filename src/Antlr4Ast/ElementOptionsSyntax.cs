// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

/// <summary>
/// A list of <see cref="ElementOptionSyntax"/> attached to an <see cref="ElementSyntax"/>.
/// </summary>
public sealed class ElementOptionsSyntax : SyntaxNode
{
    /// <summary>
    /// Creates an instance of this object.
    /// </summary>
    public ElementOptionsSyntax()
    {
        Items = new List<ElementOptionSyntax>();
    }

    /// <summary>
    /// Gets the list of <see cref="ElementOptionSyntax"/>.
    /// </summary>
    public List<ElementOptionSyntax> Items { get; }
    
    /// <inheritdoc />
    protected override void ToTextImpl(StringBuilder builder, AntlrFormattingOptions options)
    {
        builder.Append(" <");
        for (var i = 0; i < Items.Count; i++)
        {
            var elementOptionSyntax = Items[i];
            if (i > 0) builder.Append(", ");
            elementOptionSyntax.ToText(builder, options);
        }
        builder.Append('>');
    }
}