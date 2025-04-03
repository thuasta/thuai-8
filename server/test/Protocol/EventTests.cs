using System.Text.Json;
using Thuai.Server.Protocol.Scheme;
using Xunit;

namespace Thuai.Server.Protocol.Scheme.Tests;

public class EventTests
{
    private readonly BattleUpdateEventConverter _converter = new();
    private readonly JsonSerializerOptions _options = new();
    
    // PlayerUpdateEvent
    [Fact]
    public void PlayerUpdateEvent_ShouldSerializeCorrectly()
    {
        // Arrange
        var original = new PlayerUpdateEvent { Players = new List<Player>() };

        // Act
        var json = JsonSerializer.Serialize(original);
        var result = JsonSerializer.Deserialize<PlayerUpdateEvent>(json);

        // Assert
        Assert.IsType<PlayerUpdateEvent>(result);
        Assert.Empty(result.Players);
    }

    [Fact]
    public void PlayerUpdateEvent_ShouldDeserializeCorrectly()
    {
        // Arrange
        const string json = """
        {
            "eventType": "PLAYER_UPDATE_EVENT",
            "players": []
        }
        """;

        // Act
        var result = JsonSerializer.Deserialize<PlayerUpdateEvent>(json);

        // Assert
        Assert.IsType<PlayerUpdateEvent>(result);
        Assert.Empty(result.Players);
    }

    // BulletsUpdateEvent
    [Fact]
    public void BulletsUpdateEvent_ShouldSerializeCorrectly()
    {
        // Arrange
        var original = new BulletsUpdateEvent { Bullets = new List<Bullet>() };

        // Act
        var json = JsonSerializer.Serialize(original);
        var result = JsonSerializer.Deserialize<BulletsUpdateEvent>(json);

        // Assert
        Assert.IsType<BulletsUpdateEvent>(result);
        Assert.Empty(result.Bullets);
    }

    [Fact]
    public void BulletsUpdateEvent_ShouldDeserializeCorrectly()
    {
        // Arrange
        const string json = """
        {
            "eventType": "BulletsUpdateEvent",
            "bullets": []
        }
        """;

        // Act
        var result = JsonSerializer.Deserialize<BulletsUpdateEvent>(json);

        // Assert
        Assert.IsType<BulletsUpdateEvent>(result);
        Assert.Empty(result.Bullets);
    }


    // MapUpdateEvent
    [Fact]
    public void MapUpdateEvent_ShouldSerializeCorrectly()
    {
        // Arrange
        var original = new MapUpdateEvent { Walls = new List<Wall>(), 
        Fences = new List<Fence>(),
        Traps = new List<Trap>(),
        Laser = new List<Laser>() };

        // Act
        var json = JsonSerializer.Serialize(original);
        var result = JsonSerializer.Deserialize<MapUpdateEvent>(json);

        // Assert
        Assert.IsType<MapUpdateEvent>(result);
        Assert.Empty(result.Fences);
        Assert.Empty(result.Traps);
        Assert.Empty(result.Laser);
    }

    [Fact]
    public void MapUpdateEvent_ShouldDeserializeCorrectly()
    {
        // Arrange
        const string json = """
        {
            "eventType": "MAP_UPDATE_EVENT",
            "walls": [],
            "fences": [],
            "traps": [],
            "laser": []
        }
        """;

        // Act
        var result = JsonSerializer.Deserialize<MapUpdateEvent>(json);

        // Assert
        Assert.IsType<MapUpdateEvent>(result);
        Assert.Empty(result.Walls);
        Assert.Empty(result.Fences);
        Assert.Empty(result.Traps);
        Assert.Empty(result.Laser);
    }

    // BuffActivateEvent
    [Fact]
    public void BuffActivateEvent_ShouldSerializeCorrectly()
    {
        // Arrange
        var original = new BuffActivateEvent { 
            BuffName = "BULLET_COUNT",
            PlayerToken = "player_123"
         };

        // Act
        var json = JsonSerializer.Serialize(original);
        var result = JsonSerializer.Deserialize<BuffActivateEvent>(json);

        // Assert
        Assert.Equal(original, result);
    }

    [Fact]
    public void BuffActivateEvent_ShouldDeserializeCorrectly()
    {
        // Arrange
        const string json = """
        {
            "eventType": "BUFF_ACTIVE_EVENT",
            "buffName": "SPEED_BOOST",
            "playerToken": "player_123"
        }
        """;

        // Act
        var result = JsonSerializer.Deserialize<BuffActivateEvent>(json);

        // Assert
        Assert.Equal("BUFF_ACTIVE_EVENT", result?.EventType);
        Assert.Equal("SPEED_BOOST", result?.BuffName);
        Assert.Equal("player_123", result?.PlayerToken);
    }

