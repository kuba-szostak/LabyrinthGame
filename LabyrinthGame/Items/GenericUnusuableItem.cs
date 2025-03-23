using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabyrinthGame.Items
{
    public class GenericUnusuableItem : IItem
    {
        public string Name { get; private set; }
        public string Icon { get; private set; }

        public GenericUnusuableItem(string name, string icon)
        {
            this.Name = name;
            this.Icon = icon;
        }

        public void ApplyEffect(Player player)
        {
           // unusable items have no effects
        }

        public string GetName() => Name;

        public void SubstractEffect(Player player) { }
    }
}
