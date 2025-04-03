using System.Text.Json;
using Thuai.Server.Protocol.Scheme;
using Xunit;

namespace Thuai.Server.Protocol.Scheme.Tests;

public class StageTests
{
    [Theory]
    [InlineData(Stage.REST, "REST")]
    [InlineData(Stage.BATTLE, "BATTLE")]
    [InlineData(Stage.END, "END")]
    public void Serialize_ShouldProduceCorrectString(Stage stage, string expectedJson)
    {
        // Act
        var json = JsonSerializer.Serialize(stage);

        // Assert
        Assert.Equal($"\"{expectedJson}\"", json);
    }
}