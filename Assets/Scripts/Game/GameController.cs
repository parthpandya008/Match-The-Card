using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CardMatch.Audio;
using CardMatch.Game.Cards;
using CardMatch.Game.Cards.Factory;
using CardMatch.Game.Events;
using CardMatch.Game.State;
using CardMatch.Score;
using UnityEngine;


namespace CardMatch.Game
{
    // Game controller - handles all game logic and flow
    public class GameController 
    {        
        #region Dependencies
        // Injected dependencies
        private GameEvents gameEvents;
        private ISpriteProvider spriteProvider;
        
        private CardFactory cardFactory;
        private ScoreManager scoreManager;
        private IAudioService audioService;
        private AudioConfig audioConfig;
        #endregion

        #region Internal Systems
        // Internal systems
        private CardLayoutManager layoutManager;
        private ICardAllocationStrategy allocationStrategy;        
        #endregion

        #region Game Feilds
        // Game state
        private List<ICard> cards = new List<ICard>();             
        private int currentRows;
        private int currentCols;
        #endregion

        #region Card Match Feilds
        //Card match
        private int firstSelectedCardId = -1;
        private int secondSelectedCardId = -1;
        private int remainingPairs;
        private bool isProcessingMatch = false;
        private float cardMatchTimer;

        //TODO:Add MATCH_CHECK_WAIT_DURATION into a config or difficulty settings/ GameData(Scriptable Object)
        //Or as per the game requirement
        private const int MATCH_CHECK_WAIT_DURATION = 1000; // milliseconds
        #endregion

        #region  States
        private IGameState idleState;
        private IGameState initializingState;
        private IGameState revealingState;
        private IGameState playingState;
        private IGameState completedState;       
        private IGameState currentState;

        public IGameState PlayState => playingState;
        public IGameState RevealState => revealingState;
        #endregion

        public float CardMatchTimer => cardMatchTimer;
        //TODO: Add this to a config or difficulty settings/ GameData (Scriptable Object)
        public float MisMatchPanelty => 1;

        public GameController(GameEvents events, 
                                ISpriteProvider spriteProvider, 
                                ObjectPoolManager objectPoolManager, 
                                CardFactoryConfig cardFactoryConfig,
                                ScoreManager scoreManager,
                                IAudioService audioService,
                                AudioConfig audioConfig,
                                RectTransform gamePanel,
                                RectTransform cardContainer)
        {
            if(events == null || spriteProvider == null || 
                objectPoolManager == null || cardFactoryConfig == null)
            {
                Logger.LogError("GameController initialization failed due to null dependencies!");
                return;
            }
            gameEvents = events;
            this.spriteProvider = spriteProvider;            
            layoutManager = new CardLayoutManager(gamePanel);
            allocationStrategy = new RandomCardAllocationStrategy();
            cardFactory = new CardFactory(cardContainer,spriteProvider, objectPoolManager, cardFactoryConfig);
            this.scoreManager = scoreManager;
            this.audioService = audioService;
            this.audioConfig = audioConfig;            

            InitializeStates();
            ChangeState(idleState);      
            
            Logger.Log("GameController initialized");
        }

        public void GameUpdate(float deltaTime)
        {
            currentState?.Update(deltaTime);
        }   

        #region Card Setup + Interaction
        // Initialize cards based on grid size
        public void InitializeCards()
        {
            // Clear existing
            cardFactory.DestroyAllCards();
            cards.Clear();

            // Calculate layout ( supports rows x cols)
            CardLayoutData layout = layoutManager?.CalculateLayout(currentRows, currentCols);
            remainingPairs = layout.TotalCards / 2;
            firstSelectedCardId = -1;
            secondSelectedCardId = -1;
            
            // Spawn cards
            for (int i = 0; i < layout.TotalCards; i++)
            {
                Vector3 pos = layoutManager.GetCardPosition(layout, i);
                ICard card = cardFactory.CreateCard(CardType.Default, i, pos, layout.CellScale);
                if(card == null)
                {
                    Logger.LogError($"Failed to create card at index {i}");
                    return;
                }
                card.OnCardClicked += OnCardClicked;
                cards.Add(card);
            }

            // Allocate sprites to cards
            allocationStrategy.AllocateSprites(cards.ToArray(), spriteProvider.AvailableSpritesCount);

            Logger.Log($"Setup {cards.Count} cards");
            gameEvents.RaiseGameStarted();
            gameEvents.RaiseRemainingCardsChanged(remainingPairs);
        }
       

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
            if (currentState is not PlayingState || isProcessingMatch || card.IsAnimating)
            {
                return;
            }                                

