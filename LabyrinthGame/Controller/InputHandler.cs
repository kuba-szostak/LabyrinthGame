using LabyrinthGame.Model;
using LabyrinthGame.Model.Interfaces;
using LabyrinthGame.Model.Items.Potions;
using LabyrinthGame.View;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabyrinthGame.Controller
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

    public class MovementHandler : InputHandler
    {
        public override void HandleInput(ConsoleKey key, Player player)
        {
            if (key == ConsoleKey.W || key == ConsoleKey.S || key == ConsoleKey.A || key == ConsoleKey.D)
            {
                try
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
                catch
                {
                    DisplayManager.Instance.DisplayInvalidInput();
                }
            }
            else
            {
                base.HandleInput(key, player);
            }
        }
    }

    public class PickupHandler : InputHandler
    {
        public override void HandleInput(ConsoleKey key, Player player)
        {
            if (key == ConsoleKey.E)
            {
                try
                {
                    player.PickUpItem();
                }
                catch
                {
                    DisplayManager.Instance.DisplayInvalidInput();
                }
            }
            else
            {
                base.HandleInput(key, player);
            }
        }
    }
    public class DropHandler : InputHandler
    {
        public override void HandleInput(ConsoleKey key, Player player)
        {
            if (key == ConsoleKey.Q)
            {
                try
                {
                    int index = InputHelpers.SelectItemToDrop(player);
                    player.DropItem(index);
                    DisplayManager.Instance.DisplayConsoleClear();
                }
                catch
                {
                    DisplayManager.Instance.DisplayInvalidInput();
                }
            }
            else
            {
                base.HandleInput(key, player);
            }
        }
    }

    public class DropAllHandler : InputHandler
    {
        public override void HandleInput(ConsoleKey key, Player player)
        {
            if (key == ConsoleKey.G)
            {
                try
                {
                    player.DropAllItems();
                    DisplayManager.Instance.DisplayConsoleClear();
                }
                catch
                {
                    DisplayManager.Instance.DisplayInvalidInput();
                }
            }
            else
            {
                base.HandleInput(key, player);
            }
        }
    }

    public class InstructionsHandler : InputHandler
    {
        private readonly GameInstructions instructions;

        public InstructionsHandler(GameInstructions instructions)
        {
            this.instructions = instructions;
        }

        public override void HandleInput(ConsoleKey key, Player player)
        {
            if (key == ConsoleKey.I)
            {
                DisplayManager.Instance.DisplayInstructions(instructions);
                Console.ReadKey(true);
            }
            else
            {
                base.HandleInput(key, player);
            }
        }
    }

    public class RightHandHandler : InputHandler
    {
        public override void HandleInput(ConsoleKey key, Player player)
        {
            if (key == ConsoleKey.R)
            {
                try
                {
                    int index = InputHelpers.SelectItemToEquip(player, player.Inventory.OfType<IWeapon>().ToList(), false);
                    player.EquipFromInventory(false, index);
                    DisplayManager.Instance.DisplayConsoleClear();
                }
                catch
                {
                    DisplayManager.Instance.DisplayInvalidInput();
                }
            }
            else
            {
                base.HandleInput(key, player);
            }
        }
    }

    public class LeftHandHandler : InputHandler
    {
        public override void HandleInput(ConsoleKey key, Player player)
        {
            if (key == ConsoleKey.L)
            {
                try
                {
                    int index = InputHelpers.SelectItemToEquip(player, player.Inventory.OfType<IWeapon>().ToList(), true);
                    player.EquipFromInventory(true, index);
                    DisplayManager.Instance.DisplayConsoleClear();
                }
                catch
                {
                    DisplayManager.Instance.DisplayInvalidInput();
                }

            }
            else
            {
                base.HandleInput(key, player);
            }
        }
    }

    public class NormalAttackHandler : InputHandler
    {
        public override void HandleInput(ConsoleKey key, Player player)
        {
            if (key == ConsoleKey.Z)
            {
                try
                {
                    player.Attack(AttackType.Normal);
                }
                catch
                {
                    DisplayManager.Instance.DisplayInvalidInput();
                }

            }
            else
            {
                base.HandleInput(key, player);
            }
        }
    }
    public class StealthAttackHandler : InputHandler
    {
        public override void HandleInput(ConsoleKey key, Player player)
        {
            if (key == ConsoleKey.X)
            {
                try
                {
                    player.Attack(AttackType.Stealth);
                }
                catch
                {
                    DisplayManager.Instance.DisplayInvalidInput();
                }
            }
            else
            {
                base.HandleInput(key, player);
            }
        }
    }
    public class MagicAttackHandler : InputHandler
    {
        public override void HandleInput(ConsoleKey key, Player player)
        {
            if (key == ConsoleKey.C)
            {
                try
                {
                    player.Attack(AttackType.Magic);
                }
                catch
                {
                    DisplayManager.Instance.DisplayInvalidInput();
                }
            }
            else
            {
                base.HandleInput(key, player);
            }
        }
    }

    public class PotionHandler : InputHandler
    {
        public override void HandleInput(ConsoleKey key, Player player)
        {
            if (key == ConsoleKey.P)
            {
                try
                {
                    int index = InputHelpers.SelectPotionToDrink(player,
                        player.Inventory.OfType<Potion>().ToList());
                    player.DrinkPotion(index);
                    DisplayManager.Instance.DisplayConsoleClear();
                }
                catch
                {
                    DisplayManager.Instance.DisplayInvalidInput();
                }
            }
            else
            {
                base.HandleInput(key, player);
            }
        }
    }

    public class ExitHandler : InputHandler
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
            DisplayManager.Instance.DisplayInvalidInput();
        }
    }

    public class InputHandlerFactory
    {
        public static InputHandler CreateInputHandlerChain(GameInstructions instructions, Dungeon dungeon)
        {
            List<InputHandler> inputHandlers = new List<InputHandler>();

            if (instructions.ContainsInstruction("movement"))
            {
                InputHandler wsadHandler = new MovementHandler();
                inputHandlers.Add(wsadHandler);
            }
            if (instructions.ContainsInstruction("pickup"))
            {
                InputHandler eHandler = new PickupHandler();
                inputHandlers.Add(eHandler);

                InputHandler gHandler = new DropAllHandler();
                inputHandlers.Add(gHandler);

                InputHandler qHandler = new DropHandler();
                inputHandlers.Add(qHandler);

            }
            if (instructions.ContainsInstruction("equip"))
            {
                InputHandler lHandler = new LeftHandHandler();
                inputHandlers.Add(lHandler);

                InputHandler rHandler = new RightHandHandler();
                inputHandlers.Add(rHandler);

                InputHandler zHandler = new NormalAttackHandler();
                inputHandlers.Add(zHandler);
                InputHandler xHandler = new StealthAttackHandler();
                inputHandlers.Add(xHandler);
                InputHandler cHandler = new MagicAttackHandler();
                inputHandlers.Add(cHandler);
            }
            if (instructions.ContainsInstruction("potion"))
            {
                InputHandler pHandler = new PotionHandler();
                inputHandlers.Add(pHandler);
            }


            InputHandler iHandler = new InstructionsHandler(instructions);
            inputHandlers.Add(iHandler);

            InputHandler escHandler = new ExitHandler();
            inputHandlers.Add(escHandler);

            InputHandler defaultHandler = new DefaultHandler(dungeon);
            inputHandlers.Add(defaultHandler);

            for (int i = 0; i < inputHandlers.Count() - 1; i++)
            {
                inputHandlers[i].SetNext(inputHandlers[i + 1]);
            }

            return inputHandlers.First();
        }
    }
}




