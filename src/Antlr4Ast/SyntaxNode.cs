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


    public abstract void ToText(StringBuilder builder);


    public sealed override string ToString()
    {
        var builder = new StringBuilder();
        ToText(builder);
        return builder.ToString();
    }
}