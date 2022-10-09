// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

/// <summary>
/// An element used in a lexer/parser rule that defines a dot `.` rule.
/// </summary>
public sealed class DotSyntax : ElementSyntax
{
    /// <inheritdoc />
    protected override void ToTextImpl(StringBuilder builder, AntlrFormattingOptions options)
    {
        builder.Append('.');
    }
}