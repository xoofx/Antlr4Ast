// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

public class ParserRuleSyntax : RuleSyntax
{
    public ParserRuleSyntax(string name, AlternativeListSyntax alternativeList) : base(name)
    {
        this.AlternativeList = alternativeList;
    }

    public AlternativeListSyntax AlternativeList { get; set; }

    public override void ToText(StringBuilder builder)
    {
        builder.Append(Name);
        builder.Append(": ");
        builder.Append(AlternativeList);
        builder.Append(";");
    }
}