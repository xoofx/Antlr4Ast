// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

/// <summary>
/// Defines the lexer commands `-&gt;` attached to a lexer alternative <see cref="AlternativeSyntax.LexerCommands"/> (e.g TOKEN: 'a' -&gt; channel(HIDDEN);).
/// </summary>
public sealed class LexerCommandsSyntax : SyntaxNode
{
    /// <summary>
    /// Creates a new instance of this object.
    /// </summary>
    public LexerCommandsSyntax()
    {
        Items = new List<LexerCommandSyntax>();
    }

    /// <summary>
    /// Gets the commands.
    /// </summary>
    public List<LexerCommandSyntax> Items { get; }

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