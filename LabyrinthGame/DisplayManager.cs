using LabyrinthGame.Items;
using LabyrinthGame.Items.Currency;
using LabyrinthGame.Items.Decorators;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabyrinthGame
{
    public class DisplayManager
    {
        public static void StartProgram()
        {
            Map map = new Map(40, 20);
            Point position = new Point(1, 1);
            Player player = new Player(position, map);
            DisplayManager.PlacePredefinedItems(map);

            Console.CursorVisible = false;

            while (true)
            {
                Console.SetCursorPosition(0, 0);
                map.PrintMap(player);

                Console.SetCursorPosition(map.width + 2, 0);
                Console.WriteLine($"Press I for instructions");

                DisplayManager.DisplayItemInfo(player, map);
                DisplayManager.DisplayInventory(player, map.width + 2, 10);
                DisplayManager.DisplayAttributes(player, map.width + 2, 2);
                DisplayManager.DisplayHands(player, map.width + 20, 10);

                player.ProcessMovement();

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
            Console.WriteLine($"\tDexerity: {player.attributes.Dexerity}");

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

        public static void DisplayItemInfo(Player player, Map map)
        {
            if (map.ItemMap[player.position.X, player.position.Y] != null
               && map.ItemMap[player.position.X, player.position.Y].Any())
            {
                IItem item = map.ItemMap[player.position.X, player.position.Y][0];
                Console.SetCursorPosition(0, map.height + 1);
                Console.WriteLine($"Press E to pick up {item.GetName()}");
            }
            else
            {
                Console.SetCursorPosition(0, map.height + 1);
                Console.Write(new string(' ', 100));
            }
        }


        public static void PlacePredefinedItems(Map map)
        {
            // Adding Sword (Sharpness) (Lucky)
            IWeapon baseSword = new Weapon("Sword", "s", 10, 1);
            IWeapon sharpSword = new DamageWeaponDecorator<IWeapon>(
                baseSword,
                "Sharpness",
                baseDamage => baseDamage + 5);
            IWeapon luckySharpSword = new EffectWeaponDecorator<IWeapon>(
                sharpSword,
                "Lucky",
                player => player.attributes.Luck += 5);
            map.ItemMap[5, 5].Add(luckySharpSword);

            // Adding BigAssSword (Smite) (Smart) - two-handed weapon
            IWeapon bigassSword = new Weapon("BigAssSword", "B", 20, 2);
            IWeapon sharpBigAssSword = new DamageWeaponDecorator<IWeapon>(
                bigassSword,
                "Smite",
                baseDamage => baseDamage + 4);
            IWeapon smartSharpBigAssSword = new EffectWeaponDecorator<IWeapon>(
                sharpBigAssSword,
                "Smart",
                player => player.attributes.Wisdom += 10);
            map.ItemMap[10, 1].Add(smartSharpBigAssSword);

            // Adding Sword (Fire Aspect) (Healthy)
            IWeapon flamethrower = new Weapon("Sword", "s", 10, 1);
            IWeapon fireAspectFlamethrower = new DamageWeaponDecorator<IWeapon>(
                flamethrower,
                "Fire Aspect",
                baseDamage => baseDamage + 2);
            IWeapon healthyFireAspectFlamethrower = new EffectWeaponDecorator<IWeapon>(
                fireAspectFlamethrower,
                "Healthy",
                player => player.attributes.Health += 25);
            map.ItemMap[10, 5].Add(healthyFireAspectFlamethrower);

            // Adding a coin
            IItem coin = new Coin();
            map.ItemMap[6, 7].Add(coin);

            // Adding Gold in multiple locations
            IItem gold = new Gold();
            map.ItemMap[15, 15].Add(gold);
            map.ItemMap[18, 18].Add(gold);
            map.ItemMap[30, 17].Add(gold);
            map.ItemMap[2, 18].Add(gold);

            // Adding Axe (Powerful) (Heavy)
            IWeapon axe = new Weapon("Axe", "a", 10, 1);
            IWeapon powerfulAxe = new DamageWeaponDecorator<IWeapon>(
                axe,
                "Powerful",
                baseDamage => baseDamage + 5);
            IWeapon heavyAxe = new EffectWeaponDecorator<IWeapon>(
                powerfulAxe,
                "Heavy",
                player => player.attributes.Dexerity -= 5);
            map.ItemMap[10, 10].Add(heavyAxe);

            // Adding unusable items - Book, Paper, Redstone
            IItem book = new GenericUnusuableItem("Book", "b");
            IItem paper = new GenericUnusuableItem("Paper", "p");
            IItem redstone = new GenericUnusuableItem("Redstone", "r");
            map.ItemMap[1, 2].Add(book);
            map.ItemMap[10, 9].Add(paper);
            map.ItemMap[37, 18].Add(redstone);


        }

    }

}
