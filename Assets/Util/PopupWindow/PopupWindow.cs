using UnityEngine;
using UnityEngine.UI;
using System;

public class PopupCheckConfirmEventArgs : EventArgs
{
    public UnityEngine.Events.UnityAction events;
    public string eventName;
}

public class PopupWindow : MonoBehaviour {

    public static PopupWindow instance;
    public static event EventHandler<PopupCheckConfirmEventArgs> ConfirmEvent;

    public enum MsgType { login, notice, error, warning, exit  };

    public GameObject popupView;
    
    public Text m_MessageType;
    public Text m_Msg;
    public Button Btn_Exit;
    public Button Btn_OK;
    public Button Btn_Cancle;

	// Use this for initialization
	void Start () {
        instance = this;
        Btn_Exit.onClick.AddListener(CloseView);
        Btn_Cancle.onClick.AddListener(CloseView);
        ConfirmEvent += OnConfirmOnClick;
    }

    public void CloseView()
    {
        popupView.SetActive(false);
        Btn_OK.gameObject.SetActive(false);
        Btn_Cancle.gameObject.SetActive(false);
    }

    public void PopupCheckWindowOpen(UnityEngine.Events.UnityAction action, string actionName, MsgType msgtype, string msg)
    {
        Btn_OK.gameObject.SetActive(true);
        Btn_Cancle.gameObject.SetActive(true);
        Btn_OK.onClick.AddListener(delegate { ConfirmEvent(this, new PopupCheckConfirmEventArgs() { events = action, eventName = actionName }); });
        PopupWindowOpen(msgtype, msg);
    }

    public void OnConfirmOnClick(object sender, PopupCheckConfirmEventArgs e)
    {
        e.events();
        Btn_OK.onClick.RemoveAllListeners();
        CloseView();
    }

	public void PopupWindowOpen(MsgType msgtype, string msg)
    {
        popupView.SetActive(true);

        if (msgtype == MsgType.login)
        {
            m_MessageType.text = "로그인";
            m_Msg.text = msg;
            m_MessageType.color = Color.white;
        }
        else if (msgtype == MsgType.error)
        {
            m_MessageType.text = "Error !";
            m_Msg.text = msg;
            m_MessageType.color = Color.red;
        }
        else if (msgtype == MsgType.warning)
        {
            m_MessageType.text = "Warning !";
            m_Msg.text = msg;
            m_MessageType.color = Color.yellow;
        }
        else if (msgtype == MsgType.exit)
        {
            m_MessageType.text = "EXIT";
            m_Msg.text = msg;
            m_MessageType.color = Color.white;
        }
        else
        {
            m_MessageType.text = "Notice";
            m_Msg.text = msg;
            m_MessageType.color = Color.white;
        }
    }
}
