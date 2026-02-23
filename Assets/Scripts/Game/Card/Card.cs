using System;
using CardMatch.Game.Cards.Factory;
using UnityEngine;
using UnityEngine.UI;

namespace CardMatch.Game.Cards
{
    // Card controller - manages card state and coordinates with view
    public class Card : MonoBehaviour, ICard, ICardInitializer
    {
        #region Fields
        [SerializeField] private CardView cardView;
        [SerializeField] private CardType cardType = CardType.Default;

        private int id;
        private int spriteID = -1;
        private bool isFlipped;
        private bool isMatched = false;


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
        public bool IsAnimating => cardView.IsAnimating;

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
            // Can't click if already matched or face-up
            if (isMatched || isFlipped || cardView.IsAnimating) return;

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
            if (cardView.IsAnimating) return;  // Can't flip while animating

            cardView.PlayFlipAnimation(() =>
            {
                // This runs at 90° midpoint
                isFlipped = faceUp;
                ChangeSprite();
            });

            /*isFlipped = faceUp;
            ChangeSprite();
            Logger.Log($"Card {ID} flipped to {(isFlipped ? "FACE" : "BACK")}", this);*/
        }

        public void SetMatched()
        {
            isMatched = true;
            isFlipped = true;  // Keep showing face

            cardView?.SetMatchCardUI();            
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

            cardView?.UpdateSprite(sprite);
        }

        public void SetBackSprite()
        {
            if (spriteProvider != null)
            {
                Sprite sprite = spriteProvider.GetCardBackSprite();
                cardView?.UpdateSprite(sprite);
            }                          
        }

        public void Reset()
        {
            Flip(false);
            isMatched = false;
            cardView?.Reset();
        }
        #endregion
    }
}