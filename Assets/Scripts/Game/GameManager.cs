using CardMatch.Audio;
using CardMatch.Factory;
using CardMatch.GameEvent;
using CardMatch.Score;
using CardMatch.Services;
using CardMatch.UI;
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
            spriteProvider = new SpriteProvider(cardDeckConfig);
            scoreManager = new ScoreManager();
            audioService = new AudioService();
            
            // Initialize components (pass dependencies)
            gameController.Initialize(gameEvents, spriteProvider, objectPoolManager, 
                                       cardFactoryConfig, scoreManager, 
                                       audioService, audioConfig);
            uiManager.Initialize(gameEvents, scoreManager, 
                                    audioService, audioConfig);

            audioService.Play(audioConfig?.GameBGData);
            Logger.Log("GameManager initialized", this);
        } 
        #endregion

        #region Game Callbacks 
        // Called from Start button
        public void StartGame()
        {
            audioService?.Play(audioConfig?.UIButtonClickData);
            var (rows, cols) = uiManager.GetGridSize();
            Logger.Log($"Starting game: {rows}x{cols}", this);
            gameController.StartGame(rows, cols);
        }

        // Called from Stop/Menu button
        public void StopGame()
        {
            audioService?.Play(audioConfig?.UIButtonClickData);
            gameController.ResetGame();
        }
        #endregion
    }
}
    
