using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class ButtonObj : MonoBehaviour
{
    [SerializeField] protected RoundDead _linkedObj; // 버튼을 눌렀을때 영향을 받는 obj

    protected Action _action; // 버튼을 눌렀을때 진행될 동작 

    public virtual void Init()
    {
        gameObject.SetActive(true);
        _linkedObj.gameObject.SetActive(true);
        SetAction();
    }

    public void SetData(Round round)
    {
        _linkedObj.SetRound(round);
    }

    protected virtual void SetAction()
    {

    }

    protected virtual void OnTriggerStay2D(Collider2D other)
    {
        int layer = other.gameObject.layer;
        if (layer == LayerMask.NameToLayer("Player"))
        {
            if (_linkedObj != null && gameObject.activeSelf)
            {
                _action?.Invoke();
            }
        }
    }

}
