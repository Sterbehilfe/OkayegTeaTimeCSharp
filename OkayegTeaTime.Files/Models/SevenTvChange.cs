﻿#nullable disable

using System.Text.Json.Serialization;

namespace OkayegTeaTime.Files.Models;

public sealed class SevenTvChange
{
    [JsonPropertyName("key")]
    public string Key { get; set; }

    [JsonPropertyName("values")]
    public string[] Values { get; set; }
}
