using UnityEngine;

namespace TheCommunityFestival.Core
{
    /// <summary>
    /// Main game manager - singleton that persists across scenes
    /// Coordinates all other managers and core systems
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        #region Singleton
        private static GameManager _instance;
        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<GameManager>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("GameManager");
                        _instance = go.AddComponent<GameManager>();
                    }
                }
                return _instance;
            }
        }
        #endregion

        [Header("Game State")]
        [SerializeField] private GameState _currentState = GameState.MainMenu;

        [Header("Performance Settings")]
        [SerializeField] private int _targetFrameRate = 60;
        [SerializeField] private bool _enableVSync = true;

        #region Properties
        public GameState CurrentState => _currentState;
        #endregion

        #region Unity Lifecycle
        private void Awake()
        {
            // Singleton pattern
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);

            Initialize();
        }

        private void Start()
        {
            ConfigurePerformance();
        }
        #endregion

        #region Initialization
        private void Initialize()
        {
            Debug.Log("[GameManager] Initializing...");

            // TODO: Initialize all managers here
            // - KarmaManager
            // - MusicManager
            // - AIManager
            // - NetworkManager
            // etc.

            Debug.Log("[GameManager] Initialization complete");
        }

        private void ConfigurePerformance()
        {
            // Set target frame rate
            Application.targetFrameRate = _targetFrameRate;

            // Configure VSync
            QualitySettings.vSyncCount = _enableVSync ? 1 : 0;

            // M1 Pro optimizations
            QualitySettings.maxQueuedFrames = 2;

            Debug.Log($"[GameManager] Performance configured: {_targetFrameRate}fps, VSync: {_enableVSync}");
        }
        #endregion

        #region State Management
        public void ChangeState(GameState newState)
        {
            if (_currentState == newState) return;

            Debug.Log($"[GameManager] State change: {_currentState} → {newState}");

            _currentState = newState;

            // Handle state-specific logic
            switch (newState)
            {
                case GameState.MainMenu:
                    HandleMainMenuState();
                    break;
                case GameState.Festival:
                    HandleFestivalState();
                    break;
                case GameState.Paused:
                    HandlePausedState();
                    break;
            }
        }

        private void HandleMainMenuState()
        {
            Time.timeScale = 1f;
            // TODO: Load main menu scene
        }

        private void HandleFestivalState()
        {
            Time.timeScale = 1f;
            // TODO: Initialize festival systems
        }

        private void HandlePausedState()
        {
            Time.timeScale = 0f;
            // TODO: Show pause menu
        }
        #endregion

        #region Application Lifecycle
        private void OnApplicationQuit()
        {
            Debug.Log("[GameManager] Application quitting - saving data...");
            // TODO: Save all necessary data
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                Debug.Log("[GameManager] Application paused - saving data...");
                // TODO: Auto-save
            }
        }
        #endregion
    }

    /// <summary>
    /// Possible game states
    /// </summary>
    public enum GameState
    {
        MainMenu,
        Loading,
        Festival,
        Paused,
        Settings
    }
}

