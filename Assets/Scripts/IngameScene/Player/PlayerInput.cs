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
        {   // 점프 가능 여부는 각자 단말에서 판단함
            if (actor.IsGrounded)
                jump = true;
        }
        
        actor.SetMoveVelocity(x, jump);
    }
}
