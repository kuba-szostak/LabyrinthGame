using LabyrinthGame;
using System;
using System.Drawing;

class Program
{
    public static void Main(String[] Args)
    {
        Map map = new Map(40, 20);

        Point position = new Point(1, 1);
        Player player = new Player(position, map);

        Console.CursorVisible = false;

        Console.WriteLine("Press ESC to exit\n");

        while (true)
        {
            Console.SetCursorPosition(0, 0);
            map.PrintMap(player);

            Console.SetCursorPosition(map.width + 2, 0);
            Console.WriteLine("Press ESC to exit");

            player.ProcessMovement();
            
        }
    }
}