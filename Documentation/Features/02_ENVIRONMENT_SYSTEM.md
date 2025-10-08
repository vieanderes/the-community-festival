# Feature 02: Environment System

## 🎯 Overview

Create the beautiful natural festival grounds with realistic terrain, vegetation, water, day/night cycles, weather, and atmospheric effects. This is the visual foundation that makes Lumina Festival breathtaking.

---

## 📋 Dependencies

- ✅ Feature 01: Project Setup completed
- ✅ URP configured and working
- ✅ Terrain Tools package installed
- ✅ Visual Effect Graph installed

---

## 🌳 System Components

### 1. Terrain System
### 2. Vegetation System (Trees, Flowers, Grass)
### 3. Water System (Lake, Streams)
### 4. Sky & Atmosphere System
### 5. Time of Day System
### 6. Weather System
### 7. Lighting System
### 8. Ambient Audio System

---

## 🚀 Implementation Steps

### PART 1: Terrain Creation

#### Step 1.1: Create Base Terrain

1. **Create New Terrain:**
   - GameObject → 3D Object → Terrain
   - Name: "FestivalGrounds_Terrain"
   - Position: (0, 0, 0)

2. **Configure Terrain Settings:**
   - Terrain Width: 2000m
   - Terrain Length: 2000m
   - Terrain Height: 600m
   - Heightmap Resolution: 1025 (balance between quality and performance)
   - Detail Resolution: 1024
   - Control Texture Resolution: 1024

3. **Create Terrain Layers:**

**Assets → Create → Terrain Layer**

Create these layers:

**Grass Layer:**
- Name: `Grass_Meadow`
- Diffuse: Download or use Unity terrain grass texture
- Normal Map: Corresponding normal map
- Metallic: 0
- Smoothness: 0.3
- Tile Size: X=15, Y=15

**Dirt/Path Layer:**
- Name: `Dirt_Path`
- Diffuse: Dirt/earth texture
- Normal Map: Corresponding normal
- Metallic: 0
- Smoothness: 0.2
- Tile Size: X=10, Y=10

**Forest Floor Layer:**
- Name: `Forest_Floor`
- Diffuse: Dark earth with leaves texture
- Normal Map: Corresponding normal
- Metallic: 0
- Smoothness: 0.15
- Tile Size: X=12, Y=12

**Sand/Beach Layer:**
- Name: `Sand_Beach`
- Diffuse: Sand texture
- Normal Map: Corresponding normal
- Metallic: 0
- Smoothness: 0.25
- Tile Size: X=8, Y=8

4. **Paint Base Terrain:**

Use Terrain → Paint Texture to create:
- Large meadows in center and south
- Forest floor in northern woods
- Beach/sand around the lake area
- Dirt paths connecting major zones

#### Step 1.2: Sculpt Terrain Height

Use Terrain → Sculpt to create:

**Valley Basin:**
- Central flat area for Main Floor stage
- Gentle slopes surrounding it
- Total elevation change: ~50m from center to edges

**Hills:**
- Rolling hills on eastern side (3-4 hills, 20-40m high)
- One tall viewpoint hill on southwest (80m high)
- Natural amphitheater shape around main area

**Lake Basin:**
- Depressed area in northwest quadrant
- Gradual slope into water
- Beach area around edges
- Deepest point: -15m

**Forest Integration:**
- Slightly elevated forest area in north
- Undulating terrain for visual interest
- Natural pathways between zones

**Recommended Tools:**
- Raise/Lower Terrain: For major shapes
- Smooth Height: To soften harsh edges
- Set Height: For flat performance areas
- Erosion: For natural weathering look

#### Step 1.3: Create Terrain Details Script

Create: `Assets/_Project/Scripts/Gameplay/World/TerrainManager.cs`

```csharp
using UnityEngine;

namespace TheCommunityFestival.Gameplay.World
{
    /// <summary>
    /// Manages terrain details, LOD, and performance optimization
    /// </summary>
    [RequireComponent(typeof(Terrain))]
    public class TerrainManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Terrain _terrain;

        [Header("Performance Settings")]
        [SerializeField] private float _detailDistance = 80f;
        [SerializeField] private float _detailDensity = 0.8f;
        [SerializeField] private float _treeDistance = 1000f;
        [SerializeField] private float _billboardStart = 50f;

        [Header("Quality Settings")]
        [SerializeField] private int _maxMeshTrees = 500;
        [SerializeField] private bool _castShadows = true;

        private void Awake()
        {
            if (_terrain == null)
                _terrain = GetComponent<Terrain>();
        }

        private void Start()
        {
            ConfigureTerrain();
        }

        private void ConfigureTerrain()
        {
            // Get terrain data
            TerrainData terrainData = _terrain.terrainData;

            // Configure detail settings (grass, flowers)
            _terrain.detailObjectDistance = _detailDistance;
            _terrain.detailObjectDensity = _detailDensity;

            // Configure tree settings
            _terrain.treeDistance = _treeDistance;
            _terrain.treeBillboardDistance = _billboardStart;
            _terrain.treeMaximumFullLODCount = _maxMeshTrees;

            // Configure rendering
            _terrain.shadowCastingMode = _castShadows ? 
                UnityEngine.Rendering.ShadowCastingMode.On : 
                UnityEngine.Rendering.ShadowCastingMode.Off;

            // Pixel error for LOD (lower = better quality, higher = better performance)
            _terrain.heightmapPixelError = 5f; // Good balance for M1 Pro

            // Enable height-based LOD
            _terrain.allowAutoConnect = true;

            Debug.Log($"[TerrainManager] Configured terrain: {terrainData.size}");
        }

        #region Public Methods
        /// <summary>
        /// Adjust detail distance based on performance
        /// </summary>
        public void SetDetailDistance(float distance)
        {
            _detailDistance = Mathf.Clamp(distance, 20f, 150f);
            _terrain.detailObjectDistance = _detailDistance;
        }

        /// <summary>
        /// Adjust tree rendering distance
        /// </summary>
        public void SetTreeDistance(float distance)
        {
            _treeDistance = Mathf.Clamp(distance, 100f, 2000f);
            _terrain.treeDistance = _treeDistance;
        }
        #endregion
    }
}
```

