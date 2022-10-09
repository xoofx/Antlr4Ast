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
        Options = new List<OptionsSyntax>();
        this.AlternativeList = alternativeList;
    }

    public string Name { get; set; }

    public bool IsLexer => Name.Length > 0 && char.IsUpper(Name[0]);

    public List<OptionsSyntax> Options { get; }

    public AlternativeListSyntax AlternativeList { get; set; }

    public bool IsFragment { get; set; }

    protected override void ToTextImpl(StringBuilder builder, AntlrFormattingOptions options)
    {
        if (IsFragment)
        {
            builder.Append("fragment ");
        }
        builder.Append(Name);
        foreach (var optionsSyntax in Options)
        {
            if (options.MultiLineWithComments)
            {
                builder.AppendLine().Append("  ");
            }
            optionsSyntax.ToText(builder, options);
        }

        if (options.MultiLineWithComments)
        {
            builder.AppendLine().Append("  ");
        }
        builder.Append(": ");

        AlternativeList.ToText(builder, options);

        if (options.MultiLineWithComments)
        {
            builder.AppendLine().Append("  ");
        }
        builder.Append(';');
    }
}