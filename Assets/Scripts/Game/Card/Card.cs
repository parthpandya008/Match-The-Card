using System;
using CardMatch.Factory;
using UnityEngine;
using UnityEngine.UI;

namespace CardMatch
{
    // Card controller - manages card state and coordinates with view
    public class Card : MonoBehaviour, ICard
    {
        #region Fields
        [SerializeField] private CardView cardView;
        [SerializeField] private CardType cardType = CardType.Default;

        private int id;
        private int spriteID = -1;
        private bool isFlipped;


        private ISpriteProvider spriteProvider;
        #endregion

        // Event for card click
        public event Action<ICard> OnCardClicked;

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
                ChangeSprite();
            }
        }
       
        public CardType CardType => cardType;       
        public bool IsFlipped => isFlipped;                

        #endregion

        #region Unity Lifecycle
        private void Awake()
        {
            var button = GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(OnClick);
            }
        }

        private void OnClick()
        {
            Logger.Log($"Card {ID} clicked", this);
            OnCardClicked?.Invoke(this);
        }
        #endregion

        #region Initialization
        // Initialize card with dependencies 
        public void Initialize(ISpriteProvider provider)
        {
            spriteProvider = provider;
        }
        #endregion

        // Flip the card (toggle face-up/face-down)
        public void Flip(bool faceUp)
        {
            isFlipped = faceUp;

            ChangeSprite();

            Logger.Log($"Card {ID} flipped to {(isFlipped ? "FACE" : "BACK")}", this);
        }


        #region Sprite Handling
        private void ChangeSprite()
        {
            if (spriteID == -1 || cardView == null) return;

            Sprite sprite;
            if (isFlipped)
            {
                // Face-up: show card face
                sprite = spriteProvider.GetCardFaceSprite(spriteID);
            }
            else
            {
                // Face-down: show card back
                sprite = spriteProvider.GetCardBackSprite();
            }

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