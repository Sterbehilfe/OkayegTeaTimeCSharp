﻿using System;
using System.Diagnostics.CodeAnalysis;
using HLE.Strings;
using HLE.Twitch.Models;
using OkayegTeaTime.Twitch.Attributes;
using OkayegTeaTime.Twitch.Models;

#pragma warning disable IDE0052

namespace OkayegTeaTime.Twitch.Commands;

[HandledCommand(CommandType.Chatterino7)]
[SuppressMessage("ReSharper", "NotAccessedField.Local")]
public readonly ref struct Chatterino7Command
{
    public ChatMessage ChatMessage { get; }

    private readonly TwitchBot _twitchBot;
    private readonly ref PoolBufferStringBuilder _response;
    private readonly ReadOnlySpan<char> _prefix;
    private readonly ReadOnlySpan<char> _alias;

    private const string _responseMessage = "Website: 7tv.app || Releases: github.com/SevenTV/chatterino7/releases";

    public Chatterino7Command(TwitchBot twitchBot, ChatMessage chatMessage, ref PoolBufferStringBuilder response, ReadOnlySpan<char> prefix, ReadOnlySpan<char> alias)
    {
        ChatMessage = chatMessage;
        _response = ref response;
        _twitchBot = twitchBot;
        _prefix = prefix;
        _alias = alias;
    }

    public void Handle()
    {
        _response.Append(ChatMessage.Username, ", ", _responseMessage);
    }
}
