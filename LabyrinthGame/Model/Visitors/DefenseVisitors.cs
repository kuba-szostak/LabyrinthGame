using LabyrinthGame.Model;
using LabyrinthGame.Model.Interfaces;
using LabyrinthGame.Model.Items.Decorators;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabyrinthGame.Model.Visitors
{
   public class NormalDefenseVisitor : IWeaponVisitor<int>
    {
        private readonly Attributes attr;
        public NormalDefenseVisitor(Attributes attr)
        {
            this.attr = attr;
        }

        public int Visit(HeavyCategoryDecorator w) => attr.Strength + attr.Luck;
        public int Visit(LightCategoryDecorator w) => attr.Dexterity + attr.Luck;
        public int Visit(MagicCategoryDecorator w) => attr.Dexterity + attr.Luck;
        public int Visit(IItem nonWeapon) => attr.Dexterity;
    }
    
    public class StealthDefenseVisitor : IWeaponVisitor<int>
    {
        private readonly Attributes attr;
        public StealthDefenseVisitor(Attributes attr)
        {
            this.attr = attr;
        }

        public int Visit(HeavyCategoryDecorator w) => attr.Strength;
        public int Visit(LightCategoryDecorator w) => attr.Dexterity;
        public int Visit(MagicCategoryDecorator w) => 0;
        public int Visit(IItem nonWeapon) => 0;
    }
    
    public class MagicDefenseVisitor : IWeaponVisitor<int>
    {
        private readonly Attributes attr;
        public MagicDefenseVisitor(Attributes attr)
        {
            this.attr = attr;
        }

        public int Visit(HeavyCategoryDecorator w) =>  attr.Luck;
        public int Visit(LightCategoryDecorator w) =>  attr.Luck;
        public int Visit(MagicCategoryDecorator w) => attr.Wisdom;
        public int Visit(IItem nonWeapon) => attr.Luck;
    }
}
