using ImGuiNET;
using UnityEngine;

namespace JamFramework
{
    public abstract class DebugMenuCategory : MonoBehaviour
    {
        public abstract string CategoryName();

        public abstract bool Draw(bool p_open);
    }
}
