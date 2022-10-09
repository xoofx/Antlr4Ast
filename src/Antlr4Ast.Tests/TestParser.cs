using System.Text;

namespace Antlr4Ast.Tests;

public class TestParser
{
    [Test]
    public Task TestHelloWorld()
    {
        var input = @"grammar MyGrammar;
// Parser rules starting here!
expr_a_plus_b
    : TOKEN_A '+' TOKEN_B
    ;
// Lexer rules starting here!
TOKEN_A: 'a';
TOKEN_B: 'b';
";
        // Parse the grammar
        var grammar = Antlr4Parser.Parse(input);
        return Verify(grammar.ToString(GetFormattingOptions()), GetVerifySettings());
    }

    [Test]
    public Task TestParserError()
    {
        //           01234567890123456789 0123456789
        var input = "grammar HelloWorld;\nhello +\n";
        var grammar = Antlr4Parser.Parse(input, "/this/file.g4");
        return Verify(grammar.ToString(GetFormattingOptions()), GetVerifySettings());
    }

    [Test]
    public Task TestElementOptions()
    {
        var input = @"grammar TestElementOptions;
rule: <test, test2, test3 = hello, test4 = 'yes'> MY_TOKEN;
rule1: MY_TOKEN <hello1>;
rule2: 'literal' <hello2>;
rule3: rule2 <hello3>;
rule4: . <hello4>;
";
        var grammar = Antlr4Parser.Parse(input);
        return Verify(grammar.ToString(GetFormattingOptions()), GetVerifySettings());
    }

    [Test]
    public Task TestEmptyAlternative()
    {
        var input = @"grammar TestEmptyAlternative;
rule
    : test
    |
    ;
TEST
    : 'a'
    |
    ;
";
        var grammar = Antlr4Parser.Parse(input);
        return Verify(grammar.ToString(GetFormattingOptions()), GetVerifySettings());
    }

    [Test]
    public Task TestNotSet()
    {
        var input = @"grammar TestNotSet;
rule
    : ~ TOKEN
    | ~ 'a' .. 'z'
    ;
";
        var grammar = Antlr4Parser.Parse(input);
        return Verify(grammar.ToString(GetFormattingOptions()), GetVerifySettings());
    }

    [Test]
    public Task TestOptions()
    {
        var input = @"grammar TestOptions;
rule options { preview = 'xyz'; } options { hello = 'abc'; }
    : (options { helloworld = 1; string = 'hello2'; }: 'a')
    ;
TOKEN options { hello2 = hello3; }
    : 'a'
    | 'b'
    ;
";
        var grammar = Antlr4Parser.Parse(input);
        return Verify(grammar.ToString(GetFormattingOptions()), GetVerifySettings());
    }

    [Test]
    public Task TestLexerCommandWithInt()
    {
        var input = @"grammar TestLexerCommandWithInt;
TOKEN
    : 'a' -> hello(1), mode, mode(a)
    ;
";
        var grammar = Antlr4Parser.Parse(input);
        return Verify(grammar.ToString(GetFormattingOptions()), GetVerifySettings());
    }

    [Test]
    public Task TestMultiImports()
    {
        var input = @"grammar TestMultiImports;
import a, b = c, hello = world;
";
        var grammar = Antlr4Parser.Parse(input);
        return Verify(grammar.ToString(GetFormattingOptions()), GetVerifySettings());
    }


    [Test]
    public Task TestLexerLabeled()
    {
        var input = @"grammar TestLexerLabeled;
TOKEN
    : x='a'
    | y+='a' y+='b'
    ;
";
        var grammar = Antlr4Parser.Parse(input);
        return Verify(grammar.ToString(GetFormattingOptions()), GetVerifySettings());
    }

