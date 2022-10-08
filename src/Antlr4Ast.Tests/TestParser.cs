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
        return Verify(grammar.ToString(), GetVerifySettings());
    }
    
    [Test]
    public Task VerifyLexerFile()
    {
        var grammar = Antlr4Parser.Parse(File.ReadAllText(@"ANTLRv4Lexer.g4"), @"ANTLRv4Lexer.g4");
        return Verify(grammar.ToString(), GetVerifySettings());
    }
    
    [Test]
    public Task VerifyLexBasicFile()
    {
        var grammar = Antlr4Parser.Parse(File.ReadAllText(@"LexBasic.g4"), @"LexBasic.g4");
        return Verify(grammar.ToString(), GetVerifySettings());
    }

    private VerifySettings GetVerifySettings()
    {
        var settings = new VerifySettings();
        settings.UseDirectory("Snapshots");
        settings.DisableDiff();
        return settings;
    }
}