using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round0 : Round
{
    [Header("Round 0")]
    [Space(10)]
    [SerializeField] SpriteRenderer _trafficLight;
    [SerializeField] Sprite[] _trafficLightType;

    public override void StartRound()
    {
        base.StartRound();

        _trafficLight.sprite = _trafficLightType[0];
    }

    public override void ClearRound()
    {
        base.ClearRound();
    }

    private void Update()
    {
        
    }
}
