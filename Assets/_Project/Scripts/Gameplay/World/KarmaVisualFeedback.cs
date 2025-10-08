using UnityEngine;
using TheCommunityFestival.Core;
using TheCommunityFestival.Core.Systems;

namespace TheCommunityFestival.Gameplay.World
{
    /// <summary>
    /// Provides visual feedback based on karma level
    /// Changes environment, lighting, and atmosphere
    /// </summary>
    public class KarmaVisualFeedback : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Light _mainLight;
        [SerializeField] private Material _skyboxMaterial;
        [SerializeField] private ParticleSystem _positiveParticles;
        [SerializeField] private ParticleSystem _negativeParticles;

        [Header("Color Grading")]
        [SerializeField] private Color _positiveColorTint = new Color(1f, 0.98f, 0.9f);
        [SerializeField] private Color _negativeColorTint = new Color(0.9f, 0.92f, 0.95f);

        [Header("Intensity Modifiers")]
        [SerializeField] private float _positiveIntensityBoost = 1.3f;
        [SerializeField] private float _negativeIntensityReduction = 0.7f;

        private KarmaManager _karmaManager;
        private float _currentInfluence = 0.5f;
        private float _baseLightIntensity = 1.2f;

        private void Start()
        {
            _karmaManager = KarmaManager.Instance;
            
            if (_karmaManager != null)
            {
                _karmaManager.OnKarmaChanged.AddListener(OnKarmaChanged);
            }

            // Store base light intensity
            if (_mainLight != null)
            {
                _baseLightIntensity = _mainLight.intensity;
            }
        }

        private void Update()
        {
            UpdateVisuals();
        }

        private void UpdateVisuals()
        {
            if (_karmaManager == null) return;

            // Get karma influence (0 = negative, 0.5 = neutral, 1 = positive)
            float targetInfluence = Mathf.InverseLerp(-1000f, 1000f, _karmaManager.CurrentKarma);
            
            // Smooth transition
            _currentInfluence = Mathf.Lerp(_currentInfluence, targetInfluence, Time.deltaTime * 0.5f);

            // Update lighting
            UpdateLighting();

            // Update atmosphere
            UpdateAtmosphere();

            // Update particles
            UpdateParticles();
        }

        private void UpdateLighting()
        {
            if (_mainLight == null) return;

            // Adjust light color based on karma
            Color lightColor = Color.Lerp(_negativeColorTint, _positiveColorTint, _currentInfluence);
            _mainLight.color = lightColor;

            // Adjust intensity (positive karma = brighter world)
            float karmaModifier = Mathf.Lerp(_negativeIntensityReduction, _positiveIntensityBoost, _currentInfluence);
            _mainLight.intensity = _baseLightIntensity * karmaModifier;
        }

        private void UpdateAtmosphere()
        {
            if (_skyboxMaterial == null) return;

            // Adjust sky tint based on karma
            Color skyTint = Color.Lerp(_negativeColorTint, _positiveColorTint, _currentInfluence);
            
            // Only set if skybox material has these properties
            if (_skyboxMaterial.HasProperty("_SkyTint"))
            {
                _skyboxMaterial.SetColor("_SkyTint", skyTint);
            }

            // Adjust atmosphere thickness (negative karma = heavier atmosphere)
            if (_skyboxMaterial.HasProperty("_AtmosphereThickness"))
            {
                float atmosphereThickness = Mathf.Lerp(1.2f, 0.8f, _currentInfluence);
                _skyboxMaterial.SetFloat("_AtmosphereThickness", atmosphereThickness);
            }
        }

        private void UpdateParticles()
        {
            // Positive karma = light particles floating around
            if (_positiveParticles != null)
            {
                var emission = _positiveParticles.emission;
                float positiveRate = Mathf.Max(0, (_currentInfluence - 0.5f) * 100f);
                emission.rateOverTime = positiveRate;

                if (positiveRate > 0 && !_positiveParticles.isPlaying)
                    _positiveParticles.Play();
                else if (positiveRate == 0 && _positiveParticles.isPlaying)
                    _positiveParticles.Stop();
            }

            // Negative karma = dark particles
            if (_negativeParticles != null)
            {
                var emission = _negativeParticles.emission;
                float negativeRate = Mathf.Max(0, (0.5f - _currentInfluence) * 100f);
                emission.rateOverTime = negativeRate;

                if (negativeRate > 0 && !_negativeParticles.isPlaying)
                    _negativeParticles.Play();
                else if (negativeRate == 0 && _negativeParticles.isPlaying)
                    _negativeParticles.Stop();
            }
        }

        private void OnKarmaChanged(float amount, string reason)
        {
            // Spawn immediate visual feedback
            if (amount > 0)
            {
                SpawnPositiveFeedback();
            }
            else if (amount < 0)
            {
                SpawnNegativeFeedback();
            }
        }

        private void SpawnPositiveFeedback()
        {
            // TODO: Spawn particle burst, play sound, show visual effect
            Debug.Log("[Visual] ✨ Positive karma feedback");
        }

        private void SpawnNegativeFeedback()
        {
            // TODO: Spawn dark ripple, play sound
            Debug.Log("[Visual] 💔 Negative karma feedback");
        }

        private void OnDestroy()
        {
            if (_karmaManager != null)
            {
                _karmaManager.OnKarmaChanged.RemoveListener(OnKarmaChanged);
            }
        }

        #region Public Methods
        
        /// <summary>
        /// Set references manually if needed
        /// </summary>
        public void SetReferences(Light mainLight, Material skybox)
        {
            _mainLight = mainLight;
            _skyboxMaterial = skybox;
            
            if (_mainLight != null)
            {
                _baseLightIntensity = _mainLight.intensity;
            }
        }

        /// <summary>
        /// Get current karma influence (0-1)
        /// </summary>
        public float GetCurrentInfluence() => _currentInfluence;
        
        #endregion
    }
}

