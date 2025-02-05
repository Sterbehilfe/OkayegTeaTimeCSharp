using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using HLE.Text;
using OkayegTeaTime.Configuration;

namespace OkayegTeaTime.Twitch;

public sealed class MessageRegexCreator
{
    private readonly RegexPool _regexPool = new();

    private static readonly TimeSpan s_timeout = TimeSpan.FromSeconds(1);

    private const string PatternEnding = @"(\s|$)";
    private const RegexOptions Options = RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase;

    [SkipLocalsInit]
    public Regex Create(ReadOnlySpan<char> alias, ReadOnlySpan<char> prefix, [StringSyntax(StringSyntaxAttribute.Regex)] ReadOnlySpan<char> addition = default)
    {
        using ValueStringBuilder patternBuilder = new(stackalloc char[512]);
        patternBuilder.Append('^');

        int escapedItemLength;
        if (prefix.Length == 0)
        {
            patternBuilder.EnsureCapacity(patternBuilder.Length + alias.Length << 1);
            escapedItemLength = StringHelpers.RegexEscape(alias, patternBuilder.FreeBufferSpan);
            patternBuilder.Advance(escapedItemLength);

            patternBuilder.EnsureCapacity(patternBuilder.Length + GlobalSettings.Suffix.Length << 1);
            escapedItemLength = StringHelpers.RegexEscape(GlobalSettings.Suffix, patternBuilder.FreeBufferSpan);
        }
        else
        {
            patternBuilder.EnsureCapacity(patternBuilder.Length + prefix.Length << 1);
            escapedItemLength = StringHelpers.RegexEscape(prefix, patternBuilder.FreeBufferSpan);
            patternBuilder.Advance(escapedItemLength);

            patternBuilder.EnsureCapacity(patternBuilder.Length + alias.Length << 1);
            escapedItemLength = StringHelpers.RegexEscape(alias, patternBuilder.FreeBufferSpan);
        }

        patternBuilder.Advance(escapedItemLength);

        patternBuilder.Append(addition);
        patternBuilder.Append(PatternEnding);

        if (_regexPool.TryGet(patternBuilder.WrittenSpan, Options, s_timeout, out Regex? cachedPattern))
        {
            return cachedPattern;
        }

        Regex compiledRegex = new(patternBuilder.ToString(), Options, s_timeout);
        _regexPool.Add(compiledRegex);
        return compiledRegex;
    }
}
