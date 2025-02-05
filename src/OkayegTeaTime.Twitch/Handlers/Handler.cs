﻿using System.Threading.Tasks;
using HLE.Twitch.Tmi.Models;

namespace OkayegTeaTime.Twitch.Handlers;

public abstract class Handler(TwitchBot twitchBot)
{
    private protected readonly TwitchBot _twitchBot = twitchBot;

    public abstract ValueTask HandleAsync(ChatMessage chatMessage);
}
