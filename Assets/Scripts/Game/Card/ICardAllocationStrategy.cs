using UnityEngine;

namespace CardMatch.Game.Cards
{
    /// Strategy interface for card allocation algorithms
    public interface ICardAllocationStrategy
    {
        void AllocateSprites(ICard[] cards, int availableSpritesCount);
    }
}
