﻿using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HLE.Collections;
using OkayegTeaTime.Files;
using OkayegTeaTime.Twitch.Attributes;
using OkayegTeaTime.Twitch.Models;
using OkayegTeaTime.Utils;

namespace OkayegTeaTime.Twitch.Commands;

[HandledCommand(CommandType.Fill)]
public sealed class FillCommand : Command
{
    public FillCommand(TwitchBot twitchBot, TwitchChatMessage chatMessage, string alias) : base(twitchBot, chatMessage, alias)
    {
    }

    public override void Handle()
    {
        Regex pattern = PatternCreator.Create(_alias, _prefix, @"\s\S+");
        if (pattern.IsMatch(ChatMessage.Message))
        {
            List<string> messageParts = new();
            string[] split = ChatMessage.Split[1..];
            string emote = _twitchBot.Channels[ChatMessage.ChannelId]?.Emote ?? AppSettings.DefaultEmote;
            int maxLength = AppSettings.MaxMessageLength - (emote.Length + 1);
            while (true)
            {
                string? messagePart = split.Random();
                if (messagePart is null)
                {
                    break;
                }

                int currentMessageLength = messageParts.Sum(m => m.Length) + messageParts.Count + messagePart.Length;
                if (currentMessageLength <= maxLength)
                {
                    messageParts.Add(messagePart);
                }
                else
                {
                    break;
                }
            }

            Response = string.Join(' ', messageParts);
        }
    }
}
