using LabyrinthGame.Interfaces;
using LabyrinthGame.Model;
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
        private readonly string effectAttribute;
        private readonly int effectValue;

        public EffectWeaponDecorator(T item, string effectName, string effectAttribute, int effectValue)
            : base(item)
        {
            this.effectName = effectName;
            this.effectAttribute = effectAttribute;
            this.effectValue = effectValue;
        }

        public override string GetName() => $"{item.GetName()} ({effectName})";

        public override void ApplyEffect(Player player)
        {
            base.ApplyEffect(player);
            var attributeProp = player.attributes.GetType().GetProperty(effectAttribute);

            if (attributeProp == null)
                throw new Exception($"Property '{effectAttribute}' not found.");

            if (attributeProp.PropertyType == typeof(int))
            {
                int currentValue = (int)attributeProp.GetValue(player.attributes);
                attributeProp.SetValue(player.attributes, currentValue + effectValue);
            }
        }

        public override void SubstractEffect(Player player)
        {
            base.SubstractEffect(player);
            var attributeProp = player.attributes.GetType().GetProperty(effectAttribute);

            if (attributeProp == null)
                throw new Exception($"Property '{effectAttribute}' not found.");

            if (attributeProp.PropertyType == typeof(int))
            {
                int currentValue = (int)attributeProp.GetValue(player.attributes);
                attributeProp.SetValue(player.attributes, currentValue - effectValue);
            }
        }

        public int Damage => ((T)item).Damage;
        public bool IsTwoHanded => ((T)item).IsTwoHanded;
        public override string Icon => item.Icon;

    }



}
