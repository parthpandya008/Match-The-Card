using System;
using UnityEngine;

namespace CardMatch.GameEvent
{
    /// Centralized event system using Observer pattern
    public class GameEvents
    {
        // Game lifecycle events
        public event Action OnGameStarted;

        // Card events
        public event Action<int> OnCardFlipped;
        public event Action<int, int> OnCardsMatched;
        public event Action<int, int> OnCardsMismatched;


        // Methods to raise events
        public void RaiseGameStarted() => OnGameStarted?.Invoke();
        
        public void RaiseCardFlipped(int cardId) => OnCardFlipped?.Invoke(cardId);

        public void RaiseCardsMatched(int card1Id, int card2Id) => OnCardsMatched?.Invoke(card1Id, card2Id);

        public void RaiseCardsMismatched(int card1Id, int card2Id) => OnCardsMismatched?.Invoke(card1Id, card2Id);
    }
}
