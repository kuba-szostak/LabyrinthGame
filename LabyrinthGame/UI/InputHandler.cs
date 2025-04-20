using LabyrinthGame.Core;
using LabyrinthGame.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabyrinthGame.UI
{

    public abstract class InputHandler
    {
        protected InputHandler? nextHandler;
        public void SetNext(InputHandler handler)
        {
            nextHandler = handler;
        }
        public virtual void HandleInput(ConsoleKey key, Player player)
        {
            if (nextHandler != null)
            {
                nextHandler.HandleInput(key, player);
            }
        }
    }

    public class WSAD_Handler : InputHandler
    {
        public override void HandleInput(ConsoleKey key, Player player)
        {
            if (key == ConsoleKey.W || key == ConsoleKey.S || key == ConsoleKey.A || key == ConsoleKey.D)
            {
                Point newPosition = player.position;
                switch (key)
                {
                    case ConsoleKey.W:
                        newPosition.Y--;
                        break;
                    case ConsoleKey.S:
                        newPosition.Y++;
                        break;
                    case ConsoleKey.A:
                        newPosition.X--;
                        break;
                    case ConsoleKey.D:
                        newPosition.X++;
                        break;
                }
                player.Move(newPosition);
                player.UpdateEffects();
            }
            else
            {
                base.HandleInput(key, player);
            }
        }
    }

    public class E_Handler : InputHandler
    {
        public override void HandleInput(ConsoleKey key, Player player)
        {
            if (key == ConsoleKey.E)
            {
                player.PickUpItem();
            }
            else
            {
                base.HandleInput(key, player);
            }
        }
    }
    public class Q_Handler : InputHandler
    {
        public override void HandleInput(ConsoleKey key, Player player)
        {
            if (key == ConsoleKey.Q)
            {
                player.DropItem();
            }
            else
            {
                base.HandleInput(key, player);
            }
        }
    }

    public class X_Handler : InputHandler
    {
        public override void HandleInput(ConsoleKey key, Player player)
        {
            if (key == ConsoleKey.X)
            {
                player.DropAllItems();
                DisplayManager.Instance.DisplayConsoleClear();
            }
            else
            {
                base.HandleInput(key, player);
            }
        }
    }

    public class I_Handler : InputHandler
    {
        private readonly GameInstructions instructions;

        public I_Handler(GameInstructions instructions)
        {
            this.instructions = instructions;
        }

        public override void HandleInput(ConsoleKey key, Player player)
        {
            if (key == ConsoleKey.I)
            {
                DisplayManager.Instance.DisplayInstructions(instructions);
            }
            else
            {
                base.HandleInput(key, player);
            }
        }
    }

    public class R_Handler : InputHandler
    {
        public override void HandleInput(ConsoleKey key, Player player)
        {
            if (key == ConsoleKey.R)
            {
                player.EquipFromInventory(false);
            }
            else
            {
                base.HandleInput(key, player);
            }
        }
    }

    public class L_Handler : InputHandler
    {
        public override void HandleInput(ConsoleKey key, Player player)
        {
            if (key == ConsoleKey.L)
            {
                player.EquipFromInventory(true);
            }
            else
            {
                base.HandleInput(key, player);
            }
        }
    }

    public class P_Handler : InputHandler
    {
        public override void HandleInput(ConsoleKey key, Player player)
        {
            if (key == ConsoleKey.P)
            {
                player.DrinkPotion();
            }
            else
            {
                base.HandleInput(key, player);
            }
        }
    }

    public class ESC_Handler : InputHandler
    {
        public override void HandleInput(ConsoleKey key, Player player)
        {
            if (key == ConsoleKey.Escape)
            {
                Environment.Exit(0);
            }
            else
            {
                base.HandleInput(key, player);
            }
        }
    }

    public class DefaultHandler : InputHandler
    {
        private readonly Dungeon dungeon;
        public DefaultHandler(Dungeon dungeon)
        {
            this.dungeon = dungeon;
        }
        public override void HandleInput(ConsoleKey key, Player player)
        {
            DisplayManager.Instance.DisplayInvalidInput(0, dungeon.Height + 1);
        }
    }

    public class InputHandlerFactory
    {
        public static InputHandler CreateInputHandlerChain(GameInstructions instructions, Dungeon dungeon)
        {
            List<InputHandler> inputHandlers = new List<InputHandler>();

            if (instructions.ContainsInstruction("movement"))
            {
                InputHandler wsadHandler = new WSAD_Handler();
                inputHandlers.Add(wsadHandler);
            }
            if (instructions.ContainsInstruction("pickup"))
            {
                InputHandler eHandler = new E_Handler();
                inputHandlers.Add(eHandler);

                InputHandler xHandler = new X_Handler();
                inputHandlers.Add(xHandler);

                InputHandler qHandler = new Q_Handler();
                inputHandlers.Add(qHandler);

            }
            if (instructions.ContainsInstruction("equip"))
            {
                InputHandler lHandler = new L_Handler();
                inputHandlers.Add(lHandler);

                InputHandler rHandler = new R_Handler();
                inputHandlers.Add(rHandler);
            }
            if(instructions.ContainsInstruction("potion"))
            {
                InputHandler pHandler = new P_Handler();
                inputHandlers.Add(pHandler);
            }


            InputHandler iHandler = new I_Handler(instructions);
            inputHandlers.Add(iHandler);

            InputHandler escHandler = new ESC_Handler();
            inputHandlers.Add(escHandler);

            InputHandler defaultHandler = new DefaultHandler(dungeon);
            inputHandlers.Add(defaultHandler);

            for(int i = 0; i < inputHandlers.Count() -1; i++)
            {
                inputHandlers[i].SetNext(inputHandlers[i + 1]);
            }

            return inputHandlers.First();
        }
    }
}




