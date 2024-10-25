using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance { get; private set; }

    private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void CreatePool(GameObject prefab, int initialSize)
    {
        string poolKey = prefab.name;

        if (!poolDictionary.ContainsKey(poolKey))
        {
            poolDictionary[poolKey] = new Queue<GameObject>();

            for (int i = 0; i < initialSize; i++)
            {
                GameObject newObj = Instantiate(prefab);
                newObj.SetActive(false);
                poolDictionary[poolKey].Enqueue(newObj);
            }
        }
    }

    public GameObject GetObject(GameObject prefab, Transform parent, Vector3 position, Quaternion rotation)
    {
        string poolKey = prefab.name;

        if (!poolDictionary.ContainsKey(poolKey))
        {
            CreatePool(prefab, 10);
        }

        GameObject obj;
        if (poolDictionary[poolKey].Count > 0)
        {
            obj = poolDictionary[poolKey].Dequeue();
        }
        else
        {
            obj = Instantiate(prefab);
        }

        obj.transform.SetParent(parent);
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);

        return obj;
    }

    public void ReturnObject(GameObject obj)
    {
        string poolKey = obj.name.Replace("(Clone)", "").Trim();

        obj.SetActive(false);
        poolDictionary[poolKey].Enqueue(obj);
    }
}
