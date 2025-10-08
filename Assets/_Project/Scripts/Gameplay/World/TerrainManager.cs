using UnityEngine;

namespace TheCommunityFestival.Gameplay.World
{
    /// <summary>
    /// Manages terrain details, LOD, and performance optimization
    /// </summary>
    [RequireComponent(typeof(Terrain))]
    public class TerrainManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Terrain _terrain;

        [Header("Performance Settings")]
        [SerializeField] private float _detailDistance = 80f;
        [SerializeField] private float _detailDensity = 0.8f;
        [SerializeField] private float _treeDistance = 1000f;
        [SerializeField] private float _billboardStart = 50f;

        [Header("Quality Settings")]
        [SerializeField] private int _maxMeshTrees = 500;
        [SerializeField] private bool _castShadows = true;

        private void Awake()
        {
            if (_terrain == null)
                _terrain = GetComponent<Terrain>();
        }

        private void Start()
        {
            ConfigureTerrain();
        }

        private void ConfigureTerrain()
        {
            // Get terrain data
            TerrainData terrainData = _terrain.terrainData;

            // Configure detail settings (grass, flowers)
            _terrain.detailObjectDistance = _detailDistance;
            _terrain.detailObjectDensity = _detailDensity;

            // Configure tree settings
            _terrain.treeDistance = _treeDistance;
            _terrain.treeBillboardDistance = _billboardStart;
            _terrain.treeMaximumFullLODCount = _maxMeshTrees;

            // Configure rendering
            _terrain.shadowCastingMode = _castShadows ? 
                UnityEngine.Rendering.ShadowCastingMode.On : 
                UnityEngine.Rendering.ShadowCastingMode.Off;

            // Pixel error for LOD (lower = better quality, higher = better performance)
            _terrain.heightmapPixelError = 5f; // Good balance for M1 Pro

            // Enable height-based LOD
            _terrain.allowAutoConnect = true;

            Debug.Log($"[TerrainManager] Configured terrain: {terrainData.size}");
        }

        #region Public Methods
        /// <summary>
        /// Adjust detail distance based on performance
        /// </summary>
        public void SetDetailDistance(float distance)
        {
            _detailDistance = Mathf.Clamp(distance, 20f, 150f);
            _terrain.detailObjectDistance = _detailDistance;
        }

        /// <summary>
        /// Adjust tree rendering distance
        /// </summary>
        public void SetTreeDistance(float distance)
        {
            _treeDistance = Mathf.Clamp(distance, 100f, 2000f);
            _terrain.treeDistance = _treeDistance;
        }
        #endregion
    }
}

