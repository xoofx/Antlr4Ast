// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

/// <summary>
/// This class defines the import statement.
/// </summary>
public sealed class ImportSyntax : SyntaxNode
{
    /// <summary>
    /// Creates a new instance of this object.
    /// </summary>
    public ImportSyntax()
    {
        Names = new List<ImportNameSyntax>();
    }

    /// <summary>
    /// Gets the list of names.
    /// </summary>
    public List<ImportNameSyntax> Names { get; }

    /// <inheritdoc />
    protected override void ToTextImpl(StringBuilder builder, AntlrFormattingOptions options)
    {
        builder.Append("import ");
        for (var i = 0; i < Names.Count; i++)
        {
            var importName = Names[i];
            if (i > 0) builder.Append(", ");
            importName.ToText(builder, options);
        }
        builder.Append(';');
    }
}