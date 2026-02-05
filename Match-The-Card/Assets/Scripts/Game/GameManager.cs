using CardMatch.GameEvent;
using CardMatch.Services;
using CardMatch.UI;
using UnityEngine;

namespace CardMatch
{
    public class GameManager : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private UIManager uiManager;
        [SerializeField] private GameController gameController;

        [Header("Services")]
        [SerializeField] private SpriteProvider spriteProvider;
        // Shared event system
        private GameEvents gameEvents;

        [Header("Scene References")]
        [SerializeField] private Sprite cardBackSprite;
        [SerializeField] private Sprite[] cardFaceSprites;

        private void Awake()
        {
            // Create event system
            gameEvents = new GameEvents();
            spriteProvider = new SpriteProvider(cardBackSprite, cardFaceSprites);

            // Initialize components (pass dependencies)
            gameController.Initialize(gameEvents, spriteProvider);
            uiManager.Initialize(gameEvents);

            Debug.Log("GameManager initialized");
        }

        // Called from Start button
        public void StartGame()
        {
            var (rows, cols) = uiManager.GetGridSize();
            Debug.Log($"Starting game: {rows}x{cols}");
            gameController.StartGame(rows, cols);
        }

        // Called from Give Up button
        public void StopGame()
        {
            gameController.StopGame();
        }
    }
}
    
