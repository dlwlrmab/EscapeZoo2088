using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumDef;

public class PlayerInput : MonoBehaviour
{
    float _jumpHeight = 0f;
    bool _pressJump = false;

    IngameScene _ingameScene;

    private void Awake()
    {
        _ingameScene = IngameScene.Instance;
    }

    public static ROUNDTYPE _type;
    private void Update()
    {
        if (_ingameScene.State == INGAME_STATE.LOADING)
            return;

        var actor = P2PInGameManager.Instance.ControlActor;

        if (actor == null)
            return;
        if (_type != EnumDef.ROUNDTYPE.LONGJUMP)
        {
            float x = Input.GetAxisRaw("Horizontal");
            bool jump = false;
            if (Input.GetKeyDown(KeyCode.Space))
            {   // 점프 가능 여부는 각자 단말에서 판단함
                if (actor.IsGrounded)
                    jump = true;
            }

            actor.SetMoveVelocity(x, jump);
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Z) || (Input.GetKeyDown(KeyCode.C)))
            {   
                // 점프 가능 여부는 각자 단말에서 판단함
                if (actor.IsGrounded)
                    _pressJump = true;
            }

            if (_pressJump)
            {
                if(_jumpHeight <= 2f)
                _jumpHeight += 0.01f;
            }

            if ( Input.GetKeyUp(KeyCode.Z) || Input.GetKeyUp(KeyCode.C) )
            {
                actor.SetJumpHeight(_jumpHeight);
                _jumpHeight  = 0f;
                _pressJump = false;

                actor.IsLongJump = true;

                if (Input.GetKeyUp(KeyCode.Z))
                {
                    if (actor.IsGrounded)
                    {
                        actor.SetMoveVelocity(-1, true);
                    }
                }
                else
                {
                    if (actor.IsGrounded)
                    {
                        actor.SetMoveVelocity(1, true);
                    }
                }
            }

            if(!actor.IsLongJump )
            {
                actor.SetMoveVelocity(0, false);
            }

        }

    }


}
