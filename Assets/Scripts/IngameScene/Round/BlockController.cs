using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
   enum BLOCKTYPE
    {
        NONE,
        MOVEX,
        MOVEY,
    }

    [SerializeField] BLOCKTYPE _type;
    [SerializeField] float _speed = 0;

    [SerializeField] float _startPosX;
    [SerializeField] float _destPosX;
    [SerializeField] float _startPosY;
    [SerializeField] float _destPosY;

    private bool _isReverse = true;

    private void Awake()
    {
   
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
    }
}
