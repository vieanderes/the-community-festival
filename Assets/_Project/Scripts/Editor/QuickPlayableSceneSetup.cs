#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace TheCommunityFestival.Editor
{
    /// <summary>
    /// One-click setup for a complete playable scene
    /// Perfect for beginners!
    /// </summary>
    public static class QuickPlayableSceneSetup
    {
        [MenuItem("Community/Quick Start/Create Complete Playable Scene", false, 0)]
        public static void CreateCompletePlayableScene()
        {
            Debug.Log("========================================");
            Debug.Log("[Quick Start] Creating complete playable scene...");
            Debug.Log("========================================");

            // Create new scene
            var newScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
            
            // 1. Setup Tags and Layers
            ProjectSetupUtility.ConfigureTagsAndLayers();
            
            // 2. Create Environment
            GameObject terrain = EnvironmentSetupUtility.CreateFestivalEnvironment();
            
            // 3. Create Karma System
            KarmaSetupUtility.CreateKarmaSystem();
            
            // 4. Setup Camera for better view
            SetupCamera();
            
            // 5. Add helpful instructions object
            CreateInstructionsObject();
            
            // Save the scene
            string scenePath = "Assets/_Project/Scenes/FestivalGrounds.unity";
            EditorSceneManager.SaveScene(newScene, scenePath);
            
            Debug.Log("========================================");
            Debug.Log("[Quick Start] ✅ COMPLETE! Scene saved to: " + scenePath);
            Debug.Log("[Quick Start] 🎮 Press PLAY button at the top to test!");
            Debug.Log("[Quick Start] 💡 Check the 'Instructions' object in Hierarchy for tips!");
            Debug.Log("========================================");
            
            // Select instructions so user sees them
            GameObject instructions = GameObject.Find("_START_HERE_Instructions");
            if (instructions != null)
            {
                Selection.activeGameObject = instructions;
            }
        }

        private static void SetupCamera()
        {
            Camera mainCam = Camera.main;
            if (mainCam != null)
            {
                // Position camera for a nice view of the terrain
                mainCam.transform.position = new Vector3(0, 50, -100);
                mainCam.transform.rotation = Quaternion.Euler(15, 0, 0);
                
                // Make camera move-able with simple script
                if (mainCam.GetComponent<SimpleFreeCam>() == null)
                {
                    mainCam.gameObject.AddComponent<SimpleFreeCam>();
                }
                
                Debug.Log("[Quick Start] Camera positioned with free-look controls");
            }
        }

        private static void CreateInstructionsObject()
        {
            GameObject instructions = new GameObject("_START_HERE_Instructions");
            
            var textMesh = instructions.AddComponent<TextMesh>();
            textMesh.text = "WELCOME TO THE COMMUNITY FESTIVAL!\n\n" +
                           "🎮 PRESS PLAY BUTTON (top center) TO START\n\n" +
                           "🎥 Camera Controls (when playing):\n" +
                           "   • Right-Click + Drag = Look Around\n" +
                           "   • WASD = Move Camera\n" +
                           "   • Q/E = Up/Down\n\n" +
                           "💫 Test Karma System:\n" +
                           "   1. Find 'KarmaManager' in Hierarchy (left panel)\n" +
                           "   2. Right-click the component in Inspector\n" +
                           "   3. Select 'Add Positive Karma' or 'Add Negative Karma'\n" +
                           "   4. Watch console for karma changes!\n\n" +
                           "⏰ Test Time of Day:\n" +
                           "   1. Find 'Sun' in Hierarchy\n" +
                           "   2. Adjust 'Current Time' slider (0-24)\n" +
                           "   3. Watch the sun move!\n\n" +
                           "🌧️ Test Weather:\n" +
                           "   1. Find 'EnvironmentManager' in Hierarchy\n" +
                           "   2. Change 'Current Weather' dropdown\n\n" +
                           "📊 Watch the Console (bottom panel) for messages!";
            
            textMesh.fontSize = 20;
            textMesh.color = Color.white;
            textMesh.anchor = TextAnchor.MiddleCenter;
            
            instructions.transform.position = new Vector3(0, 30, 50);
            instructions.transform.localScale = new Vector3(2, 2, 2);
            
            Debug.Log("[Quick Start] Created instruction text in scene");
        }
    }

    /// <summary>
    /// Simple free camera for testing
    /// Attach to Main Camera
    /// </summary>
    public class SimpleFreeCam : MonoBehaviour
    {
        [Header("Movement")]
        public float moveSpeed = 20f;
        public float fastMoveSpeed = 40f;
        public float sensitivity = 2f;

        private float rotationX = 0f;
        private float rotationY = 0f;

        void Start()
        {
            Vector3 rot = transform.localRotation.eulerAngles;
            rotationY = rot.y;
            rotationX = rot.x;
        }

        void Update()
        {
            // Mouse look (only when right-click held)
            if (Input.GetMouseButton(1))
            {
                rotationX -= Input.GetAxis("Mouse Y") * sensitivity;
                rotationY += Input.GetAxis("Mouse X") * sensitivity;
                rotationX = Mathf.Clamp(rotationX, -90f, 90f);
                transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0);
            }

            // Movement
            float speed = Input.GetKey(KeyCode.LeftShift) ? fastMoveSpeed : moveSpeed;
            
            Vector3 move = Vector3.zero;
            
            if (Input.GetKey(KeyCode.W)) move += transform.forward;
            if (Input.GetKey(KeyCode.S)) move -= transform.forward;
            if (Input.GetKey(KeyCode.A)) move -= transform.right;
            if (Input.GetKey(KeyCode.D)) move += transform.right;
            if (Input.GetKey(KeyCode.Q)) move -= transform.up;
            if (Input.GetKey(KeyCode.E)) move += transform.up;

            transform.position += move * speed * Time.deltaTime;
        }
    }
}
#endif

