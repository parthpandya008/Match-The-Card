using System;
using UnityEngine;

namespace CardMatch.GameEvent
{
    /// Centralized event system using Observer pattern
    public class GameEvents
    {
        // Game lifecycle events
        public event Action OnGameStarted;

        // Methods to raise events
        public void RaiseGameStarted() => OnGameStarted?.Invoke();
    }
}
