using Common;
using EuNet.Core;
using EuNet.Unity;
using System.Threading.Tasks;
using UnityEngine;

public class Actor : MonoBehaviour, INetViewHandler, INetSerializable, INetViewPeriodicSync, IActorViewRpc
{
    //[SerializeField] private Renderer _renderer;

    private NetView _view;
    public NetView View => _view;
    private ActorViewRpc _actorRpc;

    private Vector2? _netSyncPosition;
    private Vector2? _netSyncVelocity;

    public Rigidbody2D _rb;
    public bool IsGrounded = false;
    public bool IsLongJump = false;
    public float jumpHeight = 1f;
    public float defualtJumpHeight = 0;
    public float moveSpeed = 2f;
    Vector2 velocity;

    private void Awake()
    {
        _view = GetComponent<NetView>();
        _actorRpc = new ActorViewRpc(_view);
        _rb = GetComponent<Rigidbody2D>();

        if(defualtJumpHeight == 0)
            defualtJumpHeight = jumpHeight;
    }

    private void Start()
    {
        ActorManager.Instance.Add(this);
    }

    private void OnDestroy()
    {
        ActorManager.Instance?.Remove(this);
    }

    private void FixedUpdate()
    {
        if (_netSyncPosition.HasValue && _netSyncVelocity.HasValue)
        {
            // 네트워크 위치와 동기화를 하자
            var diff = _netSyncPosition - _rb.position;
            Vector2 moveDistance = diff.Value * 0.1f;
            if (moveDistance.magnitude > 0.1f)
            {
                moveDistance = moveDistance.normalized * 0.1f;
                _rb.MovePosition(_rb.position+moveDistance);
            }
            else
            {
                _rb.MovePosition(_netSyncPosition.Value);
            }
            Debug.Log(this.name + _rb.position);
        }
    }

    public void SetMoveVelocity(float x, bool jump)
    {
        _actorRpc
            .ToOthers(DeliveryMethod.Unreliable)
            .OnSetMoveVelocity(x, jump);

        OnSetMoveVelocity(x, jump);
    }

    public Task OnSetMoveVelocity(float x, bool jump)
    {
        velocity = _rb.velocity;
        velocity.x = x * moveSpeed;
        if (jump)
            velocity.y += Mathf.Sqrt(2f * -Physics2D.gravity.y * jumpHeight);
        _rb.velocity = velocity;
        return Task.CompletedTask;
    }


    public void SetJumpHeight(float height)
    {
        if (height != 0)
            jumpHeight = height;
        else
            jumpHeight = defualtJumpHeight;
    }

    public void OnViewInstantiate(NetDataReader reader)
    {
        //_renderer.material.color = reader.ReadColor();
    }

    public void OnViewDestroy(NetDataReader reader)
    {

    }

    public void OnViewMessage(NetDataReader reader)
    {
        throw new System.NotImplementedException();
    }

    public bool OnViewPeriodicSyncSerialize(NetDataWriter writer)
    {
        writer.Write(_rb.velocity);
        writer.Write(_rb.position);
        return true;
    }

    public void OnViewPeriodicSyncDeserialize(NetDataReader reader)
    {
        _netSyncVelocity = reader.ReadVector2();
        _netSyncPosition = reader.ReadVector2();
    }

    public void Serialize(NetDataWriter writer)
    {
        //writer.Write(_renderer.material.color);
    }

    public void Deserialize(NetDataReader reader)
    {
        //_renderer.material.color = reader.ReadColor();
    }

    #region ##### Ground Check Trigger #####
    // Ground Check 외의 다른 기능은 다른 스크립트에 작성요망
    private void OnTriggerStay2D(Collider2D other)
    {
        int layer = other.gameObject.layer;
        if (layer == LayerMask.NameToLayer("Ground") || layer == LayerMask.NameToLayer("Ground_Move") || layer == LayerMask.NameToLayer("Player"))
        {
            if (!IsGrounded)
                IsGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        int layer = other.gameObject.layer;
        if (layer == LayerMask.NameToLayer("Ground") || layer == LayerMask.NameToLayer("Ground_Move") || layer == LayerMask.NameToLayer("Player"))
        {
            IsGrounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        int layer = other.gameObject.layer;
        if (layer == LayerMask.NameToLayer("Ground") || layer == LayerMask.NameToLayer("Ground_Move") || layer == LayerMask.NameToLayer("Player"))
        {
            IsLongJump = false;
        }
    }
    #endregion
}
