// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using Antlr4.Runtime;

namespace Antlr4Ast;

public static class Antlr4Parser
{
    public static GrammarSyntax Parse(string input, string fileName = "<input>")
    {
        var streamReader = new StringReader(input);
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
        GrammarSyntax grammar;
        if (listener.Messages.Count > 0)
        {
            grammar = new GrammarSyntax();
            grammar.ErrorMessages.AddRange(listener.Messages);
        }
        else
        {
            var visitor = new InternalAntlr4Visitor(tokens);
            grammar = (GrammarSyntax)visitor.VisitGrammarSpec(grammarSpec)!;
        }

        return grammar;
    }
    internal static TextSpan CreateSpan(IToken start, IToken stop)
    {
        return new TextSpan(start.TokenSource.SourceName)
        {
            Begin = new TextLocation(start.StartIndex, start.Line, start.Column),
            End = new TextLocation(stop.StopIndex, stop.Line, stop.Column)
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