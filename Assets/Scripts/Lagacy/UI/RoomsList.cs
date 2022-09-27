using CommonProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsList : Singleton<RoomsList>
{

    public GameObject RoomItemPrefab;
    public List<GameObject> RoomItems = new List<GameObject>();

    public void SetValues(ResSearchRoom resSearchRoom)
    {
        var parent = GameObject.Find("Content").GetComponent<Transform>();

        DestroyBeforeInfo();
        parent.DetachChildren();

        Debug.Log(resSearchRoom.Rooms.Count == 0 ? "검색된 게임이 없습니다." : "GameCount:" + resSearchRoom.Rooms.Count);
        foreach (var roomInfo in resSearchRoom.Rooms)
        {
            var child = Instantiate(RoomItemPrefab, parent);
            child.GetComponent<RoomItem>().SetValue(roomInfo);
            RoomItems.Add(child);
        }
    }

    private void DestroyBeforeInfo()
    {
        foreach (var beforeRoomInfo in RoomItems)
        {
            Destroy(beforeRoomInfo);
        }
    }


}
