#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.AI;

namespace TheCommunityFestival.Editor
{
    /// <summary>
    /// Complete playable game setup with player, NPCs, and interactions
    /// </summary>
    public static class PlayableGameSetup
    {
        [MenuItem("Community/Quick Start/Create Playable Game (With Player & NPCs)", false, 1)]
        public static void CreatePlayableGame()
        {
            Debug.Log("========================================");
            Debug.Log("[Playable Game] Creating complete playable game...");
            Debug.Log("========================================");

            // 1. Create new scene or use current
            var scene = EditorSceneManager.GetActiveScene();
            
            // 2. Setup project configuration
            ProjectSetupUtility.ConfigureTagsAndLayers();
            
            // 3. Create environment
            Debug.Log("[Playable Game] Creating environment...");
            GameObject terrain = EnvironmentSetupUtility.CreateFestivalEnvironment();
            
            // 4. Create karma system
            Debug.Log("[Playable Game] Setting up karma system...");
            KarmaSetupUtility.CreateKarmaSystem();
            
            // 5. Bake NavMesh for AI
            Debug.Log("[Playable Game] Baking NavMesh for AI...");
            BakeNavMesh();
            
            // 6. Create Player
            Debug.Log("[Playable Game] Creating player character...");
            CreatePlayer();
            
            // 7. Create NPCs
            Debug.Log("[Playable Game] Creating NPCs...");
            CreateNPCs(5); // Create 5 NPCs
            
            // 8. Save scene
            string scenePath = "Assets/_Project/Scenes/PlayableGame.unity";
            EditorSceneManager.SaveScene(scene, scenePath);
            
            Debug.Log("========================================");
            Debug.Log("[Playable Game] ✅ COMPLETE!");
            Debug.Log("[Playable Game] 🎮 Press PLAY to start!");
            Debug.Log("[Playable Game]");
            Debug.Log("[Playable Game] Controls:");
            Debug.Log("[Playable Game]   WASD - Move");
            Debug.Log("[Playable Game]   Mouse - Look");
            Debug.Log("[Playable Game]   Shift - Run");
            Debug.Log("[Playable Game]   Space - Jump");
            Debug.Log("[Playable Game]   E - Interact with NPCs");
            Debug.Log("[Playable Game]   ESC - Toggle cursor");
            Debug.Log("========================================");
        }

        private static void BakeNavMesh()
        {
            // Configure NavMesh settings
            var settings = NavMeshBuilder.defaultSettings;
            settings.agentRadius = 0.5f;
            settings.agentHeight = 2f;
            settings.agentSlope = 45f;
            settings.agentClimb = 0.4f;
            
            // Build NavMesh
            NavMeshBuilder.BuildNavMesh();
            Debug.Log("[Playable Game] NavMesh baked for AI navigation");
        }

        private static GameObject CreatePlayer()
        {
            // Create player capsule
            GameObject player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            player.name = "Player";
            player.tag = "Player";
            player.layer = LayerMask.NameToLayer("Player");
            
            // Position above terrain
            player.transform.position = new Vector3(0, 2, 0);
            
            // Remove default collider and add CharacterController
            Object.DestroyImmediate(player.GetComponent<Collider>());
            var controller = player.AddComponent<CharacterController>();
            controller.height = 2f;
            controller.radius = 0.5f;
            controller.center = Vector3.zero;
            
            // Add PlayerController script
            var playerScript = player.AddComponent<Gameplay.Characters.PlayerController>();
            
            // Create camera as child
            GameObject cameraObj = new GameObject("PlayerCamera");
            cameraObj.transform.SetParent(player.transform);
            cameraObj.transform.localPosition = new Vector3(0, 0.6f, 0); // Eye level
            cameraObj.transform.localRotation = Quaternion.identity;
            
            Camera cam = cameraObj.AddComponent<Camera>();
            cam.tag = "MainCamera";
            cam.nearClipPlane = 0.1f;
            cam.farClipPlane = 1000f;
            
            // Add AudioListener
            cameraObj.AddComponent<AudioListener>();
            
            // Assign camera to player script
            SerializedObject so = new SerializedObject(playerScript);
            so.FindProperty("_cameraTransform").objectReferenceValue = cameraObj.transform;
            so.FindProperty("_interactableLayers").intValue = 1 << LayerMask.NameToLayer("NPC");
            so.ApplyModifiedProperties();
            
            // Remove any existing Main Camera
            Camera[] cameras = Object.FindObjectsOfType<Camera>();
            foreach (Camera existingCam in cameras)
            {
                if (existingCam != cam)
                {
                    Object.DestroyImmediate(existingCam.gameObject);
                }
            }
            
            Debug.Log("[Playable Game] Player created with first-person camera");
            return player;
        }

