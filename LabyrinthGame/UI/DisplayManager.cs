using LabyrinthGame.Core;
using LabyrinthGame.Interfaces;
using LabyrinthGame.Items;
using LabyrinthGame.Items.Decorators;
using LabyrinthGame.Items.Potions;
using LabyrinthGame.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LabyrinthGame.UI
{
    public class DisplayManager
    {

        private static readonly DisplayManager instance = new DisplayManager();
        private DisplayManager()
        {

        }

        public static DisplayManager Instance
        {
            get { return instance; }
        }

        public void StartProgram()
        {
            var builder = new DungeonBuilder(40, 20)
                .BuildEmptyDungeon()
                .BuildFilledDungeon()
                //.AddPaths(20)
                //.AddChambers(5)
                .AddCentralRoom()
                //.AddItems(10)
                //.AddWeapons(10)
                .AddModifiedWeapons(10)
                .AddPotions(30);
            //.AddEnemies(3);

            Dungeon dungeon = builder.Build();
            GameInstructions instructions = builder.GetInstructions();

            Point position = new Point(20, 10);
            Player player = new Player(position, dungeon);

            InputHandler chain = InputHandlerFactory.CreateInputHandlerChain(instructions, dungeon);

            Console.CursorVisible = false;

            while (true)
            {
                Console.SetCursorPosition(0, 0);
                PrintMap(dungeon, player);

                Console.SetCursorPosition(dungeon.Width + 2, 0);
                Console.WriteLine($"Press I for instructions");

                DisplayItemInfo(player, dungeon);
                DisplayInventory(player, dungeon.Width + 2, 10);
                DisplayActiveEffects(player, dungeon.Width + 40, 2);
                DisplayAttributes(player, dungeon.Width + 2, 2);
                DisplayHands(player, dungeon.Width + 20, 10);
                DisplayEnemyInfo(player, dungeon);

                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true);

                    chain.HandleInput(key.Key, player);

                    while (Console.KeyAvailable)
                        _ = Console.ReadKey(true);
                }

                // player.UpdateEffects();
            }
        }

        public void PrintMap(Dungeon dungeon, Player player)
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
                        else if (dungeon.EnemyMap[x, y] != null && dungeon.EnemyMap[x, y].Any())
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(dungeon.EnemyMap[x, y][0].Icon);
                            Console.ResetColor();
                        }
                        else if (dungeon.ItemMap[x, y].Count > 0)
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


        public void DisplayConsoleClear()
        {
            Console.Clear();
        }

        public void DisplayInvalidInput(int cursorX, int cursorY)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(cursorX, cursorY);
            Console.WriteLine("Invalid Input");
            Console.ResetColor();
        }

        public void DisplayActiveEffects(Player player, int cursorX, int cursorY)
        {
            int line = cursorY;
            Console.SetCursorPosition(cursorX, line);
            Console.WriteLine("Active Effects: ");

            for(int i = 0; i < 10; i++)
            {
                Console.SetCursorPosition(cursorX, ++line);
                Console.Write(new string(' ', 40));
            }
            line = cursorY;

            foreach (var effect in player.ActiveEffects)
            {
                Console.SetCursorPosition(cursorX, ++line);
                Console.WriteLine($"\t {effect.GetName()} {effect.GetRemainingTurns()}");
            }
        }

        public void DisplayInventory(Player player, int cursorX, int cursorY)
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
                    Console.Write(new string(' ', 50));
                    Console.SetCursorPosition(cursorX, line + i);
                    Console.WriteLine($"{i + 1}. {item.GetName()} - {item.Icon}");
                }
            }

        }

        public void DisplayHands(Player player, int startX, int startY)
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

        public void DisplayAttributes(Player player, int cursorX, int cursorY)
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

        public void DisplayInstructions(GameInstructions instructions)
        {
            Console.Clear();
            Console.WriteLine("Instructions\n");
            Console.WriteLine(instructions.ToString());
            Console.WriteLine("\nPress any key to resume the game...");
            Console.ReadKey(true);
            Console.Clear();
        }

        public void DisplayItemInfo(Player player, Dungeon map)
        {
            if (map.ItemMap[player.position.X, player.position.Y] != null
               && map.ItemMap[player.position.X, player.position.Y].Any())
            {
                IItem item = map.ItemMap[player.position.X, player.position.Y][0];
                Console.SetCursorPosition(0, map.Height + 1);
                Console.Write(new string(' ', 70));
                Console.SetCursorPosition(0, map.Height + 1);
                Console.WriteLine($"Press E to pick up {item.GetName()}");
            }
            else
            {
                Console.SetCursorPosition(0, map.Height + 1);
                Console.Write(new string(' ', 70));
            }
        }

        public void DisplayDropItemMenu(Player player)
        {

            Console.Clear();
            Console.WriteLine("Select an item to drop:");
            DisplayInventory(player, 0, 1);
            Console.WriteLine("Press which item you want to drop");
        }

        public void DisplayEquipItemMenu(Player player, List<IWeapon> weapons, bool leftHandFlag)
        {
            if (weapons.Count == 0 && player.LeftHand == null && player.RightHand == null)
            {
                Console.Beep();
                return;
            }

            Console.Clear();
            Console.WriteLine($"Select weapon to place in {(leftHandFlag ? "left" : "right")} hand... ");
            Console.WriteLine($"\t0. Unequip current item");
            for (int i = 0; i < weapons.Count; i++)
            {
                var wpn = weapons[i];
                Console.WriteLine($"\t{i + 1}. {wpn.GetName()} (Damage: {wpn.Damage})");
            }
        }

        public void DisplayEnemyInfo(Player player, Dungeon map)
        {
            Console.SetCursorPosition(0, map.Height + 2);

            bool foundEnemy = false;

            for (int dx = -1; dx <= 1 && !foundEnemy; dx++)
            {
                for (int dy = -1; dy <= 1 && !foundEnemy; dy++)
                {

                    int nx = player.position.X + dx;
                    int ny = player.position.Y + dy;

                    if (map.InBounds(new Point(nx, ny)))
                    {
                        if (map.EnemyMap[nx, ny].Any())
                        {
                            var enemy = map.EnemyMap[nx, ny][0];
                            Console.WriteLine($"There is a {enemy.GetName()} ({enemy.Icon}) nearby!");
                            foundEnemy = true;
                        }
                    }
                }
            }
            if (!foundEnemy)
            {
                Console.Write(new string(' ', 100));
            }
        }

        public void DisplayPotionMenu(Player player, List<Potion> potions)
        {
            Console.Clear();
            Console.WriteLine("Select a potion to drink:");
            for (int i = 0; i < potions.Count; i++)
            {
                var pot = potions[i];
                Console.WriteLine($"\t{i + 1}. {pot.GetName()}");
            }
        }
    }

}
