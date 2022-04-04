using UnityEngine;

/// <summary>
/// A generic Singleton abstract base class.
/// Singletons are static Component Instances which are accessible from all scenes.
/// </summary>
public abstract class Singleton<T> : MonoBehaviour where T : Component
{
    public static T Instance;


    protected virtual void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            Instance = this as T;
            return;
        }
        
        Destroy(this.gameObject);
    }
}