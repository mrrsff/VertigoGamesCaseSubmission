using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace SpinGameDemo.Spin.Editor
{
    #if UNITY_EDITOR
    public class PresetCreationWindow : EditorWindow
    {
        private const string PresetPath = "Assets/SpinGameDemo/Spin/Resources/SpinPresets/";

        [MenuItem("Spin/Spin Preset Window")]
        public static void ShowWindow()
        {
            GetWindow<PresetCreationWindow>("Spin Preset Creation Window");
        }

        private int amount = 30;
        private Sprite defaultIcon;
        private Sprite bombIcon;
        private void OnGUI()
        {
            GUILayout.Label("Create Spin Preset", EditorStyles.boldLabel);

            amount = EditorGUILayout.IntField("Amount", amount);
            defaultIcon = (Sprite)EditorGUILayout.ObjectField("Default Icon", defaultIcon, typeof(Sprite), false);
            bombIcon = (Sprite)EditorGUILayout.ObjectField("Bomb Icon", bombIcon, typeof(Sprite), false);

            if (GUILayout.Button("Create Presets"))
            {
                AssurePresetPathExists();
                
                SpinPresetLibrary library = Resources.Load<SpinPresetLibrary>("SpinPresetLibrary");
                List<SpinPreset> presets = new List<SpinPreset>(amount);
                for (int i = 1; i <= amount; i++)
                {
                    bool safe = IsSafe(i);
                    presets.Add(CreateSpinPreset(safe));
                    
                    // If the asset already exists, delete it first
                    AssetDatabase.DeleteAsset($"{PresetPath}SpinPreset_{i}.asset");
                    AssetDatabase.CreateAsset(presets[i-1], $"{PresetPath}SpinPreset_{i}.asset");
                }
                library.SetPresets(presets);
                
                AssetDatabase.SaveAssets();
            }
        }
        private void AssurePresetPathExists()
        {
            if (!AssetDatabase.IsValidFolder(PresetPath))
            {
                Directory.CreateDirectory(PresetPath);
                AssetDatabase.Refresh();
            }
        }
        private SpinPreset CreateSpinPreset(bool safe)
        {
            SpinPreset preset = CreateInstance<SpinPreset>();
            preset.OnValidate();
            int outcomeCount = preset.outcomes.Count;
            int bombIndex = safe ? -1 : Random.Range(0, outcomeCount);
            for (int i = 0; i < outcomeCount; i++)
            {
                SpinOutcome outcome;
                if (i == bombIndex)
                {
                    outcome = new BombOutcome(); 
                    outcome.Icon = bombIcon;
                    outcome.Amount = -1;
                }
                else
                {
                    outcome = new SpinOutcome();
                    outcome.Icon = defaultIcon;
                    outcome.Amount = Random.Range(10, 101);
                }
                
                preset.outcomes[i].outcome = outcome;
                preset.outcomes[i].weight = 1;
            }
            return preset;
        }

        private static bool IsSafe(int zoneIndex)
        {
            return zoneIndex % 5 == 0; 
        }
    }
    #endif
}