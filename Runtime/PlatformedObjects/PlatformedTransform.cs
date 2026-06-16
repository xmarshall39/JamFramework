using UnityEngine;

namespace JamFramework
{
    /*
     * Some GameObjects (especially UI) are expected to have different positioning or scaling on mobile to accomodate screen size differences
     * This component records the initial transform values on this component and adds an "offset" when on matching platforms 
     * 
     */
    public class PlatformedTransform : MonoBehaviour
    {
        public PlatformedTransformData PCData, mobileData;
        [HideInInspector]
        public PlatformedTransformData InitialData { get; private set; }

        private void Start()
        {

            Apply();
        }

        public void Apply()
        {
            PlatformedTransformData offset = PCData;
            if (PlatformHelpers.IsPC) offset = PCData;
            if (PlatformHelpers.IsMobile) offset = mobileData;

            if (offset.isGlobal)
            {
                transform.position += offset.position;
                Transform temp = transform.parent;
                transform.parent = null;
                transform.localScale += offset.scale;
                transform.parent = temp;
            }
            else
            {
                transform.localPosition += offset.position;
                transform.localScale += offset.scale;
            }
        }

    }

    [System.Serializable]
    public struct PlatformedTransformData
    {
        public bool isGlobal;
        public Vector3 position;
        public Vector3 scale;
    }
}
