// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

namespace Antlr4Ast;

/// <summary>
/// The kind of comment.
/// </summary>
public enum CommentKind
{
    /// <summary>
    /// A multiline Javadoc style comment that starts by /** ... */.
    /// </summary>
    Doc,

    /// <summary>
    /// A multiline block style comment that start by /* ... */.
    /// </summary>
    Block,

    /// <summary>
    /// A single line style comment that starts by //.
    /// </summary>
    Line,
}