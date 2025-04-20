using LabyrinthGame.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabyrinthGame.Combat
{
    public static class WeaponVisitorExtensions
    {
        public static TResult Accept<TResult>(this IWeapon weapon, IWeaponVisitor<TResult> visitor)
        {
            return ((dynamic)visitor).Visit((dynamic)weapon);
        }
    }
}
