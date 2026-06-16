using UnityEngine;
using UnityEngine.UI;

namespace JamFramework
{
    /*
     * The PlatformedImage is a class made on a whim, and can be expanded if found useful
     * The idea is to have alternative components for certain Component types that vary
     * on different platform. The PlatformedImage simple has two slots for a PC and Mobile sprite,
     * swapping applying them on Start()
     * 
     */
    public class PlatformedImage : Image
    {
        public Sprite PCSprite, mobileSprite;

        protected override void Start()
        {
            base.Start();
            Apply();
        }

        public void Apply()
        {
            if (PlatformHelpers.IsPC) this.sprite = PCSprite;
            if (PlatformHelpers.IsMobile) this.sprite = mobileSprite;
        }
    }
}
