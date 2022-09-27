using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Connectors
{
    public static class ProtocolProcessGenerator
    {
        public static ProtocolProcess.BaseProtocol GetProcess(BattleProtocol.MessageType type)
        {
            switch (type)
            {
                case BattleProtocol.MessageType.BattleEnterResult:
                    return new ProtocolProcess.BattleEnterResult();
                case BattleProtocol.MessageType.BattleResult:
                    return new ProtocolProcess.BattleResult();
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}
