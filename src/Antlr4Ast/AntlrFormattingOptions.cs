// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

namespace Antlr4Ast;

// ReSharper disable once InconsistentNaming
public sealed class AntlrFormattingOptions
{
    public AntlrFormattingOptions()
    {
        MultiLineWithComments = true;
    }

    public bool MultiLineWithComments { get; set; }
}