using LabyrinthGame.Interfaces;
using LabyrinthGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabyrinthGame.Items
{
    public class Weapon : IWeapon
    {
        public string Name { get; }
        public string Icon { get; }
        public int BaseDamage { get; }
        public int HandsRequired { get; }
        
        public Weapon(string name, string icon = "W", int damage = 10, int handsRequired = 1)
        {
            Name = name;
            BaseDamage = damage;
            Icon = icon;
            HandsRequired = handsRequired;
        }   

        public int Damage => BaseDamage;
        public string GetName() => Name;
        public bool IsTwoHanded => HandsRequired == 2;  
        public void ApplyEffect(Player player)
        { 
        }
        public void SubstractEffect(Player player) { }
    }
}
