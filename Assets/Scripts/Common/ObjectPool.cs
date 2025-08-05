using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Common
{
  public class ObjectPool<T> where T : MonoBehaviour
  {
    private Queue<T> poolQueue = new Queue<T>();
    private T prefab;
    private Transform parent;

    public ObjectPool(T prefab, int initialSize, Transform parent = null)
    {
      this.prefab = prefab;
      this.parent = parent;

      for (int i = 0; i < initialSize; i++)
      {
        T obj = GameObject.Instantiate(prefab, parent);
        obj.gameObject.SetActive(false);
        poolQueue.Enqueue(obj);
      }
    }

    public T Get()
    {
      T obj = poolQueue.Count > 0 ? poolQueue.Dequeue() : GameObject.Instantiate(prefab, parent);
      obj.gameObject.SetActive(true);
      return obj;
    }

    public void ReturnToPool(T obj)
    {
      obj.gameObject.SetActive(false);
      poolQueue.Enqueue(obj);
    }
  }

}