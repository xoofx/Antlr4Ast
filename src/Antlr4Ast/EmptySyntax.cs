// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

public sealed class EmptySyntax : ElementSyntax
{
    protected override void ToTextImpl(StringBuilder builder, FormattingOptions options)
    {
    }
}