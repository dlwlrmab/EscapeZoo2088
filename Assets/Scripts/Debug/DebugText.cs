using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugText : MonoBehaviour
{
    Text debugText;
    List<Player> players = null;

    // Start is called before the first frame update
    void Start()
    {
        debugText = GetComponent<Text>();
        players = IngamePlayerController.Instance.GetPlayerList();
    }

    // Update is called once per frame
    void Update()
    {
        debugText.text = "";
        foreach(var p in players)
        {
            debugText.text += p.name + ": " + p.gameObject.transform.localPosition+ "\n";
        }
    }
}
