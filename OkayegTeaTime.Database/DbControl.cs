﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using HLE.Collections;
using OkayegTeaTime.Database.Cache;
using OkayegTeaTime.Database.Models;

namespace OkayegTeaTime.Database;

public static class DbControl
{
    public static ChannelCache Channels { get; } = new();

    public static ReminderCache Reminders { get; } = new();

    public static SpotifyUserCache SpotifyUsers { get; } = new();

    public static UserCache Users { get; } = new();

    public static void Invalidate()
    {
        Channels.Invalidate();
        Reminders.Invalidate();
        Users.Invalidate();
    }

    public static IEnumerable<string> Invalidate(string cacheName)
    {
        Regex cachePattern = new($@"^{cacheName}$", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
        PropertyInfo[] properties = typeof(DbControl).GetProperties().Where(p => cachePattern.IsMatch(p.Name)).ForEach(p =>
        {
            MethodInfo? method = p.PropertyType.GetMethod(nameof(DbCache<CacheModel>.Invalidate));
            method?.Invoke(p.GetValue(null), null);
        }).ToArray();

        return properties.Select(p => p.Name).ToArray();
    }
}
