using System.Collections;
using UnityEngine;
using Assets.Scripts.Common;

namespace Assets.Scripts.Player
{
  public class PlayerMovement : MonoBehaviour
  {
    private IPlayerInput playerInput;
    public void Init(IPlayerInput playerInput) => this.playerInput = playerInput;

    private void Update()
    {
      if(playerInput == null)
      {
        return;
      }
      transform.position += playerInput.GetMovementInput() * Constants.playerMovementSpeed * Time.deltaTime;
    }

    private void Start()
    {
      // this code should be replaced with installer. for now as we have only one input, i have dependecy this way.
      var input = FindAnyObjectByType<StandalonePlayerInput>();
      if(input != null )
      {
        Init(input);
      }
    }
  }
}