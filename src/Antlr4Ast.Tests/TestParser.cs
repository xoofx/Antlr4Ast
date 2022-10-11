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
        var grammar = Grammar.Parse(input);

        //foreach (var rule in grammar.LexerRules)
        //{
        //    Console.WriteLine($"Lexer rule `{rule.Name}`:");
        //    foreach (var alternative in rule.AlternativeList.Items)
        //    {
        //        Console.Write($" ->| ");
        //        foreach (var element in alternative.Elements)
        //        {
        //            Console.WriteLine($"`element => {element}` - {element.GetType().Name}`");
        //        }
        //    }
        //    Console.WriteLine();
        //}

        return Verify(grammar.ToString(GetFormattingOptions()), GetVerifySettings());
    }

    [Test]
    public Task TestParserError()
    {
        //           01234567890123456789 0123456789
        var input = "grammar HelloWorld;\nhello +\n";
        var grammar = Grammar.Parse(input, "/this/file.g4");
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
        var grammar = Grammar.Parse(input);
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
        var grammar = Grammar.Parse(input);
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
        var grammar = Grammar.Parse(input);
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
        var grammar = Grammar.Parse(input);
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
        var grammar = Grammar.Parse(input);
        return Verify(grammar.ToString(GetFormattingOptions()), GetVerifySettings());
    }

    [Test]
    public Task TestMultiImports()
    {
        var input = @"grammar TestMultiImports;
import a, b = c, hello = world;
";
        var grammar = Grammar.Parse(input);
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
        var grammar = Grammar.Parse(input);
        return Verify(grammar.ToString(GetFormattingOptions()), GetVerifySettings());
    }

    [Test]
    public Task VerifyLexBasicFileSpan()
    {
        var grammar = Grammar.Parse(File.ReadAllText(@"LexBasic.g4"), @"LexBasic.g4");
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
        var grammar = Grammar.Parse(File.ReadAllText(@"ANTLRv4Parser.g4"), @"ANTLRv4Parser.g4");
        return Verify(grammar.ToString(GetFormattingOptions()), GetVerifySettings());
    }
    
    [Test]
    public Task VerifyLexerFile()
    {
        var grammar = Grammar.Parse(File.ReadAllText(@"ANTLRv4Lexer.g4"), @"ANTLRv4Lexer.g4");
        return Verify(grammar.ToString(GetFormattingOptions()), GetVerifySettings());
    }
    
    [Test]
    public Task VerifyLexBasicFile()
    {
        var grammar = Grammar.Parse(File.ReadAllText(@"LexBasic.g4"), @"LexBasic.g4");
        return Verify(grammar.ToString(GetFormattingOptions()), GetVerifySettings());
    }

    [Test]
    public Task VerifyCSharpLexer()
    {
        var grammar = Grammar.Parse(File.ReadAllText(@"Grammars/CSharpLexer.g4"), @"CSharpLexer.g4");
        return Verify(grammar.ToString(GetFormattingOptions()), GetVerifySettings());
    }

    [Test]
    public Task VerifyCSharpParser()
    {
        var grammar = Grammar.Parse(File.ReadAllText(@"Grammars/CSharpParser.g4"), @"CSharpParser.g4");
        return Verify(grammar.ToString(GetFormattingOptions()), GetVerifySettings());
    }

    [Test]
    public Task VerifyCSharpPreprocessorParser()
    {
        var grammar = Grammar.Parse(File.ReadAllText(@"Grammars/CSharpPreprocessorParser.g4"), @"CSharpPreprocessorParser.g4");
        return Verify(grammar.ToString(GetFormattingOptions()), GetVerifySettings());
    }

    [Test]
    public Task VerifyMergeGrammar()
    {
        var parserGrammar = GetMergedGrammar();

        Assert.True(parserGrammar.TryGetRule("STRING_LITERAL", out _), "Unable to get STRING_LITERAL lexer rule from merged");
        Assert.True(parserGrammar.LexerModes.Any(x => x.Name == "LexerCharSet"), "Unable to find the merged LexerCharSet");
        Assert.True(parserGrammar.TryGetRule("BlockComment", out _), "Unable to get BlockComment lexer rule from merged");

        return Verify(parserGrammar.ToString(GetFormattingOptions()), GetVerifySettings());
    }
    
    [Test]
    public Task VerifyVisitor()
    {
        var parserGrammar = GetMergedGrammar();

        var visitor = new TestVisitor();
        visitor.Visit(parserGrammar);
        var result = visitor.Builder.ToString();
        return Verify(result, GetVerifySettings());
    }
    
    [Test]
    public Task VerifyTransform()
    {
        var parserGrammar = GetMergedGrammar();

        var visitor = new TestTransform();
        var pseudoNode = visitor.Visit(parserGrammar)!;
        var builder = new StringBuilder();
        pseudoNode.ToText(builder);
        var result = builder.ToString();
        return Verify(result, GetVerifySettings());
    }

    [Test]
    public Task VerifyVisitorMixed()
    {
        var parserGrammar = GetMixedGrammar();

        var visitor = new TestVisitor();
        visitor.Visit(parserGrammar);
        var result = visitor.Builder.ToString();
        return Verify(result, GetVerifySettings());
    }

    [Test]
    public Task VerifyTransformMixed()
    {
        var parserGrammar = GetMixedGrammar();

        var visitor = new TestTransform();
        var pseudoNode = visitor.Visit(parserGrammar)!;
        var builder = new StringBuilder();
        pseudoNode.ToText(builder);
        var result = builder.ToString();
        return Verify(result, GetVerifySettings());
    }

    private Grammar GetMixedGrammar()
    {
        var input = @"grammar MixedGrammar;
import a, b = c, hello = world;
rule: <test, test2, test3 = hello, test4 = 'yes'> MY_TOKEN;
rule1: MY_TOKEN <hello1>;
rule2: 'literal' <hello2>;
rule3: rule2 <hello3>;
rule4: . <hello4>;

rule5
    : test
    |
    ;
TEST
    : 'a'
    |
    ;

rule6
    : ~ TOKEN
    | ~ 'a' .. 'z'
    | ~ ('c' | 'e')
    ;

rule7 options { preview = 'xyz'; } options { hello = 'abc'; }
    : (options { helloworld = 1; string = 'hello2'; }: 'a')
    ;
TOKEN options { hello2 = hello3; }
    : 'a'
    | 'b'
    ;

TOKEN2
    : 'a' -> hello(1), mode, mode(a)
    ;

TOKEN3
    : x='a'
    | y+='a' y+='b'
    ;
";
        var grammar = Grammar.Parse(input);
        return grammar;
    }

    private Grammar GetMergedGrammar()
    {
        var parserGrammar = Grammar.Parse(File.ReadAllText(@"ANTLRv4Parser.g4"), @"ANTLRv4Parser.g4");
        var lexerGrammar = Grammar.Parse(File.ReadAllText(@"ANTLRv4Lexer.g4"), @"ANTLRv4Lexer.g4");
        var basicLexerGrammar = Grammar.Parse(File.ReadAllText(@"LexBasic.g4"), @"LexBasic.g4");

        lexerGrammar.MergeFrom(basicLexerGrammar);
        parserGrammar.MergeFrom(lexerGrammar);

        // Our parser grammar is now a full grammar
        parserGrammar.Kind = GrammarKind.Full;
        return parserGrammar;
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

    private class TestVisitor : GrammarVisitor
    {
        public TestVisitor()
        {
            Builder = new StringBuilder();
        }

        public StringBuilder Builder { get; }

        public override void DefaultVisit(SyntaxNode node)
        {
            Builder.AppendLine($"{node.GetType().Name} - {node.Span}");
            base.DefaultVisit(node);
        }
    }

    private class TestTransform : GrammarVisitor<PseudoNode>
    {
        public TestTransform()
        {
        }

        public override PseudoNode? DefaultVisit(SyntaxNode node)
        {
            base.DefaultVisit(node);
            return new PseudoNode(node, node.Children().Select(x => x.Accept(this)!).ToList());
        }
    }

    private record PseudoNode(SyntaxNode Value, List<PseudoNode> Children)
    {
        public void ToText(StringBuilder builder)
        {
            builder.AppendLine($"{Value.GetType().Name} - {Value.Span}");
            foreach (var child in Children)
            {
                child.ToText(builder);
            }
        }
    }
}