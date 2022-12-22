using MazeAdventure.Models;
using System.Collections.ObjectModel;

namespace MazeAdventure.Services;
public class MazeGeneratorService
{
    #region feilds
    private int _mazeWidth;
    private int _mazeHeight;
    private readonly Stack<Room> _mazeGeneratorStack;
    private ObservableCollection<Room> _rooms;
    private readonly Random _randomNumberGenerator;
    #endregion
    public int MazeSize
    {
        get { return _mazeWidth * _mazeHeight; }
    }
    public ObservableCollection<Room> Rooms
    {
        get { return _rooms; }
    }

    public MazeGeneratorService()
    {
        try
        {
            _randomNumberGenerator = new Random();
            _rooms = new ObservableCollection<Room>();
            _mazeGeneratorStack = new Stack<Room>();
        }
        catch (Exception ex)
        {
            throw new Exception("Maze(): " + ex.ToString());
        }
    }

    public void BuildMaze(int size)
    {
        CreateMazeRooms(size);
        Room currentRoom = ChooseRandomStartRoom();
        currentRoom.RoomState = RoomState.Visited;
        GenerateNewMaze(currentRoom);
    }
    public string GetDescription(int roomId)
    {
        return _rooms.ElementAt(roomId).Description;
    }
    public int GetEntranceRoom()
    {
        Room entranceRoom = ChooseRandomStartRoom();
        return _rooms.IndexOf(entranceRoom);
    }
    public bool HasTreasure(int roomId)
    {
        return _rooms.ElementAt(roomId).HasTreasure;
    }
    public bool CausesInjury(int roomId)
    {
        return _rooms.ElementAt(roomId).HasTrap;
    }
    public int? GetRoom(int roomId, char direction)
    {
        // Determine the indexes for the current cell's neighbours.
        int northNeighbourIndex = roomId - _mazeWidth;
        int eastNeighbourIndex = roomId + 1;
        int southNeighbourIndex = roomId + _mazeHeight;
        int westNeighbourIndex = roomId - 1;

        // Determine if the current cell is on the north/east/south/west edge of the maze - certain neighbours must be ignored if the current cell is on an edge.
        bool northEdge = roomId < _mazeWidth;
        bool eastEdge = ((roomId + 1) % _mazeWidth) == 0;
        bool westEdge = (roomId % _mazeWidth) == 0;
        bool southEdge = (roomId + _mazeWidth) >= (_mazeWidth * _mazeHeight);

        // North cell.
        if (direction == 'N' && !northEdge && IsCellIndexValid(northNeighbourIndex))
        {
            return northNeighbourIndex;
        }
        // East cell.
        else if (direction == 'E' && !eastEdge && IsCellIndexValid(eastNeighbourIndex))
        {
            return eastNeighbourIndex;
        }
        // South cell.
        else if (direction == 'S' && !southEdge && IsCellIndexValid(southNeighbourIndex))
        {
            return southNeighbourIndex;
        }
        // West cell.
        else if (direction == 'W' && !westEdge && IsCellIndexValid(westNeighbourIndex))
        {
            return westNeighbourIndex;
        }

        return null;
    }

