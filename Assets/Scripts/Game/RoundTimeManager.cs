
using Unity.Netcode;
using UnityEngine;
using Assets.Scripts.Common;
using System.Collections;

namespace Assets.Scripts.Game
{
  public class RoundTimeManager : NetworkBehaviour
  {
    private readonly NetworkVariable<int> timeRemaining = new NetworkVariable<int>(
        0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    public int TimeRemaining => timeRemaining.Value;

    private void OnGameStateChanged(GameState newState)
    {
      if (!IsServer) return;

      if (newState == GameState.Playing)
      {
        StartRound();
      }
    }

    private void StartRound()
    {
      timeRemaining.Value = Constants.RoundDuration;
      StartCoroutine(RunTimer());
    }
    IEnumerator RunTimer()
    {
      while(TimeRemaining > 0)
      {
        yield return new WaitForSeconds(1f);
        timeRemaining.Value -= 1;
        if (timeRemaining.Value <= 0f)
        {
          EndRound();
        }
      }
    }
    private void EndRound()
    {
      GameStateManager.Instance.SetGameStateServerRpc(GameState.Finished); 
    }

    private void Start()
    {
      GameStateManager.Instance.RegisterOnGameStateChanged(OnGameStateChanged);
    }

    private void OnDisable()
    {
      if(GameStateManager.Instance != null)
        GameStateManager.Instance.UnregisterOnGameStateChanged(OnGameStateChanged);
    }
  }
}