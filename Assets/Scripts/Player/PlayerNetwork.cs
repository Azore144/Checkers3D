using Mirror;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetwork : Player
{
    public static event Action ClientOnInfoUpdated;
    public static event Action<bool> AuthorotyOnLobbyOwnerStateUpdated;

    [SyncVar(hook = nameof(ClientHandleDisplayNameUpdated))]
    string displayName;
    public string DisplayName
    {
        get { return displayName; }
        [Server]
        set { displayName = value; }
    }

    [SyncVar(hook = nameof(AutorotyHandleLobbyOwnerStateUpdated))]
    bool lobbyOwner;
    public bool LobbyOwner
    {
        get { return lobbyOwner; }
        [Server]
        set { lobbyOwner = value; }
    }
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void ClientHandleDisplayNameUpdated(string oldName, string newName)
    {

        ClientOnInfoUpdated?.Invoke();
    }
    void AutorotyHandleLobbyOwnerStateUpdated(bool oldState, bool newState)
        {
            if (!hasAuthority)
            {
                return;
            }
        AuthorotyOnLobbyOwnerStateUpdated?.Invoke(newState);
        }
    public override void OnStartClient()
    {
        if (!isClientOnly) return;
        ((CheckersNetworkManager)NetworkManager.singleton).networkPlayers.Add(this);

        
    }
    public override void OnStopClient()
    {
        if (!isClientOnly) return;
        ((CheckersNetworkManager)NetworkManager.singleton).networkPlayers.Remove(this);
        ClientOnInfoUpdated?.Invoke();
    }
    [Command]
    public void CMDNextTurn()
    {
        TurnsHandler.Instance.NextTurn();
    }

}
