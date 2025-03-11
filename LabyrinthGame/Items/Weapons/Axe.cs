using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabyrinthGame.Items.Weapons
{
    public class Axe : Weapon
    {
        public Axe() : base("Axe", 10) { }

        public override string Icon => "a";
    }
}
