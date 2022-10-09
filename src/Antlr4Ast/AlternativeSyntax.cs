// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

public sealed class AlternativeSyntax : SyntaxNode
{
    public AlternativeSyntax()
    {
        Elements = new List<ElementSyntax>();
    }

    public ElementOptionsSyntax? Options { get; set; }

    public List<ElementSyntax> Elements { get; }

    public string? ParserLabel { get; set; }

    public LexerCommandsSyntax? LexerCommands { get; set; }

    protected override void ToTextImpl(StringBuilder builder, AntlrFormattingOptions options)
    {
        if (Options is not null)
        {
            Options.ToText(builder, options);
            builder.Append(' ');
        }

        for (var i = 0; i < Elements.Count; i++)
        {
            var elementSyntax = Elements[i];
            if (i > 0) builder.Append(' ');
            elementSyntax.ToText(builder, options);
        }

        if (ParserLabel is not null)
        {
            builder.Append(" # ");
            builder.Append(ParserLabel);
        }
        else if (LexerCommands is not null)
        {
            LexerCommands.ToText(builder, options);
        }
    }
}