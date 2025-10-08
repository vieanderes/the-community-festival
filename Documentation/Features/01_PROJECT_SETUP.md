# Feature 01: Project Setup & Configuration

## 🎯 Overview

Initial Unity project setup optimized for Apple Silicon M1 Pro with 16GB RAM. This establishes the foundation for all future development.

---

## 📋 Prerequisites

### Software Requirements:
- **macOS:** Sonoma 14.0 or later
- **Unity Hub:** 3.8.0 or later
- **Unity Version:** 2023.3.15f1 LTS (specifically this version)
- **Xcode:** 15.4 or later (installed via App Store)
- **Git:** 2.40+ (comes with Xcode Command Line Tools)

### Hardware Verification:
- Apple M1 Pro chip
- 16GB unified memory
- 50GB+ free disk space
- Active internet connection

---

## 🚀 Step-by-Step Implementation

### Step 1: Install Unity Hub & Editor

```bash
# Download Unity Hub from:
# https://unity.com/download

# Install Unity 2023.3.15f1 LTS via Unity Hub
# Modules to include:
# - macOS Build Support (Mono)
# - macOS Build Support (IL2CPP)
# - Documentation
# - Mac Server Build Support
```

**Important Modules:**
- ✅ macOS Build Support (Mono) - For development builds
- ✅ macOS Build Support (IL2CPP) - For release builds
- ✅ Documentation - Offline reference
- ✅ Mac Server Build Support - For multiplayer server
- ❌ iOS Build Support - Not needed initially
- ❌ Android Build Support - Not needed
- ❌ Windows Build Support - Not needed

### Step 2: Create New Unity Project

1. Open Unity Hub
2. Click "New Project"
3. Select Unity 2023.3.15f1
4. Choose "3D (URP)" template
5. Project Name: `TheCommunityFestival`
6. Location: `/Users/vie/Projects/the-festival-community/`
7. Click "Create Project"

**Why URP (Universal Render Pipeline)?**
- Optimized for Apple Silicon
- Better performance on M1 Pro
- Modern rendering features
- Excellent for realistic graphics
- Lower overhead than HDRP

### Step 3: Configure Project Settings

#### Graphics Settings

**File → Project Settings → Graphics**

```
Scriptable Render Pipeline Settings: URP-HighFidelity
```

#### Quality Settings

**File → Project Settings → Quality**

Create three quality presets:

**Low Quality (for testing):**
- Pixel Light Count: 2
- Texture Quality: Half Res
- Anisotropic Textures: Disabled
- Anti Aliasing: Disabled
- Soft Particles: No
- Shadows: Disable
- Shadow Resolution: Low
- Shadow Distance: 50
- VSync: Don't Sync
- Target Frame Rate: 30

**Medium Quality (development default):**
- Pixel Light Count: 4
- Texture Quality: Full Res
- Anisotropic Textures: Per Texture
- Anti Aliasing: 2x Multi Sampling
- Soft Particles: Yes
- Shadows: Hard Shadows
- Shadow Resolution: Medium
- Shadow Distance: 100
- VSync: Don't Sync
- Target Frame Rate: 60

**High Quality (target release):**
- Pixel Light Count: 8
- Texture Quality: Full Res
- Anisotropic Textures: Forced On
- Anti Aliasing: 4x Multi Sampling
- Soft Particles: Yes
- Shadows: Soft Shadows
- Shadow Resolution: Very High
- Shadow Distance: 150
- VSync: Every V Blank
- Target Frame Rate: 60

**Set Default Quality Level:** Medium (for development)

#### Player Settings

**File → Project Settings → Player**

**Company Name:** Your name/studio
**Product Name:** The Community Festival
**Version:** 0.1.0
**Default Icon:** (Will add later)

**Other Settings:**
- Color Space: Linear (for realistic lighting)
- Auto Graphics API: Disabled
- Graphics APIs: Metal (move to top, remove others)
- Scripting Backend: IL2CPP
- API Compatibility Level: .NET Standard 2.1
- Target SDK: macOS 14.0
- Minimum OS Version: macOS 14.0

**Optimization:**
- Prebake Collision Meshes: Enabled
- Keep Loaded Shaders Alive: Enabled
- Preloaded Assets: (empty for now)

#### Physics Settings

**File → Project Settings → Physics**

