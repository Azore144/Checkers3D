using Mirror;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckersNetworkManager : NetworkManager
{

    [SerializeField] GameObject gameOverHandlerPrefab, boardPrefab, 
        turnsHandlerPrefab;
    public static event Action ClientOnConnected;
    public static event Action ServerOnGameStarted;
    public List<PlayerNetwork> networkPlayers { get; } = new List<PlayerNetwork>();
    public override void OnStartServer()
    {
        var boardInstance = Instantiate(boardPrefab);
        NetworkServer.Spawn(boardInstance);
        var turnsHandlerInstance = Instantiate(turnsHandlerPrefab);
        NetworkServer.Spawn(turnsHandlerInstance);

    }
    /*    public override void OnServerChangeScene(string newSceneName)
        {
            var gameoverHandlerInstance = Instantiate(gameOverHandlerPrefab);
            NetworkServer.Spawn(gameoverHandlerInstance);
        }*/
    public override void OnServerSceneChanged(string sceneName)
    {
        if(sceneName.StartsWith("Game"))
        {
            ServerOnGameStarted?.Invoke();
            var gameOverHandlerInstance = Instantiate(gameOverHandlerPrefab);
            NetworkServer.Spawn(gameOverHandlerInstance);
        }
    }
    public override void OnClientConnect()
    {
        base.OnClientConnect();
        ClientOnConnected?.Invoke();
    }
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        GameObject playerInstance = Instantiate(playerPrefab);
        NetworkServer.AddPlayerForConnection(conn, playerInstance);
        var player = playerInstance.GetComponent<PlayerNetwork>();
        networkPlayers.Add(player);
        
        player.LobbyOwner = player.IsWhite = numPlayers == 1;
        player.DisplayName = player.IsWhite ? "Светлый" : "Тёмный";
    }
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        var player = conn.identity.GetComponent<PlayerNetwork>();
        networkPlayers.Remove(player);
        base.OnServerDisconnect(conn);
    }
    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        SceneManager.LoadScene("Lobby Scene");
        Destroy(gameObject);
    }
    public override void OnStopServer()
    {
        networkPlayers.Clear();
    }
}
