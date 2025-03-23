using LabyrinthGame.Items;
using LabyrinthGame.Items.Currency;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace LabyrinthGame
{
    public class Player
    {
        public Point position;
        private Map map;

        public Attributes attributes { get; set; } = new Attributes();

        public List<IItem> Inventory { get; set; } = new List<IItem>();
        public const int InventoryCapacity = 9;

        public int GoldCount { get; set; }
        public int CoinCount { get; set; }

        public IWeapon? LeftHand { get; private set; }
        public IWeapon? RightHand { get; private set; }

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

                if(key.Key == ConsoleKey.I)
                {
                    DisplayManager.DisplayInstructions();
                    return;
                }    
                if (key.Key == ConsoleKey.R)
                {
                    EquipFromInventory(false);
                    return;
                }
                if (key.Key == ConsoleKey.L)
                {
                    EquipFromInventory(true);
                    return;
                }
                if (key.Key == ConsoleKey.E)
                {
                    PickUpItem();
                    return;
                }
                if (key.Key == ConsoleKey.Q)
                {
                    DropItem();
                    return;
                }
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

      

        public void PickUpItem()
        {
            var availableItem = map.ItemMap[position.X, position.Y];
            if (availableItem != null && availableItem.Any())
            {
                Console.SetCursorPosition(0, map.height + 1);

                IItem item = availableItem[0];
                availableItem.RemoveAt(0);

                if (item.GetName().Contains("Coin"))
                {
                    CoinCount++;
                }
                else if (item.GetName().Contains("Gold"))
                {
                    GoldCount++;
                }
                else
                {
                    Inventory.Add(item);
                    // item.ApplyEffect(this);
                }
            }
        }

        public void RemoveFromInventory(int index)
        {
            IItem item = Inventory[index];
            Inventory.RemoveAt(index);
            map.ItemMap[position.X, position.Y].Add(item);
        }

        private void RecalculateAttributes()
        {
            attributes = new Attributes();

            if (LeftHand != null)
            {
                LeftHand.ApplyEffect(this);
            }
            if(RightHand != null && RightHand != LeftHand)
            {
                RightHand.ApplyEffect(this);
            }
               
        }


        public void DropItem()
        {
            if (Inventory.Count == 0)
            {
                Console.Beep();
                return;
            }

            Console.Clear();
            Console.WriteLine("Select an item to drop:");
            DisplayManager.DisplayInventory(this, 0, 1);
            Console.WriteLine("Press which item you want to drop");

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            int index = keyInfo.KeyChar - '1';

            if (index >= 0 && index < Inventory.Count)
            {
                RemoveFromInventory(index);
            }
            else
            {
                Console.Beep();
            }
            Console.Clear();
        }

        public void Equip(IWeapon weapon, bool leftHandFlag)
        {
            if (weapon.IsTwoHanded)
            {
                if (LeftHand != null)
                {
                    if (!Inventory.Contains(LeftHand))
                    {
                        Inventory.Add(LeftHand);
                    }
                }
                if (RightHand != null && RightHand != LeftHand)
                {
                    if (!Inventory.Contains(RightHand))
                    {
                        Inventory.Add(RightHand);
                    }
                }
                LeftHand = weapon;
                RightHand = weapon;
            }
            else
            {
                if (LeftHand != null && RightHand != null && LeftHand == RightHand)
                {
                    Inventory.Add(LeftHand);
                    LeftHand = null;
                    RightHand = null;
                }

                if (leftHandFlag)
                {
                    if (LeftHand != null)
                    {
                        Inventory.Add(LeftHand);
                    }
                    LeftHand = weapon;
                }
                else
                {
                    if (RightHand != null)
                    {
                        Inventory.Add(RightHand);
                    }
                    RightHand = weapon;
                }
            }
        }

        public void Unequip(bool leftHandFlag)
        {
            if(LeftHand != null && LeftHand == RightHand) // two handed weapon
            {
                Inventory.Add(LeftHand);
                LeftHand = null;
                RightHand = null;
            }
            if(leftHandFlag && LeftHand != null) // unequiping item from left hand
            {
                Inventory.Add(LeftHand);
                LeftHand = null;
            }
            if(!leftHandFlag && RightHand != null) // unequiping from right hand
            {
                Inventory.Add(RightHand);
                RightHand = null; 
            }
        }

        private void EquipFromInventory(bool leftHandFlag)
        {
            var weapons = Inventory.OfType<IWeapon>().ToList();
            if (weapons.Count == 0 && LeftHand == null && RightHand == null)
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

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            int index = keyInfo.KeyChar - '1';
            
            if(index == -1) // case user presses '0'
            {
                Unequip(leftHandFlag);
                RecalculateAttributes();
            }
            else if (index >= 0 && index < weapons.Count)
            {
                IWeapon chosenOne = weapons[index];
                Inventory.RemoveAt(index);
                Equip(chosenOne, leftHandFlag);
                RecalculateAttributes();
            }
            else
            {
                Console.Beep();
            }
            Console.Clear();
        }

       

    }
}
