// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

/// <summary>
/// An element used in a lexer rule to define an alternative list of elements ( Lexer1 | Lexer2 .... | Lexer# ).
/// </summary>
public sealed class LexerBlockSyntax : ElementSyntax
{
    /// <summary>
    /// Creates a new instance of this object.
    /// </summary>
    public LexerBlockSyntax()
    {
        Items = new List<ElementSyntax>();
    }

    /// <summary>
    /// Gets the list of elements.
    /// </summary>
    public List<ElementSyntax> Items { get; }

    /// <inheritdoc />
    public override IEnumerable<SyntaxNode> Children()
    {
        if (ElementOptions is not null) yield return ElementOptions;

        foreach (var element in Items)
        {
            yield return element;
        }
    }

    /// <inheritdoc />
    public override void Accept(Antlr4Visitor visitor)
    {
        visitor.Visit(this);
    }

    /// <inheritdoc />
    public override TResult? Accept<TResult>(Antlr4Visitor<TResult> transform) where TResult : default
    {
        return transform.Visit(this);
    }

    /// <inheritdoc />
    protected override void ToTextImpl(StringBuilder builder, AntlrFormattingOptions options)
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