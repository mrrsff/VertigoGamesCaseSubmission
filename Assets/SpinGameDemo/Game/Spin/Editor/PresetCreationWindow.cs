using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SpinGameDemo.Spin.Editor
{
    #if UNITY_EDITOR
    public class PresetCreationWindow : EditorWindow
    {
        private const string PresetPath = "Assets/SpinGameDemo/Game/Spin/Resources/SpinPresets/";
        private const string AmountKey = "PresetCreationWindow_Amount";
        private const string DefaultIconsKey = "PresetCreationWindow_DefaultIcons";
        private const string BombIconKey = "PresetCreationWindow_BombIcon";

        [MenuItem("Spin/Spin Preset Window")]
        public static void ShowWindow()
        {
            GetWindow<PresetCreationWindow>("Spin Preset Creation Window");
        }

        private int amount = 30;
        private List<Sprite> defaultIcons = new List<Sprite>();
        private Sprite bombIcon;
        private Vector2 scrollPosition;
        private const int iconsPerRow = 4;
        private const float iconSize = 70f;
        
        private void OnEnable()
        {
            // Load persistent values
            amount = EditorPrefs.GetInt(AmountKey, 30);
            
            // Load bomb icon
            string bombIconPath = EditorPrefs.GetString(BombIconKey, "");
            if (!string.IsNullOrEmpty(bombIconPath))
            {
                bombIcon = AssetDatabase.LoadAssetAtPath<Sprite>(bombIconPath);
            }
            
            // Load default icons list
            string defaultIconsData = EditorPrefs.GetString(DefaultIconsKey, "");
            if (!string.IsNullOrEmpty(defaultIconsData))
            {
                string[] iconPaths = defaultIconsData.Split('|');
                defaultIcons.Clear();
                foreach (string path in iconPaths)
                {
                    if (!string.IsNullOrEmpty(path))
                    {
                        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
                        defaultIcons.Add(sprite);
                    }
                    else
                    {
                        defaultIcons.Add(null);
                    }
                }
            }
            
            // Ensure at least one slot in the list
            if (defaultIcons.Count == 0)
            {
                defaultIcons.Add(null);
            }
        }

        private void OnDisable()
        {
            // Save persistent values
            EditorPrefs.SetInt(AmountKey, amount);
            
            // Save bomb icon
            string bombIconPath = bombIcon != null ? AssetDatabase.GetAssetPath(bombIcon) : "";
            EditorPrefs.SetString(BombIconKey, bombIconPath);
            
            // Save default icons list
            List<string> iconPaths = new List<string>();
            foreach (Sprite sprite in defaultIcons)
            {
                if (sprite != null)
                {
                    iconPaths.Add(AssetDatabase.GetAssetPath(sprite));
                }
                else
                {
                    iconPaths.Add("");
                }
            }
            EditorPrefs.SetString(DefaultIconsKey, string.Join("|", iconPaths));
        }

        private void OnGUI()
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            
            GUILayout.Label("Create Spin Preset", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            amount = EditorGUILayout.IntField("Amount", amount);
            
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Default Icons", EditorStyles.boldLabel);
            
            // Drag and drop area
            Rect dropArea = GUILayoutUtility.GetRect(0, 50, GUILayout.ExpandWidth(true));
            GUI.Box(dropArea, "Drag & Drop Sprites Here\n(or multiple sprites from Project)");
            
            HandleDragAndDrop(dropArea);
            
            EditorGUILayout.Space();
            
            // Draw icons in a grid
            int iconIndex = 0;
            while (iconIndex < defaultIcons.Count)
            {
                EditorGUILayout.BeginHorizontal();
                
                for (int col = 0; col < iconsPerRow && iconIndex < defaultIcons.Count; col++, iconIndex++)
                {
                    EditorGUILayout.BeginVertical(GUILayout.Width(iconSize + 10));
                    
                    // Draw the sprite field
                    defaultIcons[iconIndex] = (Sprite)EditorGUILayout.ObjectField(
                        defaultIcons[iconIndex], 
                        typeof(Sprite), 
                        false, 
                        GUILayout.Width(iconSize), 
                        GUILayout.Height(iconSize)
                    );
                    
                    // Draw remove button below the icon
                    if (GUILayout.Button("Remove", GUILayout.Width(iconSize)))
                    {
                        defaultIcons.RemoveAt(iconIndex);
                        iconIndex--;
                        EditorGUILayout.EndVertical();
                        break;
                    }
                    
                    EditorGUILayout.EndVertical();
                }
                
                EditorGUILayout.EndHorizontal();
            }
            
            EditorGUILayout.Space();
            
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("+ Add Icon", GUILayout.Height(30)))
            {
                defaultIcons.Add(null);
            }
            
            if (GUILayout.Button("Add From Selection", GUILayout.Height(30)))
            {
                AddSpritesFromSelection();
            }
            
            if (GUILayout.Button("Clear All", GUILayout.Height(30)))
            {
                if (EditorUtility.DisplayDialog("Clear All Icons", "Are you sure you want to remove all icons?", "Yes", "No"))
                {
                    defaultIcons.Clear();
                    defaultIcons.Add(null);
                }
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            
            bombIcon = (Sprite)EditorGUILayout.ObjectField("Bomb Icon", bombIcon, typeof(Sprite), false);

            EditorGUILayout.Space();

            if (GUILayout.Button("Create Presets", GUILayout.Height(40)))
            {
                if (defaultIcons.Count == 0 || defaultIcons.TrueForAll(s => s == null))
                {
                    EditorUtility.DisplayDialog("Error", "Please assign at least one default icon!", "OK");
                    EditorGUILayout.EndScrollView();
                    return;
                }
                
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
                EditorUtility.DisplayDialog("Success", $"Created {amount} spin presets!", "OK");
            }
            
            EditorGUILayout.EndScrollView();
        }
        
        private void HandleDragAndDrop(Rect dropArea)
        {
            Event evt = Event.current;
            
            if (dropArea.Contains(evt.mousePosition))
            {
                if (evt.type == EventType.DragUpdated)
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                    evt.Use();
                }
                else if (evt.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();
                    
                    foreach (Object draggedObject in DragAndDrop.objectReferences)
                    {
                        if (draggedObject is Sprite sprite)
                        {
                            // Remove null entries before adding
                            defaultIcons.RemoveAll(s => s == null);
                            
                            if (!defaultIcons.Contains(sprite))
                            {
                                defaultIcons.Add(sprite);
                            }
                        }
                        else if (draggedObject is Texture2D)
                        {
                            // Try to load sprites from texture
                            string path = AssetDatabase.GetAssetPath(draggedObject);
                            Object[] sprites = AssetDatabase.LoadAllAssetsAtPath(path);
                            
                            foreach (Object obj in sprites)
                            {
                                if (obj is Sprite spr)
                                {
                                    defaultIcons.RemoveAll(s => s == null);
                                    
                                    if (!defaultIcons.Contains(spr))
                                    {
                                        defaultIcons.Add(spr);
                                    }
                                }
                            }
                        }
                    }
                    
                    // Add at least one null slot if list is empty
                    if (defaultIcons.Count == 0)
                    {
                        defaultIcons.Add(null);
                    }
                    
                    evt.Use();
                }
            }
        }
        
        private void AddSpritesFromSelection()
        {
            Object[] selectedObjects = Selection.objects;
            
            if (selectedObjects.Length == 0)
            {
                EditorUtility.DisplayDialog("No Selection", "Please select sprites in the Project window first.", "OK");
                return;
            }
            
            int addedCount = 0;
            
            foreach (Object obj in selectedObjects)
            {
                if (obj is Sprite sprite)
                {
                    defaultIcons.RemoveAll(s => s == null);
                    
                    if (!defaultIcons.Contains(sprite))
                    {
                        defaultIcons.Add(sprite);
                        addedCount++;
                    }
                }
                else if (obj is Texture2D)
                {
                    // Try to load sprites from texture
                    string path = AssetDatabase.GetAssetPath(obj);
                    Object[] sprites = AssetDatabase.LoadAllAssetsAtPath(path);
                    
                    foreach (Object spriteObj in sprites)
                    {
                        if (spriteObj is Sprite spr)
                        {
                            defaultIcons.RemoveAll(s => s == null);
                            
                            if (!defaultIcons.Contains(spr))
                            {
                                defaultIcons.Add(spr);
                                addedCount++;
                            }
                        }
                    }
                }
            }
            
            if (addedCount > 0)
            {
                Debug.Log($"Added {addedCount} sprite(s) to the list.");
            }
            else
            {
                EditorUtility.DisplayDialog("No Sprites Found", "No valid sprites found in selection.", "OK");
            }
            
            // Add at least one null slot if list is empty
            if (defaultIcons.Count == 0)
            {
                defaultIcons.Add(null);
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
            
            // Get valid default icons (non-null ones)
            List<Sprite> validIcons = new List<Sprite>();
            foreach (Sprite sprite in defaultIcons)
            {
                if (sprite != null)
                {
                    validIcons.Add(sprite);
                }
            }
            
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
                    if (validIcons.Count > 0)
                    {
                        outcome.Icon = validIcons[Random.Range(0, validIcons.Count)];
                    }
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