using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LabyrinthGame.Items.Decorators
{
    // special Decorator for weapons, so that we can apply attributes that have an effect on the player
    public class EffectWeaponDecorator<T> : ItemDecorator, IWeapon where T : IWeapon
    {
        private readonly string effectName;
        private readonly Action<Player> effectAction;

        public EffectWeaponDecorator(T item, string effectName, Action<Player> effectAction)
            : base(item)
        {
            this.effectName = effectName;
            this.effectAction = effectAction;
        }

        public override string GetName() => $"{item.GetName()} ({effectName})";

        public override void ApplyEffect(Player player)
        {
            base.ApplyEffect(player);
            effectAction(player);
        }
        public int Damage => ((T)item).Damage;
        public bool IsTwoHanded => ((T)item).IsTwoHanded;
        public override string Icon => item.Icon;

    }



}
