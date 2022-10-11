// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

/// <summary>
/// A block of alternatives ( options? altRule | altRule2 | ... | altRule# ).
/// </summary>
public sealed class BlockSyntax : AlternativeListSyntax
{
    /// <summary>
    /// Gets or sets the options attached inside this block. This is optional.
    /// </summary>
    public OptionsSyntax? Options { get; set; }

    /// <inheritdoc />
    public override IEnumerable<SyntaxNode> Children()
    {
        foreach (var syntaxNode in base.Children())
        {
            yield return syntaxNode;
        }

        if (Options is not null) yield return Options;
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
        if (Options != null)
        {
            Options.ToText(builder, options);
            builder.Append(": ");
        }
        base.ToTextImpl(builder, options);
        builder.Append(" )");
    }
}