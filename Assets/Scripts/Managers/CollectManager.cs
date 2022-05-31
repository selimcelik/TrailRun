using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using System;

public class CollectManager : Singleton<CollectManager>
{
    private PlayerManager _playerManager;
    private HitDetection _hitDetection;
    private LevelManager _levelManager;
    private ObjectPooler _objectPooler;
    private GameManager _gameManager;

    public int CollectedCoin;

    public List<GameObject> CollectedObjects = new List<GameObject>();
    public List<GameObject> ObstacleObjects = new List<GameObject>();

    public int DiamondStackLimit;
    public int CoinStackLimit;

    public int doJumpIndex;

    public int stackedCoin, stackedDiamond = 0;

    Coroutine nullCoroutineCheck;

    private void Awake()
    {
        _gameManager = GameManager.Instance;
        _playerManager = PlayerManager.Instance;
        _hitDetection = HitDetection.Instance;
        _levelManager = LevelManager.Instance;
        _objectPooler = ObjectPooler.Instance;
    }

    private void FixedUpdate()
    {
        CollectableMovement();

        if (CollectedCoin < 0)
        {
            CollectedCoin = 0;
        }
    }

    private void CollectableMovement()
    {
        if (CollectedObjects.Count > 0 && _gameManager.State == GameState.StartGame)
        {
            for (int i = 0; i < CollectedObjects.Count; i++)
            {
                if (CollectedObjects[i] == null)
                {
                    CollectedObjects.Remove(CollectedObjects[i]);
                }
            }
            CollectedObjects[0].transform.DOMoveX(_playerManager.Player.transform.GetChild(0).transform.position.x, .2f);
            CollectedObjects[0].transform.DOMoveZ(_playerManager.Player.transform.position.z - .5f, .2f);
            if (CollectedObjects.Count > 1)
            {
                for (int i = 0; i < CollectedObjects.Count - 1; i++)
                {
                    CollectedObjects[i + 1].transform.DOMoveX(CollectedObjects[i].transform.position.x, .2f);
                    CollectedObjects[i + 1].transform.DOMoveZ(CollectedObjects[i].transform.position.z - .5f, .2f);

                }
            }

        }
    }

    public void AddCoin(int amount)
    {
        CollectedCoin += amount;
    }

    /*public void ActiveCollectedObject()
    {
        for (int i = 0; i < CollectedObjects.Count; i++)
        {
            CollectedObjects[i].SetActive(true);
        }
    }*/

    /*public async void ActiveObstacleObject()
    {
        for (int i = 0; i < ObstacleObjects.Count; i++)
        {
            ObstacleObjects[i].SetActive(true);
        }
        await Task.Delay(100);
        for (int i = ObstacleObjects.Count - 1; i >= 0; i--)
        {
            ObstacleObjects.RemoveAt(i);
        }
    }*/

    public void CollectableStack()
    {
        _playerManager.CollectableCountInALevel += 1;
        nullCoroutineCheck = StartCoroutine(Rescale());
    }

    public void CollectableDrop()
    {
        if (nullCoroutineCheck != null)
        {
            StopCoroutine(nullCoroutineCheck);
            nullCoroutineCheck = null;
        }
        for (int i = doJumpIndex; i <= CollectedObjects.Count - 1; i++)
        {
            CollectedObjects[i].transform.parent = _levelManager.activeLevel.transform;
            CollectedObjects[i].transform.DOKill();
            var RandomValue1 = UnityEngine.Random.Range(-5, 5);
            var RandomValue2 = UnityEngine.Random.Range(-3, 3);
            CollectedObjects[i].transform.DOJump(new Vector3(RandomValue1, CollectedObjects[i].transform.position.y, CollectedObjects[i].transform.position.z + (RandomValue2)), 3, 1, .25f);
            CollectedObjects[i].tag = "Collectable";
            CollectedObjects[i].transform.DOScale(new Vector3(1, 1, 1), .01f);
            switch (CollectedObjects[i].gameObject.GetComponent<Collectable>().collectableType)
            {
                case Collectable.CollectableType.coin:
                    AddCoin(-_hitDetection.CollectedCoinToCoinCount);
                    stackedCoin--;
                    break;
                case Collectable.CollectableType.diamond:
                    AddCoin(-_hitDetection.CollectedDiamondToCoinCount);
                    stackedDiamond--;
                    break;
            }
            _playerManager.CollectableCountInALevel--;
            CollectedObjects.Remove(CollectedObjects[i]);
        }
        doJumpIndex = 0;
    }

    IEnumerator Rescale()
    {
        if (CollectedObjects.Count > 0)
        {
            for (int i = CollectedObjects.Count - 1; i >= 0; i--)
            {
                CollectedObjects[i].tag = "Collector";
                CollectedObjects[i].transform.DOScale(new Vector3(1f, 1, 1), .01f).OnComplete(() => CollectedObjects[i].transform.DOScale(new Vector3(.5f, .5f, .5f), .01f));
                yield return new WaitForSeconds(0f);
            }
        }
    }

    public void GameStartWithStack()
    {
        int ChooseStackType = UnityEngine.Random.Range(0, 2);

        if(_levelManager.StartStack > 0)
        {
            for (int i = 0; i < _levelManager.StartStack; i++)
            {
                if(ChooseStackType == 0)
                {
                    GameObject startStackGO = _objectPooler.SpawnForGameObject("Gold", Vector3.zero, Quaternion.identity, null);
                    startStackGO.transform.DOMoveY(0.5f, .1f);
                    //CollectedObjects.Add(startStackGO);
                }
                if (ChooseStackType == 1)
                {
                    GameObject startStackGO = _objectPooler.SpawnForGameObject("Diamond", Vector3.zero,Quaternion.identity,null);
                    startStackGO.transform.DOMoveY(0.5f, .1f);
                    //CollectedObjects.Add(startStackGO);
                }
            }
        }
    }
}
