// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

/// <summary>
/// Represents a comment in an ANTLR4/g4 content.
/// </summary>
public sealed class CommentSyntax
{
    /// <summary>
    /// Creates an instance of this object.
    /// </summary>
    /// <param name="text">The text of the comment (without the leading comment kind //, /*, /**).</param>
    /// <param name="kind">The kind of comment.</param>
    public CommentSyntax(string text, CommentKind kind)
    {
        Text = text;
        Kind = kind;
    }

    /// <summary>
    /// Gets or sets the text of the comment (without the leading comment kind //, /*, /**).
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// Gets or sets the kind of comment.
    /// </summary>
    public CommentKind Kind { get; set; }

    /// <summary>
    /// Converts this comment to text to the specified <see cref="StringBuilder"/>.
    /// </summary>
    /// <param name="builder">The string builder to append the comment to.</param>
    /// <param name="options">The formatting options.</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void ToText(StringBuilder builder, AntlrFormattingOptions options)
    {
        switch (Kind)
        {
            case CommentKind.Doc:
                builder.Append("/**");
                builder.Append(Text);
                builder.Append("*/");
                break;
            case CommentKind.Block:
                builder.Append("/*");
                builder.Append(Text);
                builder.Append("*/");
                break;
            case CommentKind.Line:
                builder.Append("//");
                builder.Append(Text);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    /// <inheritdoc />
    public override string ToString()
    {
        var builder = new StringBuilder();
        ToText(builder, new AntlrFormattingOptions());
        return builder.ToString();
    }
}