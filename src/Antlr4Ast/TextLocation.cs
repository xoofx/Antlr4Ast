// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

namespace Antlr4Ast;

public record struct TextLocation(int Offset, int Line, int Column)
{
    public override string ToString()
    {
        return $"{Line}, {Column + 1}";
    }
}