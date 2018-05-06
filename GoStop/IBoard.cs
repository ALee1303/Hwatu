using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStop
{
    public interface IBoard
    {
        void StartGame();
        void EndGame();

        void AddPlayer(IHanafudaPlayer player);
        void RemovePlayer(IHanafudaPlayer player);
    }
}
