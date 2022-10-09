// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

namespace Antlr4Ast;

public sealed class AntlrErrorMessage
{
    public AntlrErrorMessage(TextSpan span, string message)
    {
        Span = span;
        Message = message;
    }
    public TextSpan Span { get; }

    public string Message { get; }
    
    public override string ToString()
    {
        return $"{Span}: error: {Message}";
    }
}