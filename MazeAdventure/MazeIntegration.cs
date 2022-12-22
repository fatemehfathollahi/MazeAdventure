using MazeAdventure.Contract;
using MazeAdventure.Services;

namespace MazeAdventure;
public class MazeIntegration : IMazeIntegration
{
    private readonly MazeGeneratorService _mazeGeneratorService;
    public MazeIntegration()
    {
        _mazeGeneratorService = new MazeGeneratorService();
    }
    public void BuildMaze(int size)
    {
        _mazeGeneratorService.BuildMaze(size);
    }
    public bool CausesInjury(int roomId)
    {
        return _mazeGeneratorService.CausesInjury(roomId);
    }
    public string GetDescription(int roomId)
    {
        return _mazeGeneratorService.GetDescription(roomId);
    }
    public int GetEntranceRoom()
    {
        return _mazeGeneratorService.GetEntranceRoom();
    }
    public int? GetRoom(int roomId, char direction)
    {
        return _mazeGeneratorService.GetRoom(roomId, direction);
    }
    public bool HasTreasure(int roomId)
    {
        return _mazeGeneratorService.HasTreasure(roomId);
    }
}