// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

public class ImportSyntax : SyntaxNode
{
    public ImportSyntax()
    {
        Names = new List<ImportNameSyntax>();
    }

    public List<ImportNameSyntax> Names { get; }
    
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