- Gravity: Y = -9.81
- Default Material: (create later)
- Bounce Threshold: 2
- Default Contact Offset: 0.01
- Sleep Threshold: 0.005
- Default Solver Iterations: 6
- Default Solver Velocity Iterations: 1
- Queries Hit Backfaces: Disabled
- Queries Hit Triggers: Enabled
- Enable Adaptive Force: Disabled
- Layer Collision Matrix: (will configure with layers)

#### Time Settings

**File → Project Settings → Time**

- Fixed Timestep: 0.02 (50 physics updates/second)
- Maximum Allowed Timestep: 0.1
- Time Scale: 1
- Maximum Particle Timestep: 0.03

#### Audio Settings

**File → Project Settings → Audio**

- Global Volume: 1
- Volume Rolloff Scale: 1
- Doppler Factor: 1
- Default Speaker Mode: Stereo
- DSP Buffer Size: Best Performance (for M1 Pro)
- Virtual Voice Count: 512
- Real Voice Count: 32
- Sample Rate: 48000 Hz

### Step 4: Configure URP Asset

**Assets → Create → Rendering → URP Asset (with Universal Renderer)**

Name it: `URP-HighFidelity`

**Settings:**

**General:**
- Depth Texture: Enabled
- Opaque Texture: Disabled (performance)
- Terrain Holes: Enabled

**Quality:**
- HDR: Enabled
- MSAA: 4x
- Render Scale: 1.0
- Upscaling Filter: Automatic

**Lighting:**
- Main Light: Enabled
  - Cast Shadows: Enabled
  - Shadow Resolution: 2048
- Additional Lights: Enabled
  - Per Object Limit: 8
  - Cast Shadows: Enabled
  - Shadow Resolution: 512
- Reflection Probes: Enabled
- Mixed Lighting: Enabled

**Shadows:**
- Max Distance: 150
- Cascade Count: 4
- Depth Bias: 1
- Normal Bias: 1
- Soft Shadows: Enabled

**Post-processing:**
- Feature Set: Post Processing
- Grading Mode: HDR
- LUT Size: 32

**Assign to Graphics:**
Edit → Project Settings → Graphics → Scriptable Render Pipeline Settings = URP-HighFidelity

### Step 5: Create Folder Structure

Create this exact structure in the Assets folder:

```
Assets/
├── _Project/
│   ├── Art/
│   │   ├── Materials/
│   │   ├── Models/
│   │   ├── Textures/
│   │   ├── Shaders/
│   │   └── Animations/
│   ├── Audio/
│   │   ├── Music/
│   │   ├── SFX/
│   │   └── Ambience/
│   ├── Prefabs/
│   │   ├── Characters/
│   │   ├── Environment/
│   │   ├── Stages/
│   │   ├── UI/
│   │   └── VFX/
│   ├── Scenes/
│   │   └── TestScenes/
│   ├── Scripts/
│   │   ├── Core/
│   │   │   ├── Managers/
│   │   │   ├── Systems/
│   │   │   └── Utilities/
│   │   ├── Gameplay/
│   │   │   ├── Characters/
│   │   │   ├── Interactions/
│   │   │   └── World/
│   │   ├── Networking/
│   │   ├── AI/
│   │   │   ├── Behaviors/
│   │   │   ├── Navigation/
│   │   │   └── Decision/
│   │   ├── Audio/
│   │   ├── UI/
│   │   │   ├── Menus/
│   │   │   ├── HUD/
│   │   │   └── Dialogs/
│   │   └── Data/
│   ├── Data/
│   │   ├── ScriptableObjects/
│   │   │   ├── Characters/
│   │   │   ├── Music/
│   │   │   ├── Stages/
│   │   │   └── Events/
│   │   └── Configuration/
│   └── Settings/
│       ├── URP/
│       └── Input/
├── Plugins/
└── ThirdParty/
```

**Create folders script (optional):**

Create a file: `Assets/Editor/CreateFolderStructure.cs`

