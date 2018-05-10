using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStop
{
    public interface IMainPlayer : IHanafudaPlayer
    {
        MonoGameComponents.HanafudaController Controller { get; }
        
    }
}
