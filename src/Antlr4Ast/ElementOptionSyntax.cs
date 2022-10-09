// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Globalization;
using System.Text;

namespace Antlr4Ast;

public sealed class ElementOptionSyntax : SyntaxNode
{
    public ElementOptionSyntax(string name)
    {
        Name = name;
    }

    public string Name { get; set; }

    public object? Value { get; set; }

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