using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Player
{
  public class Projectile : MonoBehaviour
  {
    [SerializeField] private float speed = 50f;
    [SerializeField] private float lifeTime = 2f;

    private Vector3 direction;
    private float timer;
    private System.Action<Projectile> returnToPool;

    public void Fire(Vector3 direction, System.Action<Projectile> returnToPool)
    {
      this.direction = direction.normalized;
      this.returnToPool = returnToPool;
      timer = 0f;
    }

    private void Update()
    {
      transform.position += direction * speed * Time.deltaTime;
      timer += Time.deltaTime;

      if (timer >= lifeTime)
      {
        returnToPool?.Invoke(this);
      }
    }

    private void OnTriggerEnter(Collider other)
    {
      returnToPool?.Invoke(this);
    }
  }
}