    #region private methods
    private void GenerateNewMaze(Room currentRoom)
    {
        try
        {
            if (_rooms.Any(x => x.RoomState == RoomState.Default) || _mazeGeneratorStack.Count > 0)  // The maze contains unvisited cells or the stack is not empty.
            {
                int cellIndex = _rooms.IndexOf(currentRoom);   // Retrieve the index of the current cell.

                // Determine the indexes for the current cell's neighbours.
                int northNeighbourIndex = cellIndex - _mazeWidth;
                int eastNeighbourIndex = cellIndex + 1;
                int southNeighbourIndex = cellIndex + _mazeHeight;
                int westNeighbourIndex = cellIndex - 1;

                // Determine if the current cell is on the north/east/south/west edge of the maze - certain neighbours must be ignored if the current cell is on an edge.
                bool northEdge = cellIndex < _mazeWidth;
                bool eastEdge = ((cellIndex + 1) % _mazeWidth) == 0;
                bool westEdge = (cellIndex % _mazeWidth) == 0;
                bool southEdge = (cellIndex + _mazeWidth) >= (_mazeWidth * _mazeHeight);

                // Retrieve the current cell's unvisited neighbours.
                List<Room> unvisitedNeighbours = new List<Room>();
                // North cell.
                if (!northEdge && IsCellIndexValid(northNeighbourIndex) && _rooms[northNeighbourIndex].RoomState == RoomState.Default)
                {
                    unvisitedNeighbours.Add(_rooms[northNeighbourIndex]);
                }
                // East cell.
                if (!eastEdge && IsCellIndexValid(eastNeighbourIndex) && _rooms[eastNeighbourIndex].RoomState == RoomState.Default)
                {
                    unvisitedNeighbours.Add(_rooms[eastNeighbourIndex]);
                }
                // South cell.
                if (!southEdge && IsCellIndexValid(southNeighbourIndex) && _rooms[southNeighbourIndex].RoomState == RoomState.Default)
                {
                    unvisitedNeighbours.Add(_rooms[southNeighbourIndex]);
                }
                // West cell.
                if (!westEdge && IsCellIndexValid(westNeighbourIndex) && _rooms[westNeighbourIndex].RoomState == RoomState.Default)
                {
                    unvisitedNeighbours.Add(_rooms[westNeighbourIndex]);
                }

                if (unvisitedNeighbours.Count > 0)
                {
                    // The current cell has unvisited neighbours - select a random unvisited neighbour.
                    Room selectedNeighbour = unvisitedNeighbours.ElementAt(_randomNumberGenerator.Next(unvisitedNeighbours.Count));

                    // Remove the wall between the current cell and the selected neighbour.
                    int selectedNeightbourIndex = _rooms.IndexOf(selectedNeighbour);
                    if (selectedNeightbourIndex == northNeighbourIndex)
                    {
                        currentRoom.RemoveWall(Direction.North);
                        selectedNeighbour.RemoveWall(Direction.South);
                    }
                    else if (selectedNeightbourIndex == eastNeighbourIndex)
                    {
                        currentRoom.RemoveWall(Direction.East);
                        selectedNeighbour.RemoveWall(Direction.West);
                    }
                    else if (selectedNeightbourIndex == southNeighbourIndex)
                    {
                        currentRoom.RemoveWall(Direction.South);
                        selectedNeighbour.RemoveWall(Direction.North);
                    }
                    else if (selectedNeightbourIndex == westNeighbourIndex)
                    {
                        currentRoom.RemoveWall(Direction.West);
                        selectedNeighbour.RemoveWall(Direction.East);
                    }

                    // Put the current cell on the stack.
                    _mazeGeneratorStack.Push(currentRoom);

                    // Set the selected neighbour as visited.
                    selectedNeighbour.RoomState = RoomState.Visited;

                    // Repeat the process with the selected neighbour as the new current cell.
                    GenerateNewMaze(selectedNeighbour);
                }
                else
                {
                    // Set the current cell to empty - it is now part of the maze.
                    currentRoom.RoomState = RoomState.Empty;

                    // The current cell has no unvisited neighbours - pop a cell from the stack.
                    Room previousCell = _mazeGeneratorStack.Pop();
                    previousCell.RoomState = RoomState.Empty; // Set the popped cell to empty - it is now part of the maze.

                    // Repeat the process with the popped cell as the new current cell.
                    GenerateNewMaze(previousCell);
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Maze.GenerateNewMaze(Room currentCell): " + ex.ToString());
        }
    }
    private Room ChooseRandomStartRoom()
    {
        try
        {
            // Select a random cell to start.
            int cellIndex = _randomNumberGenerator.Next(MazeSize);
            if (IsCellIndexValid(cellIndex))
            {
                Room entranceRoom = _rooms.ElementAt(cellIndex);
                return _rooms.ElementAt(cellIndex);
            }
            else
            {
                throw new Exception("Unable to choose a randmom cell.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Maze.ChooseRandomCell(): " + ex.ToString());
        }
    } 
    private bool IsCellIndexValid(int cellIndex)
    {
        return cellIndex >= 0 && cellIndex < _rooms.Count;
    }
    private void CreateMazeRooms(int mazeSize)
    {
        _mazeWidth = mazeSize;
        _mazeHeight = mazeSize;
        while (_rooms.Count != MazeSize)
        {
            var room = new Room();
            room.SetRoomType();
            room.SetDescription();
           _rooms.Add(room);
        }
        if (_rooms.Count > 0)
        {
            var roomId = ChooseRandomRoomForTreasure();
            _rooms.ElementAt(roomId).HasTreasure = true;
        }
    }
    private int ChooseRandomRoomForTreasure()
    {
        try
        {
            Random _randomChooseRoom = new Random();
            var newRooms = _rooms.Where(r => !r.HasTrap).ToList();
            int index = _randomChooseRoom.Next(newRooms.Count);
            return index;

        }
        catch (Exception ex)
        {
            throw new Exception("Maze.ChooseRandomRoom(): " + ex.ToString());
        }
    }
    #endregion
}