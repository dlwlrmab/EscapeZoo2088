using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnumDef;

public class IngameEndingController : MonoBehaviour
{
    [SerializeField] GameObject _ending;
    [SerializeField] Text _endingNotice;
    [SerializeField] Text _endingScore;
    [SerializeField] SpriteRenderer[] _players;

    private Sprite[] _animalSprites;

    private void Start()
    {
        _ending.SetActive(false);
        _animalSprites = Resources.LoadAll<Sprite>("Sprites/Animal");
    }

    public void LoadEnding(int score)
    {
        _ending.SetActive(true);
        _endingNotice.text = IngameScene.Instance.IsWinner ? "VICTORY" : "DEFEAT";
        _endingScore.text = $"FINAL SCORE : {score.ToString()}";

        List<PlayerInfo> playerInfos = GlobalData.playerInfos;
        for (int i = 0; i < _players.Length; ++i)
            _players[i].sprite = GetSprite(playerInfos[i].Animal);
    }

    private Sprite GetSprite(ANIMAL animal)
    {
        foreach (Sprite sprite in _animalSprites)
            if (sprite.name.Contains(animal.ToString().ToLower()))
                return sprite;
        return null;
    }

    public void OnClickLobby()
    {
        IngameScene.Instance.MoveLobbyScene();
    }
}