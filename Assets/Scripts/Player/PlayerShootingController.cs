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

    private void Start()
    {
      var input = FindAnyObjectByType<StandalonePlayerInput>();
      if (input != null)
      {
        Init(input);
      }
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
        var targetHealth = hit.collider.GetComponent<PlayerHealth>();
        if (targetHealth != null && targetHealth.IsAlive)
        {
          targetHealth.ApplyDamage();
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
      if (NetworkManager.LocalClientId == shooterId)
        return;
      var shootingControls = FindObjectsByType<PlayerShootingController>(FindObjectsSortMode.None);
      foreach (var player in shootingControls)
      {
        if (player.OwnerClientId == shooterId)
        {
          player.PlaySfx(); 
          break;
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
        GameStateManager.Instance.CurrentGameState == Game.GameState.Playing &&
        Time.time >= lastFireTime + fireCooldown;
  }
}
