using Assets.Scripts;
using BattleProtocol.Entities;

namespace ProtocolProcess
{
    public sealed class BattleResult : BaseProtocol
    {
        public override void OnResponse(BattleProtocol.BaseProtocol protocol)
        {
            var proto = protocol as ProtoBattleResult;
            GameManager.Instance.BattleEnd(proto);
        }
    }
}
