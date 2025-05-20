using LabyrinthGame.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabyrinthGame.Model.Items.Decorators
{
    public abstract class WeaponCategoryDecorator : ItemDecorator, IWeapon
    {
        protected readonly string categoryName;

        protected WeaponCategoryDecorator(IWeapon weapon, string categoryName)
            : base(weapon)
        {
            this.categoryName = categoryName;
        }

        public int Damage => ((IWeapon)item).Damage;
        public bool IsTwoHanded => ((IWeapon)item).IsTwoHanded;

        public override string GetName() => $"{categoryName} {item.GetName()}";

        public override string Icon => base.Icon;
    }
    public class HeavyCategoryDecorator : WeaponCategoryDecorator
    {
        public HeavyCategoryDecorator(IWeapon weapon)
            : base(weapon, "Heavy")
        { }
    }

    public class LightCategoryDecorator : WeaponCategoryDecorator
    {
        public LightCategoryDecorator(IWeapon weapon)
            : base(weapon, "Light")
        { }
    }

    public class MagicCategoryDecorator : WeaponCategoryDecorator
    {
        public MagicCategoryDecorator(IWeapon weapon)
            : base(weapon, "Magic")
        { }

    }
}
