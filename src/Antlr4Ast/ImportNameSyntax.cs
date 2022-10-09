// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

/// <summary>
/// This class defines the argument to the <see cref="ImportSyntax"/> statement.
/// </summary>
public sealed class ImportNameSyntax : SyntaxNode
{
    /// <summary>
    /// Creates a new instance of this object.
    /// </summary>
    /// <param name="name">The name of the import.</param>
    public ImportNameSyntax(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the associated identifier. May be null.
    /// </summary>
    public string? Value { get; set; }

    /// <inheritdoc />
    protected override void ToTextImpl(StringBuilder builder, AntlrFormattingOptions options)
    {
        builder.Append(Name);
        if (Value != null) builder.Append(" = ").Append(Name);
    }
}