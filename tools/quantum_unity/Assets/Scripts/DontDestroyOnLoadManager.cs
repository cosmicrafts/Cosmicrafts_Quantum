using UnityEngine;
using System.Collections.Generic;

public class DontDestroyOnLoadManager : MonoBehaviour
{
    private static DontDestroyOnLoadManager instance;
    private List<GameObject> dontDestroyOnLoadObjects = new List<GameObject>();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public static void MarkAsDontDestroyOnLoad(GameObject obj)
    {
        if (instance == null)
        {
            instance = new GameObject("DontDestroyOnLoadManager").AddComponent<DontDestroyOnLoadManager>();
            DontDestroyOnLoad(instance.gameObject);
        }

        DontDestroyOnLoad(obj);
        instance.dontDestroyOnLoadObjects.Add(obj);
    }

    public static void DestroyAll()
    {
        if (instance != null)
        {
            foreach (var obj in instance.dontDestroyOnLoadObjects)
            {
                if (obj != null)
                {
                    Destroy(obj);
                }
            }
            instance.dontDestroyOnLoadObjects.Clear();
        }
    }
}
