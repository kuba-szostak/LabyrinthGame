using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabyrinthGame
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

    public class I_Handler : InputHandler
    {
        public override void HandleInput(ConsoleKey key, Player player)
        {
            if (key == ConsoleKey.I)
            {
                DisplayManager.Instance.DisplayInstructions();
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

    public class InputHandlerFactory
    {
        public static InputHandler CreateInputHandlerChain()
        {
            InputHandler wsadHandler = new WSAD_Handler();
            InputHandler eHandler = new E_Handler();
            InputHandler qHandler = new Q_Handler();
            InputHandler iHandler = new I_Handler();
            InputHandler rHandler = new R_Handler();
            InputHandler lHandler = new L_Handler();
            InputHandler escHandler = new ESC_Handler();
            wsadHandler.SetNext(eHandler);
            eHandler.SetNext(qHandler);
            qHandler.SetNext(iHandler);
            iHandler.SetNext(rHandler);
            rHandler.SetNext(lHandler);
            lHandler.SetNext(escHandler);
            return wsadHandler;
        }
    }
}




