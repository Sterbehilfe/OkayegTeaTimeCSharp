using System;
using OkayegTeaTime.Twitch.Models;

namespace OkayegTeaTime.Twitch.Commands;

public sealed unsafe class CommandExecutor
{
    private readonly delegate*<TwitchBot, TwitchChatMessage, Response*, string, string, void>[] _executionMethods =
    {
        &ExecuteBanFromFileCommand,
        &ExecuteChatterino7Command,
        &ExecuteChatterinoCommand,
        &ExecuteChattersCommand,
        &ExecuteCheckCommand,
        &ExecuteCodeCommand,
        &ExecuteCoinFlipCommand,
        &ExecuteCSharpCommand,
        &ExecuteFillCommand,
        &ExecuteFormula1Command,
        &ExecuteFuckCommand,
        &ExecuteGachiCommand,
        &ExecuteGuidCommand,
        &ExecuteHelpCommand,
        &ExecuteIdCommand,
        &ExecuteInvalidateCommand,
        &ExecuteJoinCommand,
        &ExecuteKotlinCommand,
        &ExecuteLeaveCommand,
        &ExecuteListenCommand,
        &ExecuteMasspingCommand,
        &ExecuteMathCommand,
        &ExecutePickCommand,
        &ExecutePingCommand,
        &ExecuteRafkCommand,
        &ExecuteRedditCommand,
        &ExecuteRemindCommand,
        &ExecuteSetCommand,
        &ExecuteSkipCommand,
        &ExecuteSlotsCommand,
        &ExecuteSongRequestCommand,
        &ExecuteSpotifyCommand,
        &ExecuteStreamCommand,
        &ExecuteSuggestCommand,
        &ExecuteTuckCommand,
        &ExecuteUnsetCommand,
        &ExecuteVanishCommand,
        &ExecuteWeatherCommand
    };

    private const ushort _responseBufferSize = 2048;

    public void Execute(CommandType type, TwitchBot twitchBot, TwitchChatMessage chatMessage, string? prefix, string alias)
    {
        delegate*<TwitchBot, TwitchChatMessage, Response*, string, string, void> executionMethod = _executionMethods[(int)type];
        Span<char> responseBuffer = stackalloc char[_responseBufferSize];
        Response response = new(responseBuffer);
        executionMethod(twitchBot, chatMessage, &response, prefix!, alias);
        string responseMessage = response.ToString();
        if (string.IsNullOrWhiteSpace(responseMessage))
        {
            return;
        }

        twitchBot.Send(chatMessage.Channel, responseMessage);
    }

    private static void ExecuteBanFromFileCommand(TwitchBot twitchBot, TwitchChatMessage chatMessage, Response* response, string? prefix, string alias)
    {
        BanFromFileCommand command = new(twitchBot, chatMessage, response, prefix, alias);
        command.Handle();
    }

    private static void ExecuteChatterino7Command(TwitchBot twitchBot, TwitchChatMessage chatMessage, Response* response, string? prefix, string alias)
    {
        Chatterino7Command command = new(twitchBot, chatMessage, response, prefix, alias);
        command.Handle();
    }

    private static void ExecuteChatterinoCommand(TwitchBot twitchBot, TwitchChatMessage chatMessage, Response* response, string? prefix, string alias)
    {
        ChatterinoCommand command = new(twitchBot, chatMessage, response, prefix, alias);
        command.Handle();
    }

    private static void ExecuteChattersCommand(TwitchBot twitchBot, TwitchChatMessage chatMessage, Response* response, string? prefix, string alias)
    {
        ChattersCommand command = new(twitchBot, chatMessage, response, prefix, alias);
        command.Handle();
    }

    private static void ExecuteCheckCommand(TwitchBot twitchBot, TwitchChatMessage chatMessage, Response* response, string? prefix, string alias)
    {
        CheckCommand command = new(twitchBot, chatMessage, response, prefix, alias);
        command.Handle();
    }

    private static void ExecuteCodeCommand(TwitchBot twitchBot, TwitchChatMessage chatMessage, Response* response, string? prefix, string alias)
    {
        CodeCommand command = new(twitchBot, chatMessage, response, prefix, alias);
        command.Handle();
    }

