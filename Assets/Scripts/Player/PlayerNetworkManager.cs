using Assets.Scripts.Game;
using Assets.Scripts.Lobby;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts.Player
{
  public class PlayerNetworkManager : NetworkBehaviour
  {
    public NetworkVariable<FixedString512Bytes> PlayerName = new NetworkVariable<FixedString512Bytes>(
        "",
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server);

    public override void OnNetworkSpawn()
    {
      if (IsOwner)
      {
        string name = UserNameInputMenu.PlayerUsername;
        SetPlayerNameServerRpc(name);
      }
    }

    [ServerRpc]
    void SetPlayerNameServerRpc(string name)
    {
      PlayerName.Value = name;
    }
  }
}