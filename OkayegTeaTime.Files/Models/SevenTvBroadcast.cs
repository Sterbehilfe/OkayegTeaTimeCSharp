﻿#nullable disable

using System.Text.Json.Serialization;

namespace OkayegTeaTime.Files.Models;

public sealed class SevenTvBroadcast
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("game_name")]
    public string Catagory { get; set; }

    [JsonPropertyName("viewer_count")]
    public int ViewerCount { get; set; }
}
