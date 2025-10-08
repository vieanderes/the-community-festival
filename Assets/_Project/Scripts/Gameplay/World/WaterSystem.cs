using UnityEngine;

namespace TheCommunityFestival.Gameplay.World
{
    /// <summary>
    /// Animates water surface with waves and ripples
    /// </summary>
    public class WaterSystem : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Material _waterMaterial;

        [Header("Wave Settings")]
        [SerializeField] private float _waveSpeed = 0.1f;
        [SerializeField] private float _waveScale = 0.2f;

        [Header("Reflection")]
        [SerializeField] private bool _enableReflection = true;

        private Vector2 _offset;

        private void Update()
        {
            AnimateWater();
        }

        private void AnimateWater()
        {
            if (_waterMaterial == null) return;

            // Scroll water texture to simulate movement
            _offset.x += _waveSpeed * Time.deltaTime * 0.1f;
            _offset.y += _waveSpeed * Time.deltaTime * 0.05f;

            // Apply to material (if using texture)
            _waterMaterial.SetTextureOffset("_BaseMap", _offset);
        }

        private void OnValidate()
        {
            if (_waterMaterial != null)
            {
                // Update material parameters in editor
                _waterMaterial.SetFloat("_WaveScale", _waveScale);
            }
        }
    }
}

