// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

namespace Antlr4Ast;

/// <summary>
/// A formatting option for <see cref="SyntaxNode.ToString(AntlrFormattingOptions)"/>.
/// </summary>
// ReSharper disable once InconsistentNaming
public sealed class AntlrFormattingOptions
{
    /// <summary>
    /// Creates a new instance of this object.
    /// </summary>
    public AntlrFormattingOptions()
    {
        MultiLineWithComments = true;
    }

    /// <summary>
    /// Gets or sets a boolean indicating whether to output new lines and comments.
    /// </summary>
    public bool MultiLineWithComments { get; set; }
}