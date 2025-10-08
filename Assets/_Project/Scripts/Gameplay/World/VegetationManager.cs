using UnityEngine;

namespace TheCommunityFestival.Gameplay.World
{
    /// <summary>
    /// Manages grass, flowers, and detail vegetation
    /// Handles procedural distribution and wind animation
    /// </summary>
    public class VegetationManager : MonoBehaviour
    {
        [Header("Detail Settings")]
        [SerializeField] private Terrain _terrain;

        [Header("Distribution")]
        [SerializeField] private float _grassDensity = 0.7f; // Reserved for future use
        [SerializeField] private float _flowerDensity = 0.3f; // Reserved for future use

        [Header("Wind")]
        [SerializeField] private float _windSpeed = 0.5f;
        [SerializeField] private float _windStrength = 0.3f;

        private void Start()
        {
            SetupVegetation();
            ConfigureWind();
        }

        private void SetupVegetation()
        {
            if (_terrain == null)
            {
                _terrain = GetComponent<Terrain>();
            }

            if (_terrain == null)
            {
                Debug.LogWarning("[VegetationManager] No terrain found!");
                return;
            }

            // Grass and flowers will be added via Unity Terrain detail painter
            // This script manages their parameters

            // Configure grass wave settings
            float waveSpeed = _windSpeed;
            float waveSize = _windStrength;
            float waveAmount = 0.5f;

            // Apply to terrain
            _terrain.terrainData.wavingGrassSpeed = waveSpeed;
            _terrain.terrainData.wavingGrassStrength = waveSize;
            _terrain.terrainData.wavingGrassAmount = waveAmount;

            Debug.Log("[VegetationManager] Vegetation configured");
        }

        private void ConfigureWind()
        {
            if (_terrain == null) return;

            // Wind settings for grass movement
            float tint = 0.3f; // Slight color tint when grass moves
            _terrain.terrainData.wavingGrassTint = new Color(0.9f, 0.95f, 0.8f, tint);
        }

        #region Public Methods
        public void SetWindStrength(float strength)
        {
            _windStrength = Mathf.Clamp01(strength);
            if (_terrain != null)
                _terrain.terrainData.wavingGrassStrength = _windStrength;
        }

        public void SetWindSpeed(float speed)
        {
            _windSpeed = Mathf.Clamp(speed, 0f, 2f);
            if (_terrain != null)
                _terrain.terrainData.wavingGrassSpeed = _windSpeed;
        }
        #endregion
    }
}

