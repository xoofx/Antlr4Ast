// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

public abstract class SyntaxNode
{
    protected SyntaxNode()
    {
        CommentsBefore = new List<CommentSyntax>();
        CommentsAfter = new List<CommentSyntax>();
    }

    public List<CommentSyntax> CommentsBefore { get; }


    public List<CommentSyntax> CommentsAfter { get; }


    public TextSpan Span { get; set; }


    public void ToText(StringBuilder builder, FormattingOptions options)
    {
        if (options.DisplayComment)
        {
            foreach (var comment in CommentsBefore)
            {
                builder.AppendLine(comment.Text);
            }
        }

        ToTextImplBefore(builder, options);
        ToTextImpl(builder, options);
        ToTextImplAfter(builder, options);

        if (options.DisplayComment)
        {
            foreach (var comment in CommentsAfter)
            {
                builder.Append(' ').AppendLine(comment.Text);
            }
        }
    }

    protected virtual void ToTextImplBefore(StringBuilder builder, FormattingOptions options) {}

    protected abstract void ToTextImpl(StringBuilder builder, FormattingOptions options);

    protected virtual void ToTextImplAfter(StringBuilder builder, FormattingOptions options) { }

    public sealed override string ToString()
    {
        return ToString(new FormattingOptions()
        {
            ShouldDisplayRulesAsMultiLine = false,
            DisplayComment = false
        });
    }

    public string ToString(FormattingOptions options)
    {
        var builder = new StringBuilder();
        ToText(builder, options);
        return builder.ToString();
    }
}