using LabyrinthGame.Items;
using LabyrinthGame.Items.Decorators;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.Json;

namespace LabyrinthGame
{
    public enum Tile { Wall, Floor }

    public class Dungeon
    {

        public int Width { get; private set; }
        public int Height { get; private set; }
        public Tile[,] Tiles { get; private set; }
        public List<IItem>[,] ItemMap { get; private set; }

        public Dungeon(int width = 40, int height = 20)
        {
            Width = width;
            Height = height;
            Tiles = new Tile[width, height];
            ItemMap = new List<IItem>[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    ItemMap[x, y] = new List<IItem>();
                }
            }
        }
        public void CreateEmpty()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    // If we are on the boundary, set the tile to Wall
                    if (x == 0 || x == Width - 1 || y == 0 || y == Height - 1)
                    {
                        Tiles[x, y] = Tile.Wall;
                    }
                    // Otherwise, set the tile to Floor
                    else
                    {
                        Tiles[x, y] = Tile.Floor;
                    }
                }
            }
        }
        public void CreateFilled()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Tiles[x, y] = Tile.Wall;
                }
            }
        }
        public void AddRandomPaths()
        {
            Random rand = new Random();
            for (int i = 0; i < 10; i++)
            {
                int row = rand.Next(1, Height - 1);
                for (int x = 1; x < Width - 1; x++)
                {
                    if (rand.NextDouble() > 0.5)
                    {
                        Tiles[x, row] = Tile.Floor;
                    }
                }
            }
        }
        public void AddChambers()
        {
            Random rand = new Random();
            for (int i = 0; i < 3; i++)
            {
                int chamberWidth = rand.Next(3, 7);
                int chamberHeight = rand.Next(3, 7);
                int startX = rand.Next(1, Width - chamberWidth - 1);
                int startY = rand.Next(1, Height - chamberHeight - 1);
                for (int x = startX; x < startX + chamberWidth; x++)
                {
                    for (int y = startY; y < startY + chamberHeight; y++)
                    {
                        Tiles[x, y] = Tile.Floor;
                    }
                }
            }
        }
        public void AddCentralRoom()
        {
            int roomWidth = Width / 2;
            int roomHeight = Height / 2;
            int startX = (Width - roomWidth) / 2;
            int startY = (Height - roomHeight) / 2;
            for (int x = startX; x < startX + roomWidth; x++)
            {
                for (int y = startY; y < startY + roomHeight; y++)
                {
                    Tiles[x, y] = Tile.Floor;
                }
            }
        }

        public void DistributeItems(int numberOfItems)
        {
            if (!HasFloorTiles())
            {
                return;
            }
            string json = File.ReadAllText("../../../gameData.json");
            GameData? gameData = JsonSerializer.Deserialize<GameData>(json);
            if (gameData == null)
            {
                throw new InvalidOperationException("Failed to load game data.");
            }
            Random rand = new Random();

            for (int i = 0; i < numberOfItems; i++)
            {
                ItemTemplate template = gameData.Items[rand.Next(gameData.Items.Count)];
                IItem item = new GenericUnusuableItem(template.Name, template.Icon);
                while (true)
                {
                    int x = rand.Next(0, Width);
                    int y = rand.Next(0, Height);
                    if (Tiles[x, y] == Tile.Floor)
                    {
                        ItemMap[x, y].Add(item);
                        break;
                    }
                }
            }
        }

        public void DistributeWeapons(int numberOfWeapons)
        {
            if (!HasFloorTiles())
            {
                return;
            }

            string json = File.ReadAllText("../../../gameData.json");
            GameData? gameData = JsonSerializer.Deserialize<GameData>(json);
            if (gameData == null)
            {
                throw new InvalidOperationException("Failed to load game data.");
            }

            Random rand = new Random();

            for (int i = 0; i < numberOfWeapons; i++)
            {
                WeaponTemplate template = gameData.Weapons[rand.Next(gameData.Weapons.Count)];
                IWeapon weapon = new Weapon(
                    template.Name,
                    template.Icon,
                    template.BaseDamage,
                    template.HandsRequired
                );


                while (true)
                {
                    int x = rand.Next(0, Width);
                    int y = rand.Next(0, Height);
                    if (Tiles[x, y] == Tile.Floor)
                    {
                        ItemMap[x, y].Add(weapon);
                        break;
                    }
                }
            }
        }


        public void DistributeModifiedWeapons(int numberOfWeapons)
        {
            if (!HasFloorTiles())
            {
                return;
            }

            string json = File.ReadAllText("../../../gameData.json");

            GameData? gameData = JsonSerializer.Deserialize<GameData>(json);

            if (gameData == null)
            {
                throw new InvalidOperationException("Failed to load game data.");
            }

            Random rand = new Random();

            for (int i = 0; i < numberOfWeapons; i++)
            {
                WeaponTemplate template = gameData.Weapons[rand.Next(gameData.Weapons.Count)];
                IWeapon weapon = new Weapon(
                    template.Name,
                    template.Icon,
                    template.BaseDamage,
                    template.HandsRequired
                );

                int numberOfModifiers = rand.Next(1, 4);
                for (int j = 0; j < numberOfModifiers; j++)
                {
                    double p = rand.NextDouble();

                    if(p > 0.5)
                    {
                        WeaponDamageDecoratorTemplate modifier = gameData.WeaponDamageDecorators[rand.Next(gameData.WeaponDamageDecorators.Count)];

                        weapon = new DamageWeaponDecorator<IWeapon>(weapon, modifier.Name, baseDamage => (int)(baseDamage + (modifier.DamageBonus ?? 0)));
                    }
                    else
                    {
                        WeaponEffectDecoratorTemplate modifier = gameData.WeaponEffectDecorators[rand.Next(gameData.WeaponEffectDecorators.Count)];
                        weapon = new EffectWeaponDecorator<IWeapon>(weapon, modifier.Name, modifier.EffectAttribute, modifier.EffectValue ?? 0);
                    }

                }

                while (true)
                {
                    int x = rand.Next(0, Width);
                    int y = rand.Next(0, Height);
                    if (Tiles[x, y] == Tile.Floor)
                    {
                        ItemMap[x, y].Add(weapon);
                        break;
                    }
                }
            }
        }

        public void DistributePotions()
        {
        }

        public void DistributeEnemies()
        {
        }

        private bool HasFloorTiles()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (Tiles[x, y] == Tile.Floor)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        internal bool InBounds(Point p)
        {
            if (p.X < 0 || p.X >= Width || p.Y < 0 || p.Y >= Height)
                return false;
            return true;
        }
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

    public class GameData
    {
        public List<WeaponTemplate> Weapons { get; set; } = new List<WeaponTemplate>();
        public List<ItemTemplate> Items { get; set; } = new List<ItemTemplate>();
        public List<WeaponDamageDecoratorTemplate> WeaponDamageDecorators { get; set; } = new List<WeaponDamageDecoratorTemplate>();
        public List<WeaponEffectDecoratorTemplate> WeaponEffectDecorators { get; set; } = new List<WeaponEffectDecoratorTemplate>();
    }
}
