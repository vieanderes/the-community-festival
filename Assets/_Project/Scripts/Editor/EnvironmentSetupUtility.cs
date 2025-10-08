#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace TheCommunityFestival.Editor
{
    /// <summary>
    /// Utility to quickly set up the festival environment
    /// </summary>
    public static class EnvironmentSetupUtility
    {
        [MenuItem("Community/Setup/Create Festival Environment")]
        public static GameObject CreateFestivalEnvironment()
        {
            Debug.Log("[Environment] Creating festival environment...");

            // Create Terrain
            GameObject terrainObj = CreateTerrain();
            
            // Create Sun (Directional Light)
            GameObject sunObj = CreateSun();
            
            // Create Environment Manager
            GameObject envManager = CreateEnvironmentManager(terrainObj, sunObj);

            // Select the environment manager
            Selection.activeGameObject = envManager;

            Debug.Log("[Environment] Festival environment created! Configure terrain details in the inspector.");
            
            return terrainObj;
        }

        private static GameObject CreateTerrain()
        {
            // Check if terrain already exists
            Terrain existing = Object.FindObjectOfType<Terrain>();
            if (existing != null)
            {
                Debug.Log("[Environment] Using existing terrain");
                return existing.gameObject;
            }

            // Create terrain data
            TerrainData terrainData = new TerrainData();
            terrainData.size = new Vector3(2000, 600, 2000); // 2km x 2km, 600m height
            terrainData.heightmapResolution = 1025;
            terrainData.SetDetailResolution(1024, 16);

            // Save terrain data
            if (!AssetDatabase.IsValidFolder("Assets/_Project/Data/Terrain"))
            {
                AssetDatabase.CreateFolder("Assets/_Project/Data", "Terrain");
            }
            
            AssetDatabase.CreateAsset(terrainData, "Assets/_Project/Data/Terrain/FestivalGrounds_TerrainData.asset");
            AssetDatabase.SaveAssets();

            // Create terrain game object
            GameObject terrainObj = Terrain.CreateTerrainGameObject(terrainData);
            terrainObj.name = "FestivalGrounds_Terrain";
            terrainObj.transform.position = Vector3.zero;

            // Add TerrainManager component
            Terrain terrain = terrainObj.GetComponent<Terrain>();
            terrain.gameObject.AddComponent<Gameplay.World.TerrainManager>();
            terrain.gameObject.AddComponent<Gameplay.World.VegetationManager>();

            Debug.Log("[Environment] Terrain created: 2000x2000m");
            return terrainObj;
        }

        private static GameObject CreateSun()
        {
            // Check if directional light exists
            Light existingLight = Object.FindObjectOfType<Light>();
            if (existingLight != null && existingLight.type == LightType.Directional)
            {
                Debug.Log("[Environment] Using existing directional light");
                existingLight.gameObject.name = "Sun";
                
                // Add TimeOfDaySystem if not present
                if (existingLight.GetComponent<Gameplay.World.TimeOfDaySystem>() == null)
                {
                    existingLight.gameObject.AddComponent<Gameplay.World.TimeOfDaySystem>();
                }
                
                return existingLight.gameObject;
            }

            // Create sun
            GameObject sunObj = new GameObject("Sun");
            Light sun = sunObj.AddComponent<Light>();
            sun.type = LightType.Directional;
            sun.color = new Color(1f, 0.98f, 0.94f); // Warm white
            sun.intensity = 1.2f;
            sun.shadows = LightShadows.Soft;
            sun.shadowStrength = 0.8f;
            
            sunObj.transform.rotation = Quaternion.Euler(50f, 170f, 0f);
            
            // Add time of day system
            sunObj.AddComponent<Gameplay.World.TimeOfDaySystem>();
            
            Debug.Log("[Environment] Sun created with TimeOfDay system");
            return sunObj;
        }

        private static GameObject CreateEnvironmentManager(GameObject terrain, GameObject sun)
        {
            GameObject managerObj = new GameObject("EnvironmentManager");
            
            // Add weather system
            var weather = managerObj.AddComponent<Gameplay.World.WeatherSystem>();
            
            Debug.Log("[Environment] Environment manager created");
            return managerObj;
        }

        [MenuItem("Community/Setup/Create Water Plane")]
        public static void CreateWaterPlane()
        {
            // Create plane for water
            GameObject waterObj = GameObject.CreatePrimitive(PrimitiveType.Plane);
            waterObj.name = "Lake_Main";
            waterObj.transform.position = new Vector3(0, 50, 0); // Adjust height as needed
            waterObj.transform.localScale = new Vector3(20, 1, 20); // 200x200m

            // Add water system component
            waterObj.AddComponent<Gameplay.World.WaterSystem>();

            // Create basic water material
            Material waterMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            waterMat.name = "Water_Lake";
            waterMat.color = new Color(0.39f, 0.59f, 0.71f, 0.7f); // Light blue-green
            waterMat.SetFloat("_Surface", 1); // Transparent
            waterMat.SetFloat("_Blend", 0); // Alpha
            waterMat.SetFloat("_Smoothness", 0.9f);
            waterMat.SetFloat("_Metallic", 0.1f);

            // Save material
            if (!AssetDatabase.IsValidFolder("Assets/_Project/Art/Materials/Environment"))
            {
                AssetDatabase.CreateFolder("Assets/_Project/Art/Materials", "Environment");
            }
            AssetDatabase.CreateAsset(waterMat, "Assets/_Project/Art/Materials/Environment/Water_Lake.mat");
            AssetDatabase.SaveAssets();

            // Assign material
            waterObj.GetComponent<Renderer>().material = waterMat;

            Selection.activeGameObject = waterObj;
            Debug.Log("[Environment] Water plane created. Adjust position to match terrain lake area.");
        }
    }
}
#endif

