using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyMenu : MonoBehaviour
{
    [SerializeField] Button startGameButton;
    [SerializeField] Text[] playerNameTexts = new Text[2];
    private void Start()
    {
        PlayerNetwork.AuthorotyOnLobbyOwnerStateUpdated += AuthorotyHandleLobbyOwnerStateUpdated;
        PlayerNetwork.ClientOnInfoUpdated += ClientHandleInfoUpdated;
    }
    private void OnDestroy()
    {
        PlayerNetwork.ClientOnInfoUpdated -= ClientHandleInfoUpdated;
        PlayerNetwork.AuthorotyOnLobbyOwnerStateUpdated -= AuthorotyHandleLobbyOwnerStateUpdated;
    }
    private void ClientHandleInfoUpdated()
    {
        List<PlayerNetwork> players = ((CheckersNetworkManager)NetworkManager.singleton).networkPlayers;
        for(int i = 0; i < players.Count; i++)
        {
            playerNameTexts[i].text = players[i].DisplayName;
        }
        for(int i = players.Count; i > playerNameTexts.Length; i++)
        {
            playerNameTexts[i].text = "∆дЄм игрока...";
        }
        startGameButton.interactable = players.Count > 1;
    }
    private void AuthorotyHandleLobbyOwnerStateUpdated(bool state)
    {
        startGameButton.gameObject.SetActive(state);
    }

    public void StartGame()
    {
        NetworkManager.singleton.ServerChangeScene("Game Scene");
    }
}
