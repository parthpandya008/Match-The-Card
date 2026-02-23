using UnityEngine;

namespace CardMatch.Game.Cards
{
    /// Interface for sprite management
    public interface ISpriteProvider
    {
        Sprite GetCardFaceSprite(int id);
        Sprite GetCardBackSprite();
        int AvailableSpritesCount { get; }
    }
}
