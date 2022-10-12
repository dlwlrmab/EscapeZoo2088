using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundObjMovingwork : MonoBehaviour
{
    [SerializeField] private bool _front = false;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private float _spriteInitSizeX = 0;

    private Rigidbody2D _myPlayer;

    private void Awake()
    {
        _spriteInitSizeX = _spriteRenderer.size.x;
        _spriteRenderer.transform.localRotation = new Quaternion(0, _front ? 180 : 0, 0, 0);
    }

    private void FixedUpdate()
    {
        _spriteRenderer.size += Vector2.right * Time.deltaTime * 1.7f;
        if (_spriteRenderer.size.x >= _spriteInitSizeX + 0.5f)
            _spriteRenderer.size = new Vector2(_spriteInitSizeX, _spriteRenderer.size.y);

        if (_myPlayer != null)
            _myPlayer.velocity += Vector2.left * 1.2f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        int layer = other.gameObject.layer;
        if (layer == LayerMask.NameToLayer("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player.IsMine)
                _myPlayer = other.GetComponent<Rigidbody2D>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        int layer = other.gameObject.layer;
        if (layer == LayerMask.NameToLayer("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player.IsMine)
                _myPlayer = null;
        }
    }
}