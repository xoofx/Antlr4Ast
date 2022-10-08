// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

public class OptionSyntax : SyntaxNode
{
    public OptionSyntax(string name, object? value)
    {
        Name = name;
        Value = value;
    }

    public string Name { get; set; }

    public object? Value { get; set; }

    public override void ToText(StringBuilder builder)
    {
        builder.Append(Name);
        builder.Append(" = ");
        builder.Append(Value);
    }
}