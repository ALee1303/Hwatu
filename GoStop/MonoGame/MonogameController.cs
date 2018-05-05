using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

using MG_Library;

namespace GoStop
{
    internal class MonogameController
    {
        private InputManager inputManager;

        public MonogameController()
        {
            inputManager = new InputManager();
        }
    }
}
