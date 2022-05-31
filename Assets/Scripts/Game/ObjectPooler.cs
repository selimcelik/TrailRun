using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : Singleton<ObjectPooler>
{
    [System.Serializable]
    public class Pool
    {
        public string name;
        public List<GameObject> prefab;
        public int size;
    }

    public List<Pool> pools;

    public Dictionary<string, Queue<GameObject>> poolDictionary;

    public GameObject poolParent;

    public GameObject SpawnForGameObject(string name, Vector3 position, Quaternion rotation, Transform parent)
    {
        GameObject objectToSpawn = poolDictionary[name].Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.parent = parent;
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        poolDictionary[name].Enqueue(objectToSpawn);
        return objectToSpawn;
    }

    public void CreatePoolObjects()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                for (int a = 0; a < pool.prefab.Count; a++)
                {
                    GameObject obj = Instantiate(pool.prefab[a], poolParent.transform.GetChild(0).transform);
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                }
            }
            poolDictionary.Add(pool.name, objectPool);
        }
    }


}