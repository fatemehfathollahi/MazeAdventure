namespace MazeAdventure.Models;
public class Room
{
    public RoomType RoomType { get; set; } = RoomType.Normal;
    public RoomState RoomState { get; set; } = RoomState.Default;
    public bool NorthWall { get; private set; } = true;
    public bool EastWall { get; private set; } = true;
    public bool SouthWall { get; private set; } = true;
    public bool WestWall { get; private set; } = true;
    public string Description { get; private set; }
    public bool HasTreasure { get; set; }
    public bool HasTrap { get; private set; }

    public void RemoveWall(Direction roomWall)
    {
        try
        {
            switch (roomWall)
            {
                case Direction.North:
                    NorthWall = false;
                    break;

                case Direction.East:
                    EastWall = false;
                    break;

                case Direction.South:
                    SouthWall = false;
                    break;

                case Direction.West:
                    WestWall = false;
                    break;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("MazeRoom.RemoveWall(roomWall roomWall): " + ex.ToString());
        }
    }
    public void SetDescription()
    {
        switch (RoomType)
        {
            case RoomType.Normal:
                Description = "This room is a normal";
                break;
            case RoomType.Forest:
                Description = "You see a lot of thick and tall trees, but be careful of wild animals while you walk";
                break;
            case RoomType.Dessert:
                {
                    Description = "There is stunning beauty in this area, especially at night, but be sure to bring water with you";
                    HasTrap = true;
                }
                break;
            case RoomType.Marsh:
                {
                    Description = "Enjoy the marshes but beware they are very dangerous while beautiful";
                    HasTrap = true;
                }
                break;
            case RoomType.Hills:
                {
                    Description = "You run on the hills and watch the wildflowers and you feel happy";
                }
                break;
            default:
                break;
        }
    }
    public void SetRoomType()
    {
        try
        {
            Type type = typeof(RoomType);
            Array values = type.GetEnumValues();
            Random _randomChooseRoomType = new Random();
            int index = _randomChooseRoomType.Next(values.Length);
            RoomType value = (RoomType)values.GetValue(index);
            RoomType = value;

        }
        catch (Exception ex)
        {
            throw new Exception("Maze.ChooseRandomRoomType(): " + ex.ToString());
        }
    }
}