# Feature 06: Multiplayer System

## 🎯 Overview

Seamless multiplayer using Mirror Networking where real players and AI bots coexist in the same festival world.

---

## 📋 Core Components

1. **Network Manager Setup**
2. **Player Synchronization**
3. **AI Bot Integration (Server-Controlled)**
4. **Chat & Communication**
5. **Shared World State**
6. **Karma Synchronization**

---

## 🌐 Architecture

**Server Authoritative:**
- Player positions
- Karma values
- World state (time, weather)
- AI character behaviors

**Client Predicted:**
- Movement
- Interactions
- UI updates

---

## 🚀 Key Files

### `NetworkGameManager.cs` - Multiplayer coordination
### `NetworkPlayer.cs` - Synchronized player component
### `NetworkAICharacter.cs` - Server-controlled AI
### `ChatSystem.cs` - Communication between players
### `KarmaSync.cs` - Synchronize karma across network

---

## 📡 Mirror Networking Setup

1. Install Mirror from Package Manager
2. Create NetworkManager prefab
3. Setup NetworkIdentity on player prefab
4. Implement [Command] and [ClientRpc] for actions
5. Synchronize karma using SyncVars
6. Test with multiple clients

---

*Estimated Time: 14-18 hours*  
*Difficulty: Advanced*  
*Requires: Mirror Networking package*

