using UnityEngine;

namespace TheCommunityFestival.Gameplay.World
{
    /// <summary>
    /// Controls weather effects (rain, clouds, fog)
    /// Weather responds to collective karma
    /// </summary>
    public class WeatherSystem : MonoBehaviour
    {
        [Header("Current Weather")]
        [SerializeField] private WeatherType _currentWeather = WeatherType.Clear;
        [SerializeField] private float _weatherIntensity = 0f;

        [Header("Transition")]
        [SerializeField] private float _transitionSpeed = 0.1f;

        [Header("Effects")]
        [SerializeField] private ParticleSystem _rainEffect;
        [SerializeField] private ParticleSystem _cloudParticles;

        [Header("Audio")]
        [SerializeField] private AudioSource _weatherAudio;
        [SerializeField] private AudioClip _rainSound;

        private float _targetIntensity;

        private void Update()
        {
            UpdateWeatherTransition();
            UpdateEffects();
        }

        private void UpdateWeatherTransition()
        {
            // Smooth transition to target intensity
            _weatherIntensity = Mathf.Lerp(_weatherIntensity, _targetIntensity, 
                _transitionSpeed * Time.deltaTime);
        }

        private void UpdateEffects()
        {
            // Update particle systems based on weather
            switch (_currentWeather)
            {
                case WeatherType.Clear:
                    SetRainIntensity(0f);
                    SetFog(0f);
                    break;

                case WeatherType.Cloudy:
                    SetRainIntensity(0f);
                    SetFog(0.3f * _weatherIntensity);
                    break;

                case WeatherType.Rain:
                    SetRainIntensity(_weatherIntensity);
                    SetFog(0.5f * _weatherIntensity);
                    break;
            }

            // Update audio
            if (_weatherAudio != null && _rainSound != null)
            {
                _weatherAudio.volume = _weatherIntensity * 0.5f;
            }
        }

        private void SetRainIntensity(float intensity)
        {
            if (_rainEffect != null)
            {
                var emission = _rainEffect.emission;
                emission.rateOverTime = intensity * 1000f; // Max 1000 particles/sec

                if (intensity > 0 && !_rainEffect.isPlaying)
                    _rainEffect.Play();
                else if (intensity == 0 && _rainEffect.isPlaying)
                    _rainEffect.Stop();
            }
        }

        private void SetFog(float density)
        {
            RenderSettings.fogDensity = density * 0.01f;
            RenderSettings.fog = density > 0;
        }

        #region Public Methods
        public void SetWeather(WeatherType weather, float intensity = 1f)
        {
            _currentWeather = weather;
            _targetIntensity = Mathf.Clamp01(intensity);

            Debug.Log($"[WeatherSystem] Changing to {weather} (intensity: {intensity})");
        }

        public WeatherType GetCurrentWeather() => _currentWeather;
        public float GetWeatherIntensity() => _weatherIntensity;
        #endregion
    }

    public enum WeatherType
    {
        Clear,
        Cloudy,
        Rain,
        // Future: Fog, Storm, etc.
    }
}

