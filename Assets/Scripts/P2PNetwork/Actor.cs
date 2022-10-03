using Common;
using EuNet.Core;
using EuNet.Unity;
using System.Threading.Tasks;
using UnityEngine;

public class Actor : MonoBehaviour, INetViewHandler, INetSerializable, INetViewPeriodicSync, IActorViewRpc
{
    [SerializeField] private Renderer _renderer;

    private NetView _view;
    public NetView View => _view;
    private ActorViewRpc _actorRpc;
    private float _moveDirection;
    private Vector3? _netSyncPosition;
    public float _moveSpeed = 2f;
    public Rigidbody2D _rb;
    public bool IsJump = false;
    public float jumpHeight = 1f;
    Vector2 velocity;

    private void Awake()
    {
        _view = GetComponent<NetView>();
        _actorRpc = new ActorViewRpc(_view);
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        ActorManager.Instance.Add(this);
    }

    private void OnDestroy()
    {
        ActorManager.Instance?.Remove(this);
    }

    void Update()
    {
        _moveDirection = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            IsJump = true;
        }
    }

    private void FixedUpdate()
    {
        velocity = _rb.velocity;
        var moveDelta = _moveDirection * _moveSpeed * Time.deltaTime;

        if (_netSyncPosition.HasValue)
        {/*
            // 네트워크 위치와 동기화를 하자
            _netSyncPosition += moveDelta;

            var dist = _netSyncPosition.Value - transform.localPosition;
            moveDelta = dist * Mathf.Min(Time.deltaTime * 10f, 1f);*/
        }
        if (IsJump)
        {
            velocity.y += Mathf.Sqrt(2f * 9.81f * jumpHeight);
            IsJump = false;
        }
        velocity.x = _moveDirection * _moveSpeed;
        SetMoveVelocity(velocity.x, velocity.y);
    }

    public void SetMoveVelocity(float x, float y)
    {
        _actorRpc
            .ToOthers(DeliveryMethod.Unreliable)
            .OnSetMoveVelocity(x, y);

        OnSetMoveVelocity(x, y);
    }

    public Task OnSetMoveVelocity(float x, float y)
    {
        velocity.x = x;
        velocity.y = y;
        _rb.velocity = velocity;
        return Task.CompletedTask;
    }

    public void OnViewInstantiate(NetDataReader reader)
    {
        _renderer.material.color = reader.ReadColor();
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
        writer.Write(_moveDirection);
        writer.Write(transform.localPosition);
        return true;
    }

    public void OnViewPeriodicSyncDeserialize(NetDataReader reader)
    {
        _moveDirection = reader.ReadSingle();
        _netSyncPosition = reader.ReadVector3();
    }

    public void Serialize(NetDataWriter writer)
    {
        writer.Write(_renderer.material.color);
    }

    public void Deserialize(NetDataReader reader)
    {
        _renderer.material.color = reader.ReadColor();
    }
}
