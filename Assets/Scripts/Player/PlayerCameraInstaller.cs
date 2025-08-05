using UnityEngine;
using Unity.Netcode;
using Unity.Cinemachine;

namespace Assets.Scripts.Player
{
  public class PlayerCameraInstaller : NetworkBehaviour
  {
    private void Start()
    {
      if(IsOwner)
      {
        var cam = FindAnyObjectByType<CinemachineCamera>();
        if(cam != null )
        {
          cam.Follow = transform;
        }
      }
    }
  }
}