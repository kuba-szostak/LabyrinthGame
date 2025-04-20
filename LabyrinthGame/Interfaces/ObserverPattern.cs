using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabyrinthGame.Interfaces
{

    public interface IObserver
    {
        void Update(Player player);

        string GetName();

        string GetRemainingTurns();
    }

    public interface ISubject
    {
        void Attach(IObserver observer);
        void Detach(IObserver observer);
        void Notify();
    }

}
