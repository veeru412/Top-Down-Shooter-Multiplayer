using Unity.Netcode;
using System.Collections;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Player.HealthContols
{
  public class PlayerHealth : NetworkBehaviour
  {
    public NetworkVariable<float> Health = new NetworkVariable<float>(
        100f,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );
    private Vector2 startPos;
    [SerializeField] Animation respawnAnimation;

    public override void OnNetworkSpawn()
    {
      HealthBarManager.Instance.RegisterHealthBar(this);
    }

    public bool IsAlive => Health.Value > 0;

    public void ApplyDamage()
    {
      Health.Value -= Constants.damageAmount;
      if(Health.Value <= 0 )
      {
        RespawnClientRpc(OwnerClientId);
      }
    }

    [ClientRpc]
    private void RespawnClientRpc(ulong playerId)
    {
      if (NetworkManager.Singleton.LocalClientId == playerId)
      {
        transform.position = startPos;
        StartCoroutine(ReSpawn());
      }
    }
    private IEnumerator ReSpawn()
    {
      yield return new WaitForSeconds( 2.0f );
      Health.Value = 100;
    }
    private void Start()
    {
      startPos = transform.position;
    }
  }
}