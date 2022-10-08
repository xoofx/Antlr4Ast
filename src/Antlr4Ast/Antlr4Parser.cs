// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using Antlr4.Runtime;

namespace Antlr4Ast;

public static class Antlr4Parser
{
    public static GrammarSyntax Parse(string input, string fileName = "<input>")
    {
        var grammar = new GrammarSyntax();
        TextWriter? outputWriter = null;
        TextWriter? errorWriter = null;
        
        var streamReader = new StringReader(input);
        var str = new AntlrInputStream(streamReader)
        {
            name = fileName
        };

        var lexer = new ANTLRv4Lexer(str, outputWriter ?? Console.Out, errorWriter ?? Console.Error);
        var tokens = new CommonTokenStream(lexer);
        var parser = new ANTLRv4Parser(tokens);
        var grammarSpec = parser.grammarSpec();

        var visitor = new InternalAntlr4Visitor(tokens, grammar);
        visitor.VisitGrammarSpec(grammarSpec);
    
        return grammar;
    }
}