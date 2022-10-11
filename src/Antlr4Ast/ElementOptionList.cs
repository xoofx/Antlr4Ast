// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

/// <summary>
/// A list of <see cref="ElementOption"/> attached to an <see cref="SyntaxElement"/>.
/// </summary>
public sealed class ElementOptionList : SyntaxNode
{
    /// <summary>
    /// Creates an instance of this object.
    /// </summary>
    public ElementOptionList()
    {
        Items = new List<ElementOption>();
    }

    /// <summary>
    /// Gets the list of <see cref="ElementOption"/>.
    /// </summary>
    public List<ElementOption> Items { get; }

    /// <inheritdoc />
    public override IEnumerable<SyntaxNode> Children()
    {
        foreach (var elementOptionSyntax in Items)
        {
            yield return elementOptionSyntax;
        }
    }

    /// <inheritdoc />
    public override void Accept(GrammarVisitor visitor)
    {
        visitor.Visit(this);
    }

    /// <inheritdoc />
    public override TResult? Accept<TResult>(GrammarVisitor<TResult> transform) where TResult : default
    {
        return transform.Visit(this);
    }

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