```csharp
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

public class CreateFolderStructure
{
    [MenuItem("Tools/Setup/Create Folder Structure")]
    static void CreateFolders()
    {
        string[] folders = new string[]
        {
            "Assets/_Project",
            "Assets/_Project/Art",
            "Assets/_Project/Art/Materials",
            "Assets/_Project/Art/Models",
            "Assets/_Project/Art/Textures",
            "Assets/_Project/Art/Shaders",
            "Assets/_Project/Art/Animations",
            "Assets/_Project/Audio",
            "Assets/_Project/Audio/Music",
            "Assets/_Project/Audio/SFX",
            "Assets/_Project/Audio/Ambience",
            "Assets/_Project/Prefabs",
            "Assets/_Project/Prefabs/Characters",
            "Assets/_Project/Prefabs/Environment",
            "Assets/_Project/Prefabs/Stages",
            "Assets/_Project/Prefabs/UI",
            "Assets/_Project/Prefabs/VFX",
            "Assets/_Project/Scenes",
            "Assets/_Project/Scenes/TestScenes",
            "Assets/_Project/Scripts",
            "Assets/_Project/Scripts/Core",
            "Assets/_Project/Scripts/Core/Managers",
            "Assets/_Project/Scripts/Core/Systems",
            "Assets/_Project/Scripts/Core/Utilities",
            "Assets/_Project/Scripts/Gameplay",
            "Assets/_Project/Scripts/Gameplay/Characters",
            "Assets/_Project/Scripts/Gameplay/Interactions",
            "Assets/_Project/Scripts/Gameplay/World",
            "Assets/_Project/Scripts/Networking",
            "Assets/_Project/Scripts/AI",
            "Assets/_Project/Scripts/AI/Behaviors",
            "Assets/_Project/Scripts/AI/Navigation",
            "Assets/_Project/Scripts/AI/Decision",
            "Assets/_Project/Scripts/Audio",
            "Assets/_Project/Scripts/UI",
            "Assets/_Project/Scripts/UI/Menus",
            "Assets/_Project/Scripts/UI/HUD",
            "Assets/_Project/Scripts/UI/Dialogs",
            "Assets/_Project/Scripts/Data",
            "Assets/_Project/Data",
            "Assets/_Project/Data/ScriptableObjects",
            "Assets/_Project/Data/ScriptableObjects/Characters",
            "Assets/_Project/Data/ScriptableObjects/Music",
            "Assets/_Project/Data/ScriptableObjects/Stages",
            "Assets/_Project/Data/ScriptableObjects/Events",
            "Assets/_Project/Data/Configuration",
            "Assets/_Project/Settings",
            "Assets/_Project/Settings/URP",
            "Assets/_Project/Settings/Input",
            "Assets/Plugins",
            "Assets/ThirdParty"
        };

        foreach (string folder in folders)
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
                Debug.Log($"Created folder: {folder}");
            }
        }

        AssetDatabase.Refresh();
        Debug.Log("Folder structure created successfully!");
    }
}
#endif
```

After saving, go to: **Tools → Setup → Create Folder Structure**

### Step 6: Configure Layers & Tags

**File → Project Settings → Tags and Layers**

**Layers:**
- 0: Default
- 1: TransparentFX
- 2: Ignore Raycast
- 3: (empty)
- 4: Water
- 5: UI
- 6: Ground
- 7: Player
- 8: NPC
- 9: Environment
- 10: Interactable
- 11: Stage
- 12: Building
- 13: VFX
- 14: PostProcessing
- 15: (empty)

**Tags:**
- Player
- MainCamera
- GameController
- Stage
- NPC
- Interactable
- Volunteer
- Guest
- Artist
- KarmaSource

**Sorting Layers:**
- Default (0)
- Background (100)
- Midground (200)
- Foreground (300)
- UI (400)
- Overlay (500)

### Step 7: Install Essential Packages

**Window → Package Manager**

**Install these packages:**

1. **Cinemachine** (com.unity.cinemachine)
   - Version: 2.9.x
   - For advanced camera control

2. **ProBuilder** (com.unity.probuilder)
   - Version: 5.2.x
   - For quick prototyping geometry

3. **Terrain Tools** (com.unity.terrain-tools)
   - Version: 5.0.x
   - For creating beautiful natural terrain

4. **Input System** (com.unity.inputsystem)
   - Version: 1.7.x
   - Modern input handling

5. **TextMeshPro** (com.unity.textmeshpro)
   - Version: 3.0.x
   - For beautiful UI text

6. **Visual Effect Graph** (com.unity.visualeffectgraph)
   - Version: 14.0.x
   - For music-reactive particles

