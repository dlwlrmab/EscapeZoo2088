using EuNet.Rpc;
using System.Threading.Tasks;

namespace Common
{
    public interface IActorViewRpc : IViewRpc
    {
        Task OnSetMoveVelocity(float x, bool jump);
    }
}
