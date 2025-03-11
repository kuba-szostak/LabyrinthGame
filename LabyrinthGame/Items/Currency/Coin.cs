using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabyrinthGame.Items.Currency
{
    public class Coin : IItem
    {
        public string Icon => "$";

        public void ApplyEffect(Player player)
        {
        }

        public string GetName() => "Coin";
    }
}
