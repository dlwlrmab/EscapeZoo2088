using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class RoundObjButton : MonoBehaviour
{
    [SerializeField] protected RoundObjDead _linkeDeadObj; // 버튼을 눌렀을 때 없어지는 죽음 오브젝트
    [SerializeField] protected GameObject _linkedAppearObj; // 버튼을 눌렀을 때 나타나는 오브젝트

    protected Action _action;

    public virtual void StartRound()
    {
        gameObject.SetActive(true);
        if (_linkeDeadObj != null)
            _linkeDeadObj.gameObject.SetActive(true);
        if (_linkedAppearObj != null)
            _linkedAppearObj.gameObject.SetActive(false);

        SetAction();
    }

    protected virtual void SetAction()
    {
        _action = () =>
        {
            gameObject.SetActive(false);
            if (_linkeDeadObj != null)
                _linkeDeadObj.gameObject.SetActive(false);
            if (_linkedAppearObj != null)
                _linkedAppearObj.gameObject.SetActive(true);
        };
    }

    protected virtual void OnTriggerStay2D(Collider2D other)
    {
        int layer = other.gameObject.layer;
        if (layer == LayerMask.NameToLayer("Player"))
        {
            _action?.Invoke();
        }
    }
}