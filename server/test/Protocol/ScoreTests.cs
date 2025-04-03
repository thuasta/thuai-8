using System.Text.Json;
using Thuai.Server.Protocol.Scheme;
using Xunit;

namespace Thuai.Server.Protocol.Scheme.Tests;

public class ScoreTests
{
    [Fact]
    public void SerializeAndDeserialize_ShouldBeEquivalent()
    {
        
        // Arrange
        var original = new Score
        {
            Token = "",
            score = 0           
        };

        // Act
        var json = JsonSerializer.Serialize(original);
        var deserialized = JsonSerializer.Deserialize<Score>(json);

        // Assert
        Assert.Equal(original, deserialized);
    }

    [Fact]
    public void Deserialize_FromJsonFile_ShouldMatchExactly()
    {
        // Arrange
        const string json = """
        {
            "token": "",
            "score": 0
        }
        """;
    
        // Act
        var score = JsonSerializer.Deserialize<Score>(json);
    
        // Assert
        Assert.Equal("", score?.Token);
        Assert.Equal(0, score?.score);
    }
}