using LabyrinthGame.Items;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private Dungeon dungeon;

        public Attributes attributes { get; set; } = new Attributes();

        public List<IItem> Inventory { get; set; } = new List<IItem>();
        public const int InventoryCapacity = 8;

        public int GoldCount { get; set; }
        public int CoinCount { get; set; }

        public IWeapon? LeftHand { get; private set; }
        public IWeapon? RightHand { get; private set; }

        public Player(Point _position, Dungeon _dungeon)
        {
            position = _position;
            dungeon = _dungeon;
        }

        public void Move(Point newPositon)
        {
            if (newPositon != position && dungeon.InBounds(newPositon) && dungeon.Tiles[newPositon.X, newPositon.Y] != Tile.Wall)
            {
                position = newPositon;
            }
            else if (newPositon != position)
            {
                Console.Beep();
            }
        }


        public void PickUpItem()
        {
            var availableItem = dungeon.ItemMap[position.X, position.Y];
            if (availableItem != null && availableItem.Any() && Inventory.Count() < InventoryCapacity)
            {
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
                else if (item.GetName().Contains("Potion"))
                {
                    item.ApplyEffect(this);
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
            dungeon.ItemMap[position.X, position.Y].Add(item);
        }

        public void DropItem()
        {
            if (Inventory.Count == 0)
            {
                Console.Beep();
                return;
            }

            DisplayManager.Instance.DisplayDropItemMenu(this);

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
        }

        public void Equip(IWeapon weapon, bool leftHandFlag)
        {
            if (weapon.IsTwoHanded)
            {
                if (LeftHand != null)
                {
                    Inventory.Add(LeftHand);
                    LeftHand.SubstractEffect(this);
                }
                if (RightHand != null && RightHand != LeftHand)
                {
                    Inventory.Add(RightHand);
                    RightHand.SubstractEffect(this);
                }
                LeftHand = weapon;
                RightHand = weapon;
                weapon.ApplyEffect(this);
            }
            else
            {
                if (leftHandFlag)
                {
                    if (LeftHand != null)
                    {
                        Inventory.Add(LeftHand);
                        LeftHand.SubstractEffect(this);
                    }
                    LeftHand = weapon;
                    weapon.ApplyEffect(this);
                }
                else
                {
                    if (RightHand != null)
                    {
                        Inventory.Add(RightHand);
                        RightHand.SubstractEffect(this);
                    }
                    RightHand = weapon;
                    weapon.ApplyEffect(this);
                }
            }
        }

        public void Unequip(bool leftHandFlag)
        {
            if (LeftHand != null && LeftHand == RightHand) // two handed weapon
            {
                Inventory.Add(LeftHand);
                LeftHand.SubstractEffect(this);
                LeftHand = null;
                RightHand = null;
            }
            if (leftHandFlag && LeftHand != null) // unequiping item from left hand
            {
                Inventory.Add(LeftHand);
                LeftHand.SubstractEffect(this);
                LeftHand = null;
            }
            if (!leftHandFlag && RightHand != null) // unequiping from right hand
            {
                Inventory.Add(RightHand);
                RightHand.SubstractEffect(this);
                RightHand = null;
            }
        }

        public void EquipFromInventory(bool leftHandFlag)
        {
            var weapons = Inventory.OfType<IWeapon>().ToList();

            DisplayManager.Instance.DisplayEquipItemMenu(this, weapons, leftHandFlag);

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            int index = keyInfo.KeyChar - '1';

            if (index == -1) // case user presses '0'
            {
                Unequip(leftHandFlag);
            }
            else if (index >= 0 && index < weapons.Count)
            {
                IWeapon chosenOne = weapons[index];
                Equip(chosenOne, leftHandFlag);
                Inventory.RemoveAt(index);
            }
            else
            {
                Console.Beep();
            }
            Console.Clear();
        }

    }
}
