using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapElementAutoMove : MonoBehaviour
{
    int mapSize = 10;
    public float speed = 10;

    void Update()
    {
        transform.localPosition += Vector3.right * Time.deltaTime * speed;

        if (transform.localPosition.x >= mapSize)
            transform.localPosition = new Vector3(-mapSize, transform.localPosition.y);
    }
}
