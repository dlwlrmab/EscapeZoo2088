using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumDef;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    float speed;
    [SerializeField]
    float jumpHeight;

    Rigidbody2D rb;
    public float inputDirection;

    public bool jump = false;
    bool isGrounded = true;

    float _initJump = 1f;
    float _jumpX = 0f;
    bool _jumpKeyDown = false;
    int _direction = 1;
    Transform _playerPerent;
    ROUNDTYPE _type;

    public void Init(ROUNDTYPE type)
    {
        _type = type;
        _jumpX = 0f;
        _jumpKeyDown = false;
        speed = 2f;
        jumpHeight = 1f;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _playerPerent = transform.parent;
        _initJump = jumpHeight;
    }

    // 여기서 구현하면 다른 플레이어도 내가 움직이게 됨
    void Update()
    {   // 입력만 처리
        if (_type != ROUNDTYPE.LONGJUMP)
        {
            inputDirection = Input.GetAxis("Horizontal");

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isGrounded)
                    jump = true;
            }
            var actor = P2PInGameManager.Instance.ControlActor;
            actor.SetMoveVelocity(inputDirection, jump);
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isGrounded)
                    _jumpKeyDown = true;
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                if (isGrounded)
                {
                    _jumpKeyDown = false;
                    jump = true;
                }
            }

            if (isGrounded)
            {
                if (Input.GetKeyDown(KeyCode.RightArrow))
                    _direction = 1;
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                    _direction = -1;
            }

            if (_jumpKeyDown)
            {
                if (jumpHeight < 2f)
                {
                    jumpHeight += 0.01f;
                    _jumpX += 0.03f;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        // 물리 작용 처리
        float jumpSpeed = rb.velocity.y;

        if (_type != ROUNDTYPE.LONGJUMP)
        {
            if (jump)
            {
                isGrounded = false;
                jumpSpeed += Mathf.Sqrt(2f * -Physics2D.gravity.y * jumpHeight);
                jump = false;
            }

            rb.velocity = new Vector2(inputDirection * speed, jumpSpeed);
        }
        else
        {
            if (jump)
            {
                isGrounded = false;
                jumpSpeed += Mathf.Sqrt(2f * -Physics2D.gravity.y * jumpHeight);
                jump = false;
            }

            if (!isGrounded)
                rb.velocity = new Vector2(_direction * _jumpX, jumpSpeed);
        }
    }

    #region ##### Ground Check Trigger #####

    private void OnTriggerStay2D(Collider2D other)
    {
        // 땅이나 플레이어 위에 있는 경우는 점프 가능
        int layer = other.gameObject.layer;
        if (layer == LayerMask.NameToLayer("Ground") || layer == LayerMask.NameToLayer("Ground_Move") || layer == LayerMask.NameToLayer("Player"))
        {
            if (!isGrounded)
                _jumpX = 0;

            isGrounded = true;
            rb.velocity = new Vector2(0, rb.velocity.y);

            if (layer == LayerMask.NameToLayer("Ground_Move"))
                transform.SetParent(other.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        int layer = other.gameObject.layer;
        if (layer == LayerMask.NameToLayer("Ground") || layer == LayerMask.NameToLayer("Ground_Move") || layer == LayerMask.NameToLayer("Player"))
        {
            isGrounded = false;
            transform.SetParent(_playerPerent);
        }

        jumpHeight = _initJump;
    }

    #endregion
}