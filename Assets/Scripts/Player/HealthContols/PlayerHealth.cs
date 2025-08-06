using Unity.Netcode;
using System.Collections;
using UnityEngine;
using Assets.Scripts.Common;
using System;

namespace Assets.Scripts.Player.HealthContols
{
  public class PlayerHealth : NetworkBehaviour
  {
    [SerializeField] private GameObject deathParthicle;
    public NetworkVariable<float> Health = new NetworkVariable<float>(
        100f,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    private Vector3 startPos;

    public override void OnNetworkSpawn()
    {
      if (IsServer)
      {
        startPos = transform.position;
      }

      HealthBarManager.Instance.RegisterHealthBar(this);
    }

    public bool IsAlive => Health.Value > 0;

    public void ApplyDamage()
    {
      Health.Value -= Constants.damageAmount;

      if (Health.Value <= 0)
      {
        HideClientRpc(OwnerClientId);
      }
    }

    [ClientRpc]
    private void HideClientRpc(ulong playerId)
    {
      if (NetworkManager.Singleton.LocalClientId == playerId)
      {
        deathParthicle.SetActive(true);
        StartCoroutine(ServerRespawnCoroutine());
      }
    }

    private IEnumerator ServerRespawnCoroutine()
    {
      yield return new WaitForSeconds(1f);

      var playerTransform = NetworkManager.Singleton.ConnectedClients[OwnerClientId].PlayerObject.transform;
      playerTransform.position = startPos;
      deathParthicle.SetActive(false);
      RequestHealthServerRpc();
    }

    [ServerRpc]
    private void RequestHealthServerRpc()
    {
      Health.Value = 100;
    }

    public override void OnDestroy()
    {
      base.OnDestroy();
      HealthBarManager.Instance.UnRegisterHealthBar(this);
    }
  }
}
