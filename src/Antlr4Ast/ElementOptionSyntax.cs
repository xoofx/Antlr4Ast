// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

/// <summary>
/// An attached option for an <see cref="ElementSyntax"/>.
/// </summary>
public sealed class ElementOptionSyntax : SyntaxNode
{
    /// <summary>
    /// Creates a new instance of this object.
    /// </summary>
    /// <param name="name">The name of the option.</param>
    public ElementOptionSyntax(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Gets or sets the name of this option.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the value of this option. The value can be an identifier (a string) or a literal (<see cref="LiteralSyntax"/>).
    /// </summary>
    public object? Value { get; set; }

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