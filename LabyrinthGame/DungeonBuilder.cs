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
        private GameInstructions gameInstructions = new GameInstructions();

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

        public DungeonBuilder AddPaths(int x)
        {
            dungeon.AddRandomPaths(x);
            gameInstructions.AddInstruction("movement", "W / A / S / D: Move your character");
            return this;
        }

        public DungeonBuilder AddChambers(int x)
        {
            dungeon.AddChambers(x);
            gameInstructions.AddInstruction("movement", "W / A / S / D: Move your character");
            return this;
        }

        public DungeonBuilder AddCentralRoom()
        {
            dungeon.AddCentralRoom();
            gameInstructions.AddInstruction("movement", "W / A / S / D: Move your character");
            return this;
        }

        public DungeonBuilder AddItems(int x = 10)
        {
            dungeon.DistributeItems(x);
            gameInstructions.AddInstruction("pickup", "Press E: Pick up an item");
            gameInstructions.AddInstruction("drop", "Press Q: Drop an item");
            return this;
        }

        public DungeonBuilder AddWeapons(int x = 10)
        {
            dungeon.DistributeWeapons(x);
            gameInstructions.AddInstruction("pickup", "Press E: Pick up an item");
            gameInstructions.AddInstruction("drop", "Press Q: Drop an item");
            gameInstructions.AddInstruction("equip", "Press L/R: Equip a weapon");
            return this;
        }

        public DungeonBuilder AddModifiedWeapons(int x = 10)
        {
            dungeon.DistributeModifiedWeapons(x);
            gameInstructions.AddInstruction("pickup", "Press E: Pick up an item");
            gameInstructions.AddInstruction("drop", "Press Q: Drop an item");
            gameInstructions.AddInstruction("equip", "Press L/R: Equip a weapon");
            return this;
        }

        public DungeonBuilder AddPotions(int x = 10)
        {
            dungeon.DistributePotions(x);
            gameInstructions.AddInstruction("pickup", "Press E: Pick up an item");
            gameInstructions.AddInstruction("drop", "Press Q: Drop an item");
            gameInstructions.AddInstruction("potion", "Press P: Drink a potion");
            return this;
        }

        public DungeonBuilder AddEnemies(int x)
        {
            dungeon.DistributeEnemies(x);
            gameInstructions.AddInstruction("enemy", "Be cautious! Enemies are present.");
            return this;
        }
        public Dungeon Build()
        {
            return dungeon;
        }

        public GameInstructions GetInstructions()
        {
            return gameInstructions;
        }
    }
}
