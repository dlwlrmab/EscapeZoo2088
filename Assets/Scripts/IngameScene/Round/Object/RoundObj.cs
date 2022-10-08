using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumDef;

public class RoundObj : MonoBehaviour
{
    [SerializeField] BLOCKTYPE _type;
    [SerializeField] float _speed = 0;

    [SerializeField] float _startPosX;
    [SerializeField] float _destPosX;
    [SerializeField] float _startPosY;
    [SerializeField] float _destPosY;

    [SerializeField] bool _isReverse = true;  // true = 왼쪽에서 오른쪽
    [SerializeField] bool _isPlayerTake = false;  // 플레이어 탐승 여부

    private Transform _playerPerent;

    private void Awake()
    {
        _playerPerent = IngameScene.Instance.PlayerController.transform;
    }

    public void StartRound()
    {
        gameObject.SetActive(true);
        transform.localPosition = new Vector3(_startPosX, _startPosY, 0);
    }

    private void FixedUpdate()
    {
        if (_type == BLOCKTYPE.MOVEX)
        {
            if (!_isReverse)
            {
                transform.localPosition = new Vector3(transform.localPosition.x + 0.1f * _speed, transform.localPosition.y, 0);
                if (transform.localPosition.x >= _destPosX)
                    _isReverse = true;
            }
            else
            {
                transform.localPosition = new Vector3(transform.localPosition.x + -0.1f * _speed, transform.localPosition.y, 0);
                if (transform.localPosition.x <= _startPosX)
                    _isReverse = false;
            }
        }
        else if (_type == BLOCKTYPE.MOVEY)
        {
            if (!_isReverse)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 0.1f * _speed, 0);
                if (transform.localPosition.y >= _destPosY)
                    _isReverse = true;
            }
            else
            {
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + -0.1f * _speed, 0);
                if (transform.localPosition.y <= _startPosY)
                    _isReverse = false;
            }
        }
        else if (_type == BLOCKTYPE.BALL)
        {

            if (_isReverse)
            {
                transform.localPosition = new Vector3(transform.localPosition.x + 0.1f * _speed, transform.localPosition.y, 0);
                if (transform.localPosition.x > _destPosX)
                    transform.localPosition = new Vector3(_startPosX, transform.localPosition.y, 0);
            }
            else
            {
                transform.localPosition = new Vector3(transform.localPosition.x + -0.1f * _speed, transform.localPosition.y, 0);
                if (transform.localPosition.x < _destPosX)
                    transform.localPosition = new Vector3(_startPosX, transform.localPosition.y, 0);
            }

        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (_isPlayerTake == false)
            return;

        int layer = other.gameObject.layer;

        if (layer == LayerMask.NameToLayer("Player"))
        {
            other.transform.parent = transform;
        }

        if(other.gameObject.name == "GroundCheckCollider")
        {
            other.transform.parent.parent = transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (_isPlayerTake == false)
            return;

        int layer = other.gameObject.layer;
        if (layer == LayerMask.NameToLayer("Player"))
        {
            other.transform.parent = _playerPerent;
        }

        if (other.gameObject.name == "GroundCheckCollider")
        {
            other.transform.parent.parent = _playerPerent;
        }
    }
}