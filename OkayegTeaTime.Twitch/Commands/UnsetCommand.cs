﻿using System.Text.RegularExpressions;
using OkayegTeaTime.Database.Models;
using OkayegTeaTime.Twitch.Attributes;
using OkayegTeaTime.Twitch.Models;
using OkayegTeaTime.Utils;

namespace OkayegTeaTime.Twitch.Commands;

[HandledCommand(CommandType.Unset)]
public readonly unsafe ref struct UnsetCommand
{
    public TwitchChatMessage ChatMessage { get; }

    public Response* Response { get; }

    private readonly TwitchBot _twitchBot;
    private readonly string? _prefix;
    private readonly string _alias;

    public UnsetCommand(TwitchBot twitchBot, TwitchChatMessage chatMessage, Response* response, string? prefix, string alias)
    {
        ChatMessage = chatMessage;
        Response = response;
        _twitchBot = twitchBot;
        _prefix = prefix;
        _alias = alias;
    }

    public void Handle()
    {
        Regex pattern = PatternCreator.Create(_alias, _prefix, @"\sprefix");
        if (pattern.IsMatch(ChatMessage.Message))
        {
            if (ChatMessage.IsModerator || ChatMessage.IsBroadcaster)
            {
                Channel? channel = _twitchBot.Channels[ChatMessage.ChannelId];
                if (channel is null)
                {
                    Response->Append(ChatMessage.Username, PredefinedMessages.CommaSpace, PredefinedMessages.AnErrorOccurredWhileTryingToSetThePrefix);
                    return;
                }

                channel.Prefix = null;
                Response->Append(ChatMessage.Username, PredefinedMessages.CommaSpace, PredefinedMessages.ThePrefixHasBeenUnset);
            }
            else
            {
                Response->Append(ChatMessage.Username, PredefinedMessages.CommaSpace, PredefinedMessages.YouArentAModOrTheBroadcaster);
            }

            return;
        }

        pattern = PatternCreator.Create(_alias, _prefix, @"\sreminder\s\d+");
        if (pattern.IsMatch(ChatMessage.Message))
        {
            Response->Append(ChatMessage.Username, PredefinedMessages.CommaSpace);
            int reminderId = int.Parse(ChatMessage.Split[2]);
            bool removed = _twitchBot.Reminders.Remove(ChatMessage.UserId, ChatMessage.Username, reminderId);
            Response->Append(removed ? PredefinedMessages.TheReminderHasBeenUnset : PredefinedMessages.TheReminderCouldntBeUnset);
            return;
        }

        pattern = PatternCreator.Create(_alias, _prefix, @"\semote");
        if (pattern.IsMatch(ChatMessage.Message))
        {
            Response->Append(ChatMessage.Username, PredefinedMessages.CommaSpace);
            if (ChatMessage.IsModerator || ChatMessage.IsBroadcaster)
            {
                Channel? channel = _twitchBot.Channels[ChatMessage.ChannelId];
                if (channel is null)
                {
                    Response->Append(PredefinedMessages.AnErrorOccurredWhileTryingToSetTheEmote);
                    return;
                }

                channel.Emote = null;
                Response->Append(PredefinedMessages.TheEmoteHasBeenUnset);
            }
            else
            {
                Response->Append(PredefinedMessages.YouArentAModOrTheBroadcaster);
            }

            return;
        }

        pattern = PatternCreator.Create(_alias, _prefix, @"\slocation");
        if (pattern.IsMatch(ChatMessage.Message))
        {
            User? user = _twitchBot.Users[ChatMessage.UserId];
            if (user is null)
            {
                Response->Append(ChatMessage.Username, PredefinedMessages.CommaSpace, PredefinedMessages.YouHaventSetYourLocationYet);
                return;
            }

            user.Location = null;
            Response->Append(ChatMessage.Username, PredefinedMessages.CommaSpace, PredefinedMessages.YourLocationHasBeenUnset);
        }
    }
}
