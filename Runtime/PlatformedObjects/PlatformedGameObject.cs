using UnityEngine;

namespace JamFramework
{
    public class PlatformedGameObject : MonoBehaviour
    {
        public Platform targetPlatform;

        private void OnEnable()
        {
            bool allowEnable = false;
            if (targetPlatform.HasFlag(Platform.MOBLIE)) allowEnable |= PlatformHelpers.IsMobile;
            if (targetPlatform.HasFlag(Platform.PC)) allowEnable |= PlatformHelpers.IsPC;
            if (targetPlatform.HasFlag(Platform.WEBGL)) allowEnable |= PlatformHelpers.IsWebGL;
 
         
            this.gameObject.SetActive(allowEnable);
        }
    }

    public static class PlatformHelpers
    {
        public static bool IsMobile => UnityEngine.Device.Application.isMobilePlatform;

        public static bool IsWebGL => Application.platform == RuntimePlatform.WebGLPlayer;

        public static bool IsPC => Application.platform == RuntimePlatform.WindowsPlayer ||
                        Application.platform == RuntimePlatform.LinuxPlayer ||
                        Application.platform == RuntimePlatform.OSXPlayer;
    }

    [System.Flags]
    public enum Platform
    {
        PC,
        WEBGL,
        MOBLIE
    }
}
