﻿using System;
using HLE.Strings;
using OkayegTeaTime.Database.Models;
using OkayegTeaTime.Models.Json;
using OkayegTeaTime.Twitch.Models;
using OkayegTeaTime.Utils;

namespace OkayegTeaTime.Twitch;

public static class BotActions
{
    private const string _yourself = "yourself";
    private const string _reminderFromSpace = "reminder from ";

    public static void SendComingBack(this TwitchBot twitchBot, long userId, string channel)
    {
        User? user = twitchBot.Users[userId];
        if (user is null)
        {
            return;
        }

        SendComingBack(twitchBot, user, channel);
    }

    public static void SendComingBack(this TwitchBot twitchBot, User user, string channel)
    {
        AfkCommand cmd = twitchBot.CommandController[user.AfkType];
        string afkMessage = new AfkMessage(user, cmd).ComingBack;
        twitchBot.Send(channel, afkMessage);
    }

    public static void SendReminder(this TwitchBot twitchBot, string channel, ReadOnlySpan<Reminder> reminders)
    {
        if (reminders.Length == 0)
        {
            return;
        }

        string creator = reminders[0].Creator == reminders[0].Target ? _yourself : reminders[0].Creator;
        TimeSpan span = DateTime.UtcNow - DateTimeOffset.FromUnixTimeMilliseconds(reminders[0].Time);
        Span<char> spanBuffer = stackalloc char[100];
        int spanLength = span.Format(spanBuffer);

        ValueStringBuilder builder = stackalloc char[2048];
        builder.Append(reminders[0].Target, ", ", _reminderFromSpace, creator, " (", spanBuffer[..spanLength], " ago)");
        twitchBot.Reminders.Remove(reminders[0].Id);
        if (reminders[0].Message?.Length > 0)
        {
            builder.Append(": ", reminders[0].Message);
        }

        foreach (Reminder r in reminders[1..])
        {
            twitchBot.Reminders.Remove(r.Id);
            creator = r.Creator == r.Target ? _yourself : r.Creator;
            span = DateTime.UtcNow - DateTimeOffset.FromUnixTimeMilliseconds(r.Time);
            spanLength = span.Format(spanBuffer);
            builder.Append(" || ", creator, " (", spanBuffer[..spanLength], " ago)");
            if (r.Message?.Length > 0)
            {
                builder.Append(": ", r.Message);
            }
        }

        twitchBot.Send(channel, builder.ToString());
    }

    public static void SendTimedReminder(this TwitchBot twitchBot, Reminder reminder)
    {
        string creator = reminder.Target == reminder.Creator ? _yourself : reminder.Creator;
        TimeSpan span = DateTime.UtcNow - DateTimeOffset.FromUnixTimeMilliseconds(reminder.Time);
        Span<char> spanBuffer = stackalloc char[100];
        int spanLength = span.Format(spanBuffer);

        ValueStringBuilder builder = stackalloc char[500];
        builder.Append(reminder.Target, ", ", _reminderFromSpace, creator, " (", spanBuffer[..spanLength], " ago)");
        if (!string.IsNullOrWhiteSpace(reminder.Message))
        {
            builder.Append(": ", reminder.Message);
        }

        twitchBot.Reminders.Remove(reminder.Id);
        twitchBot.Send(reminder.Channel, builder.ToString());
    }
}
