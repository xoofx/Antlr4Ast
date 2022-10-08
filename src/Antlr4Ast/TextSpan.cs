// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

namespace Antlr4Ast;

public struct TextSpan
{
    public TextSpan(string? filePath) : this()
    {
        FilePath = filePath;
    }

    public string? FilePath { get; set; }

    public TextLocation Begin { get; set; }

    public TextLocation End { get; set; }

    public override string ToString()
    {
        return $"{FilePath}({Begin}, {End})";
    }
}