    private static void ExecuteCoinFlipCommand(TwitchBot twitchBot, TwitchChatMessage chatMessage, Response* response, string? prefix, string alias)
    {
        CoinflipCommand command = new(twitchBot, chatMessage, response, prefix, alias);
        command.Handle();
    }

    private static void ExecuteCSharpCommand(TwitchBot twitchBot, TwitchChatMessage chatMessage, Response* response, string? prefix, string alias)
    {
        CSharpCommand command = new(twitchBot, chatMessage, response, prefix, alias);
        command.Handle();
    }

    private static void ExecuteFillCommand(TwitchBot twitchBot, TwitchChatMessage chatMessage, Response* response, string? prefix, string alias)
    {
        FillCommand command = new(twitchBot, chatMessage, response, prefix, alias);
        command.Handle();
    }

    private static void ExecuteFormula1Command(TwitchBot twitchBot, TwitchChatMessage chatMessage, Response* response, string? prefix, string alias)
    {
        Formula1Command command = new(twitchBot, chatMessage, response, prefix, alias);
        command.Handle();
    }

    private static void ExecuteFuckCommand(TwitchBot twitchBot, TwitchChatMessage chatMessage, Response* response, string? prefix, string alias)
    {
        FuckCommand command = new(twitchBot, chatMessage, response, prefix, alias);
        command.Handle();
    }

    private static void ExecuteGachiCommand(TwitchBot twitchBot, TwitchChatMessage chatMessage, Response* response, string? prefix, string alias)
    {
        GachiCommand command = new(twitchBot, chatMessage, response, prefix, alias);
        command.Handle();
    }

    private static void ExecuteGuidCommand(TwitchBot twitchBot, TwitchChatMessage chatMessage, Response* response, string? prefix, string alias)
    {
        GuidCommand command = new(twitchBot, chatMessage, response, prefix, alias);
        command.Handle();
    }

    private static void ExecuteHelpCommand(TwitchBot twitchBot, TwitchChatMessage chatMessage, Response* response, string? prefix, string alias)
    {
        HelpCommand command = new(twitchBot, chatMessage, response, prefix, alias);
        command.Handle();
    }

    private static void ExecuteIdCommand(TwitchBot twitchBot, TwitchChatMessage chatMessage, Response* response, string? prefix, string alias)
    {
        IdCommand command = new(twitchBot, chatMessage, response, prefix, alias);
        command.Handle();
    }

    private static void ExecuteInvalidateCommand(TwitchBot twitchBot, TwitchChatMessage chatMessage, Response* response, string? prefix, string alias)
    {
        InvalidateCommand command = new(twitchBot, chatMessage, response, prefix, alias);
        command.Handle();
    }

    private static void ExecuteJoinCommand(TwitchBot twitchBot, TwitchChatMessage chatMessage, Response* response, string? prefix, string alias)
    {
        JoinCommand command = new(twitchBot, chatMessage, response, prefix, alias);
        command.Handle();
    }

    private static void ExecuteKotlinCommand(TwitchBot twitchBot, TwitchChatMessage chatMessage, Response* response, string? prefix, string alias)
    {
        KotlinCommand command = new(twitchBot, chatMessage, response, prefix, alias);
        command.Handle();
    }

    private static void ExecuteLeaveCommand(TwitchBot twitchBot, TwitchChatMessage chatMessage, Response* response, string? prefix, string alias)
    {
        LeaveCommand command = new(twitchBot, chatMessage, response, prefix, alias);
        command.Handle();
    }

    private static void ExecuteListenCommand(TwitchBot twitchBot, TwitchChatMessage chatMessage, Response* response, string? prefix, string alias)
    {
        ListenCommand command = new(twitchBot, chatMessage, response, prefix, alias);
        command.Handle();
    }

    private static void ExecuteMasspingCommand(TwitchBot twitchBot, TwitchChatMessage chatMessage, Response* response, string? prefix, string alias)
    {
        MasspingCommand command = new(twitchBot, chatMessage, response, prefix, alias);
        command.Handle();
    }

    private static void ExecuteMathCommand(TwitchBot twitchBot, TwitchChatMessage chatMessage, Response* response, string? prefix, string alias)
    {
        MathCommand command = new(twitchBot, chatMessage, response, prefix, alias);
        command.Handle();
    }

