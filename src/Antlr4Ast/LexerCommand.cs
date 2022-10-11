// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

/// <summary>
/// A lexer command stored in a <see cref="LexerCommandList"/>.
/// </summary>
public sealed class LexerCommand : SyntaxNode
{
    /// <summary>
    /// Creates a new instance of this object.
    /// </summary>
    /// <param name="name">The name of the command.</param>
    public LexerCommand(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Gets or sets the name of this command.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the associated expression. Might be null, and in that case only the name if written otherwise the expression is put inside parenthesis.
    /// </summary>
    public object? Expression { get; set; }

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
        builder.Append(Name);
        if (Expression != null)
        {
            builder.Append('(').Append(Expression).Append(')');
        }
    }
}