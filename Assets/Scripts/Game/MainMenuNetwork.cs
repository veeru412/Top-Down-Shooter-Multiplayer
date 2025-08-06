using System.Collections;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using Assets.Scripts.Common;
using System;

namespace Assets.Scripts.Game
{
  public class MainMenuNetwork : NetworkBehaviour
  {
    [SerializeField] private GameObject startbtn;
    [SerializeField] private TextMeshProUGUI playerCountText;
    private void Awake()
    {
      startbtn.SetActive(false);
    }
   
    private void OnClientConnected(ulong obj)
    {
      if(GameStateManager.Instance.CurrentGameState == GameState.Waiting)
      {
        var connectedPlayers = NetworkManager.Singleton.ConnectedClients.Count;
        if (IsServer)
        {
          startbtn.SetActive(connectedPlayers >= Constants.MinPlayersRequiredToStartGame);
        }
        playerCountText.SetText($"Player in lobby: {connectedPlayers}");
      }
    }
    private void OnGameStateChange(GameState obj)
    {
      OnClientConnected(OwnerClientId);
    }
    public override void OnNetworkSpawn()
    {
      base.OnNetworkSpawn();
      NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
      GameStateManager.Instance.RegisterOnGameStateChanged(OnGameStateChange);
    }

    private void OnDisable()
    {
      NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
      GameStateManager.Instance.UnregisterOnGameStateChanged(OnGameStateChange);
    }
  }
}