using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LabyrinthGame
{
    public class Player
    {
        public Point position;
        private Map map;

        public Player(Point _position, Map _map)
        {
            position = _position;
            map = _map;
        }

        public void ProcessMovement()
        {
            if (map == null || map.Data == null)
                throw new Exception();

            if (Console.KeyAvailable)
            {
                Point newPosition = position;
                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Escape)
                    Environment.Exit(0);
                if (key.Key == ConsoleKey.A)
                    newPosition.X--;
                if (key.Key == ConsoleKey.D)
                    newPosition.X++;
                if (key.Key == ConsoleKey.W)
                    newPosition.Y--;
                if (key.Key == ConsoleKey.S)
                    newPosition.Y++;


                while (Console.KeyAvailable)
                    _ = Console.ReadKey(true);

                if (newPosition != position && map.InBounds(newPosition) && map.Data[newPosition.X, newPosition.Y] != Tile.Wall)
                {
                    position = newPosition;
                }
                else if (newPosition != position)
                {
                    Console.Beep();
                }
            }
        }
    }
}
