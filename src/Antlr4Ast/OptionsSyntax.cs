// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

/// <summary>
/// Defines attached options (to a <see cref="GrammarSyntax.Options"/>, <see cref="RuleSyntax.Options"/> or a <see cref="BlockSyntax.Options"/>).
/// </summary>
public sealed class OptionsSyntax : SyntaxNode
{
    /// <summary>
    /// Creates an instance of this object.
    /// </summary>
    public OptionsSyntax()
    {
        Items = new List<OptionSyntax>();
    }

    /// <summary>
    /// Gets the list of options.
    /// </summary>
    public List<OptionSyntax> Items { get; }

    /// <inheritdoc />
    protected override void ToTextImpl(StringBuilder builder, AntlrFormattingOptions options)
    {
        builder.Append("options { ");
        foreach(var option in Items)
        {
            option.ToText(builder, options);
            builder.Append("; ");
        }

        builder.Append("}");
    }
}