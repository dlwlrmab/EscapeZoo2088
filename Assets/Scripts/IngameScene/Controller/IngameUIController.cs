using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameUIController : MonoBehaviour
{
    [Header("Round")]
    [SerializeField] Text _roundText;
    [SerializeField] Text _competeText;
    [SerializeField] Image _myRoundBar;
    [SerializeField] Image _enemyRoundBar;

    [Header("Dead")]
    [SerializeField] GameObject _dead;
    [SerializeField] Image _deadAnimal;
    [SerializeField] Text _retryText;

    private int _saveRoundIndex = -1;
    private int _retryCount = 1;

    private Sprite[] _animalSprites;

    private void Awake()
    {
        gameObject.SetActive(false);
        _dead.SetActive(false);

        _animalSprites = Resources.LoadAll<Sprite>("Sprites/Animal");
    }

    public void LoadRound()
    {
        gameObject.SetActive(false);
        _dead.SetActive(false);
    }

    public void StartRound()
    {
        gameObject.SetActive(true);
        _dead.SetActive(false);

        _roundText.text = "Round " + (GlobalData.roundIndex + 1) + " / " + GlobalData.roundMax;
        SetRoundBar();
        SetRetry();
    }

    public void ShowDeadAnimal(EnumDef.ANIMAL animal)
    {
        _dead.SetActive(true);
        _deadAnimal.sprite = GetSprite(animal);
    }

    private Sprite GetSprite(EnumDef.ANIMAL animal)
    {
        foreach (Sprite sprite in _animalSprites)
            if (sprite.name.Contains(animal.ToString().ToLower()))
                return sprite;
        return null;
    }

    public void ClearGame()
    {
        gameObject.SetActive(false);
        _dead.SetActive(false);
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

    public void OnClickLobby()
    {
        IngameScene.Instance.PacketHandler.SendExitGame();
    }
}
