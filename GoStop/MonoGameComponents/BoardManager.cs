using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using GoStop.Minhwatu;

namespace GoStop.MonoGameComponents
{
    // TODO: separate into multiple types of board for GoStop and Minhwatu
    // Currently only supports Minhwatu 1p 1v1 with CPU
    public class BoardManager : GameComponent
    {
        private IBoard vsCpuBoard;
        private IHanafudaPlayer _mainPlayer;

        public BoardManager(Game game) : base(game)
        {
            vsCpuBoard = new MinhwatuBoard();
        }

        public virtual void Start1pVsCPU()
        { }

        public virtual void MainPlayerEntered(IHanafudaPlayer player)
        {
            var mainPlayer = (MainMinhwatuPlayer)player;
            if (mainPlayer == null)
                return;
            _mainPlayer = mainPlayer;
            vsCpuBoard.SubscribePlayer(_mainPlayer);
        }

        public virtual void MainPlayerExit(IHanafudaPlayer player)
        { }

        protected virtual void board_AllPlayerRemoved()
        {

        }

        protected virtual void board_NewPlayerTurn()
        {

        }

        protected virtual void board_MultipleMatchesFound()
        { }
    }
}
