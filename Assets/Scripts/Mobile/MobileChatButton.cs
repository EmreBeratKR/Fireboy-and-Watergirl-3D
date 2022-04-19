using UnityEngine;

public class MobileChatButton : MonoBehaviour
{
    public void OnPressed()
    {
        MobileOnlyEventSystem.RaiseChatButtonPressed();
    }
}
