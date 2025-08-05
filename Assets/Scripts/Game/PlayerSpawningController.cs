using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts.Game
{
  public class PlayerSpawningController
  {
    private GameObject playerPrefab;
    private List<GameObject> spawnList;
    private Vector3[] spawnPositions = new Vector3[8]
    {
      new Vector3(10,0, 10),
      new Vector3(-10,0, -10),
      new Vector3(10,0, -10),
      new Vector3(-10,0, 10),
      new Vector3(0, 0, 0),
      new Vector3(2,0, 10),
      new Vector3(0,0, 2),
      new Vector3(-5,0, 10)
    };


    public PlayerSpawningController()
    {
      playerPrefab = Resources.Load("Player") as GameObject;
      spawnList= new List<GameObject>();
    }

    public void OnGameStateChange(GameState state)
    {
      if(state == GameState.Playing)
      {
        foreach (var clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
          var instance = TrySpawnPlayer(clientId);
          if(instance != null)
          {
            spawnList.Add(instance);
          }
        }
      }
      else
      {
        foreach(var instance in spawnList)
        {
          GameObject.Destroy(instance);
        }
      }
    }

    private GameObject TrySpawnPlayer(ulong clientId)
    {
      if (NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject != null)
        return null;

      int index = new List<ulong>(NetworkManager.Singleton.ConnectedClientsIds).IndexOf(clientId);
      if (index >= spawnPositions.Length)
      {
        Debug.LogWarning("Not enough spawn positions defined. Reusing last.");
        index = spawnPositions.Length - 1;
      }
      var spawnPoint = spawnPositions[index];
      GameObject player = GameObject.Instantiate(playerPrefab, spawnPoint, Quaternion.identity);
      player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);

      return player;
    }
  }
}