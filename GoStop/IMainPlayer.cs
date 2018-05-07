using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStop
{
    public interface IMainPlayer
    {
        MonoGameComponents.HanafudaController Controller { get; }

        Task TakeTurnAsync();
    }
}
