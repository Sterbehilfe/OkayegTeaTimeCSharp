﻿using System;
using System.Linq;
using System.Threading.Tasks;
using HLE.Text;
using HLE.Twitch.Tmi.Models;
using OkayegTeaTime.Database;
using OkayegTeaTime.Database.Models;
using OkayegTeaTime.Configuration;
using OkayegTeaTime.Spotify;
using OkayegTeaTime.Twitch.Attributes;
using OkayegTeaTime.Twitch.Models;
using OkayegTeaTime.Utils;

namespace OkayegTeaTime.Twitch.Commands;

[HandledCommand<SpotifyCommand>(CommandType.Spotify)]
public readonly struct SpotifyCommand(TwitchBot twitchBot, ChatMessage chatMessage, ReadOnlyMemory<char> prefix, ReadOnlyMemory<char> alias)
    : IChatCommand<SpotifyCommand>
{
    public PooledStringBuilder Response { get; } = new(GlobalSettings.MaxMessageLength);

    public ChatMessage ChatMessage { get; } = chatMessage;

    private readonly TwitchBot _twitchBot = twitchBot;
    private readonly ReadOnlyMemory<char> _prefix = prefix;
    private readonly ReadOnlyMemory<char> _alias = alias;

    public static void Create(TwitchBot twitchBot, ChatMessage chatMessage, ReadOnlyMemory<char> prefix, ReadOnlyMemory<char> alias, out SpotifyCommand command)
        => command = new(twitchBot, chatMessage, prefix, alias);

    public async ValueTask HandleAsync()
    {
        if (GlobalSettings.Settings.Spotify is null)
        {
            Response.Append($"{ChatMessage.Username}, {Texts.TheCommandHasNotBeenConfiguredByTheBotOwner}");
            return;
        }

        using ChatMessageExtension messageExtension = new(ChatMessage);
        string username = GetUsername(messageExtension);

        bool targetIsSender = ChatMessage.Username.AsSpan().Equals(username, StringComparison.OrdinalIgnoreCase);
        SpotifyUser? user = _twitchBot.SpotifyUsers[username];
        if (user is null)
        {
            if (targetIsSender)
            {
                Response.Append($"{ChatMessage.Username}, can't get your currently playing song, you have to register first");
                return;
            }

            Response.Append($"{ChatMessage.Username}, can't get the currently playing song of {username.Antiping()}, they have to register first");
            return;
        }

        SpotifyItem? item;
        try
        {
            item = await SpotifyController.GetCurrentlyPlayingItemAsync(user);
        }
        catch (SpotifyException ex)
        {
            Response.Append($"{ChatMessage.Username}, {ex.Message}");
            return;
        }
        catch (AggregateException ex)
        {
            Response.Append($"{ChatMessage.Username}, ");
            if (ex.InnerException is null)
            {
                await DbController.LogExceptionAsync(ex);
                Response.Append(Texts.ApiError);
                return;
            }

            Response.Append(ex.InnerException.Message);
            return;
        }

        Response.Append(ChatMessage.Username);
        Response.Append(", ");
        switch (item)
        {
            case null:
                Response.Append(targetIsSender ? "you aren't listening to anything" : $"{username.Antiping()} is not listening to anything");
                return;
            case SpotifyTrack track:
                Response.Append($"{track.Name} by ");

                string[] artists = track.Artists.Select(static a => a.Name).ToArray();
                int joinLength = StringHelpers.Join(", ", artists, Response.FreeBufferSpan);
                Response.Advance(joinLength);

                Response.Append($" || {(track.IsLocal ? "local file" : track.Uri)}");
                return;
            case SpotifyEpisode episode:
                Response.Append($"{episode.Name} by {episode.Show.Name} || {(episode.IsLocal ? "local file" : episode.Uri)}");
                return;
            default:
                Response.Append(Texts.ListeningToAnUnknownSpotifyItemTypeMonkaS);
                return;
        }
    }

    private string GetUsername(ChatMessageExtension messageExtension)
    {
        ReadOnlyMemory<char> firstParameter = messageExtension.LowerSplit[1];
        if (messageExtension.LowerSplit.Length == 1)
        {
            return ChatMessage.Channel;
        }

        ReadOnlySpan<char> firstParameterSpan = firstParameter.Span;
        return firstParameterSpan is "me" ? ChatMessage.Username.ToString() : StringPool.Shared.GetOrAdd(firstParameterSpan);
    }

    public void Dispose() => Response.Dispose();

    public bool Equals(SpotifyCommand other) =>
        _twitchBot.Equals(other._twitchBot) && _prefix.Equals(other._prefix) && _alias.Equals(other._alias) &&
        Response.Equals(other.Response) && ChatMessage.Equals(other.ChatMessage);

    public override bool Equals(object? obj) => obj is SpotifyCommand other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(_twitchBot, _prefix, _alias, Response, ChatMessage);

    public static bool operator ==(SpotifyCommand left, SpotifyCommand right) => left.Equals(right);

    public static bool operator !=(SpotifyCommand left, SpotifyCommand right) => !left.Equals(right);
}
