using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Collectable : MonoBehaviour
{
    private CollectManager _collectManager;
    private HitDetection _hitDetection;
    private ObjectPooler _objectPooler;

    public enum CollectableType
    {
        diamond,
        coin,
    }
    public CollectableType collectableType;

    private void Awake()
    {
        _collectManager = CollectManager.Instance;
        _hitDetection = HitDetection.Instance;
        _objectPooler = ObjectPooler.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Collectable" && gameObject.tag == "Collector")
        {
            if (other.gameObject.GetComponent<Collectable>().collectableType == CollectableType.coin)
            {
                if(_collectManager.CoinStackLimit > _collectManager.stackedCoin)
                {
                    other.transform.DOScale(new Vector3(.5f, .5f, .5f), .1f);

                    _collectManager.AddCoin(_hitDetection.CollectedCoinToCoinCount);
                    _collectManager.stackedCoin++;
                    _collectManager.CollectedObjects.Add(other.gameObject);
                    other.gameObject.GetComponent<BoxCollider>().isTrigger = true;
                    other.gameObject.transform.parent = gameObject.transform.parent.transform;
                    _collectManager.CollectableStack();
                    GameObject particleGO = _objectPooler.SpawnForGameObject("MoneyCoinBlast", other.gameObject.transform.position, Quaternion.identity, _objectPooler.poolParent.transform.GetChild(0).transform);
                    Destroy(particleGO, 1);
                }

            }
            if(other.gameObject.GetComponent<Collectable>().collectableType == CollectableType.diamond)
            {
                if (_collectManager.DiamondStackLimit > _collectManager.stackedDiamond)
                {
                    other.transform.DOScale(new Vector3(.5f, .5f, .5f), .1f);

                    _collectManager.AddCoin(_hitDetection.CollectedDiamondToCoinCount);
                    _collectManager.stackedDiamond++;

                    _collectManager.CollectedObjects.Add(other.gameObject);
                    other.gameObject.GetComponent<BoxCollider>().isTrigger = true;
                    other.gameObject.transform.parent = gameObject.transform.parent.transform;
                    _collectManager.CollectableStack();
                    GameObject particleGO1 = _objectPooler.SpawnForGameObject("MoneyCoinBlast", other.gameObject.transform.position, Quaternion.identity, _objectPooler.poolParent.transform.GetChild(0).transform);
                    Destroy(particleGO1, 1);
                }
            }

        }

        if(gameObject.tag == "Collector" && other.tag == "Obstacle")
        {
            _collectManager.CollectableDrop();
            GameObject particleGO = _objectPooler.SpawnForGameObject("MysticExplosionWhite", other.gameObject.transform.position, Quaternion.identity, _objectPooler.poolParent.transform.GetChild(0).transform);
            Destroy(particleGO, 1);

        }
    }

    
}
