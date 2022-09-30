using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionBlocker : MonoBehaviour
{
    public BoxCollider2D playerCollider;
    public BoxCollider2D blockCollider;
    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreCollision(playerCollider, blockCollider, true);
    }
}
