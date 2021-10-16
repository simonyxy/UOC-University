using System.Collections.Generic;
using UnityEngine;

public class DontDestroyHelper : MonoBehaviour
{
    private static List<Object> _cacheGameObjects;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private static void Init()
    {
        _cacheGameObjects = new List<Object>();
        GameObject go = new GameObject();
        go.name = "DontDestroyHelper";
        go.AddComponent<DontDestroyHelper>();
    }

    public static void DontDestroy(Object go)
    {
        if(null == _cacheGameObjects)
        {
            Init();
        }

        if(null == go) return;

        DontDestroyOnLoad(go);
        _cacheGameObjects.Add(go);
    }

    public static void DestroyAll()
    {
        if(null == _cacheGameObjects)
        {
            Init();
        }

        foreach(var go in _cacheGameObjects)
        {
            if(null == go) continue;
            GameObject.Destroy(go);
        }
        _cacheGameObjects.Clear();
    }
}