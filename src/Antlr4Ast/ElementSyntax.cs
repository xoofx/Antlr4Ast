// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

public abstract class ElementSyntax : SyntaxNode
{
    public bool IsNot { get; set; }

    public SuffixKind Suffix { get; set; }

    public string? Label { get; set; }

    public LabelKind LabelKind { get; set; }

    public ElementOptionsSyntax? ElementOptions { get; set; }

    protected override void ToTextImplBefore(StringBuilder builder, AntlrFormattingOptions options)
    {
        if (Label != null) builder.Append(Label).Append(LabelKind.ToText());
        if (IsNot) builder.Append("~ ");
    }
    
    protected override void ToTextImplAfter(StringBuilder builder, AntlrFormattingOptions options)
    {
        ElementOptions?.ToText(builder, options);
        builder.Append(Suffix.ToText());
    }
}