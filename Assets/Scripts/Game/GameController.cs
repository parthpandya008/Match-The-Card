using System;
using System.Collections;
using System.Collections.Generic;
using CardMatch.Factory;
using CardMatch.GameEvent;
using CardMatch.GameState;
using CardMatch.Layout;
using Unity.IO.LowLevel.Unsafe;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UIElements;

namespace CardMatch
{
    // Game controller - handles all game logic and flow
    public class GameController : MonoBehaviour
    {
        [Header("Scene References")]               
        [SerializeField] private RectTransform gamePanel;
        [SerializeField] private RectTransform cardContainer;

        // Injected dependencies
        private GameEvents gameEvents;
        private ISpriteProvider spriteProvider;
        private ObjectPoolManager objectPoolManager;
        private CardFactory cardFactory;

        // Internal systems
        private CardLayoutManager layoutManager;
        private ICardAllocationStrategy allocationStrategy;

        // Game state
        private List<ICard> cards = new List<ICard>();
        private IGameState currentState;

        private int currentRows;
        private int currentCols;

        public void Initialize(GameEvents events, ISpriteProvider spriteProvider, 
                    ObjectPoolManager objectPoolManager, CardFactoryConfig cardFactoryConfig)
        {
            gameEvents = events;
            this.spriteProvider = spriteProvider;
            this.objectPoolManager = objectPoolManager;
            layoutManager = new CardLayoutManager(gamePanel);
            allocationStrategy = new RandomCardAllocationStrategy();
            cardFactory = new CardFactory(cardContainer,spriteProvider, objectPoolManager, cardFactoryConfig);
            ChangeState(new IdleState(this, gameEvents));
            Logger.Log("GameController initialized", this);
        }

        private void Update()
        {
            currentState?.Update(Time.deltaTime);
        }

        #region Card Setup
        // Initialize cards based on grid size
        public void InitializeCards()
        {
            // Clear existing
            cardFactory.DestroyAllCards();
            cards.Clear();

            // Calculate layout ( supports rows x cols)
            CardLayoutData layout = layoutManager?.CalculateLayout(currentRows, currentCols);            

            // Spawn cards
            for (int i = 0; i < layout.TotalCards; i++)
            {
                Vector3 pos = layoutManager.GetCardPosition(layout, i);
                ICard card = cardFactory.CreateCard(CardType.Default, i, pos, layout.CellScale);
                card.OnCardClicked += OnCardClicked;
                cards.Add(card);
            }

            // Allocate sprites to cards
            allocationStrategy.AllocateSprites(cards.ToArray(), spriteProvider.AvailableSpritesCount);

            Logger.Log($"Setup {cards.Count} cards", this);
            gameEvents.RaiseGameStarted();
        }
        #endregion

        // Flip all cards to hide them
        public void FlipAllCards(bool faceUp)
        {            
            foreach (var card in cards)
            {
                card.Flip(faceUp);
            }
        }        

        private void OnCardClicked(ICard card)
        {
            Logger.Log($"GameController received click on card {card.ID}", this);
            if (!(currentState is PlayingState)) return;

            card.Flip(!card.IsFlipped);
        }

        #region Game Callbacks 
        public void StartGame(int rows, int cols)
        {
            currentRows = rows;
            currentCols = cols;
            ChangeState(new InitializingState(this, gameEvents));
        }

        public void StopGame()
        {
            ChangeState(new IdleState(this, gameEvents));
        }
        #endregion

        public void ChangeState(IGameState newState)
        {
            currentState?.Exit();
            currentState = newState;
            currentState.Enter();
        } 
    }
}
