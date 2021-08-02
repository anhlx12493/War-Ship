using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;

public interface ObserverPlayerMessage
{
    public void OnPlayerMessage(string message);
}

public class PlayerManager : NetworkBehaviour, ObserverShipDead, ObserverChatBoxEnter
{
    NetworkVariableString networkVariablePlayerName = new NetworkVariableString(new NetworkVariableSettings
    {
        WritePermission = NetworkVariablePermission.ServerOnly,
        ReadPermission = NetworkVariablePermission.Everyone
    });

    static int index = 1;
    static PlayerManager _mainPlayer;
    public static PlayerManager mainPlayer
    {
        get
        {
            return _mainPlayer;
        }
    }

    public GameObject prefab;
    Player player;

    List<ObserverPlayerMessage> observerPlayerMessages = new List<ObserverPlayerMessage>();

    private void Start()
    {
        if (IsServer)
        {
            player = new Player();
            networkVariablePlayerName.Value = "Player " + index++;
        }
        if (IsOwner)
        {
            _mainPlayer = this;
            SpawServerRpc();
            if (ChatBox.singleton)
            {
                ChatBox.singleton.ResignObserverPlayerSubmit(this);
            }
        }
        if (ChatBox.singleton)
        {
            ResignObserverPlayerMessage(ChatBox.singleton);
        }
    }

    [ServerRpc]
    void SpawServerRpc()
    {
        GamePlayShip gamePlayShip = PrefabManager.InstantiatePrefab(PrefabManager.prefabGamePlayShipCannon).GetComponent<GamePlayShip>();
        gamePlayShip.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);
        gamePlayShip.SetPlayer(player);
        gamePlayShip.ResignObserverShipDead(this);
    }

    [ServerRpc]
    void PlayerMessageServerRpc(string message)
    {
        PlayerMessageClientRpc(message);
    }

    [ClientRpc]
    void PlayerMessageClientRpc(string message)
    {
        foreach(ObserverPlayerMessage observer in observerPlayerMessages)
        {
            if (IsOwner)
            {
                observer.OnPlayerMessage("Me" + ": " + message);
            }
            else
            {
                observer.OnPlayerMessage(networkVariablePlayerName.Value + ": " + message);
            }
        }
    }

    void Spaw()
    {
        GamePlayShip gamePlayShip = PrefabManager.InstantiatePrefab(PrefabManager.prefabGamePlayShipCannon).GetComponent<GamePlayShip>();
        gamePlayShip.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);
        gamePlayShip.SetPlayer(player);
        gamePlayShip.ResignObserverShipDead(this);
    }

    public void ResignObserverPlayerMessage(ObserverPlayerMessage observer)
    {
        if (!observerPlayerMessages.Contains(observer))
        {
            observerPlayerMessages.Add(observer);
        }
    }

    public void OnShipDead(GamePlayShip ship)
    {
        Invoke("Spaw", 1);
    }

    public void OnPlayerSubmit(string message)
    {
        if (IsServer)
        {
            PlayerMessageClientRpc(message);
        }
        else
        {
            PlayerMessageServerRpc(message);
        }
    }
}
