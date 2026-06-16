using ImGuiNET;
using UnityEngine;

namespace JamFramework
{
    public class DebugMenuCategory_Essentials : DebugMenuCategory
    {
        public override string CategoryName() => "Essentials";
        public override bool Draw(bool p_open)
        {
            if (ImGui.BeginTabItem(CategoryName(), ref p_open))
            {
                
                ImGui.EndTabItem();
                return true;
            }

            return false;
        }
    }
}
