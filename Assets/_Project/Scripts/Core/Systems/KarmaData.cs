using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheCommunityFestival.Core.Systems
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

