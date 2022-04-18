using System;
using UnityEngine;
using UnityEngine.UI;

public class ShiftFunctionalKey : FunctionalTouchKey
{
    public override FunctionalTouchKeyType Type => FunctionalTouchKeyType.Shift;

    [SerializeField] private ShiftPlaceholders placeholders;
    [SerializeField] private Image placeholder;
    private ShiftKeyState state;


    public override void OnPressed()
    {
        if (state == ShiftKeyState.PermaUpper)
        {
            state = ShiftKeyState.Lower;
        }
        else
        {
            state = ((ShiftKeyState)(state + 1));
        }

        UpdateKeys();

        UpdatePlaceHolder();
    }


    public void OnCharAppended()
    {
        if (state == ShiftKeyState.TempUpper)
        {
            state = ShiftKeyState.Lower;
        }

        UpdateKeys();

        UpdatePlaceHolder();
    }

    private void Start()
    {
        state = ShiftKeyState.Lower;
    }

    private void UpdateKeys()
    {
        if (state == ShiftKeyState.Lower)
        {
            TouchKeyboard.AllLower();
            return;
        }

        TouchKeyboard.AllUpper();
    }

    private void UpdatePlaceHolder()
    {
        placeholder.sprite = placeholders.Get(state);
    }

}

internal enum ShiftKeyState
{
    Lower,
    TempUpper,
    PermaUpper
}

[Serializable]
internal struct ShiftPlaceholders
{
    [SerializeField] private Sprite lower;
    [SerializeField] private Sprite tempUpper;
    [SerializeField] private Sprite permaUpper;


    public Sprite Get(ShiftKeyState state)
    {
        if (state == ShiftKeyState.Lower) return lower;

        if (state == ShiftKeyState.TempUpper) return tempUpper;

        return permaUpper;
    }
}