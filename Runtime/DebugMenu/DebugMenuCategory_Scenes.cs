using ImGuiNET;
using UnitySceneManagement = UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;
using UnityEngine;

namespace JamFramework
{
    public class DebugMenuCategory_Scenes : DebugMenuCategory
    {
        public bool fadeMusicOnLoad = true;
        public UnitySceneManagement.LoadSceneMode loadMode;

        public override string CategoryName() => "Scenes";

        public override bool Draw(bool p_open)
        {
            var SM = SceneManager.Instance;
            Vector2 windowSize = ImGui.GetWindowSize();
            if (ImGui.BeginTabItem(CategoryName()))
            {
                ImGui.Text("Scenes in Build");
                ImGui.Checkbox("Fade Music", ref fadeMusicOnLoad);

                int mode = (int)loadMode;
                if (ImGui.Combo("Load Mode", ref mode, System.Enum.GetNames(typeof(UnitySceneManagement.LoadSceneMode)), System.Enum.GetValues(typeof(UnitySceneManagement.LoadSceneMode)).Length))
                {
                    loadMode = (UnitySceneManagement.LoadSceneMode)mode;
                }
                ImGui.PushID("Scenes");
                if (ImGui.BeginListBox("", windowSize/3f))
                {
                    for (int i = 0; i < UnitySceneManager.sceneCountInBuildSettings; ++i)
                    {
                        ImGui.PushID(i);
                        string scenePath = UnitySceneManagement.SceneUtility.GetScenePathByBuildIndex(i);
                        string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
                        ImGui.Text(sceneName);
                        ImGui.SameLine();
                        if (ImGui.Button("Load"))
                        {
                            SM.LoadScene(i, loadMode, fadeMusicOnLoad);
                        }
                        ImGui.PopID();
                    }

                    ImGui.EndListBox();
                }
                ImGui.PopID();
                ImGui.EndTabItem();
                return true;
            }

            return false;
        }
    }
}
