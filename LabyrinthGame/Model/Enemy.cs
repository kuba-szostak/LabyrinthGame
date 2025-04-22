using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabyrinthGame.Model
{
    public class Enemy
    {
        public string Name { get; private set; }
        public string Icon { get; private set; }

        public int HP { get; private set; }

        public int Armor { get; }
        public int Damage { get; }

        public bool IsDead => HP <= 0;

        public Enemy(string name, string icon, int hp, int armor, int damage)
        {
            Name = name;
            Icon = icon;
            HP = hp;
            Armor = armor;
            Damage = damage;
        }

        public string GetName() => Name + $" ({HP} HP)";

        public void ReceiveDamage(int damage)
        {
            int net = Math.Max(damage - Armor, 0);
            HP -= net;
        }
    }
}
