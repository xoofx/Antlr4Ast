// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

namespace Antlr4Ast;

public sealed class FormattingOptions
{
    public FormattingOptions()
    {
        ShouldDisplayRulesAsMultiLine = true;
    }

    public bool ShouldDisplayRulesAsMultiLine { get; set; }

    public bool DisplayComment { get; set; }
}