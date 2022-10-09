// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

namespace Antlr4Ast;

/// <summary>
/// A span of text within a file.
/// </summary>
public struct TextSpan
{
    /// <summary>
    /// Creates a new instance of this object.
    /// </summary>
    /// <param name="filePath">The associated filepath.</param>
    public TextSpan(string filePath) : this()
    {
        FilePath = filePath;
    }

    /// <summary>
    /// Gets or sets the file path associated to this span.
    /// </summary>
    public string FilePath { get; set; }

    /// <summary>
    /// Gets or sets the beginning of the span.
    /// </summary>
    public TextLocation Begin { get; set; }

    /// <summary>
    /// Gets or sets the end of the span.
    /// </summary>
    public TextLocation End { get; set; }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{FilePath}({Begin}, {End})";
    }
}