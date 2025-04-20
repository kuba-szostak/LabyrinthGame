using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabyrinthGame.Model
{
    public class Attributes
    {
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Health { get; set; }
        public int Luck { get; set; }
        public int Aggression { get; set; }
        public int Wisdom{ get; set; }

        public Attributes()
        {
            Strength = 10;
            Dexterity = 10;
            Health = 100;
            Luck = 10;
            Aggression = 10;
            Wisdom = 10;
        }
    }
}
