using System.Collections;
using UnityEngine;
using Assets.Scripts.Common;
using Unity.Netcode;
using Assets.Scripts.Game;
using Assets.Scripts.Player.HealthContols;

namespace Assets.Scripts.Player
{
  public class PlayerMovement : NetworkBehaviour
  {
    private IPlayerInput playerInput;
    private PlayerNetworkManager playerHealth;
    public void Init(IPlayerInput playerInput) => this.playerInput = playerInput;

    private void Update()
    {
      if(!CanProcessPlayerInput)
      {
        return;
      }
      Move();
      Rotate();
    }

    private void Move()
    {
      Vector3 moveDirection = new Vector3(playerInput.MoveInput.x, 0, playerInput.MoveInput.y).normalized;
      transform.position += moveDirection * Constants.playerMovementSpeed * Time.deltaTime;
    }

    private void Rotate()
    {
      Vector3 direction = playerInput.LookPosition - transform.position;
      direction.y = 0;  
      if (direction.sqrMagnitude > 0.01f)
      {
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
      }
    }

    private void Start()
    {
      var input = FindAnyObjectByType<StandalonePlayerInput>();
      if(input != null )
      {
        Init(input);
      }
      playerHealth = GetComponent<PlayerNetworkManager>();
    }
    private bool CanProcessPlayerInput =>  playerInput != null && IsOwner && GameStateManager.Instance.CurrentGameState == Game.GameState.Playing && playerHealth.IsAlive;
    
  }
}