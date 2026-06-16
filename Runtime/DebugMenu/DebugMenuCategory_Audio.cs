using ImGuiNET;
using UnityEngine;

namespace JamFramework
{
    public class DebugMenuCategory_Audio : DebugMenuCategory
    {
        public bool music_playFromCurrentPos;

        public override string CategoryName() => "Audio";
        public override bool Draw(bool p_open)
        {
            var AM = AudioManager.Instance;
            Vector2 windowSize = ImGui.GetWindowSize();
            if (ImGui.BeginTabItem(CategoryName(), ref p_open))
            {
                ImGui.Text($"Current Song: {(AM.MainMusicSource.clip != null ? AM.MainMusicSource.clip.name : "NULL")}");
                float musicVol = AM.GetVolume(AudioManager.MUSIC_VOLUME_KEY);
                float sfxVol = AM.GetVolume(AudioManager.SFX_VOLUME_KEY);

                if (ImGui.SliderFloat("Music Volume", ref musicVol, 0.0001f, 1f))
                {
                    AM.SetVolume(AudioManager.MUSIC_VOLUME_KEY, musicVol);
                }

                if (ImGui.SliderFloat("SFX Volume", ref sfxVol, AudioManager.MIN_SLIDER_VALUE, 1f))
                {
                    AM.SetVolume(AudioManager.SFX_VOLUME_KEY, sfxVol);

                }

                ImGui.BeginGroup();
                ImGui.PushID("SFX Sources");
                ImGui.Text("SFX Sources");
                if (ImGui.BeginListBox("", windowSize / 3f))
                {

                    for (int i = 0; i < AM.allSources.Count; ++i)
                    {
                        ImGui.PushID(i);
                        var source = AM.allSources[i];
                        if (source.isPlaying)
                        {
                            ImGui.Text($"{i}: Playing {source.clip.name}");
                        }
                        else if (source.gameObject.activeSelf)
                        {
                            ImGui.Text($"{i}: Not Playing");
                        }
                        else
                        {
                            ImGui.Text($"{i}: Disabled");
                        }
                        ImGui.PopID();
                    }
                    ImGui.EndListBox();
                }
                ImGui.PopID();
                ImGui.EndGroup();

                ImGui.SameLine();

                ImGui.BeginGroup();
                ImGui.PushID("SFX Lib");
                ImGui.Text("SFX Library");
                if (ImGui.BeginListBox("", windowSize / 3f))
                {
                    ImGui.BeginGroup();
                    ImGui.Text("Fallback");
                    ImGui.SameLine();
                    if (ImGui.Button("Play"))
                    {
                        AM.PlayOneShotSFX("");
                    }
                    ImGui.EndGroup();

                    for (int i = 0; i < AM.SFXLibrary.Sounds.Count; ++i)
                    {
                        ImGui.PushID(i);
                        ImGui.BeginGroup();
                        ImGui.Text(AM.SFXLibrary.Sounds[i].name);
                        ImGui.SameLine();
                        if (ImGui.Button("Play"))
                        {
                            AM.PlayOneShotSFX(AM.SFXLibrary.Sounds[i].name);
                        }
                        ImGui.EndGroup();
                        ImGui.PopID();
                    }
                    ImGui.EndListBox();

                }
                ImGui.PopID();
                ImGui.EndGroup();

                ImGui.BeginGroup();

                ImGui.PushID("Music Lib");
                
                ImGui.Text("Music Library");
                ImGui.Checkbox("Play From Current Playback Position", ref music_playFromCurrentPos);
                if (ImGui.BeginListBox("", windowSize / 3f))
                {
                    ImGui.BeginGroup();
                    ImGui.Text("Fallback");
                    ImGui.SameLine();
                    if (ImGui.Button("Play"))
                    {
                        AM.PlayMusicImmidiate("", music_playFromCurrentPos);
                    }
                    ImGui.EndGroup();

                    for (int i = 0; i < AM.SFXLibrary.Sounds.Count; ++i)
                    {
                        ImGui.PushID(i);
                        ImGui.BeginGroup();
                        ImGui.Text(AM.SFXLibrary.Sounds[i].name);
                        ImGui.SameLine();
                        if (ImGui.Button("Play"))
                        {
                            AM.PlayOneShotSFX(AM.SFXLibrary.Sounds[i].name);
                        }
                        ImGui.EndGroup();
                        ImGui.PopID();
                    }
                    ImGui.EndListBox();

                }
                ImGui.PopID();
                ImGui.EndGroup();

                ImGui.EndTabItem();
                return true;
            }

            return false;
        }
    }
}
