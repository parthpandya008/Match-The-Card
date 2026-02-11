using CardMatch.Factory;
using CardMatch.GameEvent;
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
        [SerializeField] private ObjectPoolManager objectPoolManager;
        // Shared event system

        [Header("Scene References")]
        [SerializeField] private Sprite cardBackSprite;
        [SerializeField] private Sprite[] cardFaceSprites;
        [SerializeField] private CardFactoryConfig cardFactoryConfig;
        #endregion

        private GameEvents gameEvents;

        #region Unity Lifecycle
        private void Awake()
        {
            // Create event system
            gameEvents = new GameEvents();
            spriteProvider = new SpriteProvider(cardBackSprite, cardFaceSprites);

            // Initialize components (pass dependencies)
            gameController.Initialize(gameEvents, spriteProvider, 
                objectPoolManager, cardFactoryConfig);
            uiManager.Initialize(gameEvents);

            Logger.Log("GameManager initialized", this);
        }
        #endregion

        #region Game Callbacks 
        // Called from Start button
        public void StartGame()
        {
            var (rows, cols) = uiManager.GetGridSize();
            Logger.Log($"Starting game: {rows}x{cols}", this);
            gameController.StartGame(rows, cols);
        }

        // Called from Stop/Menu button
        public void StopGame()
        {
            gameController.ResetGame();
        }
        #endregion
    }
}
    
