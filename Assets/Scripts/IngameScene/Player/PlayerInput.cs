using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private void Update()
    {
        var actor = P2PInGameManager.Instance.ControlActor;
        if (actor == null)
            return;

        float x = Input.GetAxisRaw("Horizontal");
        bool jump = false;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
        }
        actor.SetMoveVelocity(x, jump);
    }
}
