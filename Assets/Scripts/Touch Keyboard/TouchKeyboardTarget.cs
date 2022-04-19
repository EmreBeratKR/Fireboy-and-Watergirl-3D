using System;
using UnityEngine;

public abstract class TouchKeyboardTarget : MonoBehaviour
{
    public TouchInputValidation validation;
    public abstract bool IsFull { get; }
    public abstract string Content { get; }
    public abstract bool TryAppendChar(char input);
    public abstract bool TryRemoveChar();

    protected void OnSelect(string content)
    {
        TouchKeyboard.Show();
    }
}

[Serializable]
public struct TouchInputValidation
{
    [SerializeField] private bool allowLetter;
    [SerializeField] private bool allowDigit;
    [SerializeField] private bool allowSpace;
    [SerializeField] private bool allowPunctuation;


    public bool IsValid(char input)
    {
        if (char.IsLetter(input) && !allowLetter) return false;

        if (char.IsDigit(input) && !allowDigit) return false;

        if (input == ' ' && !allowSpace) return false;

        if (char.IsPunctuation(input) && !allowPunctuation) return false;

        return true;
    }
}