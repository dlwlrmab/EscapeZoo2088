using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyScene : MonoBehaviour
{
    [SerializeField] Text _notiText;
    
    SceneLoadManager _scenemanager = null;
    GameObject _exButton = null;

    private void Awake()
    {
        _scenemanager = SceneLoadManager.Instance;
        _scenemanager.PlayFadeIn();
        GlobalData.animalIndex = -1;
    }
        
    public void OnClickPlayGame_Test()
    {
        if (GlobalData.animalIndex == -1)
        {
            _notiText.text = "캐릭터를 선택하지 않았습니다.";
            return;
        }
        _notiText.text = "";
        _scenemanager.PlayFadeout(null, "IngameScene");
    }

    public void ChoiceAnimal(GameObject obj)
    {
        if (_exButton != null)
        {
            _exButton.GetComponent<Image>().color = Color.white;
        }
            _exButton = obj;
            GlobalData.animalIndex = int.Parse(obj.name);
            obj.GetComponent<Image>().color = Color.green;
    }
}
