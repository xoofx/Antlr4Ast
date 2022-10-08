// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

public sealed class LexerCommandSyntax : SyntaxNode
{
    public LexerCommandSyntax(string name)
    {
        Name = name;
    }

    public string Name { get; set; }

    public object? Expression { get; set; }
    
    protected override void ToTextImpl(StringBuilder builder, FormattingOptions options)
    {
        builder.Append(Name);
        if (Expression != null)
        {
            builder.Append('(').Append(Expression).Append(')');
        }
    }
}