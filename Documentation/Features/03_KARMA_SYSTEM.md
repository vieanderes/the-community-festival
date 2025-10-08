# Feature 03: Karma System

## 🎯 Overview

The heart of Lumina Festival - a system that tracks player actions, influences world responsiveness, creates visual feedback, and teaches real-life values through meaningful gameplay consequences.

---

## 📋 Dependencies

- ✅ Feature 01: Project Setup
- ✅ Feature 02: Environment System (for visual feedback)

---

## 🌟 System Components

### 1. Karma Tracking & Storage
### 2. Action Recognition System
### 3. World Response System
### 4. Visual Feedback System
### 5. Guidance & Teaching System
### 6. Karma Persistence

---

## 📊 Karma Architecture

### Karma Structure:
```
Total Karma = Base Karma + Recent Actions Modifier
Range: -1000 to +1000
Neutral: 0
Positive Threshold: +100
Negative Threshold: -100
```

### Karma Levels:
- **Radiant (800-1000):** Pure light, everything flourishes
- **Harmonious (400-799):** Positive vibes, community thrives
- **Balanced (100-399):** Good person, world is welcoming
- **Neutral (-99 to 99):** Mixed energy, world is neutral
- **Dissonant (-100 to -399):** Negative actions noticed
- **Shadowed (-400 to -799):** Dark energy, world resists
- **Void (-800 to -1000):** Lost in darkness, help offered

---

## 🚀 Implementation

### Step 1: Create Karma Data Structure

Create: `Assets/_Project/Scripts/Core/Systems/KarmaData.cs`

```csharp
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LuminaFestival.Core.Systems
{
    /// <summary>
    /// Stores all karma-related data for a player
    /// </summary>
    [Serializable]
    public class KarmaData
    {
        [Header("Current State")]
        public float currentKarma = 0f;
        public KarmaLevel currentLevel = KarmaLevel.Neutral;

        [Header("Statistics")]
        public int totalPositiveActions = 0;
        public int totalNegativeActions = 0;
        public float lifetimeKarmaGained = 0f;
        public float lifetimeKarmaLost = 0f;

        [Header("History")]
        public List<KarmaEvent> recentEvents = new List<KarmaEvent>();
        public const int MAX_HISTORY = 50;

        /// <summary>
        /// Add karma and record the event
        /// </summary>
        public void AddKarma(float amount, string reason, KarmaActionType actionType)
        {
            // Update karma
            currentKarma = Mathf.Clamp(currentKarma + amount, -1000f, 1000f);

            // Update statistics
            if (amount > 0)
            {
                totalPositiveActions++;
                lifetimeKarmaGained += amount;
            }
            else if (amount < 0)
            {
                totalNegativeActions++;
                lifetimeKarmaLost += Mathf.Abs(amount);
            }

            // Record event
            RecordEvent(amount, reason, actionType);

            // Update level
            UpdateLevel();
        }

        private void RecordEvent(float amount, string reason, KarmaActionType actionType)
        {
            KarmaEvent newEvent = new KarmaEvent
            {
                amount = amount,
                reason = reason,
                actionType = actionType,
                timestamp = DateTime.Now
            };

            recentEvents.Insert(0, newEvent);

            // Keep only recent events
            if (recentEvents.Count > MAX_HISTORY)
            {
                recentEvents.RemoveAt(recentEvents.Count - 1);
            }
        }

        private void UpdateLevel()
        {
            KarmaLevel oldLevel = currentLevel;

            if (currentKarma >= 800f)
                currentLevel = KarmaLevel.Radiant;
            else if (currentKarma >= 400f)
                currentLevel = KarmaLevel.Harmonious;
            else if (currentKarma >= 100f)
                currentLevel = KarmaLevel.Balanced;
            else if (currentKarma >= -99f)
                currentLevel = KarmaLevel.Neutral;
            else if (currentKarma >= -399f)
                currentLevel = KarmaLevel.Dissonant;
            else if (currentKarma >= -799f)
                currentLevel = KarmaLevel.Shadowed;
            else
                currentLevel = KarmaLevel.Void;

            // Level changed
            if (oldLevel != currentLevel)
            {
                OnLevelChanged?.Invoke(oldLevel, currentLevel);
            }
        }

        /// <summary>
        /// Event triggered when karma level changes
        /// </summary>
        public event Action<KarmaLevel, KarmaLevel> OnLevelChanged;

        /// <summary>
        /// Get karma ratio (positive vs negative actions)
        /// </summary>
        public float GetKarmaRatio()
        {
            int total = totalPositiveActions + totalNegativeActions;
            if (total == 0) return 0.5f;
            return (float)totalPositiveActions / total;
        }
    }

    /// <summary>
    /// Individual karma event record
    /// </summary>
    [Serializable]
    public struct KarmaEvent
    {
        public float amount;
        public string reason;
        public KarmaActionType actionType;
        public DateTime timestamp;
    }

    /// <summary>
    /// Karma levels
    /// </summary>
    public enum KarmaLevel
    {
        Void = -3,
        Shadowed = -2,
        Dissonant = -1,
        Neutral = 0,
        Balanced = 1,
        Harmonious = 2,
        Radiant = 3
    }

    /// <summary>
    /// Types of karma-affecting actions
    /// </summary>
    public enum KarmaActionType
    {
        // Positive Actions
        Helping,
        Teaching,
        Sharing,
        Creating,
        Healing,
        Volunteering,
        Encouraging,
        Protecting,

        // Negative Actions
        Ignoring,
        Hoarding,
        Damaging,
        Littering,
        Conflict,
        Selfishness,

        // Neutral/Special
        Learning,
        Participating,
        Observing
    }
}
```

