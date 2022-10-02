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

    [SerializeField] bool _isReverse = true;  // true = 왼쪽에서 오른족

    private Transform _parentObj;

    private void Awake()
    {
        _parentObj = transform.parent;
    }
    public void Init()
    {
        if(_parentObj != null)
            transform.SetParent(_parentObj);

        transform.localPosition = new Vector3(_startPosX, _startPosY, 0);
        gameObject.SetActive(true);
    }

    private void FixedUpdate()
    {
        if(_type == BLOCKTYPE.MOVEX)
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
        else if(_type == BLOCKTYPE.MOVEY)
        {
            if (!_isReverse)
            {
                transform.localPosition = new Vector3(transform.localPosition.x , transform.localPosition.y + 0.1f * _speed, 0);
                if (transform.localPosition.y >= _destPosY)
                    _isReverse = true;
            }
            else
            {
                transform.localPosition = new Vector3(transform.localPosition.x , transform.localPosition.y + -0.1f * _speed, 0);
                if (transform.localPosition.y <= _startPosY)
                    _isReverse = false;
            }
        }
        else if(_type == BLOCKTYPE.BALL)
        {
            
            if (_isReverse)
            {
                transform.localPosition = new Vector3(transform.localPosition.x + 0.1f * _speed, transform.localPosition.y, 0);
                if (transform.localPosition.x  > _destPosX)
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

    public BLOCKTYPE GetObjectType()
    {
        return _type;
    }
}
