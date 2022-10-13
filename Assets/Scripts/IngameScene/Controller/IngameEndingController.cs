using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnumDef;

public class IngameEndingController : MonoBehaviour
{
    [SerializeField] GameObject _ending;
    [SerializeField] Text _endingNotice;
    [SerializeField] Text[] _mbtiText;
    [SerializeField] Image[] _mbtiImage;
    [SerializeField] SpriteRenderer[] _players;

    private Sprite[] _animalSprites;

    private void Start()
    {
        _ending.SetActive(false);
        _animalSprites = Resources.LoadAll<Sprite>("Sprites/Animal");
    }

    public void LoadEnding()
    {
        _ending.SetActive(true);
        _endingNotice.text = GlobalData.isWinner ? "VICTORY" : "DEFEAT";

        for (int i = 0; i < _players.Length; ++i)
        {
            _mbtiImage[i].gameObject.SetActive(false);
            _players[i].gameObject.SetActive(false);
        }

        List<PlayerInfos> playerInfos = GlobalData.playerInfos;
        for (int i = 0; i < playerInfos.Count; ++i)
        {
            Sprite sprite = GetSprite((ANIMAL)playerInfos[i].animal);

            _mbtiText[i].text = playerInfos[i].mbti;
            _mbtiImage[i].gameObject.SetActive(true);
            _mbtiImage[i].sprite = sprite;

            _players[i].gameObject.SetActive(true);
            _players[i].sprite = sprite;
        }
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
        IngameScene.Instance.PacketHandler.SendExitGame();
    }
}