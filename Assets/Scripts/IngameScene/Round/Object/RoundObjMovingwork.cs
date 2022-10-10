using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundObjMovingwork : MonoBehaviour
{
    [SerializeField] private bool _front = false;
    [SerializeField] private Transform _autoMove;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private Transform _playerPerent;
    private float _spriteInitSizeX = 0;

    private void Awake()
    {
        _playerPerent = IngameScene.Instance.PlayerController.transform;
        _spriteInitSizeX = _spriteRenderer.size.x;
        _spriteRenderer.transform.localRotation = new Quaternion(0, _front ? 180 : 0, 0, 0);
    }

    void Update()
    {
        // 올라탄 플레이어가 없다면 움직임 멈추기
        if (_autoMove.transform.childCount == 0)
        {
            if (_autoMove.transform.localPosition.x != 0)
                _autoMove.transform.localPosition = Vector3.zero;
        }
        else
        {
            _autoMove.transform.localPosition += (_front ? Vector3.right : Vector3.left) * Time.deltaTime * 1f;
        }

        _spriteRenderer.size += Vector2.right * Time.deltaTime * 1.7f;
        if (_spriteRenderer.size.x >= _spriteInitSizeX + 0.5f)
            _spriteRenderer.size = new Vector2(_spriteInitSizeX, _spriteRenderer.size.y);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        int layer = other.gameObject.layer;
        
        if (layer == LayerMask.NameToLayer("Player"))
        {
            List<Player> playerInfos =  IngameScene.Instance.PlayerController.GetPlayerList();
            Player player  = other.GetComponent<Player>();

            if(player == playerInfos[1])
                other.transform.parent = _autoMove; 
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        int layer = other.gameObject.layer;
        if (layer == LayerMask.NameToLayer("Player"))
        {
            List<Player> playerInfos = IngameScene.Instance.PlayerController.GetPlayerList();
            Player player = other.GetComponent<Player>();

            if (player == playerInfos[1])
                other.transform.parent = _playerPerent;
        }
    }
}
