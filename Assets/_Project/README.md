# The Community Festival - Unity Project

## 🎯 Overview

This is the Unity project for **The Community Festival** - a visually stunning, karma-based multiplayer festival simulation game.

## 🛠️ Technical Setup

- **Unity Version:** 6000.2.7f2 (Unity 6)
- **Render Pipeline:** Universal Render Pipeline (URP)
- **Platform:** macOS (Apple Silicon M1 Pro optimized)
- **Target:** 60 FPS on 16GB RAM
- **Language:** C# (.NET Standard 2.1)

## 📁 Project Structure

```
Assets/_Project/
├── Art/           - Materials, Models, Textures, Shaders, Animations
├── Audio/         - Music, SFX, Ambience
├── Prefabs/       - Characters, Environment, Stages, UI, VFX
├── Scenes/        - Game scenes and test scenes
├── Scripts/       - All C# scripts organized by feature
│   ├── Core/      - Managers, Systems, Utilities
│   ├── Gameplay/  - Characters, Interactions, World
│   ├── AI/        - Behaviors, Navigation, Decision
│   ├── Audio/     - Music and sound systems
│   ├── UI/        - Menus, HUD, Dialogs
│   └── Data/      - Data structures
├── Data/          - ScriptableObjects and Configuration
└── Settings/      - URP and Input settings
```

## 🚀 Core Systems

1. **Karma System** - Player actions influence the world
2. **AI Characters** - Intelligent NPCs with personalities
3. **Music System** - Audio-reactive visuals
4. **Environment** - Dynamic day/night, weather
5. **Multiplayer** - Mirror networking
6. **Building** - Collaborative construction
7. **Social** - Meaningful interactions

## 📚 Documentation

Full implementation documentation is available in `/Documentation/Features/`

## 🎮 Getting Started

1. Open project in Unity 6
2. Open `MainMenu` scene in `Assets/_Project/Scenes/`
3. Press Play to test

## 📝 Notes

- All core managers use singleton pattern
- Namespace: `TheCommunityFestival.*`
- Target performance: 60 FPS on M1 Pro

---

*See root `/Documentation/` for complete implementation guides*

