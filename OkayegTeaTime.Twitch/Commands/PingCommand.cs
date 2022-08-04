﻿using HLE.Time;
using OkayegTeaTime.Twitch.Attributes;
using OkayegTeaTime.Twitch.Models;

namespace OkayegTeaTime.Twitch.Commands;

[HandledCommand(CommandType.Ping)]
public class PingCommand : Command
{
    public PingCommand(TwitchBot twitchBot, TwitchChatMessage chatMessage, string alias)
        : base(twitchBot, chatMessage, alias)
    {
    }

    public override void Handle()
    {
        Response = $"Pongeg, I'm here! {_twitchBot.SystemInfo} || Ping: {GetPing()}ms";
    }

    private long GetPing()
    {
        return TimeHelper.Now() - ChatMessage.TmiSentTs;
    }
}
