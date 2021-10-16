using System;
public abstract class Singleton<T>:IDisposable where T:class,new()
{
    static T _instance = default(T);

    public static T instance
    {
        get => _instance ??(_instance = new T());
        set => _instance = value;
    }

    public virtual void Dispose()
    {
        _instance = null;
    }
}