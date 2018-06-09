
namespace Hwatu
{
    public interface IMainPlayer : IHanafudaPlayer
    {
        MonoGameComponents.HanafudaController Controller { get; }
        
    }
}
