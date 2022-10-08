// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

namespace Antlr4Ast;

public class CommentSyntax
{
    public CommentSyntax(string text, CommentKind kind)
    {
        Text = text;
        Kind = kind;
    }

    public string Text { get; set; }
    public CommentKind Kind { get; set; }
}