// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

/// <summary>
/// Defines the list of token ids pre-defined in a <see cref="Grammar.TokenSpecs"/>.
/// </summary>
public sealed class TokenSpecList : SyntaxNode
{
    /// <summary>
    /// Creates a new instance of this object.
    /// </summary>
    public TokenSpecList()
    {
        Ids = new List<string>();
    }

    /// <summary>
    /// Gets the list of token ids.
    /// </summary>
    public List<string> Ids { get; }

    /// <inheritdoc />
    public override IEnumerable<SyntaxNode> Children()
    {
        yield break;
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
        builder.Append("tokens { ");
        for (var i = 0; i < Ids.Count; i++)
        {
            var option = Ids[i];
            if (i > 0) builder.Append(", ");
            builder.Append(option);
        }

        builder.Append(" }");
    }
}