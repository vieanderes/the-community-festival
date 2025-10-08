# Feature 04: AI Character System

## 🎯 Overview

Intelligent NPCs that feel alive - with personalities, memories, goals, and realistic behaviors. AI characters respond to player karma, help guide positive actions, and create meaningful social interactions.

---

## 📋 Dependencies

- ✅ Feature 01: Project Setup
- ✅ Feature 02: Environment System
- ✅ Feature 03: Karma System

---

## 🤖 System Components

### 1. Character Data & Profiles
### 2. Behavior Tree System
### 3. Navigation & Movement
### 4. Memory & Relationships
### 5. Dialogue System
### 6. Path Discovery AI (analyzes player behavior)
### 7. Helper/Guide Characters

---

## 📊 Character Architecture

### Character Types:
- **Festival-Goers:** Dance, socialize, explore
- **Volunteers:** Work tasks, help others
- **Artists/DJs:** Perform, interact with fans
- **Organizers:** Manage, coordinate, solve problems
- **Helpers:** Guide players toward positive actions

### AI Layers:
1. **Personality Layer:** Defines character traits
2. **Needs Layer:** Hunger, energy, social needs
3. **Goal Layer:** Current objectives
4. **Behavior Layer:** How to achieve goals
5. **Interaction Layer:** Social behaviors
6. **Memory Layer:** Remember player and events

---

## 🚀 Implementation

### Step 1: Create Character Data Structure

Create: `Assets/_Project/Scripts/Data/CharacterProfile.cs`

```csharp
using UnityEngine;
using System;
using System.Collections.Generic;

namespace LuminaFestival.Data
{
    /// <summary>
    /// ScriptableObject defining a character's permanent traits
    /// </summary>
    [CreateAssetMenu(fileName = "Character_", menuName = "Lumina/Character Profile")]
    public class CharacterProfile : ScriptableObject
    {
        [Header("Identity")]
        public string characterName;
        [TextArea(2, 4)]
        public string bio;
        public CharacterRole role;
        public Sprite portrait;

        [Header("Personality")]
        [Range(0f, 1f)] public float friendliness = 0.7f; // How approachable
        [Range(0f, 1f)] public float energy = 0.6f; // How active
        [Range(0f, 1f)] public float creativity = 0.5f; // How artistic
        [Range(0f, 1f)] public float helpfulness = 0.8f; // How much they help
        [Range(0f, 1f)] public float wisdom = 0.5f; // How much they teach

        [Header("Preferences")]
        public MusicGenre favoriteMusic;
        public List<string> interests = new List<string>();
        public List<string> skills = new List<string>();

        [Header("Dialogue")]
        public List<string> greetings = new List<string>();
        public List<string> helpOffers = new List<string>();
        public List<string> teachings = new List<string>();
        public List<string> encouragements = new List<string>();

        [Header("Visual")]
        public GameObject characterPrefab;
        public Color accentColor = Color.white;
    }

    public enum CharacterRole
    {
        FestivalGoer,
        Volunteer,
        Artist,
        Organizer,
        Helper, // Special guides
        Newcomer
    }

    public enum MusicGenre
    {
        Techno,
        House,
        Bass,
        Breakbeat,
        Downtempo,
        Ambient,
        AllGenres
    }
}
```

### Step 2: Create Character Instance (Runtime Data)

Create: `Assets/_Project/Scripts/AI/AICharacter.cs`

