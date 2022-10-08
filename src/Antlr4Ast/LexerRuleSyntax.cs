// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System.Text;

namespace Antlr4Ast;

public class LexerRuleSyntax : RuleSyntax
{
    public LexerRuleSyntax(string name) : base(name)
    {
    }

    public override void ToText(StringBuilder builder)
    {
        throw new NotImplementedException();
    }
}