# 🏂 Snow Boarder

> **A 2D snowboarding arcade game built with Unity**

## 👥 Team Members
| Name | ID |
|------|----|
| Nguyễn Hữu Hoàng | SE170060 |
| Phạm Văn Học | SE184940 |
| Ngô Gia Bảo | SE181581 |

---

## 📜 Overview
**Snow Boarder** is an exciting **2D sports arcade** game where players control a snowboarder navigating downhill slopes, avoiding obstacles, and performing tricks.  
With **realistic physics** and **dynamic challenges**, each playthrough offers new excitement and mastery opportunities.

🎯 *Goal:* Collect coins, perform flips, avoid obstacles, and achieve the highest score possible.

---

## 🎮 Features
### 🏔️ Multiple Scenes
- **Main Menu:** Start game, view instructions, or exit.  
- **Level Scenes (1-1, 1-2):** Snowy slopes with obstacles, coins, and ramps.  
- **Game Over Scene:** Displays your score and highest record with restart options.

### ⚙️ Dynamic Gameplay
- Realistic Unity 2D Physics (friction, gravity, slope acceleration).  
- Fence obstacles that reduce speed or cause crashes.  
- Trick system with score bonuses.  
- Score multiplier for maintaining combos and speed.  

---

## 🧩 Game Elements
### Player
- Controllable snowboarder with rotation & jump mechanics.
- Speed varies based on slope and inputs.
- Performs full flips for bonus points.

### Obstacles
- **Fence:** Reduces speed or ends the game (–10 pts if hit).
- **Death Zones:** Instant game over.

### Collectibles
- **Coins:** +10 points each.  
- **Flips:** +50 points each successful full rotation.

---

## 🗺️ Game Scenes
| Scene | Description |
|--------|-------------|
| **Main Menu** | Play, Instructions, Exit |
| **Gameplay** | Real-time snowboarding level |
| **Game Over** | Shows final score & highest score |

---

## ⚙️ Gameplay Mechanics
| Mechanic | Description |
|-----------|-------------|
| **Movement** | Left/Right (A/D or Arrow keys) |
| **Speed Control** | Up arrow to boost, Down arrow to brake |
| **Jump** | Spacebar |
| **Physics** | Gravity, slope, and terrain affect velocity |
| **Scoring** | Collect coins (+10), perform flips (+50) |
| **Difficulty** | Harder slopes, more obstacles over time |

---

## 💾 Installation & Run
1. Clone or download this repository.  
2. Open project in **Unity Editor** (recommended version 2022+).  
3. Open `MainMenu.unity` scene.  
4. Click ▶️ *Play* to start the game.

---

## 🧠 Controls
| Action | Key |
|--------|-----|
| Move Left | ⬅️ / A |
| Move Right | ➡️ / D |
| Jump | Space |
| Speed Up | ⬆️ |
| Brake | ⬇️ |
| Perform Flip | Rotate mid-air with A/D or Arrows |

---

## 🧩 Hack & Developer Tools (Cheats)
> ⚠️ *For testing & debugging only. Disable in release builds.*

| Combo | Effect |
|--------|--------|
| **H + G** | Add +500 points instantly |
| **K + L** | Toggle Invincibility (ignore death collisions) |
| **N + M** | Toggle Magnet Mode (auto-attract nearby coins) |

### 🧲 Magnet Details
- Magnet radius: 12 units  
- Pulls all coins toward the player gradually  
- Works best when coins have `Rigidbody2D (gravityScale = 0)`

### ♻️ Reset on Restart
When restarting the level:
- Invincibility resets to `false`  
- Fence hit counter resets  
- Score resets to `0`

---

## 🏆 Scoring System 

| Action | Points|
|--------------------|--------------------|
| **Collect a Coin | +10 points |
| **Hit a Fence (first time) | Reduce speed by 50% |
| **Hit a Fence (second time) | Game Over (kết thúc trò chơi) |

---

## 🧰 Technical Notes
- Built using **Unity 2D physics** and **SurfaceEffector2D** for slope control.  
- Uses `TextMeshPro` for HUD text.  
- Scripts include:  
  - `PlayerController.cs` (movement, jump, magnet, cheats)  
  - `PlayerCollision.cs` (collision & invincible logic)  
  - `GameManager.cs` (score, UI, and persistence)  
  - `GameOverController.cs` (restart and reset logic)

---

## 🪄 Recommended Future Improvements
- Save highest score permanently via `PlayerPrefs`.  
- Add sound toggle & difficulty selection in Main Menu.  
- Implement new power-ups (speed boost, shield).  
- Expand levels with progressive terrain and weather effects.

---

## 🧑‍💻 Authors & License
Developed by **FPT University Students — Team Snow Boarder**  
© 2025 — Licensed under the **MIT License**

---

## 🇻🇳 Ghi chú nhanh cho dev Việt
- Hack nằm trong `PlayerController.cs → HandleCheatCombos()`  
- Reset hack trong `GameOverController.cs` bằng:  
  ```csharp
  PlayerCollision.ResetInvincible();
  PlayerCollision.ResetFenceHitCount();


  ---------------------------


**UI Format:**  
