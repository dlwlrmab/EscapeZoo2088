using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class RoundObjButton : MonoBehaviour
{
    [SerializeField] protected RoundObjDead _linkeDeadObj; // 버튼을 눌렀을때 영향을 받는 obj
    [SerializeField] protected GameObject _linkedAppearObj; // 버튼을 눌렀을때 나타나는 obj

    protected Action _action; // 버튼을 눌렀을때 진행될 동작 

    public virtual void Init()
    {
        gameObject.SetActive(true);

        if (_linkeDeadObj != null)
            _linkeDeadObj.gameObject.SetActive(true);

        SetAction();
    }

    public void SetData(Round round)
    {
        if(_linkeDeadObj != null)
            _linkeDeadObj.SetRound(round);
    }

    protected virtual void SetAction()
    {
        _action = () =>
        {
            if (_linkedAppearObj != null)
                _linkedAppearObj.SetActive(true);

            gameObject.SetActive(false);
        };
    }

    protected virtual void OnTriggerStay2D(Collider2D other)
    {
        int layer = other.gameObject.layer;
        if (layer == LayerMask.NameToLayer("Player"))
        {
            if ((_linkeDeadObj != null || _linkedAppearObj != null)&& gameObject.activeSelf)
            {
                _action?.Invoke();
            }
        }
    }

}
