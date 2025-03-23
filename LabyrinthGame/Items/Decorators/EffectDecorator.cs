using System;
using System.Reflection;

namespace LabyrinthGame.Items.Decorators
{
    // Generic decorator for applying attribute effects to any IItem
    public class EffectDecorator : ItemDecorator
    {
        private readonly string effectName;
        private readonly int effectValue;

        public EffectDecorator(IItem item, string effectName, int effectValue)
            : base(item)
        {
            this.effectName = effectName;
            this.effectValue = effectValue;
        }

        public override void ApplyEffect(Player player)
        {
            base.ApplyEffect(player);

            var attributeField = player.attributes.GetType().GetField(effectName);

            if (attributeField == null)
            {
                throw new Exception($"Wrong Atribute Name");
            }

            if (attributeField.FieldType == typeof(int))
            {
                int currentValue = (int)attributeField.GetValue(player.attributes);
                attributeField.SetValue(player.attributes, currentValue + effectValue);
            }
        }
        public override void SubstractEffect(Player player)
        {
            base.SubstractEffect(player);

            var attributeField = player.attributes.GetType().GetField(effectName);

            if (attributeField == null)
            {
                throw new Exception($"Wrong Atribute Name");
            }

            if (attributeField.FieldType == typeof(int))
            {
                int currentValue = (int)attributeField.GetValue(player.attributes);
                attributeField.SetValue(player.attributes, currentValue - effectValue);
            }
        }

        public override string GetName() => $"{item.GetName()} ({effectName})";
    }
}
