using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    #region Singleton
    private static ObjectPooler _instance;
    public static ObjectPooler Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    public int groundCount = 1; // Also being accessed via the RespawnManager.cs script.
    private float spawnGroundYPos = -170.567f;
    private float spawnGroundZPos = 468.6f;

    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        objectToSpawn.SetActive(true);
        // Spawn position is multiplied by groundCount which will move the platform ahead each time a new ground is spawned.
        objectToSpawn.transform.SetPositionAndRotation(new Vector3(0f, spawnGroundYPos * groundCount, spawnGroundZPos * groundCount), Quaternion.Euler(0, 0, 0));
        // Ground has been spawned, now iterate the counter to update what to multiply the value by.
        groundCount++;

        poolDictionary[tag].Enqueue(objectToSpawn);
        return objectToSpawn;
    }
}