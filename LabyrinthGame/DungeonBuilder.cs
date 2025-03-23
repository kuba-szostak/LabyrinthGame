using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabyrinthGame
{
    public class DungeonBuilder
    {
        private Dungeon dungeon;

        public DungeonBuilder(int width = 40, int height = 20)
        {
            dungeon = new Dungeon(width, height);
        }
        public DungeonBuilder BuildEmptyDungeon()
        {
            dungeon.CreateEmpty();
            return this;
        }
        public DungeonBuilder BuildFilledDungeon()
        {
            dungeon.CreateFilled();
            return this;
        }

        public DungeonBuilder AddPaths()
        {
            dungeon.AddRandomPaths();
            return this;
        }

        public DungeonBuilder AddChambers()
        {
            dungeon.AddChambers();
            return this;
        }

        public DungeonBuilder AddCentralRoom()
        {
            dungeon.AddCentralRoom();
            return this;
        }

        public DungeonBuilder AddItems(int x = 10)
        {
            dungeon.DistributeItems(x);
            return this;
        }

        public DungeonBuilder AddWeapons(int x = 10)
        {
            dungeon.DistributeWeapons(x);
            return this;
        }

        public DungeonBuilder AddModifiedWeapons(int x = 10)
        {
            dungeon.DistributeModifiedWeapons(x);
            return this;
        }

        public DungeonBuilder AddPotions()
        {
            dungeon.DistributePotions();
            return this;
        }

        public DungeonBuilder AddEnemies()
        {
            dungeon.DistributeEnemies();
            return this;
        }
        public Dungeon Build()
        {
            return dungeon;
        }
    }
}
