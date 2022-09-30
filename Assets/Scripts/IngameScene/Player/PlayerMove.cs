using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    float speed;
    [SerializeField]
    float jumpHeight;

    Rigidbody2D rb;
    float inputDirection;

    bool jump = false;
    public bool isGrounded = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {   // 입력만 처리
        inputDirection = Input.GetAxis("Horizontal");
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(isGrounded)
                jump = true;
        }
    }

    private void FixedUpdate()
    {   // 물리 작용 처리
        float jumpSpeed = rb.velocity.y;
        if (jump)
        {
            isGrounded = false;
            jumpSpeed += Mathf.Sqrt(2f * -Physics2D.gravity.y * jumpHeight);
            jump = false;
        }
        rb.velocity = new Vector2(inputDirection * speed, jumpSpeed);
    }

    #region ##### Ground Check Trigger #####
    private void OnTriggerEnter2D(Collider2D other)
    {   // 땅이나 플레이어 위에 있는 경우는 점프 가능
        int layer = other.gameObject.layer;
        if (layer == LayerMask.NameToLayer("Ground") || layer == LayerMask.NameToLayer("Player"))
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        int layer = other.gameObject.layer;
        if (layer == LayerMask.NameToLayer("Ground") || layer == LayerMask.NameToLayer("Player"))
        {
            isGrounded = false;
        }
    }
    #endregion
}
