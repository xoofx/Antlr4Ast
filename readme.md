# Antlr4Ast [![ci](https://github.com/xoofx/Antlr4Ast/actions/workflows/ci.yml/badge.svg)](https://github.com/xoofx/Antlr4Ast/actions/workflows/ci.yml) [![Coverage Status](https://coveralls.io/repos/github/xoofx/Antlr4Ast/badge.svg?branch=main)](https://coveralls.io/github/xoofx/Antlr4Ast?branch=main) [![NuGet](https://img.shields.io/nuget/v/Antlr4Ast.svg)](https://www.nuget.org/packages/Antlr4Ast/)

<img align="right" width="160px" height="160px" src="img/antlr4ast.png">

Antlr4Ast is a .NET library that provides a parser and abstract syntax tree (AST) for [ANTLR4](https://www.antlr.org/)/g4 files. 

> This can be useful if you are looking for building code generation tools from ANTLR4/g4 files.
  
## Features

- Allow to **parse** and **merge** ANTLR4/g4 files to a simple AST.
  - Should support almost all grammar features of ANTLR4/g4 except actions.
- Provides **precise source location** for most of the AST elements.
- Provides **access to comments** attached to syntax nodes.
- Provides **visitor** and **transform**.
- Library with nullable annotations.
- Compatible with `netstandard2.0`

> Limitations
>
> As this library is mainly for codegen scenarios, it does not preserve whitespaces or the position of some comments within elements.
> There is no plan to support high fidelity roundtrip ANTLR4 parser.

## Usage

The entry point for parsing an ANTLR4/g4 grammar is to use the `Grammar.Parse` methods:

```c#
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
// Print the grammar
Console.WriteLine(
    grammar.ToString(
        new AntlrFormattingOptions() { 
                    MultiLineWithComments = true 
        }
    )
);
```

will print the following:

```antlr
grammar MyGrammar;

// Parser rules starting here!
expr_a_plus_b
  : TOKEN_A '+' TOKEN_B
  ;

// Lexer rules starting here!
TOKEN_A
  : 'a'
  ;

TOKEN_B
  : 'b'
  ;
```

### Documentation

You will find more details about how to use Antlr4Ast in this [user guide](https://github.com/xoofx/Antlr4Ast/blob/main/doc/readme.md).

## License

This software is released under the [BSD-Clause 2 license](https://opensource.org/licenses/BSD-2-Clause). 

## Author

Alexandre Mutel aka [xoofx](http://xoofx.com).
