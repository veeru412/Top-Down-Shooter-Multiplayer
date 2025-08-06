using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Player.HealthContols
{
  public class HealthBarUI : MonoBehaviour
  {
    [SerializeField] private Image helathFillImage;
    private Transform target;
    private Camera mainCamera;
    private PlayerNetworkManager playerHealth;
    private Transform mTransform;

    public void Init(PlayerNetworkManager health, Transform followTarget)
    {
      playerHealth = health;
      target = followTarget;
      mainCamera = Camera.main;
      mTransform= transform;
    }

    void Update()
    {
      if (target == null || playerHealth == null || mainCamera == null) return;

      Vector3 worldPos = target.position;
      Vector3 screenPos = mainCamera.WorldToScreenPoint(worldPos);
      screenPos.y -= 50;
      mTransform.position = screenPos;
      helathFillImage.fillAmount = playerHealth.Health.Value / 100f;
    }
  }
}