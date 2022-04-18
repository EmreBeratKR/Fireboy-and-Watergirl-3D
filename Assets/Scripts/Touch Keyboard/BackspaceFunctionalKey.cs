using UnityEngine;

public class BackspaceFunctionalKey : FunctionalTouchKey
{
    public override FunctionalTouchKeyType Type => FunctionalTouchKeyType.Backspace;

    public override void OnPressed()
    {
        TouchKeyboard.RemoveChar();
    }
}
