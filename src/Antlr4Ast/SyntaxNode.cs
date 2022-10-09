// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

/// <summary>
/// The base class for all ANTLR4 AST nodes.
/// </summary>
public abstract class SyntaxNode
{
    /// <summary>
    /// Creates a new instance of this node.
    /// </summary>
    protected SyntaxNode()
    {
        CommentsBefore = new List<CommentSyntax>();
        CommentsAfter = new List<CommentSyntax>();
    }

    /// <summary>
    /// The list of comments attached before this node.
    /// </summary>
    public List<CommentSyntax> CommentsBefore { get; }
    
    /// <summary>
    /// The list of comments attached after this node.
    /// </summary>
    public List<CommentSyntax> CommentsAfter { get; }
    
    /// <summary>
    /// The location of this node in the source text.
    /// </summary>
    public TextSpan Span { get; set; }


    /// <summary>
    /// Converts this node into a textual representation.
    /// </summary>
    /// <param name="builder">The output string builder.</param>
    /// <param name="options">The formatting options.</param>
    public void ToText(StringBuilder builder, AntlrFormattingOptions options)
    {
        if (CanOutputComments)
        {
            if (options.MultiLineWithComments)
            {
                foreach (var comment in CommentsBefore)
                {
                    comment.ToText(builder, options);
                    builder.AppendLine();
                }
            }
        }

        ToTextImplBefore(builder, options);
        ToTextImpl(builder, options);
        ToTextImplAfter(builder, options);

        if (CanOutputComments)
        {
            if (options.MultiLineWithComments)
            {
                foreach (var comment in CommentsAfter)
                {
                    builder.Append(' ');
                    comment.ToText(builder, options);
                    builder.AppendLine();
                }
            }
        }
    }

    /// <summary>
    /// Gets a boolean indicating whether to output comments.
    /// </summary>
    protected virtual bool CanOutputComments => true;

    /// <summary>
    /// A callback called just before calling the main <see cref="ToTextImpl"/>.
    /// </summary>
    /// <param name="builder">The output string builder.</param>
    /// <param name="options">The formatting options.</param>
    protected virtual void ToTextImplBefore(StringBuilder builder, AntlrFormattingOptions options) {}

    /// <summary>
    /// The main callback implementation of <see cref="ToText"/>.
    /// </summary>
    /// <param name="builder">The output string builder.</param>
    /// <param name="options">The formatting options.</param>
    protected abstract void ToTextImpl(StringBuilder builder, AntlrFormattingOptions options);

    /// <summary>
    /// A callback called just after calling the main <see cref="ToTextImpl"/>.
    /// </summary>
    /// <param name="builder">The output string builder.</param>
    /// <param name="options">The formatting options.</param>
    protected virtual void ToTextImplAfter(StringBuilder builder, AntlrFormattingOptions options) { }

    /// <inheritdoc />
    public sealed override string ToString()
    {
        return ToString(new AntlrFormattingOptions()
        {
            MultiLineWithComments = false,
        });
    }

    /// <summary>
    /// Return a string representation of this node with the specified formatting options.
    /// </summary>
    /// <param name="options">The formatting options.</param>
    /// <returns>A string representation of this node.</returns>
    public string ToString(AntlrFormattingOptions options)
    {
        var builder = new StringBuilder();
        ToText(builder, options);
        return builder.ToString();
    }
}