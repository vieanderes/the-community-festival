using UnityEngine;
using UnityEngine.Events;
using TheCommunityFestival.Core.Systems;

namespace TheCommunityFestival.Core
{
    /// <summary>
    /// Central manager for all karma-related operations
    /// Singleton that persists across scenes
    /// </summary>
    public class KarmaManager : Singleton<KarmaManager>
    {
        [Header("Player Karma")]
        [SerializeField] private KarmaData _playerKarma = new KarmaData();

        [Header("World Karma (Collective)")]
        [SerializeField] private float _worldKarma = 0f;
        [SerializeField] private float _worldKarmaDecayRate = 0.1f; // Per hour

        [Header("Karma Values")]
        [SerializeField] private KarmaActionValues _actionValues = new KarmaActionValues();

        [Header("Events")]
        public UnityEvent<float, string> OnKarmaChanged = new UnityEvent<float, string>();
        public UnityEvent<KarmaLevel> OnLevelChanged = new UnityEvent<KarmaLevel>();

        #region Properties
        public KarmaData PlayerKarma => _playerKarma;
        public float CurrentKarma => _playerKarma.currentKarma;
        public KarmaLevel CurrentLevel => _playerKarma.currentLevel;
        public float WorldKarma => _worldKarma;
        #endregion

        protected override void Awake()
        {
            base.Awake();
            
            // Subscribe to level changes
            _playerKarma.OnLevelChanged += HandleLevelChange;
        }

        private void Update()
        {
            UpdateWorldKarma();
        }

        #region Karma Operations

        /// <summary>
        /// Add karma to player
        /// </summary>
        public void AddKarma(KarmaActionType actionType, string reason = "")
        {
            float amount = GetKarmaValue(actionType);
            AddKarma(amount, reason, actionType);
        }

        /// <summary>
        /// Add specific karma amount
        /// </summary>
        public void AddKarma(float amount, string reason, KarmaActionType actionType)
        {
            // Record in player data
            _playerKarma.AddKarma(amount, reason, actionType);

            // Affect world karma (collective)
            _worldKarma += amount * 0.1f; // 10% of player karma affects world
            _worldKarma = Mathf.Clamp(_worldKarma, -1000f, 1000f);

            // Trigger events
            OnKarmaChanged?.Invoke(amount, reason);

            // Log for debugging
            string sign = amount >= 0 ? "+" : "";
            Debug.Log($"[Karma] {sign}{amount:F1} - {reason} ({actionType}) | Total: {CurrentKarma:F1} | Level: {CurrentLevel}");
        }

        /// <summary>
        /// Get karma value for an action type
        /// </summary>
        private float GetKarmaValue(KarmaActionType actionType)
        {
            return _actionValues.GetValue(actionType);
        }

        #endregion

        #region World Karma

        private void UpdateWorldKarma()
        {
            // World karma slowly decays toward neutral
            if (_worldKarma > 0)
            {
                _worldKarma -= _worldKarmaDecayRate * Time.deltaTime;
                _worldKarma = Mathf.Max(0, _worldKarma);
            }
            else if (_worldKarma < 0)
            {
                _worldKarma += _worldKarmaDecayRate * Time.deltaTime;
                _worldKarma = Mathf.Min(0, _worldKarma);
            }
        }

        /// <summary>
        /// Get world karma influence (0-1 where 1 is maximum positive)
        /// </summary>
        public float GetWorldKarmaInfluence()
        {
            return Mathf.InverseLerp(-1000f, 1000f, _worldKarma);
        }

        #endregion

        #region Level Changes

        private void HandleLevelChange(KarmaLevel oldLevel, KarmaLevel newLevel)
        {
            Debug.Log($"[Karma] Level changed: {oldLevel} → {newLevel}");
            OnLevelChanged?.Invoke(newLevel);

            // Show notification
            ShowLevelChangeNotification(oldLevel, newLevel);
        }