    private static void ExecutePickCommand(TwitchBot twitchBot, TwitchChatMessage chatMessage, Response* response, string? prefix, string alias)
    {
        PickCommand command = new(twitchBot, chatMessage, response, prefix, alias);
        command.Handle();
    }

    private static void ExecutePingCommand(TwitchBot twitchBot, TwitchChatMessage chatMessage, Response* response, string? prefix, string alias)
    {
        PingCommand command = new(twitchBot, chatMessage, response, prefix, alias);
        command.Handle();
    }

    private static void ExecuteRafkCommand(TwitchBot twitchBot, TwitchChatMessage chatMessage, Response* response, string? prefix, string alias)
    {
        RafkCommand command = new(twitchBot, chatMessage, response, prefix, alias);
        command.Handle();
    }

    private static void ExecuteRedditCommand(TwitchBot twitchBot, TwitchChatMessage chatMessage, Response* response, string? prefix, string alias)
    {
        RedditCommand command = new(twitchBot, chatMessage, response, prefix, alias);
        command.Handle();
    }

    private static void ExecuteRemindCommand(TwitchBot twitchBot, TwitchChatMessage chatMessage, Response* response, string? prefix, string alias)
    {
        RemindCommand command = new(twitchBot, chatMessage, response, prefix, alias);
        command.Handle();
    }

    private static void ExecuteSetCommand(TwitchBot twitchBot, TwitchChatMessage chatMessage, Response* response, string? prefix, string alias)
    {
        SetCommand command = new(twitchBot, chatMessage, response, prefix, alias);
        command.Handle();
    }

    private static void ExecuteSkipCommand(TwitchBot twitchBot, TwitchChatMessage chatMessage, Response* response, string? prefix, string alias)
    {
        SkipCommand command = new(twitchBot, chatMessage, response, prefix, alias);
        command.Handle();
    }

    private static void ExecuteSlotsCommand(TwitchBot twitchBot, TwitchChatMessage chatMessage, Response* response, string? prefix, string alias)
    {
        SlotsCommand command = new(twitchBot, chatMessage, response, prefix, alias);
        command.Handle();
    }

    private static void ExecuteSongRequestCommand(TwitchBot twitchBot, TwitchChatMessage chatMessage, Response* response, string? prefix, string alias)
    {
        SongRequestCommand command = new(twitchBot, chatMessage, response, prefix, alias);
        command.Handle();
    }

    private static void ExecuteSpotifyCommand(TwitchBot twitchBot, TwitchChatMessage chatMessage, Response* response, string? prefix, string alias)
    {
        SpotifyCommand command = new(twitchBot, chatMessage, response, prefix, alias);
        command.Handle();
    }

    private static void ExecuteStreamCommand(TwitchBot twitchBot, TwitchChatMessage chatMessage, Response* response, string? prefix, string alias)
    {
        StreamCommand command = new(twitchBot, chatMessage, response, prefix, alias);
        command.Handle();
    }

    private static void ExecuteSuggestCommand(TwitchBot twitchBot, TwitchChatMessage chatMessage, Response* response, string? prefix, string alias)
    {
        SuggestCommand command = new(twitchBot, chatMessage, response, prefix, alias);
        command.Handle();
    }

    private static void ExecuteTuckCommand(TwitchBot twitchBot, TwitchChatMessage chatMessage, Response* response, string? prefix, string alias)
    {
        TuckCommand command = new(twitchBot, chatMessage, response, prefix, alias);
        command.Handle();
    }

    private static void ExecuteUnsetCommand(TwitchBot twitchBot, TwitchChatMessage chatMessage, Response* response, string? prefix, string alias)
    {
        UnsetCommand command = new(twitchBot, chatMessage, response, prefix, alias);
        command.Handle();
    }

    private static void ExecuteVanishCommand(TwitchBot twitchBot, TwitchChatMessage chatMessage, Response* response, string? prefix, string alias)
    {
        VanishCommand command = new(twitchBot, chatMessage, response, prefix, alias);
        command.Handle();
    }

    private static void ExecuteWeatherCommand(TwitchBot twitchBot, TwitchChatMessage chatMessage, Response* response, string? prefix, string alias)
    {
        WeatherCommand command = new(twitchBot, chatMessage, response, prefix, alias);
        command.Handle();
    }
}
