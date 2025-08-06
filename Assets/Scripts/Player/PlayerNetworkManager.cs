using Assets.Scripts.Common;
using System.Collections;
using Assets.Scripts.Lobby;
using Assets.Scripts.Player.HealthContols;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using Assets.Scripts.Game;
using System;

namespace Assets.Scripts.Player
{
  public class PlayerNetworkManager : NetworkBehaviour
  {
    #region Name

    public NetworkVariable<FixedString512Bytes> PlayerName = new NetworkVariable<FixedString512Bytes>(
        "",
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server);

   
    private void ChangeMeshColor()
    {
      var mesh = GetComponentInChildren<MeshRenderer>();
      if (mesh != null)
      {
        mesh.material.color = Color.red;
      }
    }

    [ServerRpc]
    void SetPlayerNameServerRpc(string name)
    {
      PlayerName.Value = name;
    }

    #endregion
   
    #region Health

    [SerializeField] private GameObject deathParthicle;
    public NetworkVariable<float> Health = new NetworkVariable<float>(
        100f,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    private Vector3 startPos;

    public bool IsAlive => Health.Value > 0;
    
    public void ApplyDamage(out bool isDead)
    {
      Health.Value -= Constants.damageAmount;
      isDead = Health.Value <= 0;

      if (isDead)
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

    #endregion


    #region Score

    public NetworkVariable<int> kills = new NetworkVariable<int>(
      0,
      NetworkVariableReadPermission.Everyone,
      NetworkVariableWritePermission.Server);

    public void AddKills() => kills.Value++;
    private void OnKillsUpdate(int previousValue, int newValue) => GameStateManager.Instance.TriggerLeaderBoardStateChange();
    #endregion

    public override void OnNetworkSpawn()
    {
      base.OnNetworkSpawn();
      if (IsOwner)
      {
        string name = UserNameInputMenu.PlayerUsername;
        SetPlayerNameServerRpc(name);
        ChangeMeshColor();
      }
      startPos = transform.position;
      HealthBarManager.Instance.RegisterHealthBar(this);
      kills.OnValueChanged += OnKillsUpdate;
    }

    public override void OnDestroy()
    {
      base.OnDestroy();
      kills.OnValueChanged -= OnKillsUpdate;
      HealthBarManager.Instance.UnRegisterHealthBar(this);
    }
  }
}