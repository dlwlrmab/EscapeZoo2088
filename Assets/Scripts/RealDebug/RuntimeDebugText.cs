using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuntimeDebugText : MonoBehaviour
{
    private Text debugText;
    private List<Player> players = null;

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
        foreach(var player in players)
        {
            debugText.text += player.name + " " + player.gameObject.GetComponent<Rigidbody2D>().position + "\n";
        }
        Debug.Log(debugText.text);
    }
}
