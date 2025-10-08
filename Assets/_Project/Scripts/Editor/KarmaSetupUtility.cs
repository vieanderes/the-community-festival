#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace TheCommunityFestival.Editor
{
    /// <summary>
    /// Utility to set up the Karma system
    /// </summary>
    public static class KarmaSetupUtility
    {
        [MenuItem("Community/Setup/Create Karma System")]
        public static void CreateKarmaSystem()
        {
            Debug.Log("[Karma] Setting up Karma System...");

            // Create or find GameManager
            GameObject gameManagerObj = GameObject.Find("GameManager");
            if (gameManagerObj == null)
            {
                gameManagerObj = new GameObject("GameManager");
                gameManagerObj.AddComponent<Core.GameManager>();
                Debug.Log("[Karma] Created GameManager");
            }

            // Add KarmaManager
            Core.KarmaManager karmaManager = Object.FindObjectOfType<Core.KarmaManager>();
            if (karmaManager == null)
            {
                GameObject karmaObj = new GameObject("KarmaManager");
                karmaManager = karmaObj.AddComponent<Core.KarmaManager>();
                Debug.Log("[Karma] Created KarmaManager");
            }

            // Add visual feedback system to environment
            Gameplay.World.KarmaVisualFeedback feedback = Object.FindObjectOfType<Gameplay.World.KarmaVisualFeedback>();
            if (feedback == null)
            {
                GameObject feedbackObj = new GameObject("KarmaVisualFeedback");
                feedback = feedbackObj.AddComponent<Gameplay.World.KarmaVisualFeedback>();
                
                // Try to find and assign references
                Light mainLight = Object.FindObjectOfType<Light>();
                if (mainLight != null && mainLight.type == LightType.Directional)
                {
                    SerializedObject so = new SerializedObject(feedback);
                    so.FindProperty("_mainLight").objectReferenceValue = mainLight;
                    so.ApplyModifiedProperties();
                    Debug.Log("[Karma] Assigned main light to visual feedback");
                }
                
                Debug.Log("[Karma] Created KarmaVisualFeedback");
            }

            // Create positive and negative particle systems
            CreateKarmaParticles();

            Selection.activeGameObject = karmaManager.gameObject;
            Debug.Log("[Karma] Karma System setup complete!");
            Debug.Log("[Karma] Use Context Menu (right-click KarmaManager) to test karma actions");
        }

        private static void CreateKarmaParticles()
        {
            // Create positive karma particles
            GameObject positiveParticlesObj = GameObject.Find("PositiveKarmaParticles");
            if (positiveParticlesObj == null)
            {
                positiveParticlesObj = new GameObject("PositiveKarmaParticles");
                ParticleSystem ps = positiveParticlesObj.AddComponent<ParticleSystem>();
                
                var main = ps.main;
                main.startColor = new Color(1f, 0.95f, 0.6f, 0.5f); // Golden
                main.startSize = 0.2f;
                main.startLifetime = 3f;
                main.startSpeed = 2f;
                main.maxParticles = 500;
                
                var emission = ps.emission;
                emission.rateOverTime = 0; // Will be controlled by KarmaVisualFeedback
                
                ps.Stop(); // Start stopped
                
                Debug.Log("[Karma] Created positive karma particles");
            }

            // Create negative karma particles
            GameObject negativeParticlesObj = GameObject.Find("NegativeKarmaParticles");
            if (negativeParticlesObj == null)
            {
                negativeParticlesObj = new GameObject("NegativeKarmaParticles");
                ParticleSystem ps = negativeParticlesObj.AddComponent<ParticleSystem>();
                
                var main = ps.main;
                main.startColor = new Color(0.3f, 0.3f, 0.4f, 0.5f); // Dark
                main.startSize = 0.3f;
                main.startLifetime = 2f;
                main.startSpeed = 1f;
                main.maxParticles = 300;
                
                var emission = ps.emission;
                emission.rateOverTime = 0; // Will be controlled by KarmaVisualFeedback
                
                ps.Stop(); // Start stopped
                
                Debug.Log("[Karma] Created negative karma particles");
            }

            // Assign to KarmaVisualFeedback
            Gameplay.World.KarmaVisualFeedback feedback = Object.FindObjectOfType<Gameplay.World.KarmaVisualFeedback>();
            if (feedback != null)
            {
                SerializedObject so = new SerializedObject(feedback);
                so.FindProperty("_positiveParticles").objectReferenceValue = positiveParticlesObj.GetComponent<ParticleSystem>();
                so.FindProperty("_negativeParticles").objectReferenceValue = negativeParticlesObj.GetComponent<ParticleSystem>();
                so.ApplyModifiedProperties();
                
                Debug.Log("[Karma] Assigned particles to visual feedback");
            }
        }

        [MenuItem("Community/Test/Add Positive Karma")]
        public static void TestAddPositiveKarma()
        {
            Core.KarmaManager km = Object.FindObjectOfType<Core.KarmaManager>();
            if (km != null)
            {
                km.AddKarma(Core.Systems.KarmaActionType.Helping, "Test - Helped someone");
                Debug.Log("[Test] Added positive karma!");
            }
            else
            {
                Debug.LogWarning("[Test] No KarmaManager found. Create one first: Community → Setup → Create Karma System");
            }
        }

        [MenuItem("Community/Test/Add Negative Karma")]
        public static void TestAddNegativeKarma()
        {
            Core.KarmaManager km = Object.FindObjectOfType<Core.KarmaManager>();
            if (km != null)
            {
                km.AddKarma(Core.Systems.KarmaActionType.Ignoring, "Test - Ignored someone in need");
                Debug.Log("[Test] Added negative karma!");
            }
            else
            {
                Debug.LogWarning("[Test] No KarmaManager found. Create one first: Community → Setup → Create Karma System");
            }
        }
    }
}
#endif

