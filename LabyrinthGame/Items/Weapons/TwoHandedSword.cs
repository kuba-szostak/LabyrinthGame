using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabyrinthGame.Items.Weapons
{
    public class TwoHandedSword : Weapon
    {
        public override bool IsTwoHanded => true;
        public TwoHandedSword() : base("TwoHandedSword", 20) { }

        public override string Icon => "B";
    }
}
