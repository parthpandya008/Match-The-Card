using System.Collections.Generic;
using System.Threading.Tasks;
using CardMatch.Audio;
using CardMatch.Game.Events;
using UnityEngine;

namespace CardMatch.Game.Cards
{
    // Handles card default matching logic
    public class DefaultMatchProcessor : IMatchProcessor
    {
        private GameEvents gameEvents;
        private IAudioService audioService;
        private AudioConfig audioConfig;

        private ICard firstSelectedCard = null;
        private ICard secondSelectedCard = null;
        private bool isProcessing = false;

        public bool IsProcessing => isProcessing;

        public DefaultMatchProcessor(GameEvents gameEvents, IAudioService audioService, AudioConfig audioConfig)
        {
            this.gameEvents = gameEvents;
            this.audioService = audioService;
            this.audioConfig = audioConfig;
        }

        public bool ProcessSelection(ICard card)
        {
            if(card == null)
            {
                Logger.LogError("Card is null in ProcessSelection");
                return false;
            }
            card.Flip(!card.IsFlipped);
            gameEvents?.RaiseCardFlipped(card.ID);
            audioService?.Play(audioConfig?.CardClickData);

            if (firstSelectedCard == null)
            {
                // First card
                firstSelectedCard = card;
                Logger.Log($"First card selected: {card.ID} (Sprite: {card.SpriteID})");
                return false;
            }
            else if (card.ID != firstSelectedCard.ID)
            {
                // Second card
                secondSelectedCard = card;
                Logger.Log($"Second card selected: {card.ID} (Sprite: {card.SpriteID})");
                isProcessing = true;
                return true;
            }
            return false;
        }

        public async Task<MatchResult> CheckMatchAsync(int waitDuration)
        {            
            await Task.Delay(waitDuration);
            
            bool isMatch = firstSelectedCard.SpriteID == secondSelectedCard.SpriteID;

            if (isMatch)
            {
                Logger.Log($"MATCH! Cards {firstSelectedCard.ID} and {secondSelectedCard.ID} " +
                                    $"(Sprite: {firstSelectedCard.SpriteID})");
                firstSelectedCard.SetMatched();
                secondSelectedCard.SetMatched();
                gameEvents?.RaiseCardsMatched(firstSelectedCard.ID, secondSelectedCard.ID);
            }
            else
            {
                Logger.Log($"No match: {firstSelectedCard.ID} != {secondSelectedCard.ID}");
                firstSelectedCard.Flip(false);
                secondSelectedCard.Flip(false);
                gameEvents?.RaiseCardsMismatched(firstSelectedCard.ID, secondSelectedCard.ID);
            }

            var result = new MatchResult(isMatch, firstSelectedCard.ID, secondSelectedCard.ID);
            Reset();

            return result;
        }

        public void Reset()
        {
            firstSelectedCard = null;
            secondSelectedCard = null;
            isProcessing = false;
        }
    }
}
