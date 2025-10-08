# 🗺️ NavMesh Setup Guide - Quick Fix for NPC Navigation

## The Issue:

You're seeing: **"Failed to create agent because there is no valid NavMesh"**

This means the NPCs can't walk around because there's no navigation mesh baked.

## ✅ Quick Fix (2 minutes):

### Step 1: Open Navigation Window

In Unity, click: **`Window → AI → Navigation (Obsolete)`**

(Don't worry about "Obsolete" - it still works in Unity 6!)

### Step 2: Select Your Terrain

1. In the **Hierarchy** (left panel)
2. Click on: **`FestivalGrounds_Terrain`**

### Step 3: Mark as Walkable

1. In the Navigation window, click the **"Object"** tab
2. Check the box: **"Navigation Static"** or **"Walkable"**
3. Click **"Apply"**

### Step 4: Bake the NavMesh

1. In the Navigation window, click the **"Bake"** tab
2. Settings (leave defaults, but you can adjust):
   - Agent Radius: 0.5
   - Agent Height: 2
   - Max Slope: 45
3. Click the big **"Bake"** button at the bottom
4. Wait 10-20 seconds (blue overlay will appear on terrain when done)

### Step 5: Done!

Press Play again - NPCs will now walk around! 🎉

---

## What is NavMesh?

Think of it like a "walkable map" for AI characters. Unity calculates where NPCs can walk, and they use this to navigate intelligently around obstacles.

---

## Visual Guide:

```
Navigation Window
┌─────────────────────────┐
│ Object | Bake           │ ← Click Bake tab
├─────────────────────────┤
│ Agent Size              │
│   Radius: 0.5          │
│   Height: 2.0          │
│                         │
│ Max Slope: 45          │
│                         │
│ [    BAKE    ]         │ ← Click this!
└─────────────────────────┘
```

When done, your terrain will have a blue mesh overlay showing walkable areas!

---

## Troubleshooting:

**Can't find Navigation window?**
- Try: `Window → AI → Navigation`
- Or: Search "Navigation" in Window menu

**Bake button is grayed out?**
- Make sure terrain is selected
- Make sure it's marked as "Navigation Static"

**Baking takes forever?**
- Your terrain might be huge
- Reduce terrain size or increase cell size in Bake settings

---

**After baking, press Play - NPCs will start walking around!** 🚶‍♂️🚶‍♀️

