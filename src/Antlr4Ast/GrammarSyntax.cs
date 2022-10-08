// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

public class GrammarSyntax : SyntaxNode
{
    public GrammarSyntax()
    {
        Name = string.Empty;
        Options = new List<OptionsSyntax>();
        Imports = new List<ImportSyntax>();
        Tokens = new List<TokensSyntax>();
        Channels = new List<ChannelsSyntax>();
        ParserRules = new List<RuleSyntax>();
        LexerRules = new List<RuleSyntax>();
        LexerModes = new List<LexerModeSyntax>();
    }

    public string Name { get; set; }

    public GrammarKind Kind { get; set; }

    public List<OptionsSyntax> Options { get; }

    public List<ImportSyntax> Imports { get; }

    public List<TokensSyntax> Tokens { get; }

    public List<ChannelsSyntax> Channels { get; }

    public List<RuleSyntax> ParserRules { get; }

    public List<RuleSyntax> LexerRules { get; }

    public List<LexerModeSyntax> LexerModes { get; }

    protected override void ToTextImpl(StringBuilder builder, FormattingOptions options)
    {
        if (Kind != GrammarKind.Full)
        {
            builder.Append(Kind.ToText()).Append(' ');
        }
        builder.AppendLine($"grammar {Name};");

        if (options.ShouldDisplayRulesAsMultiLine)
        {
            builder.AppendLine();
        }

        foreach (var option in Options)
        {
            option.ToText(builder, options);
            builder.AppendLine();
            if (options.ShouldDisplayRulesAsMultiLine)
            {
                builder.AppendLine();
            }
        }

        foreach (var import in Imports)
        {
            import.ToText(builder, options);
            builder.AppendLine();
            if (options.ShouldDisplayRulesAsMultiLine)
            {
                builder.AppendLine();
            }
        }

        foreach (var token in Tokens)
        {
            token.ToText(builder, options);
            builder.AppendLine();
            if (options.ShouldDisplayRulesAsMultiLine)
            {
                builder.AppendLine();
            }
        }

        foreach (var channel in Channels)
        {
            channel.ToText(builder, options);
            builder.AppendLine();
            if (options.ShouldDisplayRulesAsMultiLine)
            {
                builder.AppendLine();
            }
        }
        
        foreach (var parserRule in ParserRules)
        {
            parserRule.ToText(builder, options);
            builder.AppendLine();
            if (options.ShouldDisplayRulesAsMultiLine)
            {
                builder.AppendLine();
            }
        }

        foreach (var lexerRule in LexerRules)
        {
            lexerRule.ToText(builder, options);
            builder.AppendLine();
            if (options.ShouldDisplayRulesAsMultiLine)
            {
                builder.AppendLine();
            }
        }

        foreach (var lexerMode in LexerModes)
        {
            lexerMode.ToText(builder, options);
        }
    }
}