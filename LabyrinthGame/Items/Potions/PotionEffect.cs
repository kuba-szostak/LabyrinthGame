using LabyrinthGame.Interfaces;
using System;
using System.Reflection;

namespace LabyrinthGame.Items.Potions
{
 
    public class PotionEffect : IObserver
    {
        private string Name;
        private int remainingTurns;
        private readonly int totalDuration;
        private readonly string effectAttribute;
        private readonly int effectValue;
        private readonly bool isEffectStable;
        private int baseValue;

        public PotionEffect(string name,string effectAttribute, int effectValue, int duration, bool isEffectStable)
        {
            Name = name;
            this.effectAttribute = effectAttribute;
            this.effectValue = effectValue;
            totalDuration = duration;
            remainingTurns = duration;
            this.isEffectStable = isEffectStable;
        }

        public string GetName() => Name;

        public string GetRemainingTurns() => $"for {remainingTurns.ToString()} turns";


        public void Apply(Player player)
        {
            PropertyInfo prop = player.attributes.GetType().GetProperty(effectAttribute);
            if (prop == null || prop.PropertyType != typeof(int))
                throw new Exception("Invalid attribute for potion effect.");

            baseValue = (int)prop.GetValue(player.attributes);
            if (isEffectStable)
            {
                int current = baseValue;
                prop.SetValue(player.attributes, current + effectValue);
            }
            else
            {
                prop.SetValue(player.attributes, baseValue * totalDuration);
            }
        }

        public void Update(Player player)
        {
            remainingTurns--;

            PropertyInfo prop = player.attributes.GetType().GetProperty(effectAttribute);
            if (prop == null || prop.PropertyType != typeof(int))
                throw new Exception("Invalid attribute for potion effect.");

            if (isEffectStable)
            {
                if (remainingTurns <= 0)
                {
                    int current = (int)prop.GetValue(player.attributes);
                    prop.SetValue(player.attributes, current - effectValue);
                    player.Detach(this);
                }
            }
            else
            {
                if (remainingTurns > 0)
                {
                    prop.SetValue(player.attributes, baseValue * remainingTurns);
                }
                else
                {
                    prop.SetValue(player.attributes, baseValue);
                    player.Detach(this);
                }
            }

        }
    }
}

