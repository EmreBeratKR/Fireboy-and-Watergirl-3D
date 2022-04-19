using UnityEngine;

public class EnterFunctionalKey : FunctionalTouchKey
{
    public override FunctionalTouchKeyType Type => FunctionalTouchKeyType.Enter;

    public override void OnPressed()
    {
        MobileOnlyEventSystem.RaiseEnterTouchKeyPressed();

        TouchKeyboard.Hide();
    }
}
