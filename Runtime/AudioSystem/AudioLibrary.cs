using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System;
using MacFsWatcher;

namespace JamFramework
{
    [CreateAssetMenu(fileName = "AudioLibrary", menuName = "Scriptable Objects/JamFramework/AudioLibrary")]
    public class AudioLibrary : ScriptableObject
    {
        [SerializeField] Sound fallbackSound;
        
        [SerializeField] List<Sound> sounds;

        [Tooltip("Something simple like \"Music\" or \"SFX\" will do")]
        public string Name;
        public Sound Fallback { get => fallbackSound; }
        public List<Sound> Sounds { get => sounds; }

        public bool Find(string soundName, out Sound sound)
        {
            sound = sounds.Find(x => x.name == soundName);
            if (sound == null)
            {
                sound = fallbackSound;
                Debug.LogWarning($"\"{soundName}\" not found in the SFX Library!! Using Fallback instead...");
                
                if (fallbackSound == null)
                {
                    Debug.LogWarning("SFX Library does not have a fallback sound! Please assign one");
                    return false;
                }

                return true;
            }
            return true;
        }
        public bool Find(Enum audioEnum, out Sound sound)
        {
            int enumInt = ((IConvertible)audioEnum).ToInt32(null);
            sound = sounds.Find(x => x.enumKey == enumInt);

            if (sound == null)
            {
                sound = fallbackSound;
                Debug.LogWarning($"\"{audioEnum}\" not found in the SFX Library!! Using Fallback instead...");

                if (fallbackSound == null)
                {
                    Debug.LogWarning("SFX Library does not have a fallback sound! Please assign one");
                    return false;
                }

                return true;
            }
            return true;
        }


#if UNITY_EDITOR
        
        public void RunCodegen()
        {
            if (string.IsNullOrEmpty(Name))
            {
                Debug.LogError("Please give this AudioLibrary a name to use CodeGen!!");
                return;
            }
            // If it not present, create a new file called AudioLibrary.Codegen.cs
            // If it is present, read the file and parse it and cache all the enum data
            // Then update the cache with this Lib's sounds
            // Finally, write the cache back to a new copy of the file
            Dictionary<string, List<string>> allEnums = new Dictionary<string, List<string>>();
            string fileDirectory = Path.Combine(Application.dataPath, "Codegen");
            string filepath = Path.Combine(fileDirectory, "AudioLibrary.Codegen.cs");
            if (File.Exists(filepath))
            {
                // Load File into dictionary
                string[] fileData = File.ReadAllLines(filepath);
                string currentLib = string.Empty;
                foreach (var line in fileData)
                {
                    if (line.StartsWith("//END_LIB")) break;

                    if (line.StartsWith("//START_LIB:"))
                    {
                        currentLib = line.Split(":")[1];
                        continue;
                    }

                    if (!string.IsNullOrEmpty(currentLib))
                    {
                        if (!allEnums.ContainsKey(currentLib))
                        {
                            allEnums[currentLib] = new List<string>();
                        }

                        allEnums[currentLib].Add(line.Replace(",", ""));
                    }
                }
            }
            int i = 0;
            allEnums[Name] = new List<string>();
            
            for (i = 0; i < sounds.Count; ++i)
            {
                allEnums[Name].Add(sounds[i].GetCodegenName(Name));
            }

            MD5 md5Hasher = MD5.Create();

            StringBuilder codeFile = new StringBuilder();
            codeFile.AppendLine($"// AUTO-GENERATED FILE. DO NOT EDIT!!");
            codeFile.AppendLine("public enum AudioEnums");
            codeFile.AppendLine("{");
            i = 0;
            foreach (var kvp in allEnums)
            {
                codeFile.AppendLine($"//START_LIB:{kvp.Key}");
                for(int j = 0; j < kvp.Value.Count; ++j)
                {
                    bool lastOne = i == allEnums.Count - 1 && j == kvp.Value.Count - 1;

                    // 
                    if (!kvp.Value[j].Contains("="))
                    {
                        var hashed = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(kvp.Value[j]));
                        var ivalue = BitConverter.ToInt32(hashed, 0);

                        codeFile.AppendLine($"{kvp.Value[j]} = {ivalue}{(lastOne ? "" : ",")}");
                    }
                    else
                    {
                        codeFile.AppendLine($"{kvp.Value[j]}{(lastOne ? "" : ",")}");

                    }
                }
                ++i;
            }
            codeFile.AppendLine("//END_LIB");
            codeFile.AppendLine("}");

            if (!Directory.Exists(fileDirectory)) Directory.CreateDirectory(fileDirectory);
            File.WriteAllText(filepath, codeFile.ToString());
            for (i = 0; i < sounds.Count; ++i)
            {
                var hashed = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(sounds[i].GetCodegenName(Name)));
                sounds[i].enumKey = BitConverter.ToInt32(hashed, 0);
            }
        }
#endif
    }
}
