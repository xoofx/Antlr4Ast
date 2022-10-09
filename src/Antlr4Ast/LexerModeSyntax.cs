// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

public sealed class LexerModeSyntax : SyntaxNode
{
    public LexerModeSyntax(string name)
    {
        Name = name;
        LexerRules = new List<RuleSyntax>();
    }

    public string Name { get; set; }

    public List<RuleSyntax> LexerRules { get;  }

    protected override void ToTextImpl(StringBuilder builder, AntlrFormattingOptions options)
    {
        builder.Append("mode ").Append(Name).Append(';').AppendLine();
        foreach (var lexerRule in LexerRules)
        {
            lexerRule.ToText(builder, options);
            builder.AppendLine();
        }
    }
}