using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CommonProtocol;

public class PopupRanking : MonoBehaviour
{
    [SerializeField] List<Text> _teamNameList;
    [SerializeField] List<Text> _memberList;
    [SerializeField] List<Text> _scoreList;

    public void SetData(ResRanking data, bool success)
    {
        var teamList = data.teamNameList;
        var scoreList = data.scoreList;
        var memberList = data.memberList;

        if (success)
        {
            for (int i = 0; i < 10; i++)
            {
                string memberListString = "";
                if (i > teamList.Count)
                {
                    _teamNameList[i].text = "-";
                    _scoreList[i].text = "-";
                    _memberList[i].text = "-";
                }
                else
                {
                    _teamNameList[i].text = teamList[i];
                    _scoreList[i].text = scoreList[i].ToString(); ;
                    foreach (string member in memberList[i])
                    {
                        memberListString += member;
                    }

                    _memberList[i].text = memberListString;
                }
            }
        }
        else
        {
            for (int i = 0; i < 10; i++)
            {
                _teamNameList[i].text = "-";
                _scoreList[i].text = "-";
                _teamNameList[i].text = "-";
            }
        }
    }

    public void CloseRanking()
    {
        gameObject.SetActive(false);
    }
}
