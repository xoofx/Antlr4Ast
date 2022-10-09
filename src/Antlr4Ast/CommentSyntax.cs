// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

public sealed class CommentSyntax
{
    public CommentSyntax(string text, CommentKind kind)
    {
        Text = text;
        Kind = kind;
    }

    public string Text { get; set; }

    public CommentKind Kind { get; set; }

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

    public override string ToString()
    {
        var builder = new StringBuilder();
        ToText(builder, new AntlrFormattingOptions());
        return builder.ToString();
    }
}