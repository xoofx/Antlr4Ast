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

    protected virtual bool CanOutputComments => true;

    protected virtual void ToTextImplBefore(StringBuilder builder, AntlrFormattingOptions options) {}

    protected abstract void ToTextImpl(StringBuilder builder, AntlrFormattingOptions options);

    protected virtual void ToTextImplAfter(StringBuilder builder, AntlrFormattingOptions options) { }

    public sealed override string ToString()
    {
        return ToString(new AntlrFormattingOptions()
        {
            MultiLineWithComments = false,
        });
    }

    public string ToString(AntlrFormattingOptions options)
    {
        var builder = new StringBuilder();
        ToText(builder, options);
        return builder.ToString();
    }
}