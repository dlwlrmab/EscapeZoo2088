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
                transform.localPosition += Vector3.right * Time.deltaTime * _speed;
                transform.localPosition = new Vector3(transform.localPosition.x + 0.1f * _speed, transform.localPosition.y, 0);
                if (transform.localPosition.x >= _destPosX)
                    _isReverse = true;
            }
            else
            {
                transform.localPosition += Vector3.left * Time.deltaTime * _speed;
                if (transform.localPosition.x <= _startPosX)
                    _isReverse = false;
            }
        }
        else if (_type == BLOCKTYPE.MOVEY)
        {
            if (!_isReverse)
            {
                transform.localPosition += Vector3.up * Time.deltaTime * _speed;
                if (transform.localPosition.y >= _destPosY)
                    _isReverse = true;
            }
            else
            {
                transform.localPosition += Vector3.down * Time.deltaTime * _speed;
                if (transform.localPosition.y <= _startPosY)
                    _isReverse = false;
            }
        }
        else if (_type == BLOCKTYPE.BALL)
        {
            transform.Rotate(Vector3.forward, Time.deltaTime * _speed * 3);

            if (_isReverse)
            {
                transform.localPosition += Vector3.right * Time.deltaTime * _speed;
                if (transform.localPosition.x > _destPosX)
                    transform.localPosition = new Vector3(_startPosX, transform.localPosition.y, 0);
            }
            else
            {
                transform.localPosition += Vector3.left * Time.deltaTime * _speed;
                if (transform.localPosition.x < _destPosX)
                    transform.localPosition = new Vector3(_startPosX, transform.localPosition.y, 0);
            }
        }
    }
}