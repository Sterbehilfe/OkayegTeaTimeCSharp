﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HLE.Collections;
using OkayegTeaTime.Twitch.Attributes;
using OkayegTeaTime.Twitch.Models;
using OkayegTeaTime.Utils;

namespace OkayegTeaTime.Twitch.Commands;

[HandledCommand(CommandType.Slots)]
public class SlotsCommand : Command
{
    private const byte _emoteSlotCount = 3;

    public SlotsCommand(TwitchBot twitchBot, TwitchChatMessage chatMessage, string alias)
        : base(twitchBot, chatMessage, alias)
    {
    }

    public override void Handle()
    {
        Regex? emotePattern = null;
        Regex pattern = PatternCreator.Create(_alias, _prefix, @"\s\S+");
        if (pattern.IsMatch(ChatMessage.Message))
        {
            try
            {
                emotePattern = new(ChatMessage.Split[1], RegexOptions.IgnoreCase | RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));
            }
            catch (ArgumentException)
            {
                Response = $"{ChatMessage.Username}, your provided pattern is invalid";
                return;
            }
        }

        IEnumerable<string> ffzEmotes = _twitchBot.EmoteController.GetFfzEmotes(ChatMessage.ChannelId)
            .Concat(_twitchBot.EmoteController.FfzGlobalEmotes).Select(e => e.Name);
        IEnumerable<string> bttvEmotes = _twitchBot.EmoteController.GetBttvEmotes(ChatMessage.ChannelId)
            .Concat(_twitchBot.EmoteController.BttvGlobalEmotes).Select(e => e.Name);
        IEnumerable<string> sevenTvEmotes = _twitchBot.EmoteController.GetSevenTvEmotes(ChatMessage.ChannelId).Select(e => e.Name)
            .Concat(_twitchBot.EmoteController.SevenTvGlobalEmotes.Select(e => e.Name));
        string[] emotes = ffzEmotes.Concat(bttvEmotes).Concat(sevenTvEmotes).ToArray();

        if (!emotes.Any())
        {
            Response = $"{ChatMessage.Username}, there are no third party emotes enabled in this channel";
            return;
        }

        if (emotePattern is not null)
        {
            emotes = emotes.Where(e => emotePattern.IsMatch(e)).ToArray();
        }

        if (!emotes.Any())
        {
            Response = $"{ChatMessage.Username}, there is no emote matching your provided pattern";
            return;
        }

        string[] randomEmotes = new string[_emoteSlotCount];
        for (int i = 0; i < _emoteSlotCount; i++)
        {
            randomEmotes[i] = emotes.Random()!;
        }

        string msgEmotes = string.Join(' ', randomEmotes);
        Response = $"{ChatMessage.Username}, [ {msgEmotes} ] ({emotes.Length} emote{(emotes.Length > 1 ? 's' : string.Empty)})";
    }
}
