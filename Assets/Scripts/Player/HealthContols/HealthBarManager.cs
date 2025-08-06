using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts.Player.HealthContols
{
  public class HealthBarManager : MonoBehaviour
  {
    public static HealthBarManager Instance;
    public GameObject healthBarPrefab;
    public Transform overlayCanvas;

    private Dictionary<ulong, GameObject> healthBars = new Dictionary<ulong, GameObject>();

    void Awake()
    {
      Instance = this;
    }
    private void OnDestroy()
    {
      Instance= null;
    }

    public void RegisterHealthBar(PlayerHealth playerHealth)
    {
      ulong id = playerHealth.OwnerClientId;

      if (healthBars.ContainsKey(id)) return;

      GameObject bar = Instantiate(healthBarPrefab, overlayCanvas);
      var barUI = bar.GetComponent<HealthBarUI>();
      barUI.Init(playerHealth, playerHealth.transform);

      healthBars[id] = bar;
    }
    public void UnRegisterHealthBar(PlayerHealth playerHealth)
    {
      ulong id = playerHealth.OwnerClientId;
      healthBars.TryGetValue(id, out GameObject barUI);
      if(barUI != null)
      {
        Destroy(barUI);
        healthBars.Remove(id);
      }
    }
  }  
}