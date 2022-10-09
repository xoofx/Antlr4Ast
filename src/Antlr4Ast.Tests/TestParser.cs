namespace Antlr4Ast.Tests;

public class TestParser
{

    [Test]
    public void TestBasic()
    {
        var grammar = Antlr4Parser.Parse(@"
parser grammar HelloParser;

options { tokenVocab = HelloLexer; }

module
    : TEST EOF
    ;
");



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