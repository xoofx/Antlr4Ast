// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

public sealed class OptionsSyntax : SyntaxNode
{
    public OptionsSyntax()
    {
        Items = new List<OptionSyntax>();
    }
    
    public List<OptionSyntax> Items { get; }
    
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