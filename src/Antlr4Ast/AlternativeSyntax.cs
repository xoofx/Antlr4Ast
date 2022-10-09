// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

/// <summary>
/// An alternative in a <see cref="AlternativeListSyntax"/> composed of <see cref="ElementSyntax"/>.
/// </summary>
public sealed class AlternativeSyntax : SyntaxNode
{
    /// <summary>
    /// Creates a new instance of this object
    /// </summary>
    public AlternativeSyntax()
    {
        Elements = new List<ElementSyntax>();
    }

    /// <summary>
    /// Gets or sets the options associated to this alternative.
    /// </summary>
    public ElementOptionsSyntax? Options { get; set; }

    /// <summary>
    /// Gets the elements.
    /// </summary>
    public List<ElementSyntax> Elements { get; }

    /// <summary>
    /// Gets or sets the associated label (only valid for a parser rule).
    /// </summary>
    public string? ParserLabel { get; set; }

    /// <summary>
    /// Gets or sets the associated commands (only valid for a lexer rule).
    /// </summary>
    public LexerCommandsSyntax? LexerCommands { get; set; }

    /// <inheritdoc />
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