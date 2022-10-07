using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapElementAutoMove : MonoBehaviour
{
    [SerializeField] private bool _left = false;
    [SerializeField] private float _moveSpeed = 10;
    [SerializeField] private float _rotSpeed = 0;

    int mapSize = 10;

    void Update()
    {
        transform.Rotate(Vector3.forward, Time.deltaTime * _rotSpeed);

        if (_left)
        {
            transform.localPosition += Vector3.left * Time.deltaTime * _moveSpeed;
            if (transform.localPosition.x <= -mapSize)
                transform.localPosition = new Vector3(mapSize, transform.localPosition.y);
        }
        else
        {
            transform.localPosition += Vector3.right * Time.deltaTime * _moveSpeed;
            if (transform.localPosition.x >= mapSize)
                transform.localPosition = new Vector3(-mapSize, transform.localPosition.y);
        }
    }
}