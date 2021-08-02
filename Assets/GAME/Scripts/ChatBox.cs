using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface ObserverChatBoxEnter
{
    public void OnPlayerSubmit(string message);
}

public class ChatBox : MonoBehaviour, ObserverPlayerMessage
{
    static ChatBox _singleton;
    public static ChatBox singleton
    {
        get
        {
            return _singleton;
        }
    }

    List<ObserverChatBoxEnter> observerChatBoxEnter = new List<ObserverChatBoxEnter>();

    [SerializeField] Text textChat;
    [SerializeField] InputField inputField;

    private void Awake()
    {
        _singleton = this;
        StartCoroutine(coroutineCheckMainPlayer());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            TextEnter();
        }
    }

    public void ResignObserverPlayerSubmit(ObserverChatBoxEnter observer)
    {
        if (!observerChatBoxEnter.Contains(observer))
        {
            observerChatBoxEnter.Add(observer);
        }
    }

    void TextEnter()
    {
        if(inputField.text != "")
        {
            foreach(ObserverChatBoxEnter observer in observerChatBoxEnter)
            {
                observer.OnPlayerSubmit(inputField.text);
            }
            Debug.Log(inputField.text);
            inputField.text = "";
        }
    }

    IEnumerator coroutineCheckMainPlayer()
    {
        yield return new WaitUntil(() => PlayerManager.mainPlayer);
        ResignObserverPlayerSubmit(PlayerManager.mainPlayer);
        yield return null;
        PlayerManager[] playerManagers = FindObjectsOfType<PlayerManager>();
        foreach(PlayerManager player in playerManagers)
        {
            player.ResignObserverPlayerMessage(this);
        }
    }

    public void OnPlayerMessage(string message)
    {
        textChat.text += "\n" + message;
    }
}
