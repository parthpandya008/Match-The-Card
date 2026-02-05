using CardMatch;
using UnityEngine;

namespace CardMatch.Services
{
    public class SpriteProvider : ISpriteProvider
    {
        [SerializeField] private Sprite cardBackSprite;
        [SerializeField] private Sprite[] cardFaceSprites;
        public SpriteProvider(Sprite cardBackSprite, Sprite[] cardFaceSprites)
        {
            this.cardBackSprite = cardBackSprite;
            this.cardFaceSprites = cardFaceSprites;
        }
        public Sprite GetCardFaceSprite(int id)
        {
            if (id < 0 || id >= cardFaceSprites.Length) return null;
            return cardFaceSprites[id];
        }

        public Sprite GetCardBackSprite() => cardBackSprite;

        public int AvailableSpritesCount => cardFaceSprites.Length;
    }
}

