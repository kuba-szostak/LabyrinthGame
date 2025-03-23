using System;
using System.Reflection;

namespace LabyrinthGame.Items
{
    public class Potion : IItem
    {
        public string Name { get; private set; }
        public string Icon { get; private set; }
        public string EffectAttribute { get; private set; }
        public int EffectValue { get; private set; }

        public Potion(string name, string icon, string effectAttribute, int effectValue)
        {
            Name = name;
            Icon = icon;
            EffectAttribute = effectAttribute;
            EffectValue = effectValue;
        }

        public string GetName() => Name;

        public void ApplyEffect(Player player)
        {
            PropertyInfo? prop = player.attributes.GetType().GetProperty(EffectAttribute);
            if (prop == null || prop.PropertyType != typeof(int))
            {
                throw new Exception($"wrong attribute");
            }
            int currentValue = (int)prop.GetValue(player.attributes);
            prop.SetValue(player.attributes, currentValue + EffectValue);
        }

        public void SubstractEffect(Player player)
        {
            PropertyInfo? prop = player.attributes.GetType().GetProperty(EffectAttribute);
            if (prop == null || prop.PropertyType != typeof(int))
            {
                throw new Exception($"wrong attrinbute");
            }
            int currentValue = (int)prop.GetValue(player.attributes);
            prop.SetValue(player.attributes, currentValue - EffectValue);
        }
    }
}
