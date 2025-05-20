using LabyrinthGame.Controller;
using LabyrinthGame.View;

class Program
{
    public static void Main(String[] Args)
    {
        Console.CursorVisible = false;
        new GameLoop().Run();
    }
}