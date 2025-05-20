using LabyrinthGame.Model;
using LabyrinthGame.Model.Interfaces;
using LabyrinthGame.Model.Items.Potions;
using LabyrinthGame.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabyrinthGame.Controller
{
    public static class InputHelpers
    {
        public static int SelectItemToDrop(Player player)
        {
            if (player.Inventory.Count == 0)
                throw new InvalidOperationException();

            DisplayManager.Instance.DisplayDropItemMenu(player);
            var key = Console.ReadKey(true);
            return key.KeyChar - '1';
        }

        public static int SelectItemToEquip(Player player, List<IWeapon> weapons, bool flag)
        {
            DisplayManager.Instance.DisplayEquipItemMenu(player, weapons, flag);
            var key = Console.ReadKey(true);
            return key.KeyChar - '1';
        }

        public static int SelectPotionToDrink(Player player, List<Potion> potions)
        {
            if(potions.Count == 0)
                throw new InvalidOperationException();

            DisplayManager.Instance.DisplayPotionMenu(player, potions);
            var key = Console.ReadKey(true);
            return key.KeyChar - '1';
        }

    }
}