    [Test]
    public Task VerifyLexBasicFileSpan()
    {
        var grammar = Antlr4Parser.Parse(File.ReadAllText(@"LexBasic.g4"), @"LexBasic.g4");
        var builder = new StringBuilder();
        foreach (var rule in grammar.LexerRules)
        {
            builder.AppendLine($"[{rule.Span.Begin.Offset}, {rule.Span.End.Offset}] {rule.Span} -> {rule.Name}");
        }

        var result = builder.ToString();
        return Verify(result, GetVerifySettings());
    }

    [Test]
    public Task VerifyParserFile()
    {
        var grammar = Antlr4Parser.Parse(File.ReadAllText(@"ANTLRv4Parser.g4"), @"ANTLRv4Parser.g4");
        return Verify(grammar.ToString(GetFormattingOptions()), GetVerifySettings());
    }
    
    [Test]
    public Task VerifyLexerFile()
    {
        var grammar = Antlr4Parser.Parse(File.ReadAllText(@"ANTLRv4Lexer.g4"), @"ANTLRv4Lexer.g4");
        return Verify(grammar.ToString(GetFormattingOptions()), GetVerifySettings());
    }
    
    [Test]
    public Task VerifyLexBasicFile()
    {
        var grammar = Antlr4Parser.Parse(File.ReadAllText(@"LexBasic.g4"), @"LexBasic.g4");
        return Verify(grammar.ToString(GetFormattingOptions()), GetVerifySettings());
    }

    [Test]
    public Task VerifyCSharpLexer()
    {
        var grammar = Antlr4Parser.Parse(File.ReadAllText(@"Grammars/CSharpLexer.g4"), @"CSharpLexer.g4");
        return Verify(grammar.ToString(GetFormattingOptions()), GetVerifySettings());
    }

    [Test]
    public Task VerifyCSharpParser()
    {
        var grammar = Antlr4Parser.Parse(File.ReadAllText(@"Grammars/CSharpParser.g4"), @"CSharpParser.g4");
        return Verify(grammar.ToString(GetFormattingOptions()), GetVerifySettings());
    }

    [Test]
    public Task VerifyCSharpPreprocessorParser()
    {
        var grammar = Antlr4Parser.Parse(File.ReadAllText(@"Grammars/CSharpPreprocessorParser.g4"), @"CSharpPreprocessorParser.g4");
        return Verify(grammar.ToString(GetFormattingOptions()), GetVerifySettings());
    }

    [Test]
    public Task VerifyMergeGrammar()
    {
        var parserGrammar = Antlr4Parser.Parse(File.ReadAllText(@"ANTLRv4Parser.g4"), @"ANTLRv4Parser.g4");
        var lexerGrammar = Antlr4Parser.Parse(File.ReadAllText(@"ANTLRv4Lexer.g4"), @"ANTLRv4Lexer.g4");
        var basicLexerGrammar = Antlr4Parser.Parse(File.ReadAllText(@"LexBasic.g4"), @"LexBasic.g4");

        lexerGrammar.MergeFrom(basicLexerGrammar);
        parserGrammar.MergeFrom(lexerGrammar);

        // Our parser grammar is now a full grammar
        parserGrammar.Kind = GrammarKind.Full;

        Assert.True(parserGrammar.TryGetRule("STRING_LITERAL", out _), "Unable to get STRING_LITERAL lexer rule from merged");
        Assert.True(parserGrammar.LexerModes.Any(x => x.Name == "LexerCharSet"), "Unable to find the merged LexerCharSet");
        Assert.True(parserGrammar.TryGetRule("BlockComment", out _), "Unable to get BlockComment lexer rule from merged");

        return Verify(parserGrammar.ToString(GetFormattingOptions()), GetVerifySettings());
    }

    private AntlrFormattingOptions GetFormattingOptions()
    {
        return new AntlrFormattingOptions() { MultiLineWithComments = true };
    }

    private VerifySettings GetVerifySettings()
    {
        var settings = new VerifySettings();
        settings.UseDirectory("Snapshots");
        settings.DisableDiff();
        return settings;
    }
}