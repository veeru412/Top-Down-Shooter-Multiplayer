using System.Collections;
using Unity.Netcode;
using UnityEngine;
using System;

namespace Assets.Scripts.Game
{
  public class GameStateManager : NetworkBehaviour
  {
    public static GameStateManager Instance;
    private PlayerSpawningController spawnController;

    private readonly NetworkVariable<GameState> gameState = new NetworkVariable<GameState>(
        GameState.Waiting,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server);

    private readonly NetworkVariable<float> timeRemaining = new NetworkVariable<float>(
        0f,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    public float TimeRemaining => timeRemaining.Value;
    private event Action<GameState> gameStateChangedInternal;
    private event Action OnLeaderboardDataChanged;
    public void TriggerLeaderBoardStateChange() => OnLeaderboardDataChanged?.Invoke();
    public void RegisterLeaderBoardChange(Action onLeaderBoardChange) => OnLeaderboardDataChanged += onLeaderBoardChange;
    public void UnRegisterLeaderBoardChange(Action onLeaderBoardChange) => OnLeaderboardDataChanged -= onLeaderBoardChange;
    public GameState CurrentGameState => gameState.Value;
    public void RegisterOnGameStateChanged(Action<GameState> callback)
    {
      gameStateChangedInternal += callback;
      callback?.Invoke(gameState.Value);
    }

    public void UnregisterOnGameStateChanged(Action<GameState> callback)
    {
      gameStateChangedInternal -= callback;
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetGameStateServerRpc(GameState newState)
    {
      spawnController.OnGameStateChange(newState);
      gameState.Value = newState;
    }

    public override void OnNetworkSpawn()
    {
      gameState.OnValueChanged += OnGameStateChanged;

      if (IsServer)
        gameState.Value = GameState.Waiting;
    }

    private void OnGameStateChanged(GameState oldState, GameState newState)
    {
      gameStateChangedInternal?.Invoke(newState);
    }

    private void Awake()
    {
      if (Instance != null && Instance != this)
      {
        Destroy(gameObject);
        return;
      }

      Instance = this;
      spawnController = new PlayerSpawningController();
    }

    public override void OnDestroy()
    {
      base.OnDestroy();
      if (Instance == this)
      {
        Instance = null;
      }

      gameState.OnValueChanged -= OnGameStateChanged;
    }
  }
}