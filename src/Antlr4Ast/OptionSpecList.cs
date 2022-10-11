// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

/// <summary>
/// Defines attached options (to a <see cref="Grammar.Options"/>, <see cref="Rule.Options"/> or a <see cref="Block.Options"/>).
/// </summary>
public sealed class OptionSpecList : SyntaxNode
{
    /// <summary>
    /// Creates an instance of this object.
    /// </summary>
    public OptionSpecList()
    {
        Items = new List<OptionSpec>();
    }

    /// <summary>
    /// Gets the list of options.
    /// </summary>
    public List<OptionSpec> Items { get; }

    /// <inheritdoc />
    public override IEnumerable<SyntaxNode> Children()
    {
        return Items;
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
        builder.Append("options { ");
        foreach(var option in Items)
        {
            option.ToText(builder, options);
            builder.Append("; ");
        }

        builder.Append("}");
    }
}