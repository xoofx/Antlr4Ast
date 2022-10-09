// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

public sealed class LexerCommandsSyntax : SyntaxNode
{
    public LexerCommandsSyntax()
    {
        Items = new List<LexerCommandSyntax>();
    }

    public List<LexerCommandSyntax> Items { get; }

    protected override void ToTextImpl(StringBuilder builder, AntlrFormattingOptions options)
    {
        builder.Append(" -> ");
        for (var i = 0; i < Items.Count; i++)
        {
            var lexerCommand = Items[i];
            if (i > 0) builder.Append(", ");
            lexerCommand.ToText(builder, options);
        }
    }
}