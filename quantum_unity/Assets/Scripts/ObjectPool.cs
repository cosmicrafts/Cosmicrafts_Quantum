using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance { get; private set; }

    private Queue<GameObject> poolQueue = new Queue<GameObject>();

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

        if (ObjectPool.Instance == null)
        {
            Debug.LogError("ObjectPool instance is not initialized.");
        }
    }

    public GameObject GetObject(GameObject prefab, Transform parent)
    {
        Debug.Log($"Attempting to get object from pool for prefab: {prefab.name}");

        GameObject obj;
        if (poolQueue.Count > 0)
        {
            obj = poolQueue.Dequeue();
            obj.SetActive(true);
            Debug.Log("Dequeued object from pool.");
        }
        else
        {
            obj = Instantiate(prefab, parent);
            Debug.Log("Instantiated new object.");
        }

        if (obj == null)
        {
            Debug.LogError("Failed to instantiate or dequeue object.");
        }

        return obj;
    }


    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        poolQueue.Enqueue(obj);
    }
}
