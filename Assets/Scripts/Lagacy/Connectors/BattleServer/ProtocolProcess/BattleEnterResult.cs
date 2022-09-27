using Assets.Scripts;
using BattleProtocol.Entities;

namespace ProtocolProcess
{
    public sealed class BattleEnterResult : BaseProtocol
    {
        public override void OnResponse(BattleProtocol.BaseProtocol protocol)
        {
            var proto = protocol as ProtoBattleEnterResult;
            if (proto.ResultType == CommonType.ResultType.Success)
            {
                GameManager.Instance.MatchingSuccess(proto.PlayerType, proto.GameStartDateTimeTicks, proto.GameSecond);
            }
        }
    }
}
