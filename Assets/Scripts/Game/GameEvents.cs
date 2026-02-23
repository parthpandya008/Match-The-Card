using System;
using UnityEngine;

namespace CardMatch.Game.Events
{
    /// Centralized event system using Observer pattern
    public class GameEvents
    {
        // Game lifecycle events
        public event Action OnGameStarted;
        public event Action<float> OnGameCompleted;
        public event Action OnGameReset;

        // Card events
        public event Action<int> OnCardFlipped;
        public event Action<int, int> OnCardsMatched;
        public event Action<int, int> OnCardsMismatched;
        public event Action<int> OnRemainingPairsChanged;

        public event Action<float> OnTimeUpdated;

        // Methods to raise events
        public void RaiseGameStarted() => OnGameStarted?.Invoke();
        public void RaiseGameCompleted(float completionTime) => 
                    OnGameCompleted?.Invoke(completionTime);
        public void RaiseGameReset() => OnGameReset?.Invoke();
        
        public void RaiseCardFlipped(int cardId) => OnCardFlipped?.Invoke(cardId);
        public void RaiseCardsMatched(int card1Id, int card2Id) => OnCardsMatched?.Invoke(card1Id, card2Id);
        public void RaiseCardsMismatched(int card1Id, int card2Id) => OnCardsMismatched?.Invoke(card1Id, card2Id);
        public void RaiseRemainingCardsChanged(int remainingPair) => OnRemainingPairsChanged?.Invoke(remainingPair);

        public void RaiseTimeUpdated(float time) => OnTimeUpdated?.Invoke(time);
    }
}