7. **Shader Graph** (com.unity.shadergraph)
   - Version: 14.0.x
   - For custom visual effects

**Optional but recommended:**
- Post Processing (com.unity.postprocessing) - Additional effects
- Recorder (com.unity.recorder) - For capturing gameplay

### Step 8: Install Third-Party Packages

**Mirror Networking (for multiplayer):**

1. Open Package Manager
2. Click "+" → "Add package from git URL"
3. Enter: `https://github.com/MirrorNetworking/Mirror.git?path=/Assets/Mirror`
4. Click "Add"

**Alternative:** Download from Asset Store (free)

### Step 9: Configure Input System

**Enable Input System:**
1. Edit → Project Settings → Player
2. Scroll to "Other Settings"
3. Active Input Handling: Input System Package (New)
4. Unity will ask to restart - click "Yes"

**Create Input Actions:**

Assets → Create → Input Actions

Name: `PlayerControls`

**Action Maps:**

**Player:**
- Movement (Value, Vector2)
  - WASD Keyboard
  - Left Stick Gamepad
- Look (Value, Vector2)
  - Mouse Delta
  - Right Stick Gamepad
- Interact (Button)
  - E Keyboard
  - South Button Gamepad
- Jump (Button)
  - Space Keyboard
  - South Button Gamepad
- Sprint (Button)
  - Left Shift Keyboard
  - Left Stick Click Gamepad
- OpenMenu (Button)
  - Tab Keyboard
  - Start Button Gamepad

**UI:**
- Navigate (Value, Vector2)
  - WASD Keyboard
  - D-Pad Gamepad
- Submit (Button)
  - Enter/Return Keyboard
  - South Button Gamepad
- Cancel (Button)
  - Escape Keyboard
  - East Button Gamepad

Save and generate C# class.

### Step 10: Create Core Manager Scripts

Create: `Assets/_Project/Scripts/Core/Managers/GameManager.cs`

```csharp
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
                    _instance = FindObjectOfType<GameManager>();
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
```

Create: `Assets/_Project/Scripts/Core/Utilities/Singleton.cs`

```csharp
using UnityEngine;

namespace TheCommunityFestival.Core
{
    /// <summary>
    /// Generic singleton pattern for MonoBehaviour classes
    /// Usage: public class MyManager : Singleton<MyManager> { }
    /// </summary>
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        private static object _lock = new object();
        private static bool _applicationIsQuitting = false;

        public static T Instance
        {
            get
            {
                if (_applicationIsQuitting)
                {
                    Debug.LogWarning($"[Singleton] Instance of {typeof(T)} already destroyed. Returning null.");
                    return null;
                }

                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = FindObjectOfType<T>();

                        if (_instance == null)
                        {
                            GameObject singleton = new GameObject();
                            _instance = singleton.AddComponent<T>();
                            singleton.name = $"{typeof(T).Name} (Singleton)";

                            DontDestroyOnLoad(singleton);

                            Debug.Log($"[Singleton] Created instance of {typeof(T)}");
                        }
                    }

                    return _instance;
                }
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Debug.LogWarning($"[Singleton] Duplicate instance of {typeof(T)} detected. Destroying.");
                Destroy(gameObject);
            }
        }

        protected virtual void OnDestroy()
        {
            if (_instance == this)
            {
                _applicationIsQuitting = true;
            }
        }
    }
}
```

### Step 11: Create Initial Scenes

**Main Menu Scene:**
1. File → New Scene
2. Save as: `Assets/_Project/Scenes/MainMenu.unity`
3. Create empty GameObject: "GameManager"
4. Add GameManager component to it
5. Save scene

