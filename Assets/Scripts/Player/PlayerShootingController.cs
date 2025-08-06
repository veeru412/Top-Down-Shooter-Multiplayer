using UnityEngine;
using Unity.Netcode;
using Assets.Scripts.Common;
using Assets.Scripts.Game;
using Assets.Scripts.Player.HealthContols;

namespace Assets.Scripts.Player
{
  public class PlayerShootingController : NetworkBehaviour
  {
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireCooldown = 0.2f;
    [SerializeField] private AnimatedTexture flash;
    [SerializeField] private AudioSource gunShotSfx;

    private float lastFireTime;
    private IPlayerInput playerInput;
    private PlayerNetworkManager playerHealth;

    private void Start()
    {
      var input = FindAnyObjectByType<StandalonePlayerInput>();
      if (input != null)
      {
        Init(input);
      }
      playerHealth = GetComponent<PlayerNetworkManager>();
    }

    public void Init(IPlayerInput playerInput) => this.playerInput = playerInput;

    private void Update()
    {
      if (playerInput.CanFire && CanProcessPlayerInput)
      {
        Vector3 direction = GetFlatLookDirection();

        PlaySfx();
        FireHitScanServerRpc(OwnerClientId, firePoint.position, direction);
        FireProjectile(firePoint.position, direction);

        lastFireTime = Time.time;
      }
    }

    [ServerRpc]
    private void FireHitScanServerRpc(ulong shooterId, Vector3 origin, Vector3 direction)
    {
      Ray ray = new Ray(origin, direction);
      if (Physics.Raycast(ray, out RaycastHit hit, 100f))
      {
        var targetHealth = hit.collider.GetComponent<PlayerNetworkManager>();
        if (targetHealth != null && targetHealth.IsAlive)
        {
          targetHealth.ApplyDamage(out bool isDead);
          if (isDead)
          {
            var shooterObject = NetworkManager.Singleton.ConnectedClients[shooterId].PlayerObject;
            var shooterData = shooterObject.GetComponent<PlayerNetworkManager>();
            if (shooterData != null)
            {
              shooterData.AddKills();
            }
          }
        }
      }

      PlaySfxClientRpc(shooterId);
    }

    private void FireProjectile(Vector3 origin, Vector3 direction)
    {
      var projectile = ProjectilePooler.Instance.GetProjectile();
      projectile.transform.position = origin;
      projectile.transform.rotation = Quaternion.LookRotation(direction);
      projectile.Fire(direction, ProjectilePooler.Instance.ReturnProjectile);
    }

    [ClientRpc]
    private void PlaySfxClientRpc(ulong shooterId)
    {
      if (NetworkManager.Singleton.LocalClientId == shooterId)
        return;

      if (NetworkManager.Singleton.ConnectedClients.TryGetValue(shooterId, out var client))
      {
        var shooterObj = client.PlayerObject;
        if (shooterObj != null)
        {
          var shootingController = shooterObj.GetComponent<PlayerShootingController>();
          if (shootingController != null)
          {
            shootingController.PlaySfx();
          }
        }
      }
    }


    private void PlaySfx()
    {
      if (flash != null) flash.Play();

      if (gunShotSfx != null) gunShotSfx.Play();
    }

    private Vector3 GetFlatLookDirection()
    {
      Vector3 lookDirection = playerInput.LookPosition - firePoint.position;
      lookDirection.y = 0f;
      return lookDirection.normalized;
    }

    private bool CanProcessPlayerInput =>
        playerInput != null &&
        IsOwner &&
        playerHealth.IsAlive &&
        GameStateManager.Instance.CurrentGameState == Game.GameState.Playing &&
        Time.time >= lastFireTime + fireCooldown;
  }
}
