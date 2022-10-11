using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumDef;

public class IngameMapController : MonoBehaviour
{
    private List<Round> _roundList;

    private bool _isCreateComplete = false;
    public bool CreateComplete { get { return _isCreateComplete; } }

    public void CreateMapAndRound()
    {
        // 맵 생성
        GameObject map = Instantiate(Resources.Load<GameObject>("Prefabs/Map/Map_" + GlobalData.map.ToString().ToLower()), transform);
        map.transform.localPosition = new Vector3(0, 0, 2);

        // 라운드 생성
        _roundList = new List<Round>();
        foreach (int roundIndex in GlobalData.roundList)
        {
            GameObject round = Instantiate(Resources.Load<GameObject>("Prefabs/Round/Round_" + roundIndex), transform);
            _roundList.Add(round.GetComponent<Round>());
            _roundList[_roundList.Count - 1].CreateRound();
        }

        _isCreateComplete = true;
    }

    public void LoadRound()
    {
        foreach (var round in _roundList)
            round.gameObject.SetActive(false);

        _roundList[GlobalData.roundIndex].LoadRound();
    }

    public void StartRound()
    {
        _roundList[GlobalData.roundIndex].StartRound();
    }

    #region Get

    public Round GetCurrentRound()
    {
        return _roundList[GlobalData.roundIndex];
    }

    public string GetExplanation()
    {
        return GetCurrentRound().GetExplanation();
    }

    public List<Vector3> GetPlayerSpawn()
    {
        return GetCurrentRound().GetPlayerSpawn();
    }

    #endregion
}
