﻿using System.Collections.Generic;
using System.Linq;
using OkayegTeaTime.Database;
using OkayegTeaTime.Database.Cache;

namespace OkayegTeaTime.Twitch.Controller;

public sealed class LastMessageController
{
    private readonly Dictionary<string, string> _lastMessages;

    public LastMessageController(ChannelCache? channelCache = null)
    {
        _lastMessages = channelCache is null ? DbController.GetChannels().Select(c => c.Name).ToDictionary(c => c, _ => string.Empty) : channelCache.Select(c => c.Name).ToDictionary(c => c, _ => string.Empty);
    }

    public string this[string channel]
    {
        get => Get(channel);
        set => Set(channel, value);
    }

    private void Set(string channel, string message)
    {
        if (_lastMessages.TryAdd(channel, message))
        {
            return;
        }

        _lastMessages[channel] = message;
    }

    private string Get(string channel)
    {
        if (_lastMessages.TryGetValue(channel, out string? message))
        {
            return message;
        }

        _lastMessages.Add(channel, string.Empty);
        return string.Empty;
    }
}
