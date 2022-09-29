using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameEndingController : MonoBehaviour
{
    [SerializeField] GameObject _win;
    [SerializeField] GameObject _lose;

    public void OnLoadEnding(int rank)
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
        IngameScene.Instance.OnLoadLobbyScene();
    }

    #region Test


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            OnClickLobby();
        }
    }

    #endregion
}
