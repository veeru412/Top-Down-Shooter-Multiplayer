using UnityEngine;
using Unity.Netcode;
using UnityEngine.Audio;

namespace Assets.Scripts.Game
{
  public class MusicController : NetworkBehaviour
  {
    [SerializeField] private AudioMixer gameAudio;
    public override void OnNetworkSpawn()
    {
      if (IsServer)
      {
        gameAudio.SetFloat("master", 0);
      } 
    }
  }
}