    // BuffDisactivateEvent
    [Fact]
    public void BuffDisactivateEvent_ShouldSerializeCorrectly()
    {
        // Arrange
        var original = new BuffDisactivateEvent { 
            BuffName = "BULLET_COUNT",
            PlayerToken = "player_123"
         };

        // Act
        var json = JsonSerializer.Serialize(original);
        var result = JsonSerializer.Deserialize<BuffDisactivateEvent>(json);

        // Assert
        Assert.Equal(original, result);
    }

    [Fact]
    public void BuffDisactivateEvent_ShouldDeserializeCorrectly()
    {
        // Arrange
        const string json = """
        {
            "eventType": "BUFF_DISACTIVE_EVENT",
            "buffName": "SPEED_BOOST",
            "playerToken": "player_123"
        }
        """;

        // Act
        var result = JsonSerializer.Deserialize<BuffDisactivateEvent>(json);

        // Assert
        Assert.Equal("BUFF_DISACTIVE_EVENT", result?.EventType);
        Assert.Equal("SPEED_BOOST", result?.BuffName);
        Assert.Equal("player_123", result?.PlayerToken);
    }

    private BattleUpdateEvent Deserialize(string json)
    {
        var reader = new Utf8JsonReader(
            System.Text.Encoding.UTF8.GetBytes(json)
        );
        return _converter.Read(ref reader, typeof(BattleUpdateEvent), _options);
    }

    private string Serialize(BattleUpdateEvent @event)
    {
        using var stream = new System.IO.MemoryStream();
        using var writer = new Utf8JsonWriter(stream);
        _converter.Write(writer, @event, _options);
        writer.Flush();
        return System.Text.Encoding.UTF8.GetString(stream.ToArray());
    }

    [Theory]
    [InlineData("PLAYER_UPDATE_EVENT", typeof(PlayerUpdateEvent))]
    [InlineData("BULLETS_UPDATE_EVENT", typeof(BulletsUpdateEvent))]
    [InlineData("MAP_UPDATE_EVENT", typeof(MapUpdateEvent))]
    [InlineData("BUFF_ACTIVE_EVENT", typeof(BuffActivateEvent))]
    [InlineData("BUFF_DISACTIVE_EVENT", typeof(BuffDisactivateEvent))]
    public void Read_ShouldDeserializeToCorrectType(string eventType, Type expectedType)
    {
        // Arrange
        var json = $$"""
        {
            "eventType": "{{eventType}}",
            {{GetTestDataForEventType(eventType)}}
        }
        """;

        // Act
        var result = Deserialize(json);

        // Assert
        Assert.IsType(expectedType, result);
        Assert.Equal(eventType, result.EventType);
    }

    [Theory]
    [InlineData(typeof(PlayerUpdateEvent))]
    [InlineData(typeof(BulletsUpdateEvent))]
    [InlineData(typeof(MapUpdateEvent))]
    [InlineData(typeof(BuffActivateEvent))]
    [InlineData(typeof(BuffDisactivateEvent))]
    public void Write_ShouldSerializeCorrectEventType(Type eventType)
    {
        // Arrange
        var @event = CreateTestEvent(eventType);
        
        // Act
        var json = Serialize(@event);
        using var doc = JsonDocument.Parse(json);
        
        // Assert
        Assert.Equal(@event.EventType, doc.RootElement.GetProperty("eventType").GetString());
    }

    private string GetTestDataForEventType(string eventType)
    {
        return eventType switch
        {
            "PLAYER_UPDATE_EVENT" => "\"players\": []",
            "BULLETS_UPDATE_EVENT" => "\"bullets\": []",
            "MAP_UPDATE_EVENT" => """
            "walls": [], "fences": [], "traps": [], "laser": []
            """,
            "BUFF_ACTIVE_EVENT" => """
            "buffName": "TEST_BUFF", "playerToken": "test_token"
            """,
            "BUFF_DISACTIVE_EVENT" => """
            "buffName": "TEST_BUFF", "playerToken": "test_token"
            """,
            _ => throw new NotImplementedException()
        };
    }

    private BattleUpdateEvent CreateTestEvent(Type eventType)
    {
        return eventType.Name switch
        {
            nameof(PlayerUpdateEvent) => new PlayerUpdateEvent { Players = new List<Player>() },
            nameof(BulletsUpdateEvent) => new BulletsUpdateEvent { Bullets = new List<Bullet>() },
            nameof(MapUpdateEvent) => new MapUpdateEvent 
            { 
                Walls = new List<Wall>(), 
                Fences = new List<Fence>(),
                Traps = new List<Trap>(),
                Laser = new List<Laser>()
            },
            nameof(BuffActivateEvent) => new BuffActivateEvent 
            { 
                BuffName = "TEST", 
                PlayerToken = "TOKEN" 
            },
            nameof(BuffDisactivateEvent) => new BuffDisactivateEvent 
            { 
                BuffName = "TEST", 
                PlayerToken = "TOKEN" 
            },
            _ => throw new NotImplementedException()
        };
    }
}