using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CommonProtocol;

public class PopupMyInfo : MonoBehaviour
{
    [SerializeField] Text _id;
    [SerializeField] Text _mbti;
    [SerializeField] Text _winCnt;
    [SerializeField] Text _loseCnt;
    [SerializeField] Text _score;

    public void SetData(ResMyPage data, bool success)
    {
        if (success)
        {
            _id.text = data.userId;
            _mbti.text = data.mbti;
            _winCnt.text = data.winCnt.ToString();
            _loseCnt.text = data.lossCnt.ToString();
            _score.text = data.score.ToString();
        }
        else
        {
            _id.text = Strings.MypageFail;
            _mbti.text = Strings.MypageFail;
            _winCnt.text = Strings.MypageFail;
            _loseCnt.text = Strings.MypageFail; 
            _score.text = Strings.MypageFail;
        }
    }

    public void CloseMyInfo()
    {
        gameObject.SetActive(false);
    }
}