**Festival Grounds Scene:**
1. File → New Scene
2. Delete default Main Camera (we'll use Cinemachine)
3. Save as: `Assets/_Project/Scenes/FestivalGrounds.unity`
4. Add to Build Settings (File → Build Settings → Add Open Scenes)

**Loading Scene:**
1. File → New Scene
2. Save as: `Assets/_Project/Scenes/LoadingScreen.unity`
3. Keep minimal (just a loading indicator)

**Configure Build Settings:**
- File → Build Settings
- Scenes in Build:
  1. MainMenu
  2. LoadingScreen
  3. FestivalGrounds
- Platform: macOS
- Architecture: Apple Silicon

### Step 12: Version Control Setup

**Create `.gitignore` in project root:**

```gitignore
# Unity generated
[Ll]ibrary/
[Tt]emp/
[Oo]bj/
[Bb]uild/
[Bb]uilds/
[Ll]ogs/
[Uu]ser[Ss]ettings/

# Asset meta data should not be ignored
!/[Aa]ssets/**/*.meta

# Unity cache
*.pidb.meta
*.pdb.meta
*.mdb.meta

# Unity specific
*.unityproj
*.sln
*.csproj
*.pidb
*.booproj
*.svd
*.mdb
*.opendb
*.VC.db

# OS generated
.DS_Store
.DS_Store?
._*
.Spotlight-V100
.Trashes
ehthumbs.db
Thumbs.db

# Visual Studio / Rider
.vs/
.idea/
*.userprefs
*.suo
*.user
*.userosscache
*.sln.docstates

# Autogenerated VS/MD solution and project files
ExportedObj/
*.csproj
*.unityproj
*.sln
*.suo
*.tmp
*.user
*.userprefs
*.pidb
*.booproj
*.svd
*.pdb
*.mdb
*.opendb
*.VC.db

# Crashlytics
crashlytics-build.properties

# Mac
.DS_Store
```

**Initialize Git:**

```bash
cd /Users/vie/Projects/the-festival-community
git init
git add .
git commit -m "Initial project setup - The Community Festival v0.1.0"
```

**Create README.md:**

```markdown
# The Community Festival

A visually stunning, karma-based multiplayer festival simulation game.

## Tech Stack
- Unity 2023.3.15f1 LTS
- Universal Render Pipeline (URP)
- Mirror Networking
- C# (.NET Standard 2.1)

## Platform
- macOS (Apple Silicon optimized)
- Minimum: M1 Pro, 16GB RAM

## Setup
1. Install Unity 2023.3.15f1 LTS via Unity Hub
2. Open project in Unity
3. Let Unity import all assets (first time takes ~5 minutes)
4. Open MainMenu scene
5. Press Play

## Development
See `/Documentation/` for detailed implementation guides.

## Version
0.1.0 - Initial Setup
```

---

## ✅ Verification Checklist

After completing all steps, verify:

- [ ] Unity 2023.3.15f1 LTS is installed
- [ ] Project opens without errors
- [ ] URP is active (check Graphics settings)
- [ ] All folders are created correctly
- [ ] Layers and tags are configured
- [ ] Essential packages are installed
- [ ] Input System is active and configured
- [ ] GameManager script compiles without errors
- [ ] All three scenes are created and in build settings
- [ ] Performance is smooth (60fps in empty scene)
- [ ] Git repository is initialized
- [ ] .gitignore is working (Library folder not tracked)

**Test Performance:**
1. Open FestivalGrounds scene
2. Window → Analysis → Profiler
3. Press Play
4. Check:
   - FPS should be stable 60
   - CPU usage < 30%
   - Memory < 2GB
   - No errors in console

---

## 🎯 Next Steps

After completing this setup:

1. ✅ Verify all checklist items
2. 📖 Read `02_ENVIRONMENT_SYSTEM.md`
3. 🌿 Begin building the beautiful festival environment
4. 🎨 Start bringing the vision to life!

---

## 📝 Notes

- This setup is optimized specifically for Apple Silicon M1 Pro
- If you have M1 Max/Ultra or M2/M3, you can increase quality settings
- Keep Unity updated to latest 2023.3 LTS patch
- Back up project regularly (Time Machine or cloud)
- Test on target hardware frequently

---

## 🐛 Troubleshooting

**Unity crashes on startup:**
- Check macOS version (needs 14.0+)
- Verify Xcode is installed
- Try creating project with 3D template first, then upgrade to URP

**Low FPS in editor:**
- Window → Rendering → Lighting → Uncheck Auto Generate
- Reduce Game View resolution while developing
- Close other apps to free up RAM

**Packages won't install:**
- Check internet connection
- Window → Package Manager → Advanced → Reset Packages to Defaults
- Restart Unity

**Git not tracking files:**
- Verify .gitignore is in project root
- Run: `git check-ignore -v [file]` to debug

---

*Estimated Time: 2-3 hours*  
*Difficulty: Beginner*  
*Dependencies: None*  
*Next Feature: 02_ENVIRONMENT_SYSTEM*

