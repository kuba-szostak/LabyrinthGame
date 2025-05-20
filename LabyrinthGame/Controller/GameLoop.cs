using LabyrinthGame.Model;
using LabyrinthGame.View;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace LabyrinthGame.Controller
{
    public class GameLoop
    {

        private (Player player, Dungeon dungeon, GameInstructions instructions, InputHandler chain) Initialize()
        {
            var builder = new DungeonBuilder(40, 20)
               .BuildEmptyDungeon()
               .BuildFilledDungeon()
               .AddPaths(20)
               .AddChambers(5)
               .AddCentralRoom()
               .AddItems(10)
               .AddWeapons(10)
               .AddModifiedWeapons(10)
               .AddPotions(30)
               .AddEnemies(3);

            Dungeon dungeon = builder.Build();
            GameInstructions instructions = builder.GetInstructions();

            Point position = new Point(20, 10);
            Player player = new Player(position, dungeon);

            InputHandler chain = InputHandlerFactory.CreateInputHandlerChain(instructions, dungeon);

            return (player, dungeon, instructions, chain);
        }
        public void Run()
        {
            
            var (player, dungeon, instructions, chain) = Initialize();

            while (!player.IsDead)
            {
                
                DisplayManager.Instance.RenderFrame(player, dungeon);

                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true);

                    chain.HandleInput(key.Key, player);

                    while (Console.KeyAvailable)
                        _ = Console.ReadKey(true);
                }

                // player.UpdateEffects();
            }

            DisplayManager.Instance.DisplayConsoleClear();
            DisplayManager.Instance.DisplayGameOver();
            Console.ReadKey(true);

            Environment.Exit(0);
        }
    }
}
