using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabyrinthGame.Items.Weapons
{
    public class Sword : Weapon 
    {
        public Sword() : base("Sword", 10) { }

        public override string Icon => "s";
    }
}
