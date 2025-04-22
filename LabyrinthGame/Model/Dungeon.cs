using LabyrinthGame.Interfaces;
using LabyrinthGame.Items;
using LabyrinthGame.Items.Decorators;
using LabyrinthGame.Items.Potions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.Json;

namespace LabyrinthGame.Model
{
    public enum Tile { Wall, Floor }

    public class Dungeon
    {

        public int Width { get; private set; }
        public int Height { get; private set; }
        public Tile[,] Tiles { get; private set; }
        public List<IItem>[,] ItemMap { get; private set; }
        public List<Enemy>[,] EnemyMap { get; private set; }


        public Dungeon(int width = 40, int height = 20)
        {
            Width = width;
            Height = height;
            Tiles = new Tile[width, height];
            ItemMap = new List<IItem>[width, height];
            EnemyMap = new List<Enemy>[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    ItemMap[x, y] = new List<IItem>();
                    EnemyMap[x, y] = new List<Enemy>();
                }
            }
        }
        public void CreateEmpty()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (x == 0 || x == Width - 1 || y == 0 || y == Height - 1)
                    {
                        Tiles[x, y] = Tile.Wall;
                    }
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

        /// Logic behind AddRandomPaths is as follows, 
        /// Player starts at the middle, 
        /// We chose X points on the boundary of the map (using GetRandomBoundaryPoint)
        /// and than create paths that connect the middle with those points.
        public void AddRandomPaths(int boundaryPointsCount = 3)
        {
            Random rand = new Random();

            int centerX = Width / 2;
            int centerY = Height / 2;

            Tiles[centerX, centerY] = Tile.Floor;

            for (int i = 0; i < boundaryPointsCount; i++)
            {
                (int targetX, int targetY) = GetRandomBoundaryPoint(rand);

                int x = centerX;
                int y = centerY;

                int maxSteps = Width * Height;
                for (int step = 0; step < maxSteps; step++)
                {
                    Tiles[x, y] = Tile.Floor;

                    if (x == targetX && y == targetY)
                        break;

                    var possibleMoves = new List<(int dx, int dy)>();

                    if (x < targetX) possibleMoves.Add((1, 0));
                    else if (x > targetX) possibleMoves.Add((-1, 0));

                    if (y < targetY) possibleMoves.Add((0, 1));
                    else if (y > targetY) possibleMoves.Add((0, -1));

                    if (possibleMoves.Count == 0)
                        break;

                    var (dx, dy) = possibleMoves[rand.Next(possibleMoves.Count)];

                    x += dx;
                    y += dy;

                    x = Math.Clamp(x, 1, Width - 2);
                    y = Math.Clamp(y, 1, Height - 2);
                }
            }
        }
        private (int x, int y) GetRandomBoundaryPoint(Random rand)
        {
            int side = rand.Next(4);
            switch (side)
            {
                case 0:
                    return (rand.Next(1, Width - 1), 0);
                case 1:
                    return (rand.Next(1, Width - 1), Height - 1);
                case 2:
                    return (0, rand.Next(1, Height - 1));
                case 3:
                    return (Width - 1, rand.Next(1, Height - 1));
            }
            return (0, 0);
        }



        public void AddChambers(int chamberCount = 3)
        {
            Random rand = new Random();

            for (int i = 0; i < chamberCount; i++)
            {
                int chamberWidth = rand.Next(3, 6);
                int chamberHeight = rand.Next(3, 6);

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
            Random rand = new Random();
            int roomWidth = rand.Next(6, 11);
            int roomHeight = rand.Next(6, 11);

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

                // randomly wrapping a weapon in one of the 3 possible necessary decorators
                int category = rand.Next(0, 3);
                switch(category)
                {
                    case 0:
                        weapon = new HeavyCategoryDecorator(weapon);
                        break;
                    case 1:
                        weapon = new LightCategoryDecorator(weapon);
                        break;
                    case 2:
                        weapon = new MagicCategoryDecorator(weapon);
                        break;
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

                    if (p > 0.5)
                    {
                        WeaponDamageDecoratorTemplate modifier = gameData.WeaponDamageDecorators[rand.Next(gameData.WeaponDamageDecorators.Count)];

                        weapon = new DamageWeaponDecorator<IWeapon>(weapon, modifier.Name, baseDamage => baseDamage + (modifier.DamageBonus ?? 0));
                    }
                    else
                    {
                        WeaponEffectDecoratorTemplate modifier = gameData.WeaponEffectDecorators[rand.Next(gameData.WeaponEffectDecorators.Count)];
                        weapon = new EffectWeaponDecorator<IWeapon>(weapon, modifier.Name, modifier.EffectAttribute, modifier.EffectValue ?? 0);
                    }

                }

                // randomly wrapping a weapon in one of the 3 possible necessary decorators
                int category = rand.Next(0, 3);
                switch (category)
                {
                    case 0:
                        weapon = new HeavyCategoryDecorator(weapon);
                        break;
                    case 1:
                        weapon = new LightCategoryDecorator(weapon);
                        break;
                    case 2:
                        weapon = new MagicCategoryDecorator(weapon);
                        break;
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

        public void DistributePotions(int numberOfPotions)
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

            if (gameData.Potions.Count == 0)
            {
                return;
            }

            Random rand = new Random();

            for (int i = 0; i < numberOfPotions; i++)
            {
                PotionTemplate template = gameData.Potions[rand.Next(gameData.Potions.Count)];
                Potion potion = new Potion(template.Name, template.Icon, template.EffectAttribute, 
                    template.EffectValue, template.EffectTime, template.IsEffectStable);

                while (true)
                {
                    int x = rand.Next(0, Width);
                    int y = rand.Next(0, Height);
                    if (Tiles[x, y] == Tile.Floor)
                    {
                        ItemMap[x, y].Add(potion);
                        break;
                    }
                }
            }
        }


        public void DistributeEnemies(int numberOfEnemies)
        {
            if (!HasFloorTiles())
                return;

            string json = File.ReadAllText("../../../gameData.json");
            GameData? gameData = JsonSerializer.Deserialize<GameData>(json);
            if (gameData == null)
                throw new InvalidOperationException("Failed to load game data.");

            if (gameData.Enemies.Count == 0)
                return;

            Random rand = new Random();

            for (int i = 0; i < numberOfEnemies; i++)
            {
                EnemyTemplate template = gameData.Enemies[rand.Next(gameData.Enemies.Count)];
                Enemy enemy = new Enemy(template.Name, template.Icon, template.HP, template.Armor, template.Damage);


                while (true)
                {
                    int x = rand.Next(0, Width);
                    int y = rand.Next(0, Height);
                    if (Tiles[x, y] == Tile.Floor)
                    {
                        EnemyMap[x, y].Add(enemy);
                        break;
                    }
                }
            }
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

}
