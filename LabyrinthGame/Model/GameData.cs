using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabyrinthGame.Model
{
    /// <summary>
    /// JSON File templates
    /// </summary>
    public class GameData
    {
        public List<WeaponTemplate> Weapons { get; set; } = new List<WeaponTemplate>();
        public List<ItemTemplate> Items { get; set; } = new List<ItemTemplate>();
        public List<WeaponDamageDecoratorTemplate> WeaponDamageDecorators { get; set; } = new List<WeaponDamageDecoratorTemplate>();
        public List<WeaponEffectDecoratorTemplate> WeaponEffectDecorators { get; set; } = new List<WeaponEffectDecoratorTemplate>();
        public List<PotionTemplate> Potions { get; set; } = new List<PotionTemplate>();
        public List<EnemyTemplate> Enemies { get; set; } = new List<EnemyTemplate>();
    }

    public class WeaponTemplate
    {
        public string Name { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public int BaseDamage { get; set; }
        public int HandsRequired { get; set; }
    }

    public class ItemTemplate
    {
        public string Name { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
    }

    public class WeaponDamageDecoratorTemplate
    {
        public string Name { get; set; } = string.Empty;
        public int? DamageBonus { get; set; }
    }

    public class WeaponEffectDecoratorTemplate
    {
        public string Name { get; set; } = string.Empty;
        public string EffectAttribute { get; set; } = string.Empty;
        public int? EffectValue { get; set; }
    }

    public class PotionTemplate
    {
        public string Name { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string EffectAttribute { get; set; } = string.Empty;
        public int EffectValue { get; set; }
        public int EffectTime { get; set; }
        public bool IsEffectStable { get; set; }
    }

    public class EnemyTemplate
    {
        public string Name { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
    }

}
