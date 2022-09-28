using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 대체로 무난하지만 벽붙기를 사용할 수 있음 (벽을 계속 밀면 아래로 안떨어지고 버틸 수 있음)

    [SerializeField]
    float speed;
    [SerializeField]
    float jumpHeight;

    Rigidbody2D rigidbody;
    float inputDirection;

    bool jump = false;
    bool isGrounded = true;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        inputDirection = Input.GetAxis("Horizontal");
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(isGrounded)
                jump = true;
        }
    }

    private void FixedUpdate()
    {
        float jumpSpeed = rigidbody.velocity.y;
        if (jump)
        {
            jumpSpeed += Mathf.Sqrt(2f * -Physics2D.gravity.y * jumpHeight);
            jump = false;
        }
        
        rigidbody.velocity = new Vector2(inputDirection * speed, jumpSpeed);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("TriggerEnter: " + collision.name);
        isGrounded = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("TriggerExit: " + other.name);
        isGrounded = false;
    }
}
