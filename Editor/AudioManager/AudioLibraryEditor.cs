using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace JamFramework.Editor
{
    [CustomEditor(typeof(AudioLibrary))]
    public class AudioLibraryEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var audioLib = (AudioLibrary)target;

            base.OnInspectorGUI();

            if(GUILayout.Button("Run Codegen"))
            {
                audioLib.RunCodegen();
            }

        }
    }
}
