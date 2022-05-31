using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ComponentManager : Singleton<ComponentManager>
{

    private UIManager _uiManager;
    private LevelManager _levelManager;
    private GameManager _gameManager;
    private CollectManager _collectManager;
    private PlayerManager _playerManager;

    public Button PlayButton;

    public Button WinButton;

    public Button StartStackButton;
    

    public TextMeshProUGUI PlayButtonText;

    public TextMeshProUGUI WinButtonText;

    public TextMeshProUGUI StartStackButtonText;

    public TextMeshProUGUI CoinNumberText;

    public TextMeshProUGUI CoinNumberTextOnStartPanel;

    public TextMeshProUGUI LevelNumberTextOnStartPanel;

    public TextMeshProUGUI CollectedTotalCoinTextForWin;

    public TextMeshProUGUI StackCountText;

    public GameObject CoinHolder;

    public bool isCoinHolder;

    public GameObject LevelNumberText;

    public GameObject DiamondSlider;
    public Slider DiamondSliderComponent;

    public GameObject GoldSlider;
    public Slider GoldSliderComponent;


    private void Awake()
    {
        _uiManager = UIManager.Instance;
        _gameManager = GameManager.Instance;
        _collectManager = CollectManager.Instance;
        _levelManager = LevelManager.Instance;
        _playerManager = PlayerManager.Instance;
    }

    private void Start()
    {
        PlayButton.onClick.AddListener(() => HandlePlayButton());
        WinButton.onClick.AddListener(() => HandleNextButton());
        StartStackButton.onClick.AddListener(() => HandleStartStackButton());

        GoldSliderComponent.maxValue = _collectManager.CoinStackLimit;
        DiamondSliderComponent.maxValue = _collectManager.DiamondStackLimit;

        if (isCoinHolder)
        {
            CoinHolder.SetActive(true);
        }
        
    }

    private void Update()
    {
        CoinNumberText.text = _collectManager.CollectedCoin.ToString();
        CoinNumberTextOnStartPanel.text = _collectManager.CollectedCoin.ToString();
        LevelNumberText.GetComponent<TextMeshProUGUI>().text = "Level : " + _levelManager.DisplayLevelNumber.ToString();
        LevelNumberTextOnStartPanel.text = "Level : " + _levelManager.DisplayLevelNumber.ToString();
        CollectedTotalCoinTextForWin.text = "TOTAL COIN : " + _collectManager.CollectedCoin.ToString();
        StackCountText.text =  _playerManager.CollectableCountInALevel.ToString();
        StartStackButtonText.text = _levelManager.StartStack.ToString() + " Start Stack" + " and you can buy a stack for " + _levelManager.StackValueByCoin.ToString() + " coin";

        GoldSliderComponent.value = _collectManager.stackedCoin;
        DiamondSliderComponent.value = _collectManager.stackedDiamond;

        if (_collectManager.CollectedCoin >= _levelManager.StackValueByCoin)
        {
            StartStackButton.interactable = true;
        }
        else
        {
            StartStackButton.interactable = false;
        }
    }

    #region UI Button Options
    private void HandleNextButton()
    {
        _levelManager.NextLevel();
        //_collectManager.ActiveCollectedObject();
        //_collectManager.ActiveObstacleObject();
        _gameManager.UpdateGameState(GameState.RestartGame);
    }

    private void HandlePlayButton()
    {
        _gameManager.UpdateGameState(GameState.StartGame);
    }

    private void HandleStartStackButton()
    {
        _levelManager.StartStack++;
        _collectManager.CollectedCoin -= _levelManager.StackValueByCoin;
        _levelManager.StackValueByCoin += 20;
    }
    #endregion
}
