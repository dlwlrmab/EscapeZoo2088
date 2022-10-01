using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumDef;

public class IngameMapController : MonoBehaviour
{
    private List<Round> _roundList;
    private int _roundIndex = 0;

    private bool _isCreateComplete = false;
    public bool CreateComplete { get { return _isCreateComplete; } }

    public void CreateMapAndRound(int[] roundList)
    {
        int mapIndex = GlobalData.mapIndex;
        if (mapIndex == -1)
            mapIndex = 1;

        // 맵 생성
        GameObject map = Instantiate(Resources.Load<GameObject>("Map/Map_" + mapIndex), transform);
        map.transform.localPosition = new Vector3(0, 0, 2);

        // 라운드 생성
        _roundList = new List<Round>();
        foreach (int roundIndex in roundList)
        {
            GameObject round = Instantiate(Resources.Load<GameObject>("Round/Round_" + "1"), transform);
            _roundList.Add(round.GetComponent<Round>());
            _roundList[_roundList.Count - 1].SetMap(mapIndex);
        }

        _isCreateComplete = true;
    }

    public void LoadRound(int nextRound)
    {
        _roundIndex = nextRound;

        foreach (var round in _roundList)
            round.gameObject.SetActive(false);
        _roundList[nextRound].gameObject.SetActive(true);
    }

    public void StartRound()
    {
        _roundList[_roundIndex].StartRound();
    }

    public string GetExplanation()
    {
        return _roundList[_roundIndex].GetExplanation();
    }

    public Vector3 GetPlayerSpawn()
    {
        return _roundList[_roundIndex].GetPlayerSpawn();
    }

    public ROUNDTYPE GetCurrentRoundType()
    {
        return _roundList[_roundIndex].GetRoundType(); 
    }
}
