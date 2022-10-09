// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

namespace Antlr4Ast;

/// <summary>
/// A text location.
/// </summary>
/// <param name="Offset">The position/offset within the source text.</param>
/// <param name="Line">The line of the text location (1 based).</param>
/// <param name="Column">The column of the text location (1 based).</param>
public readonly record struct TextLocation(int Offset, int Line, int Column)
{
    /// <inheritdoc />
    public override string ToString()
    {
        return $"{Line}, {Column}";
    }
}