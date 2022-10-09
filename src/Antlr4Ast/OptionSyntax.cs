// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

public sealed class OptionSyntax : SyntaxNode
{
    public OptionSyntax(string name, object? value)
    {
        Name = name;
        Value = value;
    }

    public string Name { get; set; }

    public object? Value { get; set; }

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