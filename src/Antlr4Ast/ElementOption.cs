// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

/// <summary>
/// An attached option for an <see cref="SyntaxElement"/>.
/// </summary>
public sealed class ElementOption : SyntaxNode
{
    /// <summary>
    /// Creates a new instance of this object.
    /// </summary>
    /// <param name="name">The name of the option.</param>
    public ElementOption(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Gets or sets the name of this option.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the value of this option. The value can be an identifier (a string) or a literal (<see cref="Literal"/>).
    /// </summary>
    public object? Value { get; set; }

    /// <inheritdoc />
    public override IEnumerable<SyntaxNode> Children()
    {
        yield break;
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
        builder.Append(Name);
        if (Value is null) return;

        builder.Append(" = ");
        if (Value is SyntaxNode node)
        {
            node.ToText(builder, options);
        }
        else
        {
            builder.Append(Value);
        }
    }
}