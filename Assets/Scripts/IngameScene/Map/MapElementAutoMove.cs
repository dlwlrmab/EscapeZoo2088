using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapElementAutoMove : MonoBehaviour
{
    public enum DIRECTION
    {
        LEFT,
        RIGHT,
        UP,
        DOWN
    }

    [SerializeField] private DIRECTION _direction = DIRECTION.RIGHT;
    [SerializeField] private float _moveSpeed = 10;
    [SerializeField] private float _rotSpeed = 0;

    int mapWidth = 10;
    int mapHeight = 7;

    void Update()
    {
        transform.Rotate(Vector3.forward, Time.deltaTime * _rotSpeed);

        if (_direction == DIRECTION.LEFT)
        {
            transform.localPosition += Vector3.left * Time.deltaTime * _moveSpeed;
            if (transform.localPosition.x <= -mapWidth)
                transform.localPosition = new Vector3(mapWidth, transform.localPosition.y);
        }
        else if (_direction == DIRECTION.RIGHT)
        {
            transform.localPosition += Vector3.right * Time.deltaTime * _moveSpeed;
            if (transform.localPosition.x >= mapWidth)
                transform.localPosition = new Vector3(-mapWidth, transform.localPosition.y);
        }
        else if (_direction == DIRECTION.UP)
        {
            transform.localPosition += Vector3.up * Time.deltaTime * _moveSpeed;
            if (transform.localPosition.y >= mapHeight)
                transform.localPosition = new Vector3(transform.localPosition.x, -mapHeight);
        }
        else if (_direction == DIRECTION.DOWN)
        {
            transform.localPosition += Vector3.down * Time.deltaTime * _moveSpeed;
            if (transform.localPosition.y <= -mapHeight)
                transform.localPosition = new Vector3(transform.localPosition.x, mapHeight);
        }
    }
}