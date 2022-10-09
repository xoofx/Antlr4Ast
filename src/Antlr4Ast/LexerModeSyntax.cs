// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

/// <summary>
/// Defines the mode of a lexer. This is stored at the end of a grammar file in <see cref="GrammarSyntax.LexerModes"/>.
/// </summary>
public sealed class LexerModeSyntax : SyntaxRuleContainer
{
    /// <summary>
    /// Creates an instance of this object.
    /// </summary>
    /// <param name="name">The name of this mode.</param>
    public LexerModeSyntax(string name)
    {
        Name = name;
        LexerRules = new List<RuleSyntax>();
    }

    /// <summary>
    /// The name of this mode;
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The lexer rules associated to this mode.
    /// </summary>
    public List<RuleSyntax> LexerRules { get;  }

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

    public override IEnumerable<RuleSyntax> GetAllRules()
    {
        return LexerRules;
    }

    protected override void AddRuleImpl(RuleSyntax rule)
    {
        LexerRules.Add(rule);
    }
}