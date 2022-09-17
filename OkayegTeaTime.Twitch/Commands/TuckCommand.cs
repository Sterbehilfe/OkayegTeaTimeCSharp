﻿using System.Text.RegularExpressions;
using HLE.Emojis;
using OkayegTeaTime.Twitch.Attributes;
using OkayegTeaTime.Twitch.Models;
using OkayegTeaTime.Utils;

namespace OkayegTeaTime.Twitch.Commands;

[HandledCommand(CommandType.Tuck)]
public sealed class TuckCommand : Command
{
    public TuckCommand(TwitchBot twitchBot, TwitchChatMessage chatMessage, string alias) : base(twitchBot, chatMessage, alias)
    {
    }

    public override void Handle()
    {
        Regex pattern = PatternCreator.Create(_alias, _prefix, @"\s\w+(\s\S+)?");
        if (pattern.IsMatch(ChatMessage.Message))
        {
            string target = ChatMessage.LowerSplit[1];
            Response = $"{Emoji.PointRight} {Emoji.Bed} {ChatMessage.Username} tucked {target} to bed";
            string emote = ChatMessage.LowerSplit.Length > 2 ? ChatMessage.Split[2] : string.Empty;
            if (!string.IsNullOrEmpty(emote))
            {
                Response += $" {emote}";
            }
        }
    }
}
