using System.Text.Json.Serialization;

namespace OkayegTeaTime.Files.Models;

public sealed class OwmFeelsLike
{
    [JsonPropertyName("day")]
    public double Day { get; set; }

    [JsonPropertyName("night")]
    public double Night { get; set; }

    [JsonPropertyName("eve")]
    public double Evening { get; set; }

    [JsonPropertyName("morn")]
    public double Morning { get; set; }
}
