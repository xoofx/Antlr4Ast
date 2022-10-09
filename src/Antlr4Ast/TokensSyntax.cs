// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

public class TokensSyntax : SyntaxNode
{
    public TokensSyntax()
    {
        Ids = new List<string>();
    }

    public List<string> Ids { get; }

    protected override void ToTextImpl(StringBuilder builder, AntlrFormattingOptions options)
    {
        builder.Append("tokens { ");
        for (var i = 0; i < Ids.Count; i++)
        {
            var option = Ids[i];
            if (i > 0) builder.Append(", ");
            builder.Append(option);
        }

        builder.Append(" }");
    }
}