// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using Antlr4.Runtime;
using System.Text;

namespace Antlr4Ast;

/// <summary>
/// The main entry class representing an ANTLR4/g4 grammar content.
/// </summary>
public sealed class Grammar : SyntaxRuleContainer
{
    /// <summary>
    /// Creates a new instance of this object.
    /// </summary>
    public Grammar()
    {
        Name = string.Empty;
        ErrorMessages = new List<AntlrErrorMessage>();
        Options = new List<OptionSpecList>();
        Imports = new List<ImportSpec>();
        TokenSpecs = new List<TokenSpecList>();
        Channels = new List<ChannelList>();
        ParserRules = new List<Rule>();
        LexerRules = new List<Rule>();
        LexerModes = new List<LexerMode>();
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
    public List<OptionSpecList> Options { get; }

    /// <summary>
    /// Get the list of imports.
    /// </summary>
    public List<ImportSpec> Imports { get; }

    /// <summary>
    /// Get the list of tokens.
    /// </summary>
    public List<TokenSpecList> TokenSpecs { get; }

    /// <summary>
    /// The list of channels.
    /// </summary>
    public List<ChannelList> Channels { get; }

    /// <summary>
    /// The list of parser rules.
    /// </summary>
    public List<Rule> ParserRules { get; }

    /// <summary>
    /// The list of lexer rules.
    /// </summary>
    public List<Rule> LexerRules { get; }

    /// <summary>
    /// The list of lexer modes and associated lexer rules.
    /// </summary>
    public List<LexerMode> LexerModes { get; }

    /// <inheritdoc />
    public override IEnumerable<SyntaxNode> Children()
    {
        foreach (var option in Options)
        {
            yield return option;
        }

        foreach (var import in Imports)
        {
            yield return import;
        }

        foreach (var token in TokenSpecs)
        {
            yield return token;
        }

        foreach (var channel in Channels)
        {
            yield return channel;
        }

        foreach (var parserRule in ParserRules)
        {
            yield return parserRule;
        }

        foreach (var lexerRule in LexerRules)
        {
            yield return lexerRule;

        }

        foreach (var lexerMode in LexerModes)
        {
            yield return lexerMode;
        }
    }

    /// <inheritdoc />
    public override void Accept(GrammarVisitor visitor)
    {
        visitor.Visit(this);
    }

    /// <inheritdoc />
    public override TResult? Accept<TResult>(GrammarVisitor<TResult> transform) where TResult : default
    {
        return transform.Visit(this);
    }

    /// <summary>
    /// Parse the specified ANTLR4/g4 file.
    /// </summary>
    /// <param name="filePath">The ANTLR4/g4 file path.</param>
    /// <returns>The parsed grammar.</returns>
    public static Grammar ParseFile(string filePath)
    {
        using var reader = new StreamReader(filePath);
        return Parse(reader, filePath);
    }

    /// <summary>
    /// Parse the specified ANTLR4/g4 stream.
    /// </summary>
    /// <param name="streamReader">A stream reader to the ANTLR4/g4 content.</param>
    /// <param name="fileName">The filename used for reporting errors.</param>
    /// <returns>The parsed grammar.</returns>
    public static Grammar Parse(TextReader streamReader, string fileName = "<input>")
    {
        var str = new AntlrInputStream(streamReader)
        {
            name = fileName
        };

        var lexer = new ANTLRv4Lexer(str, TextWriter.Null, TextWriter.Null);
        var tokens = new CommonTokenStream(lexer);
        var parser = new ANTLRv4Parser(tokens, TextWriter.Null, TextWriter.Null);
        var listener = new ErrorListener();
        parser.AddErrorListener(listener);
        var grammarSpec = parser.grammarSpec();
        Grammar grammar;
        if (listener.Messages.Count > 0)
        {
            grammar = new Grammar();
            grammar.ErrorMessages.AddRange(listener.Messages);
        }
        else
        {
            var visitor = new InternalAntlr4Visitor(tokens);
            grammar = (Grammar)visitor.VisitGrammarSpec(grammarSpec)!;
            // Update the map after loading it.
            grammar.UpdateRulesMap();
        }

        return grammar;
    }

    /// <summary>
    /// Parse the specified ANTLR4/g4 content.
    /// </summary>
    /// <param name="input">A string ANTLR4/g4 content.</param>
    /// <param name="fileName">The filename used for reporting errors.</param>
    /// <returns>The parsed grammar.</returns>
    public static Grammar Parse(string input, string fileName = "<input>")
    {
        return Parse(new StringReader(input), fileName);
    }

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

        foreach (var token in TokenSpecs)
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

    /// <inheritdoc />
    public override IEnumerable<Rule> GetAllRules()
    {
        foreach (var rule in LexerRules) yield return rule;
        foreach (var rule in ParserRules) yield return rule;
    }

    /// <inheritdoc />
    protected override void AddRuleImpl(Rule rule)
    {
        if (rule.IsLexer) LexerRules.Add(rule);
        else ParserRules.Add(rule);
    }

    /// <inheritdoc />
    protected override void MergeFromImpl(SyntaxRuleContainer container)
    {
        var grammar = (Grammar)container;

        // Merge tokens
        foreach (var tokens in grammar.TokenSpecs)
        {
            var newTokens = new TokenSpecList
            {
                Span = tokens.Span
            };
            newTokens.Ids.AddRange(tokens.Ids);
            newTokens.CommentsBefore.AddRange(tokens.CommentsBefore);
            newTokens.CommentsAfter.AddRange(tokens.CommentsAfter);
            TokenSpecs.Add(newTokens);
        }

        // Merge channels
        foreach (var channels in grammar.Channels)
        {
            var newChannels = new ChannelList()
            {
                Span = channels.Span
            };
            newChannels.Ids.AddRange(channels.Ids);
            newChannels.CommentsBefore.AddRange(channels.CommentsBefore);
            newChannels.CommentsAfter.AddRange(channels.CommentsAfter);
            Channels.Add(newChannels);
        }

        // Merge lexer modes
        foreach (var mode in grammar.LexerModes)
        {
            var thisMode = LexerModes.FirstOrDefault(x => x.Name == mode.Name);
            if (thisMode != null)
            {
                thisMode.MergeFrom(mode);
            }
            else
            {
                var lexerMode = new LexerMode(mode.Name)
                {
                    Span = mode.Span
                };
                lexerMode.CommentsBefore.AddRange(mode.CommentsBefore);
                lexerMode.CommentsAfter.AddRange(mode.CommentsAfter);
                lexerMode.LexerRules.AddRange(mode.LexerRules);
                LexerModes.Add(lexerMode);
            }
        }

        // Make sure that all maps for lexer modes are up to date after a merge.
        foreach (var mode in LexerModes)
        {
            mode.UpdateRulesMap();
        }
    }

    internal static TextSpan CreateSpan(IToken start, IToken stop)
    {
        return new TextSpan(start.TokenSource.SourceName)
        {
            Begin = new TextLocation(start.StartIndex, start.Line, start.Column + 1),
            End = new TextLocation(stop.StopIndex, stop.Line, stop.Column + 1)
        };
    }

    private class ErrorListener : BaseErrorListener
    {
        public ErrorListener()
        {
            Messages = new List<AntlrErrorMessage>();
        }

        public List<AntlrErrorMessage> Messages { get; }


        public override void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            Messages.Add(new AntlrErrorMessage(CreateSpan(offendingSymbol, offendingSymbol), msg));
        }
    }

}