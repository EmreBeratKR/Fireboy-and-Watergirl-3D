using UnityEngine;

/// <summary>
/// A generic Scenegleton abstract base class.
/// Scenegletons are static Component Instances which are accessible only from the scene they in.
/// </summary>
public abstract class Scenegleton<T> : MonoBehaviour where T : Component
{
    public static T Instance;


    protected virtual void Awake() => Instance = this as T;
}
