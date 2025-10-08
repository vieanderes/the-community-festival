# Unity Setup Guide - The Community Festival

## 🚀 Quick Start

Follow these steps to get the project running in Unity:

### 1. Open Project in Unity

1. Open **Unity Hub**
2. Click **Add** → **Add project from disk**
3. Navigate to: `/Users/vie/Projects/the-festival-community`
4. Select the folder and click **Open**
5. Unity will import and compile (first time takes 2-5 minutes)

**Required Unity Version:** Unity 6000.2.7f2 (Unity 6)  
**Note:** Unity 6 is newer than Unity 2023.3 LTS - this is good! Better performance and features.

### 2. Run Project Setup Utilities

Once Unity has finished importing:

1. Go to menu: **Community → Setup → Configure Tags and Layers**
   - This sets up Player, NPC, Stage, Interactable layers
   - Adds necessary tags for the game

2. Go to menu: **Community → Setup → Configure Performance Settings**
   - Optimizes for M1 Pro
   - Sets target FPS to 60
   - Configures color space and quality

### 3. Verify Setup

Check that everything is working:

- [ ] Project compiles without errors
- [ ] Console shows: `[Setup] Tags and Layers configured successfully!`
- [ ] Console shows: `[Setup] Performance settings configured!`
- [ ] Folder structure exists in `Assets/_Project/`

### 4. Create Initial Scene

1. **File → New Scene** (or use existing `SampleScene`)
2. Create an empty GameObject: **GameObject → Create Empty**
3. Name it: `GameManager`
4. Add component: **GameManager** (from TheCommunityFestival.Core)
5. **File → Save As** → `Assets/_Project/Scenes/MainMenu.unity`

### 5. Test the Setup

1. Press **Play** in Unity
2. Check Console for: `[GameManager] Initializing...`
3. Verify no errors appear
4. FPS should show ~60 in Stats window

## 📁 What's Included

### Core Scripts
- ✅ `GameManager.cs` - Main game coordinator
- ✅ `Singleton.cs` - Singleton pattern utility

### Editor Tools
- ✅ `ProjectSetupUtility.cs` - Automated setup tools

### Folder Structure
```
Assets/_Project/
├── Art/           - Ready for materials, models, textures
├── Audio/         - Ready for music, SFX, ambience  
├── Data/          - Ready for ScriptableObjects
├── Prefabs/       - Ready for prefabs
├── Scenes/        - Save your scenes here
├── Scripts/       - All organized and ready
└── Settings/      - URP and Input settings
```

## 🎯 Next Steps

After setup is complete:

1. **Read**: `/Documentation/Features/02_ENVIRONMENT_SYSTEM.md`
2. **Implement**: Create the beautiful festival terrain
3. **Build**: Start bringing the world to life!

## 🐛 Troubleshooting

### Unity won't open the project
- Ensure you have Unity 6 installed
- Try Unity Hub → **Add** → select project folder

### "Namespace not found" errors
- Wait for Unity to finish compiling
- **Assets → Reimport All**
- Restart Unity

### Performance issues in editor
- Lower Game View resolution while developing
- Close unnecessary applications
- Check Activity Monitor for memory usage

### Menu items don't appear
- Ensure `ProjectSetupUtility.cs` is in an `Editor` folder
- **Assets → Reimport All**
- Restart Unity

## 📞 Support

- Check `/Documentation/INDEX.md` for full documentation
- Review specific feature guides in `/Documentation/Features/`

---

**You're all set! Time to build something beautiful! ✨**

