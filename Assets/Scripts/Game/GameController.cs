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
        private IAudioService audioService;
        private AudioConfig audioConfig;
        private ScoreManager scoreManager;
        #endregion

        #region Internal Systems
        // Internal systems
        private CardFactory cardFactory;        
        private CardLayoutManager layoutManager;
        private ICardAllocationStrategy allocationStrategy;  
        private IMatchProcessor matchProcessor;
        #endregion

        #region Game Feilds
        // Game state
        private List<ICard> cards = new List<ICard>();             
        private int currentRows;
        private int currentCols;
        #endregion

        #region Card Match Feilds
        //Card match       
        private int remainingPairs;       
        private float cardMatchTimer;
        // TODO: Move to grid-specific configuration
        // These timing values should be part of a GridConfig/LevelConfig ScriptableObject
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
            this.scoreManager = scoreManager;
            this.audioService = audioService;
            this.audioConfig = audioConfig;

            layoutManager = new CardLayoutManager(gamePanel);
            allocationStrategy = new RandomCardAllocationStrategy();
            cardFactory = new CardFactory(cardContainer,spriteProvider, objectPoolManager, cardFactoryConfig);
            matchProcessor = new DefaultMatchProcessor(gameEvents, audioService, audioConfig);

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
            matchProcessor?.Reset();

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
            if (currentState is not PlayingState || card.IsAnimating
                    || matchProcessor == null || matchProcessor.IsProcessing)
            {
                return;
            }
            bool shouldCheckMatch = matchProcessor.ProcessSelection(card);
            if (shouldCheckMatch)
            {
                CheckMatchAsync();
            }                     
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
        private async void CheckMatchAsync()
        {
            var result = await matchProcessor?.CheckMatchAsync(MATCH_CHECK_WAIT_DURATION);
            if (currentState is not PlayingState)
                return;

            if(result.IsMatch)
            {
                remainingPairs--;
                CheckGameWin();
                gameEvents.RaiseRemainingCardsChanged(remainingPairs);
            }            
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
            matchProcessor.Reset();
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
