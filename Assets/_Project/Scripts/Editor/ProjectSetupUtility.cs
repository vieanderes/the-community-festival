#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

namespace TheCommunityFestival.Editor
{
    /// <summary>
    /// Utility to set up project configuration
    /// </summary>
    public static class ProjectSetupUtility
    {
        [MenuItem("Community/Setup/Configure Tags and Layers")]
        public static void ConfigureTagsAndLayers()
        {
            Debug.Log("[Setup] Configuring Tags and Layers...");

            // Add Tags
            AddTag("Stage");
            AddTag("NPC");
            AddTag("Interactable");
            AddTag("Volunteer");
            AddTag("Guest");
            AddTag("Artist");
            AddTag("KarmaSource");

            // Add Layers
            AddLayer("Ground", 6);
            AddLayer("Player", 7);
            AddLayer("NPC", 8);
            AddLayer("Environment", 9);
            AddLayer("Interactable", 10);
            AddLayer("Stage", 11);
            AddLayer("Building", 12);
            AddLayer("VFX", 13);
            AddLayer("PostProcessing", 14);

            Debug.Log("[Setup] Tags and Layers configured successfully!");
        }

        [MenuItem("Community/Setup/Create Essential Folders")]
        public static void CreateEssentialFolders()
        {
            Debug.Log("[Setup] Creating folder structure...");

            string[] folders = new string[]
            {
                "Assets/_Project",
                "Assets/_Project/Art",
                "Assets/_Project/Art/Materials",
                "Assets/_Project/Art/Models",
                "Assets/_Project/Art/Textures",
                "Assets/_Project/Art/Shaders",
                "Assets/_Project/Art/Animations",
                "Assets/_Project/Audio",
                "Assets/_Project/Audio/Music",
                "Assets/_Project/Audio/SFX",
                "Assets/_Project/Audio/Ambience",
                "Assets/_Project/Prefabs",
                "Assets/_Project/Prefabs/Characters",
                "Assets/_Project/Prefabs/Environment",
                "Assets/_Project/Prefabs/Stages",
                "Assets/_Project/Prefabs/UI",
                "Assets/_Project/Prefabs/VFX",
                "Assets/_Project/Scenes",
                "Assets/_Project/Scenes/TestScenes",
                "Assets/_Project/Scripts",
                "Assets/_Project/Scripts/Core",
                "Assets/_Project/Scripts/Core/Managers",
                "Assets/_Project/Scripts/Core/Systems",
                "Assets/_Project/Scripts/Core/Utilities",
                "Assets/_Project/Scripts/Gameplay",
                "Assets/_Project/Scripts/Gameplay/Characters",
                "Assets/_Project/Scripts/Gameplay/Interactions",
                "Assets/_Project/Scripts/Gameplay/World",
                "Assets/_Project/Scripts/Networking",
                "Assets/_Project/Scripts/AI",
                "Assets/_Project/Scripts/AI/Behaviors",
                "Assets/_Project/Scripts/AI/Navigation",
                "Assets/_Project/Scripts/AI/Decision",
                "Assets/_Project/Scripts/Audio",
                "Assets/_Project/Scripts/UI",
                "Assets/_Project/Scripts/UI/Menus",
                "Assets/_Project/Scripts/UI/HUD",
                "Assets/_Project/Scripts/UI/Dialogs",
                "Assets/_Project/Scripts/Data",
                "Assets/_Project/Scripts/Editor",
                "Assets/_Project/Data",
                "Assets/_Project/Data/ScriptableObjects",
                "Assets/_Project/Data/ScriptableObjects/Characters",
                "Assets/_Project/Data/ScriptableObjects/Music",
                "Assets/_Project/Data/ScriptableObjects/Stages",
                "Assets/_Project/Data/ScriptableObjects/Events",
                "Assets/_Project/Data/Configuration",
                "Assets/_Project/Settings",
                "Assets/_Project/Settings/URP",
                "Assets/_Project/Settings/Input",
                "Assets/Plugins",
                "Assets/ThirdParty"
            };

            foreach (string folder in folders)
            {
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                    Debug.Log($"Created folder: {folder}");
                }
            }

            AssetDatabase.Refresh();
            Debug.Log("[Setup] Folder structure created successfully!");
        }

        [MenuItem("Community/Setup/Configure Performance Settings")]
        public static void ConfigurePerformanceSettings()
        {
            Debug.Log("[Setup] Configuring performance settings for M1 Pro...");

            // Set target frame rate
            Application.targetFrameRate = 60;
            
            // Configure quality settings
            QualitySettings.vSyncCount = 1;
            QualitySettings.maxQueuedFrames = 2;

            // Set color space to Linear
            PlayerSettings.colorSpace = ColorSpace.Linear;

            Debug.Log("[Setup] Performance settings configured!");
        }

        private static void AddTag(string tag)
        {
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty tagsProp = tagManager.FindProperty("tags");

            // Check if tag already exists
            bool found = false;
            for (int i = 0; i < tagsProp.arraySize; i++)
            {
                SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
                if (t.stringValue.Equals(tag))
                {
                    found = true;
                    break;
                }
            }

            // Add tag if not found
            if (!found)
            {
                tagsProp.InsertArrayElementAtIndex(tagsProp.arraySize);
                SerializedProperty n = tagsProp.GetArrayElementAtIndex(tagsProp.arraySize - 1);
                n.stringValue = tag;
                tagManager.ApplyModifiedProperties();
                Debug.Log($"Added tag: {tag}");
            }
        }

        private static void AddLayer(string layerName, int layerNumber)
        {
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty layers = tagManager.FindProperty("layers");

            if (layerNumber >= 0 && layerNumber < layers.arraySize)
            {
                SerializedProperty layerSP = layers.GetArrayElementAtIndex(layerNumber);
                
                if (string.IsNullOrEmpty(layerSP.stringValue))
                {
                    layerSP.stringValue = layerName;
                    tagManager.ApplyModifiedProperties();
                    Debug.Log($"Added layer: {layerName} at index {layerNumber}");
                }
                else if (layerSP.stringValue != layerName)
                {
                    Debug.LogWarning($"Layer {layerNumber} already contains: {layerSP.stringValue}");
                }
            }
        }
    }
}
#endif

