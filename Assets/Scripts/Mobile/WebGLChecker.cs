using System.Runtime.InteropServices;

public class WebGLChecker
{
    [DllImport("__Internal")]
    private static extern bool IsMobile();

    public static bool isMobile
    {
        get
        {
            #if !UNITY_EDITOR && UNITY_WEBGL
                return IsMobile();
            #endif
            return false;
        }
    }
}
