﻿using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HLE.Text;
using HLE.Twitch.Tmi.Models;
using OkayegTeaTime.Configuration;
using OkayegTeaTime.Twitch.Attributes;
using OkayegTeaTime.Twitch.Models;
using StringHelper = OkayegTeaTime.Utils.StringHelper;

namespace OkayegTeaTime.Twitch.Commands;

[HandledCommand<LeaveCommand>(CommandType.Leave)]
public readonly struct LeaveCommand(TwitchBot twitchBot, ChatMessage chatMessage, ReadOnlyMemory<char> prefix, ReadOnlyMemory<char> alias)
    : IChatCommand<LeaveCommand>
{
    public PooledStringBuilder Response { get; } = new(GlobalSettings.MaxMessageLength);

    public ChatMessage ChatMessage { get; } = chatMessage;

    private readonly TwitchBot _twitchBot = twitchBot;
    private readonly ReadOnlyMemory<char> _prefix = prefix;
    private readonly ReadOnlyMemory<char> _alias = alias;

    public static void Create(TwitchBot twitchBot, ChatMessage chatMessage, ReadOnlyMemory<char> prefix, ReadOnlyMemory<char> alias, out LeaveCommand command)
        => command = new(twitchBot, chatMessage, prefix, alias);

    public async ValueTask HandleAsync()
    {
        Regex pattern = _twitchBot.MessageRegexCreator.Create(_alias.Span, _prefix.Span, @"\s#?\w{3,25}");
        if (pattern.IsMatch(ChatMessage.Message.AsSpan()))
        {
            using ChatMessageExtension messageExtension = new(ChatMessage);
            if (!messageExtension.IsBotModerator)
            {
                Response.Append($"{ChatMessage.Username}, {Texts.YouArentAModeratorOfTheBot}");
                return;
            }

            string channel = StringPool.Shared.GetOrAdd(messageExtension.LowerSplit[1].Span);
            bool isValidChannel = StringHelper.FormatChannel(ref channel);
            if (!isValidChannel)
            {
                Response.Append($"{ChatMessage.Username}, {Texts.GivenChannelIsInvalid}");
                return;
            }

            bool isJoined = _twitchBot.Channels[channel] is not null;
            if (!isJoined)
            {
                Response.Append($"{ChatMessage.Username}, the bot is not connected to #{channel}");
                return;
            }

            bool success = await _twitchBot.LeaveChannelAsync(channel);
            Response.Append($"{ChatMessage.Username}, {(success ? "successfully left" : "failed to leave")} #{channel}");
        }
    }

    public void Dispose() => Response.Dispose();

    public bool Equals(LeaveCommand other) =>
        _twitchBot.Equals(other._twitchBot) && _prefix.Equals(other._prefix) && _alias.Equals(other._alias) &&
        Response.Equals(other.Response) && ChatMessage.Equals(other.ChatMessage);

    public override bool Equals(object? obj) => obj is LeaveCommand other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(_twitchBot, _prefix, _alias, Response, ChatMessage);

    public static bool operator ==(LeaveCommand left, LeaveCommand right) => left.Equals(right);

    public static bool operator !=(LeaveCommand left, LeaveCommand right) => !left.Equals(right);
}
