using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundObjFireButton : RoundObjButton
{
    [SerializeField] Transform _offFireObj;

    public override void StartRound()
    {
        base.StartRound();

        CancelInvoke("InvokeResetFire");
        InvokeEnableFire();
    }

    protected override void SetAction()
    {
        _action = () =>
        {
            DisableFire();
            Invoke("InvokeEnableFire", 3f);
        };
    }

    private void DisableFire()
    {
        _linkeDeadObj.gameObject.SetActive(false);
        gameObject.SetActive(false);
        _offFireObj.gameObject.SetActive(true);
    }

    private void InvokeEnableFire()
    {
        _linkeDeadObj.gameObject.SetActive(true);
        gameObject.SetActive(true);
        _offFireObj.gameObject.SetActive(false);
    }
}
