using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Unity.Netcode;
using Assets.Scripts.Player;
using Assets.Scripts.UIStateMachine;
using System.Collections;
using System;
using System.Linq;

namespace Assets.Scripts.Game
{
  public class InGameMenu : BaseUiMenu
  {
    [SerializeField] private TextMeshProUGUI[] availableTextElements;
    [SerializeField] private Texture2D customCursor;
    private Dictionary<ulong, PlayerNetworkManager> playerCache = new Dictionary<ulong, PlayerNetworkManager>();

    private void UpdateLeaderBoard()
    {
      var sortedPlayers = playerCache
          .Where(kvp => kvp.Value != null)
          .OrderByDescending(kvp => kvp.Value.kills.Value)
          .ToList();

      int index = 0;
      foreach (var kvp in sortedPlayers)
      {
        if (index >= availableTextElements.Length) break;

        var playerNet = kvp.Value;
        availableTextElements[index].gameObject.SetActive(true);
        availableTextElements[index].text = $"{playerNet.PlayerName.Value} - {playerNet.kills.Value} Kills";
        index++;
      }

      for (int i = index; i < availableTextElements.Length; i++)
      {
        availableTextElements[i].gameObject.SetActive(false);
      }
    }


    private void CachePlayersFromNetwork()
    {
      var clients = NetworkManager.Singleton.ConnectedClients;
      int index = 0;
      foreach (var client in clients)
      {
        ulong clientId = client.Key;

        if (playerCache.ContainsKey(clientId)) continue;

        if (client.Value.PlayerObject != null &&
            client.Value.PlayerObject.TryGetComponent<PlayerNetworkManager>(out var foundPlayerNet))
        {
          playerCache[clientId] = foundPlayerNet;
        }
        index++;
      }
      for (int i = index; i < availableTextElements.Length; i++)
      {
        availableTextElements[i].gameObject.SetActive(false);
      }
    }
    private void OnEnable()
    {
      StartCoroutine(WaitAndLoadLeaderBoard());
    }
    IEnumerator WaitAndLoadLeaderBoard()
    {
      yield return new WaitForSeconds(2);
      CachePlayersFromNetwork();
      UpdateLeaderBoard();
      GameStateManager.Instance.RegisterLeaderBoardChange(UpdateLeaderBoard);
      Cursor.SetCursor(customCursor, Vector2.zero, CursorMode.Auto);
    }

    private void OnDisable()
    {
      GameStateManager.Instance.UnRegisterLeaderBoardChange(UpdateLeaderBoard);
      Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
      playerCache.Clear();
    }
  }
}
