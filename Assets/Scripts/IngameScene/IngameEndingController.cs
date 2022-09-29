using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameEndingController : MonoBehaviour
{
    [SerializeField] GameObject _ending;
    [SerializeField] GameObject _win;
    [SerializeField] GameObject _lose;

    public void OnLoadEnding()
    {
        int a = Random.Range(0, 3);

        if (a == 0)
        {
            _win.SetActive(true);
        }
        else
        {
            _lose.SetActive(true);
        }
    }
}
