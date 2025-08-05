using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Network
{
  public class NetworkCallBackListner : MonoBehaviour
  {
    public int expectedPlayerCount = 2; 

    private void OnEnable()
    {
      NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
    }

    private void OnDisable()
    {
      NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
    }

    private void HandleClientConnected(ulong clientId)
    {
      if (!NetworkManager.Singleton.IsServer)
        return;

      int connectedCount = NetworkManager.Singleton.ConnectedClientsList.Count;

      Debug.Log($"Client {clientId} connected. Total: {connectedCount}");

      if (connectedCount >= expectedPlayerCount)
      {
        Debug.Log("All players connected. Loading GameScene...");
        NetworkManager.Singleton.SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
      }
    }
  }
}