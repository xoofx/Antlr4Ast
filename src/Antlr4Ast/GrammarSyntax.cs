// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;
using Antlr4.Runtime;

namespace Antlr4Ast;

public enum GrammarKind
{
    Full,
    Lexer,
    Parser,
}

public class ImportSyntax : SyntaxNode
{
    public ImportSyntax()
    {
        Names = new List<ImportNameSyntax>();
    }

    public List<ImportNameSyntax> Names { get; }
    
    public override void ToText(StringBuilder builder)
    {
        if (Names.Count <= 0) return;
        builder.Append("import ");
        for (var i = 0; i < Names.Count; i++)
        {
            var importName = Names[i];
            if (i > 0) builder.Append(", ");
            builder.Append(importName);
        }
        builder.Append(";");
        builder.AppendLine();
    }
}

public class ImportNameSyntax : SyntaxNode
{
    public ImportNameSyntax(string name)
    {
        Name = name;
    }

    public string Name { get; set; }

    public string? Value { get; set; }

    public override void ToText(StringBuilder builder)
    {
        builder.Append(Name);
        if (Value != null) builder.Append(" = ").Append(Name);
    }
}

public class GrammarSyntax : SyntaxNode
{
    public GrammarSyntax()
    {
        Name = string.Empty;
        Options = new List<OptionsSyntax>();
        Imports = new List<ImportSyntax>();
        Tokens = new List<TokensSyntax>();
        Channels = new List<ChannelsSyntax>();
        ParserRules = new List<ParserRuleSyntax>();
        LexerRules = new List<LexerRuleSyntax>();
    }

    public string Name { get; set; }

    public GrammarKind Kind { get; set; }

    public List<OptionsSyntax> Options { get; }

    public List<ImportSyntax> Imports { get; }

    public List<TokensSyntax> Tokens { get; }

    public List<ChannelsSyntax> Channels { get; }

    public List<ParserRuleSyntax> ParserRules { get; }

    public List<LexerRuleSyntax> LexerRules { get; }

    public override void ToText(StringBuilder builder)
    {
        if (Kind != GrammarKind.Full)
        {
            builder.Append(Kind.ToText()).Append(' ');
        }
        builder.AppendLine($"grammar {Name};");

        foreach (var option in Options)
        {
            option.ToText(builder);
            builder.AppendLine();
        }

        foreach (var import in Imports)
        {
            import.ToText(builder);
            builder.AppendLine();
        }

        foreach (var token in Tokens)
        {
            token.ToText(builder);
            builder.AppendLine();
        }

        foreach (var channel in Channels)
        {
            channel.ToText(builder);
            builder.AppendLine();
        }
        
        foreach (var parserRule in ParserRules)
        {
            parserRule.ToText(builder);
            builder.AppendLine();
        }
        
        foreach (var lexerRule in LexerRules)
        {
            lexerRule.ToText(builder);
            builder.AppendLine();
        }
    }
}