```csharp
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using LuminaFestival.Data;
using LuminaFestival.Core;

namespace LuminaFestival.AI
{
    /// <summary>
    /// Runtime instance of an AI character
    /// Handles behavior, needs, goals, and interactions
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class AICharacter : MonoBehaviour
    {
        [Header("Profile")]
        [SerializeField] private CharacterProfile _profile;

        [Header("Current State")]
        [SerializeField] private AIState _currentState = AIState.Idle;
        [SerializeField] private AIGoal _currentGoal;

        [Header("Needs (0-1)")]
        [SerializeField] private float _energy = 1f;
        [SerializeField] private float _social = 0.5f;
        [SerializeField] private float _entertainment = 0.5f;
        [SerializeField] private float _contribution = 0.5f; // Desire to help

        [Header("Relationships")]
        [SerializeField] private Dictionary<string, float> _relationships = 
            new Dictionary<string, float>(); // playerID -> relationship value (-1 to 1)

        [Header("Memory")]
        [SerializeField] private List<AIMemory> _memories = new List<AIMemory>();
        private const int MAX_MEMORIES = 20;

        [Header("Components")]
        private NavMeshAgent _agent;
        private Animator _animator;
        private AIBehaviorTree _behaviorTree;

        #region Properties
        public CharacterProfile Profile => _profile;
        public AIState CurrentState => _currentState;
        public string CharacterName => _profile != null ? _profile.characterName : "Unknown";
        #endregion

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponentInChildren<Animator>();
            _behaviorTree = GetComponent<AIBehaviorTree>();
        }

        private void Start()
        {
            Initialize();
        }

        private void Update()
        {
            UpdateNeeds();
            UpdateBehavior();
        }

        #region Initialization

        private void Initialize()
        {
            // Configure NavMesh agent
            if (_agent != null)
            {
                _agent.speed = 3.5f;
                _agent.angularSpeed = 120f;
                _agent.acceleration = 8f;
                _agent.stoppingDistance = 1f;
            }

            // Start with random need values
            _energy = Random.Range(0.6f, 1f);
            _social = Random.Range(0.3f, 0.7f);
            _entertainment = Random.Range(0.4f, 0.8f);
            _contribution = _profile != null ? _profile.helpfulness : 0.5f;

            Debug.Log($"[AI] {CharacterName} initialized as {_profile.role}");
        }

        #endregion

        #region Needs & Goals

        private void UpdateNeeds()
        {
            float deltaTime = Time.deltaTime;

            // Needs decay over time
            _energy -= 0.01f * deltaTime; // Gets tired
            _social -= 0.02f * deltaTime; // Wants interaction
            _entertainment -= 0.015f * deltaTime; // Gets bored
            _contribution -= 0.005f * deltaTime; // Wants to help

            // Clamp values
            _energy = Mathf.Clamp01(_energy);
            _social = Mathf.Clamp01(_social);
            _entertainment = Mathf.Clamp01(_entertainment);
            _contribution = Mathf.Clamp01(_contribution);

            // Determine most pressing need and set goal
            DetermineGoal();
        }

        private void DetermineGoal()
        {
            // Find lowest need
            float lowestNeed = 1f;
            AIGoal newGoal = AIGoal.Wander;

            if (_energy < lowestNeed)
            {
                lowestNeed = _energy;
                newGoal = _energy < 0.3f ? AIGoal.Rest : AIGoal.Wander;
            }

            if (_social < lowestNeed)
            {
                lowestNeed = _social;
                newGoal = AIGoal.Socialize;
            }

            if (_entertainment < lowestNeed)
            {
                lowestNeed = _entertainment;
                newGoal = AIGoal.Enjoy;
            }

            if (_contribution < 0.4f && _profile.helpfulness > 0.6f)
            {
                newGoal = AIGoal.Help;
            }

            // Only change goal if it's different
            if (newGoal != _currentGoal)
            {
                SetGoal(newGoal);
            }
        }

        private void SetGoal(AIGoal newGoal)
        {
            _currentGoal = newGoal;
            Debug.Log($"[AI] {CharacterName} new goal: {newGoal}");

            // Reset state when goal changes
            _currentState = AIState.Deciding;
        }

        #endregion

        #region Behavior

        private void UpdateBehavior()
        {
            // Behavior tree handles detailed behavior
            // This method coordinates high-level state

            switch (_currentState)
            {
                case AIState.Idle:
                    HandleIdle();
                    break;
                case AIState.Moving:
                    HandleMoving();
                    break;
                case AIState.Interacting:
                    HandleInteracting();
                    break;
                case AIState.Dancing:
                    HandleDancing();
                    break;
                case AIState.Working:
                    HandleWorking();
                    break;
                case AIState.Resting:
                    HandleResting();
                    break;
            }
        }

        private void HandleIdle()
        {
            // Look around, wait a bit, then decide what to do
            // This is placeholder - behavior tree will handle details
        }

        private void HandleMoving()
        {
            // Check if reached destination
            if (_agent != null && !_agent.pathPending && _agent.remainingDistance < 0.5f)
            {
                _currentState = AIState.Idle;
            }
        }

        private void HandleInteracting()
        {
            // Talking to someone
        }

        private void HandleDancing()
        {
            // At a stage, enjoying music
            _entertainment = Mathf.Min(1f, _entertainment + 0.05f * Time.deltaTime);
        }

        private void HandleWorking()
        {
            // Volunteering
            _contribution = Mathf.Min(1f, _contribution + 0.03f * Time.deltaTime);
        }

        private void HandleResting()
        {
            // Recharge energy
            _energy = Mathf.Min(1f, _energy + 0.1f * Time.deltaTime);

            if (_energy > 0.7f)
            {
                _currentState = AIState.Idle;
            }
        }

        #endregion

        #region Movement

        public void MoveTo(Vector3 destination)
        {
            if (_agent != null)
            {
                _agent.SetDestination(destination);
                _currentState = AIState.Moving;
            }
        }

        public void StopMoving()
        {
            if (_agent != null)
            {
                _agent.ResetPath();
                _currentState = AIState.Idle;
            }
        }

        #endregion

        #region Relationships

        public void ModifyRelationship(string characterID, float amount)
        {
            if (!_relationships.ContainsKey(characterID))
            {
                _relationships[characterID] = 0f;
            }

            _relationships[characterID] += amount;
            _relationships[characterID] = Mathf.Clamp(_relationships[characterID], -1f, 1f);

            Debug.Log($"[AI] {CharacterName} relationship with {characterID}: {_relationships[characterID]:F2}");
        }

        public float GetRelationship(string characterID)
        {
            return _relationships.ContainsKey(characterID) ? _relationships[characterID] : 0f;
        }

        #endregion

        #region Memory

        public void RememberEvent(string description, bool positive)
        {
            AIMemory memory = new AIMemory
            {
                description = description,
                isPositive = positive,
                timestamp = Time.time
            };

            _memories.Insert(0, memory);

            if (_memories.Count > MAX_MEMORIES)
            {
                _memories.RemoveAt(_memories.Count - 1);
            }
        }

        #endregion

        #region Interaction

        /// <summary>
        /// Called when player interacts with this character
        /// </summary>
        public void OnPlayerInteract(GameObject player)
        {
            _currentState = AIState.Interacting;

            // Get player's karma
            float playerKarma = KarmaManager.Instance.CurrentKarma;

            // React based on karma
            ReactToPlayer(player, playerKarma);
        }

        private void ReactToPlayer(GameObject player, float karma)
        {
            // Positive karma = friendly response
            if (karma > 200f)
            {
                Debug.Log($"[AI] {CharacterName}: Your positive energy is wonderful!");
                // TODO: Show dialogue
            }
            else if (karma < -200f)
            {
                Debug.Log($"[AI] {CharacterName}: I sense you're struggling. Can I help?");
                // Offer help to guide them back
            }
            else
            {
                Debug.Log($"[AI] {CharacterName}: {GetRandomGreeting()}");
            }
        }

        private string GetRandomGreeting()
        {
            if (_profile != null && _profile.greetings.Count > 0)
            {
                return _profile.greetings[Random.Range(0, _profile.greetings.Count)];
            }
            return "Hello!";
        }

        #endregion
    }

    #region Supporting Types

    public enum AIState
    {
        Idle,
        Deciding,
        Moving,
        Interacting,
        Dancing,
        Working,
        Resting,
        Helping
    }

    public enum AIGoal
    {
        Wander,
        Socialize,
        Enjoy, // Go to stage, dance
        Help, // Find someone to help
        Work, // Do volunteer task
        Rest,
        Explore
    }

    [System.Serializable]
    public struct AIMemory
    {
        public string description;
        public bool isPositive;
        public float timestamp;
    }

    #endregion
}
```

