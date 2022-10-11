// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

/// <summary>
/// Defines the mode of a lexer. This is stored at the end of a grammar file in <see cref="Grammar.LexerModes"/>.
/// </summary>
public sealed class LexerMode : SyntaxRuleContainer
{
    /// <summary>
    /// Creates an instance of this object.
    /// </summary>
    /// <param name="name">The name of this mode.</param>
    public LexerMode(string name)
    {
        Name = name;
        LexerRules = new List<Rule>();
    }

    /// <summary>
    /// The name of this mode;
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The lexer rules associated to this mode.
    /// </summary>
    public List<Rule> LexerRules { get;  }
    
    /// <inheritdoc />
    public override IEnumerable<SyntaxNode> Children()
    {
        return LexerRules;
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

    /// <inheritdoc />
    protected override void ToTextImpl(StringBuilder builder, AntlrFormattingOptions options)
    {
        builder.Append("mode ").Append(Name).Append(';').AppendLine();
        foreach (var lexerRule in LexerRules)
        {
            lexerRule.ToText(builder, options);
            builder.AppendLine();
        }
    }

    /// <inheritdoc />
    public override IEnumerable<Rule> GetAllRules()
    {
        return LexerRules;
    }

    /// <inheritdoc />
    protected override void AddRuleImpl(Rule rule)
    {
        LexerRules.Add(rule);
    }
}