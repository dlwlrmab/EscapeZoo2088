using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private string _ID;
    private int _animalType;

    public void SetRround(Vector3 startPos)
    {
        transform.localPosition = startPos;
    }
}