Add this component to your Terrain GameObject.

---

### PART 2: Vegetation System

#### Step 2.1: Create Tree Prefabs

**For Prototyping:** Use Unity's built-in trees or create simple tree prefabs.

**For Production:** You'll want quality tree models. For now, we'll set up the system.

Create: `Assets/_Project/Prefabs/Environment/Trees/`

**Simple Tree Prefab (Prototype):**
1. Create Cylinder for trunk
2. Create Sphere for canopy (scale: 3, 4, 3)
3. Apply materials
4. Add LOD Group component
5. Save as Prefab: `Tree_Oak_01`

**Configure LOD Group:**
- LOD 0 (0% - 60%): Full detail mesh
- LOD 1 (60% - 80%): Medium detail
- LOD 2 (80% - 100%): Billboard (flat plane with tree texture)

#### Step 2.2: Paint Trees

1. Select Terrain
2. Terrain → Paint Trees
3. Edit Trees → Add Tree
4. Add your tree prefabs
5. Configure each tree:
   - Bend Factor: 0.1 (slight wind movement)
   - Min Width/Height: 8-12m
   - Max Width/Height: 15-20m

**Tree Placement Strategy:**

**Dense Forest (North):**
- High density (80-100 trees per 100m²)
- Mix of tree types
- Taller trees (15-20m)
- Creates canopy

**Scattered Trees (Meadows):**
- Low density (5-10 trees per 100m²)
- Feature trees
- Medium height (10-15m)
- For shade and aesthetics

**Lake Shore:**
- Willow-style trees (create separate prefab)
- Drooping branches toward water
- Clustered groups

**Important:** Leave clearings for:
- Stage locations
- Camping areas
- Pathways
- Open meadows

#### Step 2.3: Create Grass/Flower Details

Create: `Assets/_Project/Scripts/Gameplay/World/VegetationManager.cs`

```csharp
using UnityEngine;

namespace TheCommunityFestival.Gameplay.World
{
    /// <summary>
    /// Manages grass, flowers, and detail vegetation
    /// Handles procedural distribution and wind animation
    /// </summary>
    public class VegetationManager : MonoBehaviour
    {
        [Header("Detail Settings")]
        [SerializeField] private Terrain _terrain;
        [SerializeField] private DetailPrototype[] _grassTypes;
        [SerializeField] private DetailPrototype[] _flowerTypes;

        [Header("Distribution")]
        [SerializeField] private float _grassDensity = 0.7f;
        [SerializeField] private float _flowerDensity = 0.3f;

        [Header("Wind")]
        [SerializeField] private float _windSpeed = 0.5f;
        [SerializeField] private float _windStrength = 0.3f;

        private void Start()
        {
            SetupVegetation();
            ConfigureWind();
        }

        private void SetupVegetation()
        {
            if (_terrain == null)
            {
                _terrain = GetComponent<Terrain>();
            }

            // Grass and flowers will be added via Unity Terrain detail painter
            // This script manages their parameters

            // Configure grass wave settings
            float waveSpeed = _windSpeed;
            float waveSize = _windStrength;
            float waveAmount = 0.5f;

            // Apply to terrain
            _terrain.terrainData.wavingGrassSpeed = waveSpeed;
            _terrain.terrainData.wavingGrassStrength = waveSize;
            _terrain.terrainData.wavingGrassAmount = waveAmount;

            Debug.Log("[VegetationManager] Vegetation configured");
        }

        private void ConfigureWind()
        {
            // Wind settings for grass movement
            float tint = 0.3f; // Slight color tint when grass moves
            _terrain.terrainData.wavingGrassTint = new Color(0.9f, 0.95f, 0.8f, tint);
        }

        #region Public Methods
        public void SetWindStrength(float strength)
        {
            _windStrength = Mathf.Clamp01(strength);
            _terrain.terrainData.wavingGrassStrength = _windStrength;
        }

        public void SetWindSpeed(float speed)
        {
            _windSpeed = Mathf.Clamp(speed, 0f, 2f);
            _terrain.terrainData.wavingGrassSpeed = _windSpeed;
        }
        #endregion
    }
}
```

