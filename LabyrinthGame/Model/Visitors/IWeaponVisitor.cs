using LabyrinthGame.Model.Interfaces;
using LabyrinthGame.Model.Items.Decorators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabyrinthGame.Model.Visitors
{
    public interface IWeaponVisitor<T>
    {
        T Visit(HeavyCategoryDecorator weapon);
        T Visit(LightCategoryDecorator weapon);
        T Visit(MagicCategoryDecorator weapon);
        T Visit(IItem nonWeapon);
    }
}
