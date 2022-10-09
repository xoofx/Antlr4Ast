// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

namespace Antlr4Ast;

public sealed class AntlrErrorMessage : IEquatable<AntlrErrorMessage>
{
    public AntlrErrorMessage(TextSpan span, string message)
    {
        Span = span;
        Message = message;
    }
    public TextSpan Span { get; set; }

    public string Message { get; set; }
    
    public override string ToString()
    {
        return $"{Span}: error: {Message}";
    }

    public bool Equals(AntlrErrorMessage? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Span.Equals(other.Span) && Message == other.Message;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is AntlrErrorMessage other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Span, Message);
    }

    public static bool operator ==(AntlrErrorMessage? left, AntlrErrorMessage? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(AntlrErrorMessage? left, AntlrErrorMessage? right)
    {
        return !Equals(left, right);
    }
}