**Paint Grass Details:**

1. Terrain → Paint Details
2. Edit Details → Add Grass Texture
3. Create grass details:
   - **Meadow Grass:** Dense, green, tall
   - **Short Grass:** Sparse, lighter, short
   - **Wildflowers:** Colorful, scattered
   - **Forest Undergrowth:** Dark, dense

4. Paint strategically:
   - Meadows: Dense grass + wildflowers
   - Forest: Sparse undergrowth
   - Paths: No grass (keep clear)
   - Lake shore: Reeds and water plants

---

### PART 3: Water System

#### Step 3.1: Create Lake

**Using Unity Terrain:**

1. In terrain height map, create depression for lake
2. Make it natural-looking (organic shape, not geometric)
3. Smooth edges for gradual slope

**Create Water Plane:**

1. GameObject → 3D Object → Plane
2. Name: "Lake_Main"
3. Scale: Adjust to fill lake basin
4. Position Y: Water level (e.g., 50)

#### Step 3.2: Create Water Shader

Create: `Assets/_Project/Art/Shaders/Water_Lake.shadergraph`

Use Shader Graph to create realistic water:

**Properties:**
- Base Color: Light blue-green
- Transparency: 0.7
- Normal Map: Water ripples
- Reflection: Skybox reflection
- Depth: Darker in deep areas
- Foam: White foam at edges

**Simple Water Material (Alternative):**

If Shader Graph is complex, create simple material:

1. Create Material: `Water_Simple`
2. Shader: Universal Render Pipeline/Lit
3. Settings:
   - Surface Type: Transparent
   - Rendering Mode: Fade
   - Base Color: (100, 150, 180, 180)
   - Smoothness: 0.9
   - Metallic: 0.1

4. Assign to Lake plane

#### Step 3.3: Animate Water

Create: `Assets/_Project/Scripts/Gameplay/World/WaterSystem.cs`

```csharp
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
```

---

### PART 4: Sky & Atmosphere

#### Step 4.1: Create Sky System

**Procedural Skybox:**

1. Window → Rendering → Lighting
2. Environment Tab
3. Skybox Material: Create new

Create: `Assets/_Project/Art/Materials/Sky_Procedural.mat`
- Shader: Skybox/Procedural
- Sun: (will link to directional light)
- Atmosphere Thickness: 1.0
- Sky Tint: Light blue (150, 180, 220)
- Ground: Earth tone (80, 70, 60)
- Exposure: 1.3

#### Step 4.2: Configure Lighting

**Directional Light (Sun):**

1. Rename existing Directional Light to "Sun"
2. Settings:
   - Color: Warm white (255, 250, 240)
   - Intensity: 1.2
   - Indirect Multiplier: 1.5
   - Shadow Type: Soft Shadows
   - Strength: 0.8
   - Resolution: Very High
   - Bias: 0.05
   - Normal Bias: 0.4

3. Add component for rotation (time of day)

Create: `Assets/_Project/Scripts/Gameplay/World/TimeOfDaySystem.cs`

```csharp
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
```

Attach this script to the Sun GameObject.

---

### PART 5: Weather System

Create: `Assets/_Project/Scripts/Gameplay/World/WeatherSystem.cs`

```csharp
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
```

---

## ✅ Implementation Checklist

- [ ] Terrain created with proper dimensions
- [ ] Terrain layers painted (grass, dirt, forest, sand)
- [ ] Terrain sculpted with valleys, hills, lake basin
- [ ] Trees added and painted in strategic locations
- [ ] Grass and flowers painted as details
- [ ] Lake created with water material
- [ ] Sky and atmosphere configured
- [ ] Directional light (sun) positioned
- [ ] TimeOfDaySystem script attached and configured
- [ ] WeatherSystem script added
- [ ] All scripts compile without errors
- [ ] Scene runs at 60fps with terrain loaded
- [ ] Lighting looks beautiful at different times of day

---

## 🎯 Testing

1. **Performance Test:**
   - Open FestivalGrounds scene
   - Play mode
   - Walk across terrain
   - Should maintain 60fps

2. **Visual Test:**
   - Change time of day (TimeOfDaySystem inspector)
   - Verify sun rotation
   - Check lighting changes
   - Confirm beautiful golden hour

3. **Weather Test:**
   - Change weather via inspector
   - Verify smooth transitions
   - Check rain particles
   - Confirm fog effects

---

## 🎨 Next Steps

After completing environment:
1. ✅ Verify all checklist items
2. 📖 Read `03_KARMA_SYSTEM.md`
3. 💫 Begin implementing the core karma mechanics

---

*Estimated Time: 8-12 hours*  
*Difficulty: Intermediate*  
*Dependencies: Feature 01*  
*Next Feature: 03_KARMA_SYSTEM*

