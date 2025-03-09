using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace LabyrinthGame
{
    public enum Tile { Wall, Floor }

    public class Map
    {
        public int width;
        public int height;
        public Tile[,]? Data;


        public Map(int width, int height)
        {
            this.width = width;
            this.height = height;

            this.Data = new Tile[width, height];

            for (int x = 0; x < width; x++)
            {
                Data[x, 0] = Tile.Wall;
                Data[x, height - 1] = Tile.Wall;
            }
            for (int y = 0; y < height; y++)
            {
                Data[0, y] = Tile.Wall;
                Data[width - 1, y] = Tile.Wall;
            }
            for (int y = 1; y < height - 1; y++)
            {
                for (int x = 1; x < width - 1; x++)
                {
                    Data[x, y] = Tile.Floor;
                }
            }
        }

        public void PrintMap(Player player)
        {
            if (Data != null)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        if (x == player.position.X && y == player.position.Y)
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write("¶");
                            Console.ResetColor();
                        }
                        else if (Data[x, y] == Tile.Wall)
                            Console.Write("█");
                        else if (Data[x, y] == Tile.Floor)
                            Console.Write(" ");
                    }
                    Console.WriteLine();
                }
            }
        }

        internal bool InBounds(Point p)
        {
            if (p.X < 0 || p.X >= width || p.Y < 0 || p.Y >= height)
                return false;
            return true;
        }
    }
}
