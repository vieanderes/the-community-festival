# 🎮 Getting Started with Unity - Complete Beginner's Guide

## For Complete Unity Beginners

This guide will help you turn the code files into an actual playable game!

---

## Step 1: Install Unity Hub & Unity Editor

### 1.1 Download Unity Hub
1. Go to https://unity.com/download
2. Click **"Download Unity Hub"**
3. Install Unity Hub on your Mac
4. Open Unity Hub

### 1.2 Install Unity Editor
1. In Unity Hub, click **"Installs"** on the left
2. Click **"Install Editor"**
3. Choose **"Unity 6"** (or the latest version, currently 6000.2.7f2)
4. Click **"Next"**
5. On modules screen, check:
   - ✅ **Mac Build Support (Mono)**
   - ✅ **Mac Build Support (IL2CPP)**
   - ✅ **Documentation**
6. Click **"Install"** (this takes 10-20 minutes)

---

## Step 2: Open The Community Festival Project

### 2.1 Add Project to Unity Hub
1. In Unity Hub, click **"Projects"** on the left
2. Click **"Add"** (or **"Open"** button)
3. Navigate to: `/Users/vie/Projects/the-festival-community`
4. Click **"Open"**

### 2.2 First Time Opening (Important!)
1. Unity will now import all files - **This takes 5-10 minutes the first time**
2. You'll see a progress bar showing "Importing assets..."
3. Don't close Unity during this process!
4. When done, you'll see the Unity Editor interface

---

## Step 3: Run Our Automated Setup

Once Unity opens, let's use the tools we created to set everything up automatically!

### 3.1 Configure Tags and Layers
1. At the top menu, click: **Community → Setup → Configure Tags and Layers**
2. Wait for console message: "Tags and Layers configured successfully!"

### 3.2 Create the Festival Environment
1. Click: **Community → Setup → Create Festival Environment**
2. You'll now have:
   - A 2km x 2km terrain (the festival grounds)
   - A sun with day/night cycle
   - Weather system
3. You should see these in the **Hierarchy** panel (left side)

### 3.3 Create the Karma System
1. Click: **Community → Setup → Create Karma System**
2. You'll now have:
   - KarmaManager (tracks your actions)
   - Visual feedback system
   - Particle effects for karma

---

## Step 4: Create a Playable Scene

### 4.1 Save the Scene
1. Click: **File → Save As...**
2. Navigate to: `Assets/_Project/Scenes/`
3. Name it: `FestivalGrounds`
4. Click **Save**

### 4.2 Add a Player Camera
1. In the **Hierarchy** panel, right-click
2. Select: **Camera**
3. Rename it to: `Main Camera` (if not already named this)
4. In the **Inspector** (right panel), set:
   - Position: X=0, Y=5, Z=-10
   - Rotation: X=10, Y=0, Z=0

---

## Step 5: Test the Game!

### 5.1 Play Mode
1. At the top center of Unity, you'll see ▶️ **Play button**
2. Click it!
3. You should see:
   - The terrain
   - The sky
   - Console messages showing systems initializing

### 5.2 Test the Karma System
While in Play Mode:

1. Find **KarmaManager** in the Hierarchy (left panel)
2. Click on it
3. In the Inspector (right panel), right-click on the **KarmaManager script**
4. Select: **Add Positive Karma (+10 Helping)**
5. Watch the console - you'll see karma increase!
6. Try: **Add Negative Karma (-5 Ignoring)**

You should see:
- Console messages showing karma changes
- Karma level updates
- Your current karma total

### 5.3 Stop Play Mode
- Click the **Play button** again to stop (or press Cmd+P)

---

## Step 6: Test the Environment Systems

### 6.1 Change Time of Day
1. In Hierarchy, find **Sun**
2. Click on it
3. In Inspector, find **Time Of Day System** component
4. While in Play Mode, adjust **Current Time** slider (0-24)
5. Watch the sun rotate and lighting change!

### 6.2 Change Weather
1. Find **EnvironmentManager** in Hierarchy
2. Click on it
3. In Inspector, find **Weather System** component
4. While in Play Mode, try different weather types:
   - Clear
   - Cloudy
   - Rain

---

## Understanding the Unity Interface

### Key Panels:

**Hierarchy (Left)**
- Shows all objects in your scene
- Think of it as a "list of everything"

**Scene View (Center)**
- Visual editor where you place objects
- Use mouse to navigate:
  - Right-click + drag = rotate camera
  - Scroll = zoom
  - Middle-click + drag = pan

**Game View (Center, next to Scene)**
- What the player sees
- Switch to this tab when in Play Mode

**Inspector (Right)**
- Shows properties of selected object
- This is where you adjust settings

**Project (Bottom)**
- All your files and assets
- Navigate to `Assets/_Project/` to see our code

**Console (Bottom)**
- Shows messages and errors
- Click to open: **Window → General → Console**

---

## What You Can Do Right Now

### ✅ Working Features:

1. **Environment System**
   - Terrain is created (though flat - you can sculpt it!)
   - Day/night cycle works
   - Weather system works

2. **Karma System**
   - Karma tracking works
   - You can add karma via context menu
   - Visual feedback system is connected
   - Level changes are logged

3. **Performance**
   - Optimized for your M1 Pro
   - Should run at 60 FPS

### ⚠️ What's Missing (We'll Add Soon):

- **Player Controller** - Can't move around yet
- **NPCs/AI Characters** - No characters yet
- **Music System** - No audio yet
- **Building/Interactions** - Can't interact yet

---

## Quick Testing Workflow

```
1. Open Unity
2. Click Play ▶️
3. Test karma in Inspector
4. Watch console messages
5. Stop Play ⏹️
6. Make changes
7. Repeat!
```

---

## Common Beginner Issues

### "I don't see anything when I press Play"
- Make sure you have a Camera in the scene
- Check camera position is not inside the terrain
- Look at Game View tab (not Scene View)

### "Console shows errors"
- Read the error message carefully
- Often just missing references - we'll fix these as we build

### "Play button is grayed out"
- Unity is still compiling code
- Wait for the spinner in bottom-right to stop

### "Scene is empty/black"
- You might need to create the environment
- Run: Community → Setup → Create Festival Environment

### "I can't find the Community menu"
- Make sure all scripts compiled successfully
- Check Console for any red errors
- Try: Assets → Reimport All

---

## Next Steps for You

### Now:
1. ✅ Open the project in Unity
2. ✅ Run the setup tools
3. ✅ Test the karma system
4. ✅ Play with time of day and weather

### Soon (We'll Add):
1. 🎮 Player character you can move
2. 🤖 AI characters walking around
3. 🎵 Music system with reactive visuals
4. 🏗️ Building and interaction systems

---

## Learning Resources

- **Unity Learn**: https://learn.unity.com/
- **Unity Documentation**: https://docs.unity3d.com/
- Our project files have comments explaining everything!

---

## 🎯 You're Ready!

You now have enough knowledge to:
- Open the project
- Test what we've built
- See the karma system in action
- Play with the environment

The game will get more fun as we add each feature. For now, you can see the foundation working!

**Press Play and explore!** 🚀✨

---

*Need help? Check the console messages - they explain what's happening!*

