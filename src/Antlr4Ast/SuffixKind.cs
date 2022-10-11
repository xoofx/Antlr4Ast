// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

namespace Antlr4Ast;

/// <summary>
/// The suffix of a <see cref="SyntaxElement"/> used in defining an <see cref="Alternative"/> for a lexer/parser rule.
/// </summary>
public enum SuffixKind
{
    /// <summary>
    /// No suffix.
    /// </summary>
    None,

    /// <summary>
    /// The star `*` suffix represents 0 or more elements.
    /// </summary>
    Star,

    /// <summary>
    /// The plus `+` suffix represents 1 or more elements.
    /// </summary>
    Plus,

    /// <summary>
    /// The question `?` suffix represents 0 or 1 element.
    /// </summary>
    Optional,

    /// <summary>
    /// The star `*?` suffix represents 0 or more elements with a greedy matching.
    /// </summary>
    StarGreedy,

    /// <summary>
    /// The plus `+?` suffix represents 1 or more elements with a greedy matching.
    /// </summary>
    PlusGreedy,

    /// <summary>
    /// The question `??` suffix represents 0 or 1 element with a greedy matching.
    /// </summary>
    OptionalGreedy,
}