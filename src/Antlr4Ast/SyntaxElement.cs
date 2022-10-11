// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

/// <summary>
/// The base class for an element used in a lexer/parser rule.
/// </summary>
public abstract class SyntaxElement : SyntaxNode
{
    /// <summary>
    /// Gets or sets if this element is negated by a ~ in the grammar.
    /// </summary>
    public bool IsNot { get; set; }

    /// <summary>
    /// Gets or sets the suffix of this element (*, +, ?, *?, +?, ??).
    /// </summary>
    public SuffixKind Suffix { get; set; }

    /// <summary>
    /// Gets or sets the label attached to this element.
    /// </summary>
    public string? Label { get; set; }

    /// <summary>
    /// Gets or sets the kind of label (assign `=` or list `+=`).
    /// </summary>
    public LabelKind LabelKind { get; set; }

    /// <summary>
    /// Gets or sets the options attached to this element.
    /// </summary>
    public ElementOptionList? ElementOptions { get; set; }

    /// <inheritdoc />
    protected override void ToTextImplBefore(StringBuilder builder, AntlrFormattingOptions options)
    {
        if (Label != null) builder.Append(Label).Append(LabelKind.ToText());
        if (IsNot) builder.Append("~ ");
    }

    /// <inheritdoc />
    protected override void ToTextImplAfter(StringBuilder builder, AntlrFormattingOptions options)
    {
        ElementOptions?.ToText(builder, options);
        builder.Append(Suffix.ToText());
    }
}