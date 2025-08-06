using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Unity.Netcode;
using Assets.Scripts.Player;
using Assets.Scripts.UIStateMachine;
using System.Collections;

namespace Assets.Scripts.Game
{
  public class InGameMenu : BaseUiMenu
  {
    [SerializeField] private TextMeshProUGUI[] currentPlayers;

    private Dictionary<ulong, PlayerNetworkManager> playerCache = new Dictionary<ulong, PlayerNetworkManager>();

    private void UpdatePlayerListFromNetwork()
    {
      var clients = NetworkManager.Singleton.ConnectedClients;
      int index = 0;

      foreach (var clinet in clients)
      {
        if (index >= currentPlayers.Length) break;

        ulong clientId = clinet.Key;

        if (!playerCache.TryGetValue(clientId, out var playerNet))
        {
          if (clinet.Value.PlayerObject != null &&
              clinet.Value.PlayerObject.TryGetComponent<PlayerNetworkManager>(out var foundPlayerNet))
          {
            playerCache[clientId] = foundPlayerNet;
            playerNet = foundPlayerNet;
          }
        }

        if (playerNet != null)
        {
          currentPlayers[index].gameObject.SetActive(true);
          currentPlayers[index].text = playerNet.PlayerName.Value.ToString();
          index++;
        }
      }

      for (int i = index; i < currentPlayers.Length; i++)
      {
        currentPlayers[i].gameObject.SetActive(false);
      }
    }
    IEnumerator Start()
    {
      yield return new WaitForSeconds(2);
      UpdatePlayerListFromNetwork();
    }
  }
}
