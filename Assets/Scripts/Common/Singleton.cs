using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Common
{
  public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
  {
    private static T instance;
    private static object _lock = new();

    public static T Instance
    {
      get
      {
        if (instance != null) return instance;

        lock (_lock)
        {
          if (instance == null)
          {
            instance = Object.FindAnyObjectByType<T>();

            if (instance == null)
            {
              Debug.LogError($"No instance of {typeof(T)} found in scene.");
            }
          }
        }

        return instance;
      }
    }

    protected virtual void Awake()
    {
      if (instance == null)
      {
        instance = this as T;
        DontDestroyOnLoad(gameObject);
      }
      else if (instance != this)
      {
        Debug.LogWarning($"{typeof(T)}: Duplicate instance found, destroying this.");
        Destroy(gameObject);
      }
    }
  }

}