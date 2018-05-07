using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using GoStop.MonoGameComponents;

namespace GoStop.Minhwatu
{
    public class MainMinhwatuPlayer : MinhwatuPlayer, IMainPlayer
    {
        private HanafudaController controller;
        public HanafudaController Controller { get => controller; }

        public MainMinhwatuPlayer(Game game) : base()
        {
            controller = new HanafudaController(game);
        }
        
        public void TakeTurn()
        {

        }

        public Task TakeTurnAsync()
        {
            return null;
        }
    }
}
