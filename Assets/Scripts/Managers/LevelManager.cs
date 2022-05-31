using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{

    private CollectManager _collectManager;
    private GameManager _gameManager;

    public int LevelNumber;
    public int StartStack;
    public int StackValueByCoin;

    [HideInInspector]
    public int MaxLevel;

    public int DisplayLevelNumber = 1;
    private int _coin;
    //private int _diamond5side;

    public GameObject LevelHolder;
    [HideInInspector]
    public GameObject[] SpawnedLevels;

    public GameObject[] Levels;

    public GameObject activeLevel;

    
    
    private void Awake()
    {
        _gameManager = GameManager.Instance;
        _collectManager = CollectManager.Instance;
        MaxLevel = Levels.Length - 1;
    }

    void Start()
    {
        LoadLevel();
        NextLevelCall();
    }

    public void NextLevel()
    {
        if (LevelNumber == MaxLevel)
        {
            LevelNumber = 0;
        }
        else
        {
            LevelNumber++;
        }
        DisplayLevelNumber++;
        SaveLevel();
        NextLevelCall();
    }

    public void NextLevelCall()
    {
        SpawnedLevels = GameObject.FindGameObjectsWithTag("Level");
        for (int i = 0; i < SpawnedLevels.Length; i++)
        {
            Destroy(SpawnedLevels[i].gameObject);
        }

        activeLevel = Instantiate(Levels[LevelNumber].gameObject, LevelHolder.transform);

    }

    public void RestartLevel()
    {
        SpawnedLevels = GameObject.FindGameObjectsWithTag("Level");
        for (int i = 0; i < SpawnedLevels.Length; i++)
        {
            Destroy(SpawnedLevels[i].gameObject);
        }
        activeLevel = Instantiate(Levels[LevelNumber].gameObject, LevelHolder.transform);
    }


    public void SaveLevel()
    {
        PlayerPrefs.SetInt("Level", LevelNumber);
        PlayerPrefs.SetInt("LevelNumber", DisplayLevelNumber);
        PlayerPrefs.SetInt("Coin", _collectManager.CollectedCoin);
        PlayerPrefs.SetInt("StartStack", StartStack);
        PlayerPrefs.SetInt("StackValue", StackValueByCoin);
        PlayerPrefs.Save();
    }

    public void LoadLevel()
    {
        if (PlayerPrefs.HasKey("Coin"))
        {
            _coin = PlayerPrefs.GetInt("Coin");
            _collectManager.CollectedCoin = _coin;
        }
        if (PlayerPrefs.HasKey("Level"))
        {
            LevelNumber = PlayerPrefs.GetInt("Level");
        }
        if (PlayerPrefs.HasKey("LevelNumber"))
        {
            DisplayLevelNumber = PlayerPrefs.GetInt("LevelNumber");
        }
        if (PlayerPrefs.HasKey("StartStack"))
        {
            StartStack = PlayerPrefs.GetInt("StartStack");
        }
        if (PlayerPrefs.HasKey("StackValue"))
        {
            StackValueByCoin = PlayerPrefs.GetInt("StackValue");
        }
        if (DisplayLevelNumber > 1)
        {
            Time.timeScale = 1;
        }
    }
}
