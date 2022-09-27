using Assets.Scripts;
using BattleProtocol;
using BattleProtocol.Entities;
using CommonType;
using MessagePack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    private CommonProtocol.ResRoomInfo _roomInfo;
    public void SetValue(CommonProtocol.ResRoomInfo roomInfo)
    {
        _roomInfo = roomInfo;
        gameObject.GetComponent<Text>().text = _roomInfo.Title;
    }

    public void OnClick()
    {
        Debug.Log(_roomInfo.Ip + ":" + _roomInfo.Port + ":" + _roomInfo.Title + ":" + _roomInfo.GameSessionId);

        BattleEnter();

    }

    private void BattleEnter()
    {
        //if (GameManager.Instance.IsTryMatching)
        //    return;

        //GameManager.Instance.IsTryMatching = true;

        //BattleServerConnector.Instance.Connect(_roomInfo.Ip, _roomInfo.Port, "0");

        //while (false == BattleServerConnector.Instance.IsConnected) ;

        //GameManager.Instance.TryMatching();
        //GameManager.Instance.Knight.GetComponent<KnightControl>().running();
        //GameManager.Instance.PlayerType = PlayerType.Player2;
        //GameManager.Instance.GameSessionId = _roomInfo.GameSessionId;

        //var resPlayer = WebManager.Instance.CreatePlayer(GameManager.Instance.GameSessionId);
        //if (resPlayer.ResponseType == CommonProtocol.ResponseType.Success)
        //{
        //    BattleServerConnector.Instance.Send(MessageType.BattleEnter,
        //        MessagePackSerializer.Serialize(new ProtoBattleEnter
        //        {
        //            Msg = MessageType.BattleEnter,
        //            UserId = GameManager.Instance.UserId,
        //            GameSessionId = _roomInfo.GameSessionId,
        //            PlayerSessionId = resPlayer.PlayerSessionId
        //        }));
        //}
    }


}
