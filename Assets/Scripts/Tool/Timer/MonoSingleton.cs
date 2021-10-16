using System;
using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour , IDisposable where T :Component
{
    static string _postFix = @"Singleton";
    protected static T _instance;
    public static T instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<T>();
                if(_instance == null)
                {
                    string instanceName = typeof(T).Name + _postFix;
                    GameObject go = new GameObject(instanceName);
                    _instance = go.AddComponent<T>();
                    DontDestroy();
                }
            }
            return _instance;
        }
    }

    //注意子类gameObject 是否有被删除
    //建议不要集成Awake，逻辑写在Start,或者base.Awake
    protected virtual void Awake()
    {
        if(_instance == null)
        {
            _instance = GetComponent<T>();
            DontDestroy();
        }
        else if(_instance.gameObject != gameObject)
        {
            selfDestroy(gameObject);
            return;
        }
    }

    public static void selfDestroy(UnityEngine.Object obj,bool isGameObject =true)
    {
        if(isGameObject)
        {
            obj = GetGameObject(obj);
        }
        if(obj == null)
        {
            return;
        }
        #if UNITY_EDITOR
        if(Application.isPlaying)
        {
            Destroy(obj);
        }
        #endif
        else
        {
            DestroyImmediate(obj);
        }
        #if UNITY_EDITOR
        #endif
    }

    public static GameObject GetGameObject(UnityEngine.Object obj)
    {
        switch(obj)
        {
            case GameObject go:
            return go;

            case Component comp:
            return comp.gameObject;

            default:
                return null;
        }
    }

    protected virtual void OnDestroy()
    {
        if(_instance != null && _instance.gameObject == gameObject)
        {
            
        #if UNITY_EDITOR
        Debug.Log("Destroy" + gameObject.name);
        _instance =null;
        #endif
        }
    }

    public void Dispose()
    {
        selfDestroy(_instance);
    }


    public static void DontDestroy()
    {
        
        #if UNITY_EDITOR
        if(Application.isPlaying)
        {
            DontDestroyHelper.DontDestroy(_instance);
        }
        #endif
    }
}