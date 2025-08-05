using UnityEngine;

namespace Assets.Scripts.Game
{
  public class HealthBar : MonoBehaviour
  {
    private Transform target;
    private Camera mainCamera;
    private RectTransform mTransform;

    private void Awake()
    {
      mainCamera = Camera.main;
      mTransform = GetComponent<RectTransform>();
    }

    public void Init(Transform target) => this.target = target;

    private void Update()
    {
      if (target == null || mainCamera == null) return;

      Vector3 screenPos = mainCamera.WorldToScreenPoint(target.position);
      screenPos.y -= 50;
      mTransform.position = screenPos;
    }
  }
}
