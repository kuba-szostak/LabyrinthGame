using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabyrinthGame.Model.Interfaces
{
    public interface IWeapon : IItem
    {
        int Damage { get;  }
        bool IsTwoHanded { get; }
    }
}
