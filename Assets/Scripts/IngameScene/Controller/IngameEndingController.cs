using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameEndingController : MonoBehaviour
{
    [SerializeField] GameObject _win;
    [SerializeField] GameObject _lose;

    public void LoadEnding(int rank)
    {
        if (rank == 1)
        {
            _win.SetActive(true);
            _lose.SetActive(false);
        }
        else
        {
            _win.SetActive(false);
            _lose.SetActive(true);
        }
    }

    public void OnClickLobby()
    {
        IngameScene.Instance.MoveLobbyScene();
    }
}
