using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabyrinthGame
{
    public class Enemy
    {
        public string Namne { get; private set; }
        public string Icon { get; private set; }

        public Enemy(string name, string icon)
        {
            Namne = name;
            Icon = icon;
        }

        public string GetName() => Namne;
    }
}