             ProcessCardSelection(card);            
        }

        public void SetCardMatchTimer(float elapsedTime)
        {
            cardMatchTimer = elapsedTime;
        }

        private string GetCurrentGridSize()
        {
            return $"{currentRows}x{currentCols}";
        }

        #endregion

        #region Match Logic
        private void  ProcessCardSelection(ICard card)
        {
            card.Flip(!card.IsFlipped);
            gameEvents.RaiseCardFlipped(card.ID);
            audioService?.Play(audioConfig?.CardClickData);
            // Track selection
            if (firstSelectedCardId == -1)
            {
                // First card selected
                firstSelectedCardId = card.ID;
                Logger.Log($"First card selected: {card.ID} (Sprite: {card.SpriteID})");
            }
            else if (secondSelectedCardId == -1 && card.ID != firstSelectedCardId)
            {
                // Second card selected
                secondSelectedCardId = card.ID;
                Logger.Log($"Second card selected: {card.ID} (Sprite: {card.SpriteID})");

                // Check for match
                isProcessingMatch = true;

                CheckMatchAsync();

            }
        }
        private async void CheckMatchAsync()
        {
            // Wait a moment so player can see both cards
            await Task.Delay(MATCH_CHECK_WAIT_DURATION); // milliseconds
            if (currentState is not PlayingState)
            {
                return;
            }

            ICard firstCard = cards[firstSelectedCardId];
            ICard secondCard = cards[secondSelectedCardId];

            if (firstCard.SpriteID == secondCard.SpriteID)
            {
                // MATCH!
                Logger.Log($"MATCH! Cards {firstCard.ID} and {secondCard.ID} (Sprite: {firstCard.SpriteID})");

                firstCard.SetMatched();
                secondCard.SetMatched();
                remainingPairs--;
                CheckGameWin();

                gameEvents.RaiseCardsMatched(firstCard.ID, secondCard.ID);
                gameEvents.RaiseRemainingCardsChanged(remainingPairs);
            }
            else
            {
                // NO MATCH
                Logger.Log($"No match. {firstCard.ID}(S:{firstCard.SpriteID}) != {secondCard.ID}(S:{secondCard.SpriteID})");

                firstCard.Flip(false);
                secondCard.Flip(false);

                gameEvents.RaiseCardsMismatched(firstCard.ID, secondCard.ID);
            }

            // Reset selection
            firstSelectedCardId = -1;
            secondSelectedCardId = -1;
            isProcessingMatch = false;
        }       
        
        private void CheckGameWin()
        {
            if (remainingPairs == 0)
            {
                string gridSize = GetCurrentGridSize();
                scoreManager.SaveBestTime(gridSize, Math.Round(cardMatchTimer, 4));
                audioService?.Play(audioConfig?.MatchWinData);
                ChangeState(completedState);                             
            }
        }
        #endregion

        #region Game Callbacks 
        public void StartGame(int rows, int cols)
        {
            currentRows = rows;
            currentCols = cols;
            ChangeState(initializingState);
        }

        //Reset the card and math fields
        public void ResetGame()
        {
            // Clear existing
            cardFactory.DestroyAllCards();
            for (int i = 0; i < cards.Count; i++)
            {
                cards[i].Reset();
                cards[i].OnCardClicked -= OnCardClicked;
            }
            cards.Clear();           
            remainingPairs = -1;
            firstSelectedCardId = -1;
            secondSelectedCardId = -1;
            isProcessingMatch = false;
            ChangeState(idleState);

            gameEvents.RaiseGameReset();
        }
        #endregion

        #region Game  - States

        private void InitializeStates()
        {
            idleState = new IdleState(this, gameEvents);
            initializingState = new InitializingState(this, gameEvents);
            revealingState = new RevealingState(this, gameEvents);
            playingState = new PlayingState(this, gameEvents);
            completedState = new CompletedState(this, gameEvents);
        }

        public void ChangeState(IGameState newState)
        {
            currentState?.Exit();
            currentState = newState;
            currentState.Enter();
        }           
        #endregion
    }
}
