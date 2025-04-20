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

        private bool hasMovement = false;
        private bool hasPickup = false;
        private bool hasPotion = false;
        private bool hasWeapon = false;
        private bool hasEnemy = false;


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
            return this;
        }

        public DungeonBuilder AddChambers(int x)
        {
            dungeon.AddChambers(x);
            hasMovement = true;
            return this;
        }

        public DungeonBuilder AddCentralRoom()
        {
            dungeon.AddCentralRoom();
            hasMovement = true;
            return this;
        }

        public DungeonBuilder AddItems(int x = 10)
        {
            dungeon.DistributeItems(x);
            hasPickup = true;
            return this;
        }

        public DungeonBuilder AddWeapons(int x = 10)
        {
            dungeon.DistributeWeapons(x);
            hasWeapon = true;
            hasPickup = true;
            return this;
        }

        public DungeonBuilder AddModifiedWeapons(int x = 10)
        {
            dungeon.DistributeModifiedWeapons(x);
            hasWeapon = true;
            hasPickup = true;
            return this;
        }

        public DungeonBuilder AddPotions(int x = 10)
        {
            dungeon.DistributePotions(x);
            hasPickup = true;
            hasPotion = true;
            return this;
        }

        public DungeonBuilder AddEnemies(int x)
        {
            dungeon.DistributeEnemies(x);
            hasEnemy = true;
            return this;
        }
        public Dungeon Build()
        {
            return dungeon;
        }

        public GameInstructions GetInstructions()
        {
            if (hasMovement)
            {
                gameInstructions.AddInstruction("movement", "W / A / S / D: Move your character");
            }
            if (hasPickup)
            {
                gameInstructions.AddInstruction("pickup", "Press E: Pick up an item\n" +
                          "Press X: Drop all items\n" +
                          "Press Q: Drop an item");
            }
            if (hasPotion)
            {
                gameInstructions.AddInstruction("potion", "Press P: Drink a potion");
            }
            if (hasWeapon)
            {
                gameInstructions.AddInstruction("equip", "Press L/R: Equip a weapon");
            }
            if (hasEnemy)
            {
                gameInstructions.AddInstruction("enemy", "Be cautious! Enemies are present.");
            }

            return gameInstructions;
        }
    }
}
