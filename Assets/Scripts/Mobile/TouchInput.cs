public class TouchInput : Scenegleton<TouchInput>
{
    private bool isRight;
    private bool isLeft;
    private bool isJump;
    private bool isPull;

    public static bool IsRight => Instance.isRight;
    public static bool IsLeft => Instance.isLeft;
    public static bool IsJump => Instance.isJump;
    public static bool IsPull => Instance.isPull;


    public void OnRightButtonDown()
    {
        isRight = true;
    }

    public void OnRightButtonUp()
    {
        isRight = false;
    }

    public void OnLeftButtonDown()
    {
        isLeft = true;
    }

    public void OnLeftButtonUp()
    {
        isLeft = false;
    }

    public void OnJumpButtonDown()
    {
        isJump = true;
    }

    public void OnJumpButtonUp()
    {
        isJump = false;
    }

    public void OnPullButtonDown()
    {
        isPull = true;
    }

    public void OnPullButtonUp()
    {
        isPull = false;
    }
}
