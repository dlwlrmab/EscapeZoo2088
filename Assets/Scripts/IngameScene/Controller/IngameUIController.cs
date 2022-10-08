using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameUIController : MonoBehaviour
{
    [SerializeField] Text _roundText;
    [SerializeField] Text _competeText;
    [SerializeField] Image _myRoundBar;
    [SerializeField] Image _enemyRoundBar;
    [SerializeField] Text _retryText;

    private int _saveRoundIndex = -1;
    private int _retryCount = 1;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void LoadRound()
    {
        gameObject.SetActive(false);
    }

    public void StartRound()
    {
        gameObject.SetActive(true);

        _roundText.text = "Round " + (GlobalData.roundIndex + 1) + " / " + GlobalData.roundMax;
        SetRoundBar();
        SetRetry();
    }

    public void ClearGame()
    {
        gameObject.SetActive(false);
    }

    public void SetRoundBar()
    {
        _myRoundBar.fillAmount = Mathf.Lerp(0f, 1f, (float)GlobalData.roundIndex / GlobalData.roundMax);
        _enemyRoundBar.fillAmount = Mathf.Lerp(0f, 1f, (float)GlobalData.enemyRoundIndex / GlobalData.roundMax);

        if (GlobalData.roundIndex < GlobalData.enemyRoundIndex)
            _competeText.text = "조금만 더 힘내세요! 썯";
        else if (GlobalData.roundIndex > GlobalData.enemyRoundIndex)
            _competeText.text = "잘하고 있군요. 낲";
        else
            _competeText.text = "";
    }

    private void SetRetry()
    {
        if (_saveRoundIndex == GlobalData.roundIndex)
            _retryCount++;
        else
            _saveRoundIndex = GlobalData.roundIndex;

        _retryText.text = _retryCount + "번째 도전 중...";
    }
}
