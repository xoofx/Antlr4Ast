// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

namespace Antlr4Ast;

/// <summary>
/// The kind of <see cref="Grammar"/>.
/// </summary>
public enum GrammarKind
{
    /// <summary>
    /// The grammar contains both parser and lexer rules.
    /// </summary>
    Full,

    /// <summary>
    /// The grammar contains only lexer rules.
    /// </summary>
    Lexer,

    /// <summary>
    /// The grammar contains only parser rules.
    /// </summary>
    Parser,
}