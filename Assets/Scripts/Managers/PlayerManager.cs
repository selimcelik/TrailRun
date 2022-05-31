using System.Threading.Tasks;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    private GameManager _gameManager;
    private CollectManager _collectManager;

    public GameObject Player;
    private GameObject playerMesh;
    [HideInInspector]
    public float PlayerSpeed;

    public float DefaultPlayerSpeed = 6;

    public int CollectableCountInALevel = 0;

    Animator _animator;

    private void Awake()
    {
        _gameManager = GameManager.Instance;
        _collectManager = CollectManager.Instance;
    }

    private void Start()
    {
        playerMesh = Player.transform.GetChild(0).gameObject;
        _animator = playerMesh.GetComponent<Animator>();
    }

    private void Update()
    {
        if (_collectManager.CoinStackLimit <= _collectManager.stackedCoin &&
            _collectManager.DiamondStackLimit <= _collectManager.stackedDiamond)
        {
            if (_animator.GetBool("canRun"))
            {
                _animator.SetBool("canRun", false);
                _animator.SetBool("canRun2", true);
            }
        }

        else
        {
            if (_animator.GetBool("canRun2"))
            {
                _animator.SetBool("canRun", true);
                _animator.SetBool("canRun2", false);
            }

        }
    }


    #region Player Start And Stop Options
    public void StopPlayer()
    {
        PlayerSpeed = 0;
    }

    public void StartPlayer()
    {
        PlayerSpeed = DefaultPlayerSpeed;
        _animator.SetBool("canRun", true);
        CollectableCountInALevel = 0;
    }

    public async void RestartPlayer()
    {
        _animator.SetBool("canRun", false);
        _animator.SetBool("canRun2", false);
        _animator.SetBool("canWin", false);
        Player.transform.position = new Vector3(0, 0, 0);
        playerMesh.transform.localPosition = new Vector3(0, 0, 0);
        await Task.Delay(1000);
        _animator.SetBool("canRun", true);
        _gameManager.UpdateGameState(GameState.StartGame);
    }
    #endregion

}
