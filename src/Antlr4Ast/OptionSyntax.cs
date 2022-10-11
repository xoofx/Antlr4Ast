// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

/// <summary>
/// An option used in a an <see cref="OptionsSyntax"/>.
/// </summary>
public sealed class OptionSyntax : SyntaxNode
{
    /// <summary>
    /// Creates a new instance of this object.
    /// </summary>
    /// <param name="name">The name of the option.</param>
    /// <param name="value">The value associated to this name. Might be null, or an identifier or an integer.</param>
    public OptionSyntax(string name, object? value)
    {
        Name = name;
        Value = value;
    }

    /// <summary>
    /// Gets or sets the name of this option.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the value of this option. Might be null, or an identifier or an integer. 
    /// </summary>
    public object? Value { get; set; }

    /// <inheritdoc />
    public override IEnumerable<SyntaxNode> Children()
    {
        yield break;
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
        builder.Append(Name);
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