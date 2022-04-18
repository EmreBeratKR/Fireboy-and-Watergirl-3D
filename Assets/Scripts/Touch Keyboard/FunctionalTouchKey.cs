using UnityEngine;

public abstract class FunctionalTouchKey : TouchKey
{
    public abstract FunctionalTouchKeyType Type { get; }
}

public enum FunctionalTouchKeyType 
{
    Enter,
    Backspace,
    Shift
}