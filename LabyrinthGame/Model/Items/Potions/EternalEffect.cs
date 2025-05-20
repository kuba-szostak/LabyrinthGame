using LabyrinthGame.Model;
using LabyrinthGame.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabyrinthGame.Model.Items.Potions
{
    public class EternalEffect : IObserver
    {
        private readonly string effectName;
        private readonly string effectAttribute;
        private readonly int effectValue;

        public EternalEffect(string effectName, string effectAttribute, int effectValue)
        {
            this.effectName = effectName;
            this.effectAttribute = effectAttribute;
            this.effectValue = effectValue;
        }

        public string GetName() => effectName;

        public string GetRemainingTurns() => "(Eternal)";

        public void Update(Player player)
        {

        }
    }
}
