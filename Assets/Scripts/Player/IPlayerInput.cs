
using UnityEngine;

namespace Assets.Scripts.Player
{
  public interface IPlayerInput 
  {
    Vector2 MoveInput { get; }
    Vector3 LookPosition { get; }
    bool CanFire { get; }
  }
}