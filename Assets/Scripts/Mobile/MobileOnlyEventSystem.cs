public static class MobileOnlyEventSystem
{
    public static event MyEventSystem.DefaultHandler OnChatButtonPressed;
    public static event MyEventSystem.DefaultHandler OnEnterTouchKeyPressed;



    public static void RaiseChatButtonPressed()
    {
        OnChatButtonPressed?.Invoke();
    }

    public static void RaiseEnterTouchKeyPressed()
    {
        OnEnterTouchKeyPressed?.Invoke();
    }
}
