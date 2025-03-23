using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabyrinthGame.Items
{
    public interface IItem
    {
        string GetName();
        string Icon { get;  }
        void ApplyEffect(Player player);
        void SubstractEffect(Player player);
    }
}
