// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

namespace Antlr4Ast;

/// <summary>
/// A container of lexer/parser <see cref="Rule"/>.
/// </summary>
public abstract class SyntaxRuleContainer : SyntaxNode
{
    private readonly Dictionary<string, Rule> _mapRules;

    /// <summary>
    /// Creates a new instance of this object.
    /// </summary>
    protected SyntaxRuleContainer()
    {
        _mapRules = new Dictionary<string, Rule>();
    }

    /// <summary>
    /// Updates the map of rules (rule name => rule) so that <see cref="TryGetRule"/>.
    /// </summary>
    public void UpdateRulesMap()
    {
        _mapRules.Clear();
        foreach (var ruleSyntax in GetAllRules())
        {
            TryAddRule(ruleSyntax.Name, ruleSyntax);
        }
    }

    /// <summary>
    /// Merge all the rules from the specified container into this instance.
    /// </summary>
    /// <param name="container">A container of lexer/parser rules.</param>
    public void MergeFrom(SyntaxRuleContainer container)
    {
        foreach (var rule in container.GetAllRules())
        {
            if (_mapRules.ContainsKey(rule.Name)) continue;
            AddRuleImpl(rule);
        }

        MergeFromImpl(container);

        UpdateRulesMap();
    }

    /// <summary>
    /// Allows to merge additional rules from the derived container.
    /// </summary>
    /// <param name="container">The container to merge rules from.</param>
    protected virtual void MergeFromImpl(SyntaxRuleContainer container)
    {
    }

    /// <summary>
    /// Tries to get rules by name.
    /// </summary>
    /// <param name="name">The name of the rule to find.</param>
    /// <param name="syntax">The associated rule if it returns true.</param>
    /// <returns><c>true</c> if the rule with the specified name was found.</returns>
    public bool TryGetRule(string name, [MaybeNullWhen(false)] out Rule syntax)
    {
        return _mapRules.TryGetValue(name, out syntax);
    }

    /// <summary>
    /// Gets all the lexer/parser rules defined by this container.
    /// </summary>
    /// <returns>An enumeration of lexer/parser rules defined by this container.</returns>
    public abstract IEnumerable<Rule> GetAllRules();

    /// <summary>
    /// A method to implement when merging a rule into this container. The rule can be a lexer or parser.
    /// </summary>
    /// <param name="rule">The rule to add to the container.</param>
    protected abstract void AddRuleImpl(Rule rule);

    private void TryAddRule(string name, Rule rule)
    {
        if (!_mapRules.ContainsKey(name))
        {
            _mapRules.Add(name, rule);
        }
    }
}