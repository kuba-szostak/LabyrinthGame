using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabyrinthGame.Items.Decorators
{
    // generic decorator, can be used to apply effects to objects other than weapons
    // so we can have for example book (smart) or smth
    public class EffectDecorator : ItemDecorator
    {
        private readonly string effectName;
        private readonly Action<Player> effectAction;

        public EffectDecorator(IItem item, string effectName, Action<Player> effectAction)
            : base(item)
        {
            this.effectName = effectName;
            this.effectAction = effectAction;
        }

        public override void ApplyEffect(Player player)
        {
            base.ApplyEffect(player);
            effectAction(player);
        }

        public override string GetName() => $"{item.GetName()} ({effectName})";
    }
}
