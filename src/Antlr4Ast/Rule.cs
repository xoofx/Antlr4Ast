// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

/// <summary>
/// Defines a lexer or parser rules (e.g AB_TOKEN: 'a' | 'b'; or ab_expr: AB_TOKEN '+' AB_TOKEN;).
/// </summary>
public sealed class Rule : SyntaxNode
{
    /// <summary>
    /// Creates a new instance of this object.
    /// </summary>
    /// <param name="name">The name of the rule (e.g AB_TOKEN or ab_expr).Lexer rules must start with a capital letter.</param>
    /// <param name="alternativeList"></param>
    public Rule(string name, AlternativeList alternativeList)
    {
        Name = name;
        Options = new List<OptionSpecList>();
        this.AlternativeList = alternativeList;
    }

    /// <summary>
    /// Gets or sets the name of this rule.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets a boolean indicating whether this is a lexer rule, if the name starts with an upper case letter.
    /// </summary>
    public bool IsLexer => Name.Length > 0 && char.IsUpper(Name[0]);

    /// <summary>
    /// Gets the list of associated options.
    /// </summary>
    public List<OptionSpecList> Options { get; }

    /// <summary>
    /// Gets the list of alternatives.
    /// </summary>
    public AlternativeList AlternativeList { get; set; }

    /// <summary>
    /// Gets or sets a boolean indicating that this rule is a lexer fragment.
    /// </summary>
    public bool IsFragment { get; set; }

    /// <inheritdoc />
    public override IEnumerable<SyntaxNode> Children()
    {
        foreach (var optionsSyntax in Options)
        {
            yield return optionsSyntax;
        }

        yield return AlternativeList;
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
        if (IsFragment)
        {
            builder.Append("fragment ");
        }
        builder.Append(Name);
        foreach (var optionsSyntax in Options)
        {
            if (options.MultiLineWithComments)
            {
                builder.AppendLine().Append("  ");
            }
            optionsSyntax.ToText(builder, options);
        }

        if (options.MultiLineWithComments)
        {
            builder.AppendLine().Append("  ");
        }
        builder.Append(": ");

        AlternativeList.ToText(builder, options);

        if (options.MultiLineWithComments)
        {
            builder.AppendLine().Append("  ");
        }
        builder.Append(';');
    }
}