// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

public class OptionsSyntax : SyntaxNode
{
    public OptionsSyntax()
    {
        Items = new List<OptionSyntax>();
    }
    
    public List<OptionSyntax> Items { get; }
    
    public override void ToText(StringBuilder builder)
    {
        builder.Append("options { ");
        foreach(var option in Items)
        {
            builder.Append(option);
            builder.Append("; ");
        }

        builder.Append(" }");
    }
}