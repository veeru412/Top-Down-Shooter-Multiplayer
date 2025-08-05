using UnityEngine;
using Assets.Scripts.Common;
using Unity.Netcode;

namespace Assets.Scripts.Player
{
  public class ProjectilePooler : MonoBehaviour
  {
    public static ProjectilePooler Instance { get; private set; }

    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private int poolSize = 20;

    private ObjectPool<Projectile> pool;

    private void Awake()
    {
      pool = new ObjectPool<Projectile>(projectilePrefab, poolSize, transform);
      Instance = this;
    }

    private void OnDestroy()
    {
      Instance = null;
    }

    public Projectile GetProjectile()
    {
      return pool.Get();
    }

    public void ReturnProjectile(Projectile projectile)
    {
      pool.ReturnToPool(projectile);
    }
  }

}