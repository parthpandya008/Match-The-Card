using CardMatch.Audio;
using CardMatch.Factory;
using CardMatch.GameEvent;
using CardMatch.Score;
using CardMatch.Services;
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
            gameController.Initialize(gameEvents, spriteProvider, objectPoolManager, 
                                       cardFactoryConfig, scoreManager, 
                                       audioService, audioConfig);
            uiManager.Initialize(gameEvents, uiEvents, scoreManager, 
                                    audioService, audioConfig);

            audioService.Play(audioConfig?.GameBGData);

            // Subscribe to UI requests
            uiEvents.OnStartButtonClicked += StartGame;
            uiEvents.OnStopButtonClicked += StopGame;

            Logger.Log("GameManager initialized", this);
        }

        private void OnDestroy()
        {
            // Unsubscribe from UI events
            uiEvents.OnStartButtonClicked -= StartGame;
            uiEvents.OnStopButtonClicked -= StopGame;
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
    
