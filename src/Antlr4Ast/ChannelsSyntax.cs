// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

/// <summary>
/// A class containing the channel ids.
/// </summary>
public sealed class ChannelsSyntax : SyntaxNode
{
    /// <summary>
    /// Creates an instance of this object.
    /// </summary>
    public ChannelsSyntax()
    {
        Ids = new List<string>();
    }

    /// <summary>
    /// Gets the channel ids.
    /// </summary>
    public List<string> Ids { get; }
    
    /// <inheritdoc />
    public override IEnumerable<SyntaxNode> Children()
    {
        yield break;
    }

    /// <inheritdoc />
    public override void Accept(Antlr4Visitor visitor)
    {
        visitor.Visit(this);
    }

    /// <inheritdoc />
    public override TResult? Accept<TResult>(Antlr4Visitor<TResult> transform) where TResult : default
    {
        return transform.Visit(this);
    }
    
    /// <inheritdoc />
    protected override void ToTextImpl(StringBuilder builder, AntlrFormattingOptions options)
    {
        builder.Append("channels { ");
        for (var i = 0; i < Ids.Count; i++)
        {
            var id = Ids[i];
            if (i > 0) builder.Append(", ");
            builder.Append(id);
        }

        builder.Append(" }");
    }
}