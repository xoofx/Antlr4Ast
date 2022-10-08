// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

namespace Antlr4Ast;

public abstract class ElementSyntax : SyntaxNode
{
    public SuffixKind Suffix { get; set; }

    public string? Label { get; set; }
}