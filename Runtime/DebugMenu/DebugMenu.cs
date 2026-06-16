using System.Collections.Generic;   
using UnityEngine;
using UnityEngine.InputSystem;
using UImGui;
using ImGuiNET;

namespace JamFramework
{
    public class DebugMenu : MonoBehaviour
    {
        public bool showDebugMenu;
        public ImGuiTabBarFlags tabBarFlags = ImGuiTabBarFlags.NoCloseWithMiddleMouseButton | ImGuiTabBarFlags.FittingPolicyScroll;

        public List<DebugMenuCategory> debugMenuCategories;
        public int selectedTab = 0;

        private void Awake()
        {
            UImGuiUtility.Layout += OnLayout;
            UImGuiUtility.OnInitialize += OnInitialize;
            UImGuiUtility.OnDeinitialize += OnDeinitialize;
        }

        private void OnLayout(UImGui.UImGui obj)
        {
            if (!showDebugMenu) return;
            ImGui.Begin("Debug Menu");
            if (ImGui.BeginTabBar("DebugTabBar"))
            {
                foreach (var category in debugMenuCategories)
                {
                    category.Draw(true);
                }
                ImGui.EndTabBar();
            }

            ImGui.End();
        }

        private void OnInitialize(UImGui.UImGui obj)
        {
            // runs after UImGui.OnEnable();
        }

        private void OnDeinitialize(UImGui.UImGui obj)
        {
            // runs after UImGui.OnDisable();
        }

        private void OnDisable()
        {
            UImGuiUtility.Layout -= OnLayout;
            UImGuiUtility.OnInitialize -= OnInitialize;
            UImGuiUtility.OnDeinitialize -= OnDeinitialize;
        }

        private void Update()
        {
            if (Keyboard.current[Key.Backquote].wasPressedThisFrame)
            {
                showDebugMenu = !showDebugMenu;
            }
        }
    }
}
