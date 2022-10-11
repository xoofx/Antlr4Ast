// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

namespace Antlr4Ast;

/// <summary>
/// The kind of label attached to an <see cref="SyntaxElement.Label"/>.
/// </summary>
public enum LabelKind
{
    /// <summary>
    /// An assign label `=`.
    /// </summary>
    Assign,

    /// <summary>
    /// An assign list label `+=`.
    /// </summary>
    PlusAssign,
}