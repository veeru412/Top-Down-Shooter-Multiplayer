using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Player
{
  public class StandalonePlayerInput : MonoBehaviour, IPlayerInput
  {

    private Vector3 moveDirection = Vector3.zero;
    public Vector3 GetMovementInput() => moveDirection;

    private void Update()
    {
      moveDirection = Vector3.zero;
      if(Input.GetKey(KeyCode.W))
      {
        moveDirection.z = 1;
      }
      if(Input.GetKey(KeyCode.S))
      {
        moveDirection.z = -1;
      }
      if(Input.GetKey(KeyCode.D))
      {
        moveDirection.x = 1;
      }
      if(Input.GetKey(KeyCode.A))
      {
        moveDirection.x = -1;
      }
    }

  }
}