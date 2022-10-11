// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

/// <summary>
/// Defines the lexer commands `-&gt;` attached to a lexer alternative <see cref="Alternative.LexerCommands"/> (e.g TOKEN: 'a' -&gt; channel(HIDDEN);).
/// </summary>
public sealed class LexerCommandList : SyntaxNode
{
    /// <summary>
    /// Creates a new instance of this object.
    /// </summary>
    public LexerCommandList()
    {
        Items = new List<LexerCommand>();
    }

    /// <summary>
    /// Gets the commands.
    /// </summary>
    public List<LexerCommand> Items { get; }
    
    /// <inheritdoc />
    public override IEnumerable<SyntaxNode> Children()
    {
        foreach (var lexerCommandSyntax in Items)
        {
            yield return lexerCommandSyntax;
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
        builder.Append(" -> ");
        for (var i = 0; i < Items.Count; i++)
        {
            var lexerCommand = Items[i];
            if (i > 0) builder.Append(", ");
            lexerCommand.ToText(builder, options);
        }
    }
}