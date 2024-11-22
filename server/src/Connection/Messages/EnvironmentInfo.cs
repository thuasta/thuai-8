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
        public Position position { get; init; } = new();

        public record Position
        {
            [JsonPropertyName("x")]
            public double X { get; init; }

            [JsonPropertyName("y")]
            public double Y { get; init; }

            [JsonPropertyName("angle")]
            public double Angle { get; init; }
        }

    }

    [JsonPropertyName("fences")]
    public List<Fence> Fences { get; init; } = new();

    public record Fence
    {
        [JsonPropertyName("position")]
        public Position position { get; init; } = new();

        public record Position
        {
            [JsonPropertyName("x")]
            public double X { get; init; }

            [JsonPropertyName("y")]
            public double Y { get; init; }

            [JsonPropertyName("angle")]
            public double Angle { get; init; }
        }

        [JsonPropertyName("health")]
        public int Health { get; init; }
    }

    [JsonPropertyName("bullets")]
    public List<Bullet> Bullets { get; init; } = new();

    public record Bullet
    {
        [JsonPropertyName("position")]
        public Position Position { get; init; } = new();

        public record Position
        {
            [JsonPropertyName("x")]
            public double X { get; init; }

            [JsonPropertyName("y")]
            public double Y { get; init; }

            [JsonPropertyName("angle")]
            public double Angle { get; init; }
        }

        [JsonPropertyName("speed")]
        public double Speed { get; init; }

        [JsonPropertyName("damage")]
        public double Damage { get; init; }

        [JsonPropertyName("traveledDistance")]
        public double TraveledDistance { get; init; }
    }

    [JsonPropertyName("playerPositions")]
    public List<PlayerPosition> PlayerPositions { get; init; } = new();

    public record PlayerPosition
    {
        [JsonPropertyName("position")]
        public Position Position { get; init; } = new();

        public record Position
        {
            [JsonPropertyName("x")]
            public double X { get; init; }

            [JsonPropertyName("y")]
            public double Y { get; init; }

            [JsonPropertyName("angle")]
            public double Angle { get; init; }
        }

        [JsonPropertyName("token")]
        public string Token { get; init; } = "";
    }
}
