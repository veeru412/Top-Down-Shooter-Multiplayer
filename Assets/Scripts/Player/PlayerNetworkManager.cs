using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts.Player
{
  public class PlayerNetworkManager : NetworkBehaviour
  {
    /*public override void OnNetworkSpawn()
    {
      if (!IsOwner) return;

      Debug.Log("Local player spawned");

      if (GameManager.Instance.IsRoundInProgress)
      {
        SpawnPlayer();
      }
      else
      {
        ShowWaitingUI();
      }
    }*/
  }
}