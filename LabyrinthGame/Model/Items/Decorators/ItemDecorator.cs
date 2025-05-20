using LabyrinthGame.Model;
using LabyrinthGame.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabyrinthGame.Model.Items.Decorators
{
    public abstract class ItemDecorator : IItem
    {
        protected IItem item;
        public ItemDecorator(IItem item)
        {
            this.item = item;
        }

        public virtual string Icon => item.Icon;

        public virtual void ApplyEffect(Player player)
        {
            item.ApplyEffect(player);
        }

        public abstract string GetName();

        public virtual void SubstractEffect(Player player)
        {
            item.SubstractEffect(player);
        }
    }
}
