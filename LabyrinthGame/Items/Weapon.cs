using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabyrinthGame.Items
{
    public class Weapon : IWeapon
    {
        public string BaseName { get; private set; }
        public int BaseDamage { get; private set; }

        public Weapon(string name, int baseDamage)
        {
            BaseName = name;
            BaseDamage = baseDamage;
        }

        public virtual int Damage => BaseDamage;

        public virtual string Icon => "?";

        public virtual bool IsTwoHanded => false;

        public virtual string GetName() => BaseName;

        public virtual void ApplyEffect(Player player) { }
    }
}
