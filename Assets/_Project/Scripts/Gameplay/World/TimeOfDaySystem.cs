using UnityEngine;

namespace TheCommunityFestival.Gameplay.World
{
    /// <summary>
    /// Controls time of day, sun/moon rotation, and lighting changes
    /// </summary>
    public class TimeOfDaySystem : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Light _sun;
        [SerializeField] private Light _moon;
        [SerializeField] private Material _skyboxMaterial;

        [Header("Time Settings")]
        [SerializeField] private float _timeScale = 1f; // Real-time minutes per game hour
        [SerializeField] private float _currentTime = 12f; // 0-24 hours
        [SerializeField] private bool _pauseTime = false;

        [Header("Sun/Moon")]
        [SerializeField] private Gradient _sunColor;
        [SerializeField] private AnimationCurve _sunIntensity;
        [SerializeField] private AnimationCurve _ambientIntensity;

        [Header("Atmosphere")]
        [SerializeField] private Gradient _skyTint;
        [SerializeField] private Gradient _equatorColor;

        private float _timeProgress; // 0-1 normalized time

        private void Start()
        {
            if (_sun == null)
                _sun = GetComponent<Light>();

            InitializeGradients();
        }

        private void Update()
        {
            if (!_pauseTime)
            {
                UpdateTime();
            }

            UpdateLighting();
            UpdateSkybox();
        }

        private void UpdateTime()
        {
            // Increment time (time scale is real minutes per game hour)
            _currentTime += (Time.deltaTime / 60f) * _timeScale;

            // Wrap around 24 hours
            if (_currentTime >= 24f)
                _currentTime = 0f;

            // Calculate 0-1 progress through day
            _timeProgress = _currentTime / 24f;
        }

        private void UpdateLighting()
        {
            // Rotate sun based on time
            float sunAngle = (_timeProgress * 360f) - 90f;
            transform.rotation = Quaternion.Euler(sunAngle, 170f, 0f);

            // Update sun color and intensity
            if (_sun != null)
            {
                _sun.color = _sunColor.Evaluate(_timeProgress);
                _sun.intensity = _sunIntensity.Evaluate(_timeProgress);
            }

            // Update ambient lighting
            RenderSettings.ambientIntensity = _ambientIntensity.Evaluate(_timeProgress);
        }

        private void UpdateSkybox()
        {
            if (_skyboxMaterial != null)
            {
                _skyboxMaterial.SetColor("_SkyTint", _skyTint.Evaluate(_timeProgress));
                _skyboxMaterial.SetColor("_GroundColor", _equatorColor.Evaluate(_timeProgress));

                // Update exposure based on time of day
                float exposure = Mathf.Lerp(0.5f, 1.5f, _ambientIntensity.Evaluate(_timeProgress));
                _skyboxMaterial.SetFloat("_Exposure", exposure);
            }
        }

        private void InitializeGradients()
        {
            // Create default gradients if not set
            if (_sunColor == null || _sunColor.colorKeys.Length == 0)
            {
                _sunColor = new Gradient();
                var colorKeys = new GradientColorKey[4];
                colorKeys[0] = new GradientColorKey(new Color(0.3f, 0.3f, 0.5f), 0f); // Midnight
                colorKeys[1] = new GradientColorKey(new Color(1f, 0.7f, 0.4f), 0.25f); // Sunrise
                colorKeys[2] = new GradientColorKey(new Color(1f, 0.95f, 0.9f), 0.5f); // Noon
                colorKeys[3] = new GradientColorKey(new Color(1f, 0.5f, 0.3f), 0.75f); // Sunset

                var alphaKeys = new GradientAlphaKey[2];
                alphaKeys[0] = new GradientAlphaKey(1f, 0f);
                alphaKeys[1] = new GradientAlphaKey(1f, 1f);

                _sunColor.SetKeys(colorKeys, alphaKeys);
            }

            // Initialize curves if not set
            if (_sunIntensity == null || _sunIntensity.length == 0)
            {
                _sunIntensity = AnimationCurve.EaseInOut(0f, 0f, 1f, 1.2f);
            }

            if (_ambientIntensity == null || _ambientIntensity.length == 0)
            {
                _ambientIntensity = AnimationCurve.EaseInOut(0f, 0.2f, 1f, 1f);
            }
        }

        #region Public Methods
        public void SetTime(float hour)
        {
            _currentTime = Mathf.Clamp(hour, 0f, 24f);
        }

        public void SetTimeScale(float scale)
        {
            _timeScale = Mathf.Max(0f, scale);
        }

        public void PauseTime(bool pause)
        {
            _pauseTime = pause;
        }

        public float GetCurrentHour() => _currentTime;
        public float GetTimeProgress() => _timeProgress;
        #endregion
    }
}

