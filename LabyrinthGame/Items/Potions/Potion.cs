using LabyrinthGame.Interfaces;
using System;
using System.Numerics;
using System.Reflection;

namespace LabyrinthGame.Items.Potions
{
    public class Potion : IItem
    {
        public string Name { get; private set; }
        public string Icon { get; private set; }
        public string EffectAttribute { get; private set; }
        public int EffectValue { get; private set; }

        /// <summary>
        ///  Effect Time in Turns, when 0 -> infinite
        /// </summary>
        public int EffectTime { get; private set; }
        /// <summary>
        ///   false means the effect fades over time
        /// </summary>
        public bool IsEffectStable { get; private set; }



        public Potion(string name, string icon, string effectAttribute, int effectValue, int effectTime, bool isEffectStable)
        {
            Name = name;
            Icon = icon;
            EffectAttribute = effectAttribute;
            EffectValue = effectValue;
            EffectTime = effectTime;
            IsEffectStable = isEffectStable;
        }

        public string GetName() => Name + (EffectTime == 0 ? " (Eternal)" : " (Temporal)");

        public void ApplyEffect(Player player)
        {
            if (EffectTime > 0)
            {
                PotionEffect effect = new PotionEffect(GetName() ,EffectAttribute, EffectValue, EffectTime, IsEffectStable);
                effect.Apply(player);
                player.Attach(effect);
            }
            else
            {
                PropertyInfo? prop = player.attributes.GetType().GetProperty(EffectAttribute);
           
                int currentValue = (int)prop.GetValue(player.attributes);
                prop.SetValue(player.attributes, currentValue + EffectValue);

                var eternal = new EternalEffect(GetName(), EffectAttribute, EffectValue);
                player.Attach(eternal);
            }
        }

        public void SubstractEffect(Player player)
        {
            if (EffectTime == 0)
            {
                PropertyInfo? prop = player.attributes.GetType().GetProperty(EffectAttribute);
               
                int currentValue = (int)prop.GetValue(player.attributes);
                prop.SetValue(player.attributes, currentValue - EffectValue);
            }
        }

    }
}