        private void ShowLevelChangeNotification(KarmaLevel oldLevel, KarmaLevel newLevel)
        {
            // TODO: Show UI notification
            // For now, just debug
            bool improved = (int)newLevel > (int)oldLevel;
            string message = improved ? 
                $"Your positive energy is growing! You've reached {newLevel}" :
                $"Your energy has shifted to {newLevel}. Perhaps it's time to reconnect?";

            Debug.Log($"[Karma Notification] {message}");
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Get color representing current karma level
        /// </summary>
        public Color GetKarmaColor()
        {
            return GetKarmaColor(_playerKarma.currentLevel);
        }

        public static Color GetKarmaColor(KarmaLevel level)
        {
            switch (level)
            {
                case KarmaLevel.Radiant:
                    return new Color(1f, 0.95f, 0.6f); // Warm golden
                case KarmaLevel.Harmonious:
                    return new Color(0.6f, 1f, 0.8f); // Soft green
                case KarmaLevel.Balanced:
                    return new Color(0.8f, 0.9f, 1f); // Light blue
                case KarmaLevel.Neutral:
                    return Color.white;
                case KarmaLevel.Dissonant:
                    return new Color(0.9f, 0.8f, 0.7f); // Muted
                case KarmaLevel.Shadowed:
                    return new Color(0.6f, 0.5f, 0.6f); // Gray purple
                case KarmaLevel.Void:
                    return new Color(0.3f, 0.3f, 0.4f); // Dark blue-gray
                default:
                    return Color.white;
            }
        }

        /// <summary>
        /// Get description of karma level
        /// </summary>
        public static string GetLevelDescription(KarmaLevel level)
        {
            switch (level)
            {
                case KarmaLevel.Radiant:
                    return "You are a beacon of light. The world flourishes around you.";
                case KarmaLevel.Harmonious:
                    return "Your positive energy creates harmony wherever you go.";
                case KarmaLevel.Balanced:
                    return "You walk a path of kindness and growth.";
                case KarmaLevel.Neutral:
                    return "You are finding your way. Each choice matters.";
                case KarmaLevel.Dissonant:
                    return "The world feels your discord. Reconnection is possible.";
                case KarmaLevel.Shadowed:
                    return "Shadows gather. Help is offered if you seek it.";
                case KarmaLevel.Void:
                    return "Lost in darkness, but never alone. Reach out.";
                default:
                    return "";
            }
        }

        #endregion

        #region Debug

        [ContextMenu("Add Positive Karma (+10 Helping)")]
        private void Debug_AddPositiveKarma()
        {
            AddKarma(KarmaActionType.Helping, "Debug test");
        }

        [ContextMenu("Add Negative Karma (-5 Ignoring)")]
        private void Debug_AddNegativeKarma()
        {
            AddKarma(KarmaActionType.Ignoring, "Debug test");
        }

        [ContextMenu("Reset Karma to Zero")]
        private void Debug_ResetKarma()
        {
            _playerKarma = new KarmaData();
            _worldKarma = 0f;
            Debug.Log("[Karma] Reset to zero");
        }

        [ContextMenu("Set Karma to Radiant (+900)")]
        private void Debug_SetRadiant()
        {
            AddKarma(900f, "Debug - Set to Radiant", KarmaActionType.Volunteering);
        }

        #endregion
    }

    /// <summary>
    /// Defines karma point values for each action type
    /// </summary>
    [System.Serializable]
    public class KarmaActionValues
    {
        [Header("Positive Actions")]
        public float helping = 10f;
        public float teaching = 15f;
        public float sharing = 8f;
        public float creating = 12f;
        public float healing = 20f;
        public float volunteering = 25f;
        public float encouraging = 5f;
        public float protecting = 18f;

        [Header("Negative Actions")]
        public float ignoring = -5f;
        public float hoarding = -8f;
        public float damaging = -15f;
        public float littering = -10f;
        public float conflict = -20f;
        public float selfishness = -12f;

        [Header("Neutral Actions")]
        public float learning = 3f;
        public float participating = 2f;
        public float observing = 1f;

        public float GetValue(KarmaActionType actionType)
        {
            switch (actionType)
            {
                case KarmaActionType.Helping: return helping;
                case KarmaActionType.Teaching: return teaching;
                case KarmaActionType.Sharing: return sharing;
                case KarmaActionType.Creating: return creating;
                case KarmaActionType.Healing: return healing;
                case KarmaActionType.Volunteering: return volunteering;
                case KarmaActionType.Encouraging: return encouraging;
                case KarmaActionType.Protecting: return protecting;
                case KarmaActionType.Ignoring: return ignoring;
                case KarmaActionType.Hoarding: return hoarding;
                case KarmaActionType.Damaging: return damaging;
                case KarmaActionType.Littering: return littering;
                case KarmaActionType.Conflict: return conflict;
                case KarmaActionType.Selfishness: return selfishness;
                case KarmaActionType.Learning: return learning;
                case KarmaActionType.Participating: return participating;
                case KarmaActionType.Observing: return observing;
                default: return 0f;
            }
        }
    }
}

