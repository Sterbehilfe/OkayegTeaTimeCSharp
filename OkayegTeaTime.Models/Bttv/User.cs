﻿using System.Text.Json.Serialization;

#nullable disable

namespace OkayegTeaTime.Models.Bttv;

public sealed class User
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("displayName")]
    public string DisplayName { get; set; }

    [JsonPropertyName("providerId")]
    public string UserId { get; set; }
}
