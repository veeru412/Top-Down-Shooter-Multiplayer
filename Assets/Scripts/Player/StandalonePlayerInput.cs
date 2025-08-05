using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Player
{
  public class StandalonePlayerInput : MonoBehaviour, IPlayerInput
  {

    [SerializeField] private LayerMask groundLayer = default;
    [SerializeField] private float maxRayDistance = 100f;

    private Camera mainCamera;

    public Vector2 MoveInput => new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

    public Vector3 LookPosition
    {
      get
      {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, maxRayDistance, groundLayer))
        {
          return hit.point;
        }
        return transform.position + transform.forward * 5f;
      }
    }

    public bool CanFire => Input.GetMouseButtonDown(0);

    private void Awake()
    {
      mainCamera = Camera.main;
      if(mainCamera == null)
      {
        Debug.LogError("Can't find main camera");
        gameObject.SetActive(false);
      }
    }

  }
}