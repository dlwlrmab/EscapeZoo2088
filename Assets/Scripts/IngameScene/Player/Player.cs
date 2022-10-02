using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumDef;

public class Player : MonoBehaviour
{
    private string _ID;
    private int _animalType;

    public bool HasKey { set; get; } = false;

    public void SetRround(Vector3 startPos)
    {
        transform.position = startPos;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null)
        {
            var obj = other.gameObject.GetComponent<RoundObj>();

            if(obj != null)
            {
                var type = obj.GetObjectType();
                if (type == BLOCKTYPE.KEY)
                {
                    var parent = other.transform.parent;
                    if(parent != null)
                    {
                        var player = parent.GetComponent<Player>();
                        if(player != null)
                        {
                            player.HasKey = false;
                        }
                    }

                    HasKey = true;
                    other.transform.SetParent(transform);
                    other.transform.localPosition = new Vector3(-0.2f, 0.2f, 0);
                }
            }
        }
    }    
}