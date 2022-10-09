// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

/// <summary>
/// The main entry class representing an ANTLR4/g4 grammar content.
/// </summary>
public sealed class GrammarSyntax : SyntaxNode
{
    /// <summary>
    /// Creates a new instance of this object.
    /// </summary>
    public GrammarSyntax()
    {
        Name = string.Empty;
        ErrorMessages = new List<AntlrErrorMessage>();
        Options = new List<OptionsSyntax>();
        Imports = new List<ImportSyntax>();
        Tokens = new List<TokensSyntax>();
        Channels = new List<ChannelsSyntax>();
        ParserRules = new List<RuleSyntax>();
        LexerRules = new List<RuleSyntax>();
        LexerModes = new List<LexerModeSyntax>();
    }

    /// <summary>
    /// Gets or sets the name of the grammar.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets the list of error messages produced when parsing an ANTLR4/g4 content.
    /// </summary>
    public List<AntlrErrorMessage> ErrorMessages { get; }

    /// <summary>
    /// Gets a boolean indicating if there are error messages.
    /// </summary>
    public bool HasErrors => ErrorMessages.Count > 0;

    /// <summary>
    /// Gets or sets the kind of grammar (full, lexer, parser).
    /// </summary>
    public GrammarKind Kind { get; set; }

    /// <summary>
    /// Get the list of options.
    /// </summary>
    public List<OptionsSyntax> Options { get; }

    /// <summary>
    /// Get the list of imports.
    /// </summary>
    public List<ImportSyntax> Imports { get; }

    /// <summary>
    /// Get the list of tokens.
    /// </summary>
    public List<TokensSyntax> Tokens { get; }

    /// <summary>
    /// The list of channels.
    /// </summary>
    public List<ChannelsSyntax> Channels { get; }

    /// <summary>
    /// The list of parser rules.
    /// </summary>
    public List<RuleSyntax> ParserRules { get; }

    /// <summary>
    /// The list of lexer rules.
    /// </summary>
    public List<RuleSyntax> LexerRules { get; }

    /// <summary>
    /// The list of lexer modes and associated lexer rules.
    /// </summary>
    public List<LexerModeSyntax> LexerModes { get; }

    /// <inheritdoc />
    protected override bool CanOutputComments => !HasErrors;

    /// <inheritdoc />
    protected override void ToTextImpl(StringBuilder builder, AntlrFormattingOptions options)
    {
        // If we have any errors, output them
        if (HasErrors)
        {
            foreach (var antlrErrorMessage in ErrorMessages)
            {
                builder.AppendLine(antlrErrorMessage.ToString());
            }

            return;
        }

        if (Kind != GrammarKind.Full)
        {
            builder.Append(Kind.ToText()).Append(' ');
        }
        builder.AppendLine($"grammar {Name};");

        if (options.MultiLineWithComments)
        {
            builder.AppendLine();
        }

        foreach (var option in Options)
        {
            option.ToText(builder, options);
            builder.AppendLine();
            if (options.MultiLineWithComments)
            {
                builder.AppendLine();
            }
        }

        foreach (var import in Imports)
        {
            import.ToText(builder, options);
            builder.AppendLine();
            if (options.MultiLineWithComments)
            {
                builder.AppendLine();
            }
        }

        foreach (var token in Tokens)
        {
            token.ToText(builder, options);
            builder.AppendLine();
            if (options.MultiLineWithComments)
            {
                builder.AppendLine();
            }
        }

        foreach (var channel in Channels)
        {
            channel.ToText(builder, options);
            builder.AppendLine();
            if (options.MultiLineWithComments)
            {
                builder.AppendLine();
            }
        }
        
        foreach (var parserRule in ParserRules)
        {
            parserRule.ToText(builder, options);
            builder.AppendLine();
            if (options.MultiLineWithComments)
            {
                builder.AppendLine();
            }
        }

        foreach (var lexerRule in LexerRules)
        {
            lexerRule.ToText(builder, options);
            builder.AppendLine();
            if (options.MultiLineWithComments)
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