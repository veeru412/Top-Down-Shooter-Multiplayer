using Assets.Scripts.Common;
using Unity.Netcode;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Lobby
{
  public class LobbySceneNetworkManager : NetworkBehaviour
  {
    public override void OnNetworkSpawn()
    {
      NetworkManager.SceneManager.LoadScene(Constants.GameSceneName, LoadSceneMode.Single);
    }
  }
}