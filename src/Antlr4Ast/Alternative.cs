// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

/// <summary>
/// An alternative in a <see cref="AlternativeList"/> composed of <see cref="SyntaxElement"/>.
/// </summary>
public sealed class Alternative : SyntaxNode
{
    /// <summary>
    /// Creates a new instance of this object
    /// </summary>
    public Alternative()
    {
        Elements = new List<SyntaxElement>();
    }

    /// <summary>
    /// Gets or sets the options associated to this alternative.
    /// </summary>
    public ElementOptionList? Options { get; set; }

    /// <summary>
    /// Gets the elements.
    /// </summary>
    public List<SyntaxElement> Elements { get; }

    /// <summary>
    /// Gets or sets the associated label (only valid for a parser rule).
    /// </summary>
    public string? ParserLabel { get; set; }

    /// <summary>
    /// Gets or sets the associated commands (only valid for a lexer rule).
    /// </summary>
    public LexerCommandList? LexerCommands { get; set; }

    /// <inheritdoc />
    public override IEnumerable<SyntaxNode> Children()
    {
        if (Options is not null)
        {
            yield return Options;
        }

        foreach (var subNode in Elements)
        {
            yield return subNode;
        }

        if (LexerCommands is not null)
        {
            yield return LexerCommands;
        }
    }

    /// <inheritdoc />
    public override void Accept(GrammarVisitor visitor)
    {
        visitor.Visit(this);
    }

    /// <inheritdoc />
    public override TResult? Accept<TResult>(GrammarVisitor<TResult> transform) where TResult : default
    {
        return transform.Visit(this);
    }

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