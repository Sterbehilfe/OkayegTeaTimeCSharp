﻿using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using OkayegTeaTime.Database.Models;
using OkayegTeaTime.Files;
using OkayegTeaTime.Spotify;
using OkayegTeaTime.Twitch.Attributes;
using OkayegTeaTime.Twitch.Models;
using OkayegTeaTime.Utils;

namespace OkayegTeaTime.Twitch.Commands;

[HandledCommand(CommandType.Listen)]
public class ListenCommand : Command
{
    public ListenCommand(TwitchBot twitchBot, TwitchChatMessage chatMessage, string alias) : base(twitchBot, chatMessage, alias)
    {
    }

    public override void Handle()
    {
        if (!AppSettings.UserLists.SecretUsers.Contains(ChatMessage.UserId))
        {
            Response = $"{ChatMessage.Username}, this command is still being tested, you aren't allowed to use this command";
            return;
        }

        Regex pattern = PatternCreator.Create(_alias, _prefix, @"\s((leave)|(stop))");
        if (pattern.IsMatch(ChatMessage.Message))
        {
            SpotifyUser? user = _twitchBot.SpotifyUsers[ChatMessage.Username];
            if (user is null)
            {
                Response = $"{ChatMessage.Username}, you aren't registered, you have to register first";
                return;
            }

            SpotifyUser? target = _twitchBot.SpotifyUsers.GetListeningTo(user);
            if (target is null)
            {
                Response = $"{ChatMessage.Username}, you aren't listening along with anybody";
                return;
            }

            target.ListeningUsers.Remove(user);
            Response = $"{ChatMessage.Username}, stopped listening along with {target.Username.Antiping()}";
            return;
        }

        pattern = PatternCreator.Create(_alias, _prefix, @"\ssync");
        if (pattern.IsMatch(ChatMessage.Message))
        {
            Task.Run(async () =>
            {
                SpotifyUser? user = _twitchBot.SpotifyUsers[ChatMessage.Username];
                if (user is null)
                {
                    Response = $"{ChatMessage.Username}, you can't sync, you have to register first";
                    return;
                }

                SpotifyUser? target = _twitchBot.SpotifyUsers.GetListeningTo(user);
                if (target is null)
                {
                    Response = $"{ChatMessage.Username}, you can't sync, because you aren't listening along with anybody";
                    return;
                }

                SpotifyItem item;
                try
                {
                    item = await user.ListenAlongWith(target);
                }
                catch (SpotifyException ex)
                {
                    Response = $"{ChatMessage.Username}, {ex.Message}";
                    return;
                }

                switch (item)
                {
                    case SpotifyTrack track:
                    {
                        string artists = string.Join(", ", track.Artists.Select(a => a.Name));
                        Response = $"{ChatMessage.Username}, synced with {target.Username.Antiping()} and playing {track.Name} by {artists} || {(track.IsLocal ? "local file" : track.Uri)}";
                        break;
                    }
                    case SpotifyEpisode episode:
                        Response = $"{ChatMessage.Username}, synced with {target.Username.Antiping()} and playing " + $"{episode.Name} by {episode.Show.Name} || {(episode.IsLocal ? "local file" : episode.Uri)}";
                        break;
                    default:
                        Response = $"{ChatMessage.Username}, synced with {target.Username.Antiping()} and playing an unknown item type monkaS";
                        break;
                }
            }).Wait();
            return;
        }

        pattern = PatternCreator.Create(_alias, _prefix, @"\s\w+");
        if (pattern.IsMatch(ChatMessage.Message))
        {
            Task.Run(async () =>
            {
                SpotifyUser? user = _twitchBot.SpotifyUsers[ChatMessage.Username];
                if (user is null)
                {
                    Response = $"{ChatMessage.Username}, you can't listen to other users, you have to register first";
                    return;
                }

                SpotifyUser? target = _twitchBot.SpotifyUsers[ChatMessage.LowerSplit[1]];
                if (target is null)
                {
                    Response = $"{ChatMessage.Username}, you can't listen to {ChatMessage.LowerSplit[1]}'s music, they have to register first";
                    return;
                }

                SpotifyItem item;
                try
                {
                    item = await user.ListenAlongWith(target);
                }
                catch (SpotifyException ex)
                {
                    Response = $"{ChatMessage.Username}, {ex.Message}";
                    return;
                }

                switch (item)
                {
                    case SpotifyTrack track:
                    {
                        string artists = string.Join(", ", track.Artists.Select(a => a.Name));
                        Response = $"{ChatMessage.Username}, now listening along with {target.Username.Antiping()} " + $"and playing {track.Name} by {artists} || {(track.IsLocal ? "local file" : track.Uri)}";
                        break;
                    }
                    case SpotifyEpisode episode:
                    {
                        Response = $"{ChatMessage.Username}, now listening along with {target.Username.Antiping()} " + $"and playing {episode.Name} by {episode.Show.Name} || {(episode.IsLocal ? "local file" : episode.Uri)}";
                        break;
                    }
                    default:
                    {
                        Response = $"{ChatMessage.Username}, now listening along with {target.Username.Antiping()} and playing an unknown item type monkaS";
                        break;
                    }
                }
            }).Wait();
        }
    }
}