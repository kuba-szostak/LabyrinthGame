using LabyrinthGame.Model;
using LabyrinthGame.Model.Interfaces;
using LabyrinthGame.Model.Items.Decorators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabyrinthGame.Model.Visitors
{
    public class NormalAttackVisitor : IWeaponVisitor<int>
    {
        public int Visit(HeavyCategoryDecorator w) => w.Damage;
        public int Visit(LightCategoryDecorator w) => w.Damage;
        public int Visit(MagicCategoryDecorator w) => 1;
        public int Visit(IItem nonWeapon) => 0;
    }

    public class StealthAttackVisitor : IWeaponVisitor<int>
    {
        public int Visit(HeavyCategoryDecorator w) => w.Damage / 2;
        public int Visit(LightCategoryDecorator w) => w.Damage * 2;
        public int Visit(MagicCategoryDecorator w) => 1;
        public int Visit(IItem nonWeapon) => 0;
    }

    public class MagicAttackVisitor : IWeaponVisitor<int>
    {
        public int Visit(HeavyCategoryDecorator w) => 1;
        public int Visit(LightCategoryDecorator w) => 1;
        public int Visit(MagicCategoryDecorator w) => w.Damage;
        public int Visit(IItem nonWeapon) => 0;
    }
}
