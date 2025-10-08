using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using TheCommunityFestival.Data;
using TheCommunityFestival.Core;
using TheCommunityFestival.Core.Systems;
using TheCommunityFestival.Gameplay.Characters;

namespace TheCommunityFestival.AI
{
    /// <summary>
    /// Runtime instance of an AI character
    /// Handles behavior, needs, goals, and interactions
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class AICharacter : MonoBehaviour, IInteractable
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

        [Header("Movement")]
        [SerializeField] private float _wanderRadius = 20f;
        [SerializeField] private float _idleTimeMin = 2f;
        [SerializeField] private float _idleTimeMax = 5f;

        private NavMeshAgent _agent;
        private Animator _animator;
        private float _idleTimer;
        private Vector3 _wanderTarget;

        #region Properties
        public CharacterProfile Profile => _profile;
        public AIState CurrentState => _currentState;
        public string CharacterName => _profile != null ? _profile.characterName : "Unknown";
        #endregion

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponentInChildren<Animator>();
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

            // Set initial idle time
            _idleTimer = Random.Range(_idleTimeMin, _idleTimeMax);

            Debug.Log($"[AI] {CharacterName} initialized as {(_profile != null ? _profile.role.ToString() : "Unknown")}");
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
            if (Random.value < 0.01f) // Only check occasionally
            {
                DetermineGoal();
            }
        }

        private void DetermineGoal()
        {
            // Simple goal selection based on needs
            if (_energy < 0.3f)
            {
                SetGoal(AIGoal.Rest);
            }
            else if (_entertainment < 0.4f)
            {
                SetGoal(AIGoal.Enjoy);
            }
            else if (_social < 0.5f)
            {
                SetGoal(AIGoal.Socialize);
            }
            else
            {
                SetGoal(AIGoal.Wander);
            }
        }

        private void SetGoal(AIGoal newGoal)
        {
            if (newGoal != _currentGoal)
            {
                _currentGoal = newGoal;
                _currentState = AIState.Deciding;
                Debug.Log($"[AI] {CharacterName} new goal: {newGoal}");
            }
        }

        #endregion

        #region Behavior

        private void UpdateBehavior()
        {
            switch (_currentState)
            {
                case AIState.Idle:
                    HandleIdle();
                    break;
                case AIState.Moving:
                    HandleMoving();
                    break;
                case AIState.Deciding:
                    HandleDeciding();
                    break;
                case AIState.Resting:
                    HandleResting();
                    break;
            }
        }

        private void HandleIdle()
        {
            _idleTimer -= Time.deltaTime;
            
            if (_idleTimer <= 0)
            {
                _currentState = AIState.Deciding;
            }
        }

        private void HandleMoving()
        {
            // Check if reached destination
            if (_agent != null && !_agent.pathPending && _agent.remainingDistance < _agent.stoppingDistance)
            {
                _currentState = AIState.Idle;
                _idleTimer = Random.Range(_idleTimeMin, _idleTimeMax);
            }
        }

        private void HandleDeciding()
        {
            // Decide what to do based on current goal
            switch (_currentGoal)
            {
                case AIGoal.Wander:
                    StartWandering();
                    break;
                case AIGoal.Rest:
                    StartResting();
                    break;
                default:
                    StartWandering(); // Default behavior
                    break;
            }
        }

        private void HandleResting()
        {
            // Recharge energy
            _energy = Mathf.Min(1f, _energy + 0.1f * Time.deltaTime);

            if (_energy > 0.7f)
            {
                _currentState = AIState.Idle;
                _idleTimer = Random.Range(_idleTimeMin, _idleTimeMax);
            }
        }

        private void StartWandering()
        {
            Vector3 randomPoint = GetRandomPointOnNavMesh(transform.position, _wanderRadius);
            MoveTo(randomPoint);
        }

        private void StartResting()
        {
            _currentState = AIState.Resting;
            StopMoving();
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
            }
        }

        private Vector3 GetRandomPointOnNavMesh(Vector3 center, float radius)
        {
            Vector3 randomDirection = Random.insideUnitSphere * radius;
            randomDirection += center;
            
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, radius, NavMesh.AllAreas))
            {
                return hit.position;
            }
            
            return center;
        }

        #endregion

        #region Interaction

        /// <summary>
        /// Called when player interacts with this character
        /// </summary>
        public void Interact(GameObject player)
        {
            _currentState = AIState.Interacting;
            StopMoving();

            // Face the player
            Vector3 directionToPlayer = player.transform.position - transform.position;
            directionToPlayer.y = 0;
            if (directionToPlayer != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(directionToPlayer);
            }

            // Get player's karma
            float playerKarma = KarmaManager.Instance != null ? KarmaManager.Instance.CurrentKarma : 0f;

            // React based on karma
            ReactToPlayer(player, playerKarma);
            
            // Give karma for social interaction
            if (KarmaManager.Instance != null)
            {
                KarmaManager.Instance.AddKarma(KarmaActionType.Participating, $"Talked with {CharacterName}");
            }

            // Increase social need
            _social = Mathf.Min(1f, _social + 0.2f);

            // Return to previous behavior after a delay
            Invoke(nameof(ReturnToBehavior), 2f);
        }

        private void ReactToPlayer(GameObject player, float karma)
        {
            string message = "";

            // Positive karma = friendly response
            if (karma > 200f)
            {
                message = _profile != null && _profile.encouragements.Count > 0 
                    ? _profile.encouragements[Random.Range(0, _profile.encouragements.Count)]
                    : "Your positive energy is wonderful!";
            }
            else if (karma < -200f)
            {
                message = _profile != null && _profile.helpOffers.Count > 0 
                    ? _profile.helpOffers[Random.Range(0, _profile.helpOffers.Count)]
                    : "I sense you're struggling. Can I help?";
            }
            else
            {
                message = GetRandomGreeting();
            }

            Debug.Log($"[AI] {CharacterName}: {message}");
            // TODO: Show dialogue UI
        }

        private string GetRandomGreeting()
        {
            if (_profile != null && _profile.greetings.Count > 0)
            {
                return _profile.greetings[Random.Range(0, _profile.greetings.Count)];
            }
            return "Hello!";
        }

        private void ReturnToBehavior()
        {
            _currentState = AIState.Deciding;
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

    #endregion
}

