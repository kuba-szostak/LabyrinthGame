using LabyrinthGame.Model.Interfaces;
using LabyrinthGame.Model.Items.Potions;
using LabyrinthGame.Model.Visitors;
using LabyrinthGame.Controller;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Marshalling;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using LabyrinthGame.View;

namespace LabyrinthGame.Model
{
    public enum AttackType { Normal, Stealth, Magic }
    public class Player : ISubject
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

        private readonly List<IObserver> _observers = new List<IObserver>();
        public IEnumerable<IObserver> ActiveEffects => _observers;

        public bool IsDead => attributes.Health <= 0;



        public Player(Point _position, Dungeon _dungeon)
        {
            position = _position;
            dungeon = _dungeon;
        }

        public void Attach(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify()
        {
            foreach (var observer in new List<IObserver>(_observers))
            {
                observer.Update(this);
            }
        }

        public void UpdateEffects()
        {
            Notify();
        }

        public void Move(Point newPositon)
        {
            if (newPositon != position && dungeon.InBounds(newPositon) && dungeon.Tiles[newPositon.X, newPositon.Y] != Tile.Wall)
            {
                position = newPositon;
            }
            else if (newPositon != position)
                throw new InvalidOperationException("Invalid move");

        }

        public void Attack(AttackType type)
        {

            //first we check if there are any enemies nearby
            var result = FindEnemy();
            if (result == null ||
                (LeftHand == null && RightHand == null)) // we also want to return if we dont have any weapons equipped
                throw new InvalidOperationException("Invalid input");

            var (enemy, enemyPos) = result.Value;

            // pick the right attack visitor
            IWeaponVisitor<int> visitor = type switch
            {
                AttackType.Normal => new NormalAttackVisitor(),
                AttackType.Stealth => new StealthAttackVisitor(),
                AttackType.Magic => new MagicAttackVisitor(),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };

            //compute damage

            int damage = 0;
            if (LeftHand == RightHand) // when the weapon is Two-Handed, we dont want to make the damage x2
            {
                damage = LeftHand?.Accept(visitor) ?? 0;
            }
            else // if it's not Two-Handed, we just sum damage of weapons from both hands
            {
                int damageL = LeftHand?.Accept(visitor) ?? 0;
                int damageR = RightHand?.Accept(visitor) ?? 0;
                damage = damageR + damageL;
            }

            // ENEMY TAKING DAMAGE
            enemy.ReceiveDamage(damage);
            if (enemy.IsDead) // if he's dead, remove him from the map
            {
                dungeon.EnemyMap[enemyPos.X, enemyPos.Y].Remove(enemy);
                return;
            }
            // else he deals back damage to player

            IWeaponVisitor<int> defenseVisitor = type switch
            {
                AttackType.Normal => new NormalDefenseVisitor(attributes),
                AttackType.Stealth => new StealthDefenseVisitor(attributes),
                AttackType.Magic => new MagicDefenseVisitor(attributes),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };

            int defense = 0;
            if (LeftHand == RightHand)
            {
                defense = LeftHand?.Accept(defenseVisitor) ?? 0;
            }
            else
            {
                int defenseR = LeftHand?.Accept(defenseVisitor) ?? 0;
                int defenseL = RightHand?.Accept(defenseVisitor) ?? 0;
                defense = defenseR + defenseL;
            }

            int damageToPlayer = Math.Max(enemy.Damage - defense, 0);
            attributes.Health -= damageToPlayer;

        }

        private (Enemy, Point)? FindEnemy()
        {

            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {

                    int nx = this.position.X + dx;
                    int ny = this.position.Y + dy;

                    if (dungeon.InBounds(new Point(nx, ny)))
                    {
                        if (dungeon.EnemyMap[nx, ny].Any())
                        {
                            var enemy = dungeon.EnemyMap[nx, ny][0];
                            Point enemyPos = new Point(nx, ny);
                            return (enemy, enemyPos);
                        }
                    }
                }
            }

            return null;
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
                //else if (item.GetName().Contains("Potion"))
                //{
                //    item.ApplyEffect(this);
                //}
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

        public void DropItem(int index)
        {

            if (index >= 0 && index < Inventory.Count)
            {
                RemoveFromInventory(index);
            }
            else
                throw new InvalidOperationException();
        }

        public void Equip(IWeapon weapon, bool leftHandFlag)
        {
            if (LeftHand != null && LeftHand == RightHand)
            {
                LeftHand.SubstractEffect(this);
                Inventory.Add(LeftHand);
                LeftHand = null;
                RightHand = null;
            }

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

        public void EquipFromInventory(bool leftHandFlag, int index)
        {
            var weapons = Inventory.OfType<IWeapon>().ToList();
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
        }

        public void DrinkPotion(int index)
        {
            var potions = Inventory.OfType<Potion>().ToList();
            if (index >= 0 && index < potions.Count)
            {
                Potion selectedPotion = potions[index];
                selectedPotion.ApplyEffect(this);
                Inventory.Remove(selectedPotion);
            }
            else
            {
                throw new InvalidOperationException("Invalid input");
            }
        }

        public void DropAllItems()
        {
            if (Inventory.Count == 0)
                throw new InvalidOperationException("No items to throw out");

            foreach (IItem item in Inventory)
            {
                dungeon.ItemMap[position.X, position.Y].Add(item);
            }
            Inventory.Clear();
        }
    }
}
