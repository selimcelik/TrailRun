using System;
using UnityEngine;


public class GameManager : Singleton<GameManager>
{
    public GameState State;
    [HideInInspector]
    public TimeState StateTime;
    private UIManager _uiManager;
    private PlayerManager _playerManager;
    private CollectManager _collectManager;
    private ComponentManager _componentManager;
    private ObjectPooler _objectPooler;

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        _uiManager = UIManager.Instance;
        _playerManager = PlayerManager.Instance;
        _collectManager = CollectManager.Instance;
        _componentManager = ComponentManager.Instance;
        _objectPooler = ObjectPooler.Instance;
    }

    private void Start()
    {
        UpdateGameState(GameState.WaitGame);
    }

    #region Game State Options
    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.WinGame:
                handleWinGame();
            break;
            case GameState.StartGame:
                handleStartGame();
            break;
            case GameState.WaitGame:
                handleWaitGame();
            break;
            case GameState.RestartGame:
                handleRestartGame();
            break;
        }
    }

    private void handleWaitGame()
    {
        _uiManager.UpdatePanelState(PanelCode.StartPanel, true);
        _playerManager.StopPlayer();
    }
    private void handleStartGame()
    {
        _collectManager.CollectedObjects.Clear();
        _collectManager.stackedCoin = 0;
        _collectManager.stackedDiamond = 0;
        _objectPooler.CreatePoolObjects();
        _uiManager.UpdatePanelState(PanelCode.GamePanel, true);
        _collectManager.GameStartWithStack();
        _playerManager.StartPlayer();
       
    }
    private void handleWinGame()
    {
        _collectManager.CollectedObjects.Clear();
        _uiManager.UpdatePanelState(PanelCode.WinPanel, true);
        _playerManager.StopPlayer();
    }
    private void handleRestartGame()
    {
        _uiManager.UpdatePanelState(PanelCode.GamePanel, true);
        _playerManager.RestartPlayer();
        _objectPooler.CreatePoolObjects();
        //_collectManager.ActiveCollectedObject();
        //_collectManager.ActiveObstacleObject();
    }
   
    private void OnValueChangedCallback()
    {
        UpdateGameState(State);
    }
    #endregion

}


public enum GameState
{
    WinGame,
    StartGame,
    WaitGame,
    RestartGame 
}

public enum TimeState
{
    HandleSetMaxTime
}