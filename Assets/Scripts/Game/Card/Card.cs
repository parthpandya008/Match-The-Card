using UnityEngine;
using UnityEngine.UI;

namespace CardMatch
{
    // Card controller - manages card state and coordinates with view
    public class Card : MonoBehaviour, ICard
    {
        [SerializeField] private CardView cardView;

        private int id;
        private int spriteID = -1;
        private ISpriteProvider spriteProvider;

        #region Public Properties
        public int ID
        {
            get => id;
            set => id = value;
        }

        public int SpriteID
        {
            get => spriteID;
            set
            {
                spriteID = value;
                UpdateSprite();
            }
        }
        #endregion

        #region Initialization
        // Initialize card with dependencies 
        public void Initialize(ISpriteProvider provider)
        {
            spriteProvider = provider;
        }
        #endregion

        #region Sprite Handling
        private void UpdateSprite()
        {
            if (spriteProvider == null || spriteID < 0) return;
            Sprite sprite = spriteProvider.GetCardFaceSprite(spriteID);
            cardView.UpdateSprite(sprite);
        }

        public void SetBackSprite()
        {
            if (spriteProvider != null)
            {
                Sprite sprite = spriteProvider.GetCardBackSprite();
                cardView.UpdateSprite(sprite);
            }                          
        }
        #endregion
    }
}