### Step 2: Create Karma Manager

Create: `Assets/_Project/Scripts/Core/Managers/KarmaManager.cs`

```csharp
using UnityEngine;
using UnityEngine.Events;
using LuminaFestival.Core.Systems;

namespace LuminaFestival.Core
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
        [SerializeField] private KarmaActionValues _actionValues;

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

            // Load action values if not set
            if (_actionValues == null)
            {
                _actionValues = CreateDefaultActionValues();
            }
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
            Debug.Log($"[Karma] {sign}{amount:F1} - {reason} ({actionType})");
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

        #region Default Values

        private KarmaActionValues CreateDefaultActionValues()
        {
            return new KarmaActionValues();
        }

        #endregion

        #region Debug

        [ContextMenu("Add Positive Karma")]
        private void Debug_AddPositiveKarma()
        {
            AddKarma(KarmaActionType.Helping, "Debug test");
        }

        [ContextMenu("Add Negative Karma")]
        private void Debug_AddNegativeKarma()
        {
            AddKarma(KarmaActionType.Ignoring, "Debug test");
        }

        [ContextMenu("Reset Karma")]
        private void Debug_ResetKarma()
        {
            _playerKarma = new KarmaData();
            _worldKarma = 0f;
            Debug.Log("[Karma] Reset to zero");
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
```

### Step 3: Visual Feedback System

Create: `Assets/_Project/Scripts/Gameplay/World/KarmaVisualFeedback.cs`

```csharp
using UnityEngine;
using LuminaFestival.Core;
using LuminaFestival.Core.Systems;

namespace LuminaFestival.Gameplay.World
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

        private KarmaManager _karmaManager;
        private float _currentInfluence = 0.5f;

        private void Start()
        {
            _karmaManager = KarmaManager.Instance;
            
            if (_karmaManager != null)
            {
                _karmaManager.OnKarmaChanged.AddListener(OnKarmaChanged);
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
            float baseIntensity = _mainLight.intensity;
            float karmaModifier = Mathf.Lerp(0.7f, 1.3f, _currentInfluence);
            // Note: This modifies base intensity, might want to store original value
        }

        private void UpdateAtmosphere()
        {
            if (_skyboxMaterial == null) return;

            // Adjust sky tint based on karma
            Color skyTint = Color.Lerp(_negativeColorTint, _positiveColorTint, _currentInfluence);
            _skyboxMaterial.SetColor("_SkyTint", skyTint);

            // Adjust atmosphere thickness (negative karma = heavier atmosphere)
            float atmosphereThickness = Mathf.Lerp(1.2f, 0.8f, _currentInfluence);
            _skyboxMaterial.SetFloat("_AtmosphereThickness", atmosphereThickness);
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
            // Could spawn immediate visual feedback here
            // E.g., a burst of light for positive action
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
            Debug.Log("[Visual] Positive karma feedback");
        }

        private void SpawnNegativeFeedback()
        {
            // TODO: Spawn dark ripple, play sound
            Debug.Log("[Visual] Negative karma feedback");
        }

        private void OnDestroy()
        {
            if (_karmaManager != null)
            {
                _karmaManager.OnKarmaChanged.RemoveListener(OnKarmaChanged);
            }
        }
    }
}
```

---

## ✅ Implementation Checklist

- [ ] KarmaData class created with proper structure
- [ ] KarmaManager singleton implemented
- [ ] Karma action values configured
- [ ] Level system working correctly
- [ ] Visual feedback system connected
- [ ] Particle effects for positive/negative karma
- [ ] Lighting responds to karma level
- [ ] Atmosphere changes with karma
- [ ] Debug tools working (context menu items)
- [ ] All scripts compile without errors

---

## 🎯 Testing

1. **Karma Tracking:**
   - Add positive karma via debug menu
   - Verify karma increases
   - Check level changes at thresholds

2. **Visual Feedback:**
   - Watch lighting change with karma
   - Observe particle effects
   - Confirm smooth transitions

3. **Persistence:**
   - Change karma
   - (Save/load will be implemented in later feature)

---

## 🎨 Next Steps

1. ✅ Complete karma system implementation
2. 📖 Read `04_AI_CHARACTER_SYSTEM.md`
3. 🤖 Create intelligent NPCs that respond to karma

---

*Estimated Time: 6-8 hours*  
*Difficulty: Intermediate*  
*Dependencies: Features 01-02*  
*Next Feature: 04_AI_CHARACTER_SYSTEM*

