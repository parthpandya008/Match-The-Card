# 🎴 Card Matching Game

A memory card matching game built with Unity, demonstrating professional architecture patterns and SOLID principles.

## 🎮 Overview

A classic memory card matching game where players flip cards to find matching pairs. The project showcases clean architecture, SOLID principles, and industry-standard design patterns in Unity development.

**Gameplay:**
1. Select grid size (2×2, 3×3, 4×4, or any custom size)
2. Cards are revealed briefly, then flipped face down
3. Click cards to flip and find matching pairs
4. Complete the game by matching all pairs
5. Best times are saved per grid size

---

## ✨ Features

### Core Gameplay
-  Flexible grid system (supports rectangular grids: 2×3, 4×5, 5×6, etc.)
-  Smooth card flip animations with 180° rotation
-  Random sprite allocation with pair matching
-  Visual feedback for matched/unmatched cards
-  Score persistence (best time per grid size)
-  Audio feedback for interactions

  ### Technical Features
-  State machine for game flow management
-  Model-View-Presenter (MVP) architecture for the UI
-  Dependency injection architecture
-  Event-driven UI updates
-  Object pooling for performance
-  JSON-based save system
-  Async/await for timing control
-  Modular, extensible codebase

### Architecture Principles
- **SOLID Principles**: Single Responsibility, Dependency Inversion, Open/Closed
- **Design Patterns**: State, Strategy, Factory, Observer
- **MVP**:  All screens,  Separate display (View) from logic (Presenter)
- **Separation of Concerns**: GameManager (Unity layer) ↔ GameController (business logic)
- **Facade** | `UIManager` | Single entry point for the rest of the UI codebase 
- **Testability**: Framework-independent core logic

## 🎨 Architecture

### High-Level Overview

- **GameManager** (Unity MonoBehaviour) delegates to GameController (plain C# class), which handles all game logic and coordinates core services:
- **IMatchProcessor** — extensible match checking
- **CardFactory** — card creation
- **CardLayoutManager** — grid layout
- **ScoreManager** — scoring
- **UIManager** acts as a facade that owns and wires all presenters:

Communication between game logic and UI is event-driven: GameEvents flow from **game → UI,** and UIEvents flow from **UI → game.**


### State Flow
Idle **→** Initializing **→** Revealing **→** Playing **→** Completed **→** (Reset) → Idle

### Key Components

| Component | Type | Responsibility |
|-----------|------|----------------|
| **GameManager** | MonoBehaviour | Unity infrastructure, dependency wiring |
| **GameController** | Plain C# | Core game logic, state coordination |
| **IMatchProcessor** | Interface | Match algorithm (extensible for game modes) |
| **CardFactory** | Factory | Card instantiation with object pooling |
| **CardLayoutManager** | Calculator | Grid positioning and sizing |
| **ScoreManager** | Service | Persistent storage (JSON) |
| **GameEvents** | Observer | Decoupled communication |
| **UIEvents** | Observer | UI → Game decoupled notifications |
| **UIManager** | Facade | UI orchestrator, owns all Presenters |
| **XxxPresenter** | Plain C# | Per-screen logic, event handling, formatting |
| **XxxView** | MonoBehaviour | Per-screen display, purely passive |


## 👤 Author

**Parth Pandya**
- GitHub: https://github.com/parthpandya008/
- LinkedIn: http://www.linkedin.com/in/parthpandya008
