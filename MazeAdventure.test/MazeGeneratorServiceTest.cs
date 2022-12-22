using MazeAdventure.Services;
using System;
using System.Linq;
using Xunit;

namespace MazeAdventure.test;
public class MazeGeneratorServiceTest
{
    private readonly MazeGeneratorService mazeGeneratorService;
    public MazeGeneratorServiceTest()
    {
        mazeGeneratorService = new MazeGeneratorService();
    }

    [Theory]
    [InlineData(3)]
    [InlineData(-5)]
    [InlineData(10)]
    public void BuildMazeTest(int size)
    {
        try
        {
            mazeGeneratorService.BuildMaze(size);
            Assert.True(true);
        }
        catch
        {
            Assert.True(false);
        }
    }
    [Fact]
    public void BuildMazeTes_with_zero_size()
    {
        Assert.Throws<Exception>(() => mazeGeneratorService.BuildMaze(0));
    }
    [Fact]
    public void GetEntranceRoomTest()
    {
        mazeGeneratorService.BuildMaze(3);
        var result = mazeGeneratorService.GetEntranceRoom();
        Assert.NotNull(result);
    }
    [Fact]
    public void GetDescriptionTest()
    {
        mazeGeneratorService.BuildMaze(3);
        var result = mazeGeneratorService.GetDescription(5);
        Assert.NotNull(result);
    }
    [Fact]
    public void HasTreasure()
    {
        mazeGeneratorService.BuildMaze(3);
        var roomId = mazeGeneratorService.Rooms.ToList().FindIndex(r => r.HasTreasure);
        Assert.True(mazeGeneratorService.HasTreasure(roomId));
    }
    [Fact]
    public void CausesInjuryTest()
    {
        mazeGeneratorService.BuildMaze(3);
        var roomId = mazeGeneratorService.Rooms.ToList().FindIndex(r => r.HasTrap);
        Assert.True(mazeGeneratorService.CausesInjury(roomId));
    }
    [Fact]
    public void GetRoomTest()
    {
        mazeGeneratorService.BuildMaze(3);
        var result = mazeGeneratorService.GetRoom(4, 'S');
        Assert.NotNull(result);
    }
    [Fact]
    public void GetRoomTest_with_null_return()
    {
        mazeGeneratorService.BuildMaze(3);
        var result = mazeGeneratorService.GetRoom(0, 'W');
        Assert.Null(result);
    }
}