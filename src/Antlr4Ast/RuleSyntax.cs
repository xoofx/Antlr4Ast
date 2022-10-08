// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

public sealed class RuleSyntax : SyntaxNode
{
    public RuleSyntax(string name, AlternativeListSyntax alternativeList)
    {
        Name = name;
        this.AlternativeList = alternativeList;
    }

    public string Name { get; set; }

    public bool IsLexer => Name.Length > 0 && char.IsUpper(Name[0]);
    
    public AlternativeListSyntax AlternativeList { get; set; }

    public bool IsFragment { get; set; }

    public OptionsSyntax? Options { get; set; }

    public override void ToText(StringBuilder builder)
    {
        if (IsFragment)
        {
            builder.Append("fragment ");
        }
        builder.Append(Name);
        builder.Append(": ");
        builder.Append(AlternativeList);
        builder.Append(';');
    }
}