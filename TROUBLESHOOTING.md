# 🔧 Troubleshooting Guide - The Community Festival

## Issue: Menu Items Not Working

If "Community → Setup → Configure Tags and Layers" doesn't do anything:

### Step 1: Check for Compilation Errors

1. **Look at the Console** (bottom of Unity)
   - Click: `Window → General → Console` if you don't see it
   - Look for any RED error messages
   - If you see errors, read them carefully - they tell you what's wrong

### Step 2: Wait for Compilation

Unity needs to compile all scripts before menu items work:

1. **Look at the bottom-right corner of Unity**
   - You should see a small spinning icon if Unity is compiling
   - Wait for it to finish (usually 10-30 seconds)
   
2. **Check the bottom status bar**
   - It should say "Ready" when done
   - If it says "Compiling..." wait for it to finish

### Step 3: Force Reimport

If scripts still don't work:

1. Click: `Assets → Reimport All`
2. Wait for Unity to reimport everything (2-5 minutes)
3. Try the menu item again

### Step 4: Restart Unity

Sometimes Unity needs a fresh start:

1. Close Unity completely
2. Reopen the project
3. Wait for compilation to complete
4. Try again

---

## How to Know if Menu Items are Working

### Finding the Community Menu:

At the top of Unity, you should see menus in this order:
```
File  Edit  Assets  GameObject  Component  Community  Window  Help
                                           ^^^^^^^^
                                           Look for this!
```

If you **DON'T see "Community"** menu:
- Scripts haven't compiled yet
- Check Console for errors
- Wait for compilation
- Try reimporting assets

### When You Click Community Menu:

You should see:
```
Community
├── Setup
│   ├── Configure Tags and Layers
│   ├── Create Essential Folders
│   ├── Configure Performance Settings
│   ├── Create Festival Environment
│   ├── Create Karma System
│   └── Create Water Plane
├── Quick Start
│   └── Create Complete Playable Scene    ← Use this one!
└── Test
    ├── Add Positive Karma
    └── Add Negative Karma
```

---

## Quick Fix: Use the One-Click Setup Instead

**EASIER OPTION:** Instead of running each setup separately, just use:

```
Community → Quick Start → Create Complete Playable Scene
```

This does EVERYTHING automatically in one click:
- ✅ Configures tags and layers
- ✅ Creates environment
- ✅ Sets up karma system
- ✅ Creates camera
- ✅ Saves scene
- ✅ Ready to play!

---

## Common Issues

### "I don't see any Console messages when I click the menu"

**Solution:**
1. Open Console: `Window → General → Console`
2. Clear it: Click the "Clear" button (looks like a prohibited sign)
3. Try the menu item again
4. Watch for new messages

### "Console shows red errors"

**Solution:**
1. Read the error message - it tells you what's wrong
2. Common errors:
   - **"Missing reference"** - Not a problem yet, we'll add these later
   - **"Namespace not found"** - Scripts need to compile, wait a moment
   - **"Syntax error"** - There might be a typo in a script file

### "Community menu doesn't exist"

**Solution:**
This means Unity hasn't compiled the Editor scripts yet.

1. Check if files exist:
   - Go to Project panel (bottom)
   - Navigate to: `Assets → _Project → Scripts → Editor`
   - You should see:
     - `ProjectSetupUtility.cs`
     - `EnvironmentSetupUtility.cs`
     - `KarmaSetupUtility.cs`
     - `QuickPlayableSceneSetup.cs`

2. If files are there but menu is missing:
   - Click on any script file
   - Look at Inspector (right panel)
   - If it shows script icon but no errors → Just wait
   - If it shows errors → Read the error message

3. Force recompile:
   - Right-click in Project panel
   - Select `Reimport All`
   - Wait 2-5 minutes

### "Menu items are grayed out"

**Solution:**
- Unity is still loading or compiling
- Wait for the spinning icon (bottom-right) to stop
- Status bar should say "Ready"

---

## Debug Steps

### Step-by-Step Debugging:

**1. Open Console**
```
Window → General → Console
```

**2. Clear Console**
Click the clear button (top-left of Console)

**3. Try Menu Item**
```
Community → Setup → Configure Tags and Layers
```

**4. Watch Console**
You should see:
```
[Setup] Configuring Tags and Layers...
Added tag: Stage
Added tag: NPC
... (more tags)
Added layer: Ground at index 6
... (more layers)
[Setup] Tags and Layers configured successfully!
```

**5. If Nothing Appears in Console:**

a) Check if menu item exists at all
b) Check for red errors in Console
c) Check status bar says "Ready" not "Compiling..."
d) Try restarting Unity

---

## Alternative: Manual Setup

If menu items don't work, you can set up manually:

### Tags (Edit → Project Settings → Tags and Layers):

Click the "+" under Tags and add:
- Stage
- NPC
- Interactable
- Volunteer
- Guest
- Artist
- KarmaSource

### Layers (same window):

Set these layer indices:
- Layer 6: Ground
- Layer 7: Player
- Layer 8: NPC
- Layer 9: Environment
- Layer 10: Interactable
- Layer 11: Stage
- Layer 12: Building
- Layer 13: VFX
- Layer 14: PostProcessing

But honestly, **use the Quick Start tool instead** - it's much easier!

---

## Still Not Working?

### Check Unity Version

1. Click `Help → About Unity`
2. You should see: Unity 6 (6000.x.x)
3. If different version: Might cause issues

### Check Script Locations

All editor scripts MUST be in a folder named "Editor":
```
Assets/_Project/Scripts/Editor/
```

If they're anywhere else, Unity won't recognize them as editor scripts!

### Last Resort: Full Reimport

1. Close Unity
2. Delete these folders (they'll regenerate):
   - `Library/`
   - `Temp/`
3. Reopen Unity
4. Wait for full reimport (5-10 minutes first time)
5. Try menu items again

---

## What Success Looks Like

When it works, you'll see:

**In Console:**
```
[Setup] Configuring Tags and Layers...
Added tag: Stage
Added tag: NPC
Added tag: Interactable
Added tag: Volunteer
Added tag: Guest
Added tag: Artist
Added tag: KarmaSource
Added layer: Ground at index 6
Added layer: Player at index 7
Added layer: NPC at index 8
Added layer: Environment at index 9
Added layer: Interactable at index 10
Added layer: Stage at index 11
Added layer: Building at index 12
Added layer: VFX at index 13
Added layer: PostProcessing at index 14
[Setup] Tags and Layers configured successfully!
```

**In Project Settings:**
- All tags are added
- All layers are configured
- Ready to continue!

---

## Quick Summary

**If menu item doesn't work:**

1. ⏰ Wait for compilation (check bottom-right spinner)
2. 👀 Check Console for errors
3. 🔄 Try `Assets → Reimport All`
4. 🔁 Restart Unity
5. 🚀 Use `Community → Quick Start → Create Complete Playable Scene` instead

**Most common cause:** Unity is still compiling scripts. Just wait 30 seconds!

---

*Need more help? Check what the Console says - it always has clues!*

