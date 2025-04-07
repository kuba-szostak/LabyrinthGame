using LabyrinthGame.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabyrinthGame.Items.Decorators
{
    // generic decorator for applying effects related to damage on weapons
    public class DamageWeaponDecorator<T> : ItemDecorator, IWeapon where T : IWeapon
    {
        private readonly Func<int, int> damageModifier;
        private readonly string effectName;

        public DamageWeaponDecorator(T weapon, string effectName, Func<int, int> damageModifier)
            : base(weapon)
        {
            this.effectName = effectName;
            this.damageModifier = damageModifier;
        }

        public int Damage => damageModifier(((T)item).Damage);

        public bool IsTwoHanded => ((T)item).IsTwoHanded;

        public override string GetName() => $"{item.GetName()} ({effectName})";
    }
}
