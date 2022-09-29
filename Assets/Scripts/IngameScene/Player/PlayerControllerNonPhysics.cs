using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerNonPhysics : MonoBehaviour
{
    // ****Cautions****
    // 리지드 바디는 사용하되 이동은 transform으로 적용 (충돌 감지, 중력은 사용) 
    // 벽붙기는 방지할 수 있으나 쉬버링 현상 나타남
    // Rigidbody Gravity Scale을 완전 0으로 두면 바닥에 제대로 안붙어서 0.1정도로 남겨둠 (자체 계산 중력은 불안정)
    // 케릭터 끼리 밀수 있음(리지드바디 바디타입 static으로 설정해보았으나 뚫고 지나가는 현상 발생함)

    [SerializeField]
    float speed;
    [SerializeField]
    float jumpHeight;

    Rigidbody2D rigidbody;
    float inputDirection;

    bool jump = false;
    bool isGrounded = true;

    Vector2 velocity;
    Vector2 lastPos;

    // Start is called before the first frame update
    void Start()
    {
        velocity = Vector2.zero;
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        velocity.y += Physics2D.gravity.y * Time.deltaTime; // 중력에 의한 속도 계산(음수)
        
        // 땅에 붙은 상태이면 y축 속도 0
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0;
        }

        // x축 이동 속도 계산
        inputDirection = Input.GetAxis("Horizontal");
        velocity.x = inputDirection * speed;

        // 점프
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                velocity.y += Mathf.Sqrt(2f * -Physics2D.gravity.y * jumpHeight);
            }
        }

        // 이동 거리 계산
        Vector2 moved = velocity * Time.deltaTime;

        // 물리 적용하지 않고 움직임
        transform.position = (rigidbody.position + moved);
        lastPos = transform.position;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("TriggerStay: " + collision.name);
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            isGrounded = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("TriggerExit: " + other.name);
        isGrounded = false;
    }
}
