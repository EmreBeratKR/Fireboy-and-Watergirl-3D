using UnityEngine;

public class MobileOnly : MonoBehaviour
{
    private void Start()
    {
        if (WebGLChecker.isMobile) return;

        Destroy(gameObject);
    }
}
