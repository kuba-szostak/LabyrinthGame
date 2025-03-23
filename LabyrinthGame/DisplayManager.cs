using LabyrinthGame.Items;
using LabyrinthGame.Items.Currency;
using LabyrinthGame.Items.Decorators;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LabyrinthGame
{
    public class DisplayManager
    {
        public static void StartProgram()
        {
            Dungeon dungeon = new DungeonBuilder(40, 20)  
                .BuildEmptyDungeon()     
                .AddItems(10)               
                .AddWeapons(10)             
                .AddModifiedWeapons(10)         
                .Build();


            Point position = new Point(1, 1);
            Player player = new Player(position, dungeon);
           

            Console.CursorVisible = false;

            while (true)
            {
                Console.SetCursorPosition(0, 0);
                DisplayManager.PrintMap(dungeon, player);

                Console.SetCursorPosition(dungeon.Width + 2, 0);
                Console.WriteLine($"Press I for instructions");

                DisplayManager.DisplayItemInfo(player, dungeon);
                DisplayManager.DisplayInventory(player, dungeon.Width + 2, 10);
                DisplayManager.DisplayAttributes(player, dungeon.Width + 2, 2);
                DisplayManager.DisplayHands(player, dungeon.Width + 20, 10);

                player.ProcessMovement();

            }
        }

        public static void PrintMap(Dungeon dungeon, Player player)
        {
            if (dungeon.Tiles != null)
            {
                for (int y = 0; y < dungeon.Height; y++)
                {
                    for (int x = 0; x < dungeon.Width; x++)
                    {
                        if (x == player.position.X && y == player.position.Y)
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write("¶");
                            Console.ResetColor();
                        }
                        else if (dungeon.Tiles[x, y] == Tile.Wall)
                        {
                            Console.Write("█");
                        }
                        else if (dungeon.ItemMap[x, y] != null && dungeon.ItemMap[x, y].Any())
                        {
                            Console.Write(dungeon.ItemMap[x, y][0].Icon);
                        }
                        else if (dungeon.Tiles[x, y] == Tile.Floor)
                        {
                            Console.Write(" ");
                        }
                    }
                    Console.WriteLine();
                }
            }
        }
        
        public static void DisplayInventory(Player player, int cursorX, int cursorY)
        {
            Console.SetCursorPosition(cursorX, cursorY);
            Console.WriteLine("Inventory:");

            Console.SetCursorPosition(cursorX, cursorY + 1);
            Console.WriteLine($"\tGold: {player.GoldCount}");

            Console.SetCursorPosition(cursorX, cursorY + 2);
            Console.WriteLine($"\tCoin: {player.CoinCount}");

            if (player.Inventory.Count > 0)
            {
                int line = cursorY + 3;
                for (int i = 0; i < player.Inventory.Count; i++)
                {
                    var item = player.Inventory[i];
                    Console.SetCursorPosition(cursorX, line + i);
                    Console.WriteLine($"{i + 1}. {item.GetName()} - {item.Icon}");
                }
            }

        }

        public static void DisplayHands(Player player, int startX, int startY)
        {
            Console.SetCursorPosition(startX, startY);
            Console.WriteLine("Equipped Weapons");

            Console.SetCursorPosition(startX, startY + 1);
            if (player.LeftHand != null)
            {
                Console.WriteLine($"\tLeft: {player.LeftHand.GetName()} (Damage: {player.LeftHand.Damage})");
            }
            else
            {
                Console.WriteLine("\tLeft: (Empty)");
            }

            Console.SetCursorPosition(startX, startY + 2);
            if (player.RightHand != null)
            {
                Console.WriteLine($"\tRight: {player.RightHand.GetName()} (Damage: {player.RightHand.Damage})");
            }
            else
            {
                Console.WriteLine("\tRight: (Empty)");
            }
        }

        public static void DisplayAttributes(Player player, int cursorX, int cursorY)
        {
            Console.SetCursorPosition(cursorX, cursorY);
            Console.WriteLine("Players Attributes:");

            Console.SetCursorPosition(cursorX, cursorY + 1);
            Console.Write(new string(' ', 30));
            Console.SetCursorPosition(cursorX, cursorY + 1);
            Console.WriteLine($"\tStrength: {player.attributes.Strength}");

            Console.SetCursorPosition(cursorX, cursorY + 2);
            Console.Write(new string(' ', 30));
            Console.SetCursorPosition(cursorX, cursorY + 2);
            Console.WriteLine($"\tDexerity: {player.attributes.Dexterity}");

            Console.SetCursorPosition(cursorX, cursorY + 3);
            Console.Write(new string(' ', 30));
            Console.SetCursorPosition(cursorX, cursorY + 3);
            Console.WriteLine($"\tHealth: {player.attributes.Health}");

            Console.SetCursorPosition(cursorX, cursorY + 4);
            Console.Write(new string(' ', 30));
            Console.SetCursorPosition(cursorX, cursorY + 4);
            Console.WriteLine($"\tLuck: {player.attributes.Luck}");

            Console.SetCursorPosition(cursorX, cursorY + 5);
            Console.Write(new string(' ', 30));
            Console.SetCursorPosition(cursorX, cursorY + 5);
            Console.WriteLine($"\tAggression: {player.attributes.Aggression}");

            Console.SetCursorPosition(cursorX, cursorY + 6);
            Console.Write(new string(' ', 30));
            Console.SetCursorPosition(cursorX, cursorY + 6);
            Console.WriteLine($"\tWisdom: {player.attributes.Wisdom}");
        }

        public static void DisplayInstructions()
        {
            Console.Clear();
            Console.WriteLine("Instructions\n");
            Console.WriteLine("W / A / S / D : Move your character");
            Console.WriteLine("E             : Pick up an item");
            Console.WriteLine("Q             : Drop an item");
            Console.WriteLine("L             : Equip weapon in left hand");
            Console.WriteLine("R             : Equip weapon in right hand");
            Console.WriteLine("I             : Show these instructions");
            Console.WriteLine("ESC           : Exit the game\n");
            Console.WriteLine("Press any key to resume the game...");
            Console.ReadKey(true);
            Console.Clear();
        }

        public static void DisplayItemInfo(Player player, Dungeon map)
        {
            if (map.ItemMap[player.position.X, player.position.Y] != null
               && map.ItemMap[player.position.X, player.position.Y].Any())
            {
                IItem item = map.ItemMap[player.position.X, player.position.Y][0];
                Console.SetCursorPosition(0, map.Height + 1);
                Console.WriteLine($"Press E to pick up {item.GetName()}");
            }
            else
            {
                Console.SetCursorPosition(0, map.Height + 1);
                Console.Write(new string(' ', 100));
            }
        }


    }

}
