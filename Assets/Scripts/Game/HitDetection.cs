using DG.Tweening;
using UnityEngine;

public class HitDetection : Singleton<HitDetection>
{
    private CollectManager _collectManager;
    private GameManager _gameManager;
    private PlayerManager _playerManager;
    private ObjectPooler _objectPooler;
    private LevelManager _levelManager;

    public PlayerMovementController playerMovementController;

    public GameObject Player;

    public int PlayerDamageCount = 1;

    public int CollectedDiamondToCoinCount;

    public int CollectedCoinToCoinCount;



    private void Awake()
    {
        _collectManager = CollectManager.Instance;
        _gameManager = GameManager.Instance;
        _playerManager = PlayerManager.Instance;
        _objectPooler = ObjectPooler.Instance;
        _levelManager = LevelManager.Instance;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Collectable")
        {
            switch (other.GetComponent<Collectable>().collectableType)
            {

                case Collectable.CollectableType.diamond:
                    if (_collectManager.DiamondStackLimit > _collectManager.stackedDiamond)
                    {
                        other.transform.DOScale(new Vector3(.5f, .5f, .5f), .1f);

                        _collectManager.AddCoin(CollectedDiamondToCoinCount);
                        _collectManager.CollectedObjects.Add(other.gameObject);
                        _collectManager.stackedDiamond++;
                        other.GetComponent<BoxCollider>().isTrigger = true;
                        other.gameObject.transform.parent = gameObject.transform.parent.transform;
                        _collectManager.CollectableStack();
                        GameObject particleGO = _objectPooler.SpawnForGameObject("MoneyCoinBlast", other.gameObject.transform.position, Quaternion.identity, _objectPooler.poolParent.transform.GetChild(0).transform);
                        Destroy(particleGO, 1);
                    }
                    break;
                case Collectable.CollectableType.coin:
                    if(_collectManager.CoinStackLimit > _collectManager.stackedCoin)
                    {
                        other.transform.DOScale(new Vector3(.5f, .5f, .5f), .1f);

                        _collectManager.AddCoin(CollectedCoinToCoinCount);
                        _collectManager.CollectedObjects.Add(other.gameObject);
                        _collectManager.stackedCoin++;
                        other.GetComponent<BoxCollider>().isTrigger = true;
                        other.gameObject.transform.parent = gameObject.transform.parent.transform;
                        _collectManager.CollectableStack();
                        GameObject particleGO1 = _objectPooler.SpawnForGameObject("MoneyCoinBlast", other.gameObject.transform.position, Quaternion.identity, _objectPooler.poolParent.transform.GetChild(0).transform);
                        Destroy(particleGO1, 1);

                    }
                    break;
            }

        }

        if (other.tag == "Finish")
        {
            _gameManager.UpdateGameState(GameState.WinGame);
            for (int i = 0; i < _playerManager.Player.transform.childCount; i++)
            {
                if (_playerManager.Player.transform.GetChild(i).GetComponent<Collectable>())
                {
                    Destroy(_playerManager.Player.transform.GetChild(i).gameObject);
                }

            }
            Player.GetComponent<Animator>().SetBool("canWin", true);
        }
    }
}