        private static void CreateNPCs(int count)
        {
            // Create parent object for organization
            GameObject npcParent = GameObject.Find("NPCs");
            if (npcParent == null)
            {
                npcParent = new GameObject("NPCs");
            }

            for (int i = 0; i < count; i++)
            {
                CreateSingleNPC(npcParent.transform, i);
            }
        }

        private static GameObject CreateSingleNPC(Transform parent, int index)
        {
            // Create NPC capsule
            GameObject npc = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            npc.name = $"NPC_{index + 1}";
            npc.tag = "NPC";
            npc.layer = LayerMask.NameToLayer("NPC");
            npc.transform.SetParent(parent);
            
            // Random position on NavMesh
            Vector3 randomPos = new Vector3(
                Random.Range(-50f, 50f),
                0,
                Random.Range(-50f, 50f)
            );
            
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPos, out hit, 100f, NavMesh.AllAreas))
            {
                npc.transform.position = hit.position + Vector3.up; // Slightly above ground
            }
            else
            {
                npc.transform.position = new Vector3(
                    Random.Range(-20f, 20f),
                    1,
                    Random.Range(-20f, 20f)
                );
            }
            
            // Add NavMeshAgent
            NavMeshAgent agent = npc.AddComponent<NavMeshAgent>();
            agent.height = 2f;
            agent.radius = 0.5f;
            agent.speed = 3.5f;
            agent.acceleration = 8f;
            agent.angularSpeed = 120f;
            agent.stoppingDistance = 1f;
            
            // Add AICharacter script
            var aiScript = npc.AddComponent<AI.AICharacter>();
            
            // Create simple character profile (we'll create proper ScriptableObjects later)
            var profile = ScriptableObject.CreateInstance<Data.CharacterProfile>();
            profile.characterName = GetRandomName();
            profile.role = (Data.CharacterRole)Random.Range(0, 3); // Random role
            profile.friendliness = Random.Range(0.6f, 1f);
            profile.helpfulness = Random.Range(0.5f, 1f);
            
            // Add some basic dialogue
            profile.greetings.Add("Hey there!");
            profile.greetings.Add("Hello, friend!");
            profile.greetings.Add("Good to see you!");
            profile.helpOffers.Add("Need any help?");
            profile.helpOffers.Add("I'm here if you need anything!");
            profile.encouragements.Add("You're doing great!");
            profile.encouragements.Add("I love your positive energy!");
            
            // Save profile
            string profilePath = $"Assets/_Project/Data/ScriptableObjects/Characters/NPC_{index + 1}_Profile.asset";
            AssetDatabase.CreateAsset(profile, profilePath);
            
            // Assign to NPC
            SerializedObject so = new SerializedObject(aiScript);
            so.FindProperty("_profile").objectReferenceValue = profile;
            so.ApplyModifiedProperties();
            
            // Add color to distinguish NPCs
            Renderer renderer = npc.GetComponent<Renderer>();
            Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            mat.color = Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.7f, 1f);
            renderer.material = mat;
            
            Debug.Log($"[Playable Game] Created {profile.characterName} ({profile.role})");
            return npc;
        }

        private static string GetRandomName()
        {
            string[] names = new string[]
            {
                "Alex", "Riley", "Jordan", "Casey", "Morgan",
                "Taylor", "Quinn", "Sage", "River", "Dakota",
                "Phoenix", "Skyler", "Rowan", "Ash", "Ember",
                "Luna", "Sol", "Nova", "Echo", "Harmony"
            };
            
            return names[Random.Range(0, names.Length)];
        }
    }
}
#endif

