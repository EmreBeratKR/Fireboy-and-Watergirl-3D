using UnityEngine;

public class EnterFunctionalKey : FunctionalTouchKey
{
    public override FunctionalTouchKeyType Type => FunctionalTouchKeyType.Enter;

    public override void OnPressed()
    {
        TouchKeyboard.Hide();
    }
}
