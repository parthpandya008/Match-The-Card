using CardMatch.Audio;
using CardMatch.Game;
using CardMatch.Game.Cards;
using CardMatch.Game.Cards.Factory;
using CardMatch.Game.Events;
using CardMatch.Score;
using CardMatch.UI;
using CardMatch.UI.Events;
using UnityEngine;

namespace CardMatch
{
    /// Top-level game manager - handles dependency injection and wiring
    public class GameManager : MonoBehaviour
    {
        #region Inspector References
        [Header("Components")]
        [SerializeField] private UIManager uiManager;
        [SerializeField] private GameController gameController;

        [Header("Services")]
        private SpriteProvider spriteProvider;
        private GameEvents gameEvents;
        private UIEvents uiEvents;
        private ScoreManager scoreManager;
        private IAudioService audioService;
        [SerializeField] private ObjectPoolManager objectPoolManager;        

        [Header("Config Scene References")]        
        [SerializeField] private CardDeckConfig cardDeckConfig;
        [SerializeField] private CardFactoryConfig cardFactoryConfig;
        [SerializeField] private AudioConfig audioConfig;

        [Header("Gameplay Panel References")]
        [SerializeField] private RectTransform gamePanel;
        [SerializeField] private RectTransform cardContainer;
        #endregion        

        #region Unity Lifecycle
        private void Awake()
        {
            // Create event system
            
            gameEvents = new GameEvents();
            uiEvents = new UIEvents();
            spriteProvider = new SpriteProvider(cardDeckConfig);
            scoreManager = new ScoreManager();
            audioService = new AudioService();
            
            // Initialize components (pass dependencies)
            gameController = new GameController(gameEvents, spriteProvider, objectPoolManager, 
                                                cardFactoryConfig, scoreManager, 
                                                audioService, audioConfig,
                                                gamePanel, cardContainer);

            uiManager.Initialize(gameEvents, uiEvents, scoreManager, 
                                    audioService, audioConfig);

            audioService.Play(audioConfig?.GameBGData);

            // Subscribe to UI requests
            uiEvents.OnStartButtonClicked += StartGame;
            uiEvents.OnStopButtonClicked += StopGame;

            Logger.Log("GameManager initialized", this);
        }

        private void Update()
        {
            gameController?.GameUpdate(Time.deltaTime);
        }
        private void OnDestroy()
        {
            // Unsubscribe from UI events
            uiEvents.OnStartButtonClicked -= StartGame;
            uiEvents.OnStopButtonClicked -= StopGame;
        }

        private void OnApplicationQuit()
        {
            gameController?.ResetGame();
            scoreManager?.OnApplicationQuit();
        }

        private void OnApplicationPause(bool pause)
        {
            //TODO: Set the PauseState (pause the time, save the current game progress (Grid size, Matched cards, Remaining pairs, etc))
            //On resume reset the game progress (from the pause state) and go to the previous state of the PauseState

            scoreManager?.OnApplicationPause(pause);
        }
        #endregion

        #region Game Callbacks 
        // Called from Start button
        private void StartGame()
        {                      
            var (rows, cols) = uiManager.GetGridSize();
            Logger.Log($"Starting game: {rows}x{cols}", this);
            gameController.StartGame(rows, cols);
        }

        // Called from Stop/Menu button
        private void StopGame()
        {            
            gameController.ResetGame();
        }
        #endregion
    }
}
    
