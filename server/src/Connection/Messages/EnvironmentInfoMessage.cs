using System.Text.Json.Serialization;

namespace GameServer.Connection;

public record EnvironmentInfoMessage : Message
{
    [JsonPropertyName("messageType")]
    public override string MessageType { get; init; } = "ENVIRONMENT_INFO";

    [JsonPropertyName("walls")]
    public List<Wall> Walls { get; init; } = new();

    public record Wall
    {
        [JsonPropertyName("position")]
        public position Position { get; init; } = new();
    }

    [JsonPropertyName("fences")]
    public List<Fence> Fences { get; init; } = new();

    public record Fence
    {
        [JsonPropertyName("position")]
        public position Position { get; init; } = new();

        [JsonPropertyName("health")]
        public int Health { get; init; }
    }

    [JsonPropertyName("bullets")]
    public List<bullet> Bullets { get; init; } = new();

    public record bullet
    {
        [JsonPropertyName("position")]
        public position Position { get; init; } = new();

        [JsonPropertyName("speed")]
        public double Speed { get; init; }

        [JsonPropertyName("damage")]
        public double Damage { get; init; }

        [JsonPropertyName("traveledDistance")]
        public double TraveledDistance { get; init; }
    }

    [JsonPropertyName("playerPositions")]
    public List<playerPositions> PlayerPositions { get; init; } = new();

    public record playerPositions
    {
        [JsonPropertyName("position")]
        public position Position { get; init; } = new();

        [JsonPropertyName("token")]
        public string Token { get; init; } = "";
    }

     public record position
        {
            [JsonPropertyName("x")]
            public double X { get; init; }

            [JsonPropertyName("y")]
            public double Y { get; init; }

            [JsonPropertyName("angle")]
            public double Angle { get; init; }
        }
}
