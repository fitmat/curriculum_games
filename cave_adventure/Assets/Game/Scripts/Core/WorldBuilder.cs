using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class WorldBuilder : MonoBehaviour
    {
        [System.Serializable]
        public class Pool
        {
            public string tag;
            public GameObject envPrefab;
            public int size;
        }

        // required variables
        public List<Pool> pools;
        public Dictionary<string, Queue<GameObject>> poolDictionary;

        #region Singleton
        public static WorldBuilder Instance;

        private void Awake()
        {
            Instance = this;
        }
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            poolDictionary = new Dictionary<string, Queue<GameObject>>();

            foreach (Pool pool in pools)
            {
                Queue<GameObject> objectPool = new Queue<GameObject>();

                for (int i = 0; i < pool.size; i++)
                {
                    GameObject obj = Instantiate(pool.envPrefab);
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                }

                poolDictionary.Add(pool.tag, objectPool);
            }

            Debug.Log("dictionary size : " + poolDictionary.Count);
            Debug.Log("if pool exist : " + poolDictionary.ContainsKey("enOne"));
        }

        public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
        {
            if (!poolDictionary.ContainsKey(tag))
            {
                Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
                return null;
            }

            GameObject objectToSpawn = poolDictionary[tag].Dequeue();

            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;

            IPooledEnviron pooledObj = objectToSpawn.GetComponent<IPooledEnviron>();
            pooledObj.onObjectSpawn();

            poolDictionary[tag].Enqueue(objectToSpawn);

            return objectToSpawn;
        }
    }
}