### Step 3: Create Helper AI (Guides)

Create: `Assets/_Project/Scripts/AI/HelperAI.cs`

```csharp
using UnityEngine;
using LuminaFestival.Core;
using LuminaFestival.Core.Systems;

namespace LuminaFestival.AI
{
    /// <summary>
    /// Special AI that appears to guide players toward positive actions
    /// Appears when player karma is low or when player seems lost
    /// </summary>
    public class HelperAI : AICharacter
    {
        [Header("Helper Settings")]
        [SerializeField] private float _appearThreshold = -200f; // Appears when karma drops below this
        [SerializeField] private float _disappearThreshold = 0f; // Leaves when karma improves
        [SerializeField] private bool _isVisible = false;

        [Header("Teaching")]
        [SerializeField] private float _teachingCooldown = 300f; // 5 minutes between lessons
        private float _lastTeachTime = -1000f;

        private KarmaManager _karmaManager;
        private Transform _player;

        private void Start()
        {
            _karmaManager = KarmaManager.Instance;
            
            // Start invisible
            SetVisibility(false);

            // Subscribe to karma changes
            if (_karmaManager != null)
            {
                _karmaManager.OnKarmaChanged.AddListener(OnKarmaChanged);
            }
        }

        private void Update()
        {
            base.Update();

            CheckIfShouldAppear();

            if (_isVisible)
            {
                UpdateHelping();
            }
        }

        private void CheckIfShouldAppear()
        {
            if (_karmaManager == null) return;

            float currentKarma = _karmaManager.CurrentKarma;

            // Should appear if karma is low
            if (!_isVisible && currentKarma < _appearThreshold)
            {
                Appear();
            }
            // Should disappear if karma improved
            else if (_isVisible && currentKarma > _disappearThreshold)
            {
                Disappear();
            }
        }

        private void Appear()
        {
            Debug.Log($"[Helper] {CharacterName} appears to offer guidance");
            
            SetVisibility(true);
            _isVisible = true;

            // Find player
            _player = FindPlayerTransform();

            if (_player != null)
            {
                // Appear near player
                Vector3 spawnPos = _player.position + Random.insideUnitSphere * 10f;
                spawnPos.y = _player.position.y;
                transform.position = spawnPos;

                // Move toward player
                MoveTo(_player.position);
            }

            // Show message
            ShowHelpMessage("Hello friend. I noticed you might be struggling. I'm here to help.");
        }

        private void Disappear()
        {
            Debug.Log($"[Helper] {CharacterName} sees improvement and leaves");
            
            ShowHelpMessage("You're finding your way. I'm proud of you. Be well.");

            // Fade out over time
            Invoke(nameof(FadeOut), 3f);
        }

        private void FadeOut()
        {
            SetVisibility(false);
            _isVisible = false;
        }

        private void UpdateHelping()
        {
            // Periodically offer teachings
            if (Time.time - _lastTeachTime > _teachingCooldown)
            {
                OfferTeaching();
                _lastTeachTime = Time.time;
            }

            // Follow player at distance
            if (_player != null && Vector3.Distance(transform.position, _player.position) > 15f)
            {
                MoveTo(_player.position);
            }
        }

        private void OfferTeaching()
        {
            if (Profile == null || Profile.teachings.Count == 0) return;

            string teaching = Profile.teachings[Random.Range(0, Profile.teachings.Count)];
            ShowHelpMessage(teaching);

            // Add small karma for listening
            if (_karmaManager != null)
            {
                _karmaManager.AddKarma(KarmaActionType.Learning, "Received guidance");
            }
        }

        private void OnKarmaChanged(float amount, string reason)
        {
            // Acknowledge positive actions
            if (_isVisible && amount > 0)
            {
                if (Random.value < 0.3f) // 30% chance to comment
                {
                    ShowHelpMessage("I saw what you did. That was kind.");
                }
            }
        }

        private void ShowHelpMessage(string message)
        {
            Debug.Log($"[{CharacterName}] {message}");
            // TODO: Show in UI dialogue system
        }

        private void SetVisibility(bool visible)
        {
            // Show/hide visual representation
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            foreach (Renderer r in renderers)
            {
                r.enabled = visible;
            }

            // Enable/disable collider
            Collider col = GetComponent<Collider>();
            if (col != null)
            {
                col.enabled = visible;
            }
        }

        private Transform FindPlayerTransform()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            return player != null ? player.transform : null;
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

- [ ] CharacterProfile ScriptableObject created
- [ ] AICharacter component implemented
- [ ] HelperAI guide system working
- [ ] NavMesh baked for festival grounds
- [ ] At least 3 character profiles created (test)
- [ ] Characters spawn and move correctly
- [ ] Needs system updates properly
- [ ] Helper AI appears when karma is low
- [ ] All scripts compile without errors
- [ ] Performance acceptable with 10+ AI characters

---

## 🎯 Next Steps

1. ✅ Complete AI character system
2. 📖 Read `05_MUSIC_SYSTEM.md`
3. 🎵 Implement audio-reactive music system

---

*Estimated Time: 10-14 hours*  
*Difficulty: Advanced*  
*Dependencies: Features 01-03*  
*Next Feature: 05_MUSIC_SYSTEM*

