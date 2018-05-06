using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

using MG_Library;

namespace GoStop
{
    internal class MonogameController : GameComponent
    {
        private IInputManager inputManager;

        public MonogameController(Game game) : base(game)
        {
            inputManager = Game.Services.GetService<IInputManager>();
        }
    }
}
