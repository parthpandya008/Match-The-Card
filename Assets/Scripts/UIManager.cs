using System;
using System.Collections;
using CardMatch.Audio;
using CardMatch.GameEvent;
using CardMatch.Score;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardMatch.UI
{
    /// Manages UI elements and responds to game events
    public class UIManager : MonoBehaviour
    {
        #region Inspector References
        [Header("Panels")]
        [SerializeField] private GameObject gameCanvas;
        [SerializeField] private GameObject mainCanvas;
        [SerializeField] private GameObject mainMenuPanel;

        [Header("Grid Size Controls")]
        [SerializeField] private TextMeshProUGUI rowsLabel;
        [SerializeField] private Slider rowsSlider;
        [SerializeField] private TextMeshProUGUI colsLabel;
        [SerializeField] private Slider colsSlider;

        [Header("Scene References")]
        [SerializeField] private TextMeshProUGUI remainingPairsLabel;
        [SerializeField] private TextMeshProUGUI timeLabel;
        [SerializeField] private TextMeshProUGUI winLabel;
        [SerializeField] private TextMeshProUGUI bestTimeText;
        [SerializeField] private TextMeshProUGUI resetButtonLabel;
        [SerializeField] private GameObject gameCompletePanel;
        #endregion

        // Injected dependencies
        private GameEvents gameEvents;
        private ScoreManager scoreManager;
        private IAudioService audioService;
        private AudioConfig audioConfig;
        private int currentRows = 2;
        private int currentCols = 2;
        public void Initialize(GameEvents events, ScoreManager scoreManager, 
                                    IAudioService audioService, AudioConfig audioConfig)
        {
            gameEvents = events;
            this.scoreManager = scoreManager;
            this.audioService = audioService;
            this.audioConfig = audioConfig;
            UpdateLabel();
            SubscribeToEvents();
            Logger.Log("UIManager initialized", this);
        }

        #region Unity Lifecycle

        private void Awake()
        {
            // Setup sliders
            rowsSlider?.onValueChanged.AddListener(OnRowsChanged);
            colsSlider?.onValueChanged.AddListener(OnColsChanged);
            OnGameReset(); // Start with main menu
        }

        private void OnDestroy()
        {
            if (gameEvents != null)
            {
                gameEvents.OnGameStarted -= OnGameStarted;
                gameEvents.OnGameCompleted -= OnGameCompleted;
                gameEvents.OnGameReset -= OnGameReset;
                gameEvents.OnRemainingPairsChanged -= OnRemainingPairsChanged;
                gameEvents.OnTimeUpdated -= OnTimeUpdated;
            }
        }
        #endregion

        private void SubscribeToEvents()
        {
            if (gameEvents != null)
            {
                gameEvents.OnGameStarted += OnGameStarted;               
                gameEvents.OnGameCompleted += OnGameCompleted;
                gameEvents.OnGameReset += OnGameReset;
                gameEvents.OnRemainingPairsChanged += OnRemainingPairsChanged;
                gameEvents.OnTimeUpdated += OnTimeUpdated;
            }            
        }
        

        #region UI Event Handlers
        private void OnRowsChanged(float value)
        {
            currentRows = Mathf.RoundToInt(value);
            UpdateLabel();
        }

        private void OnColsChanged(float value)
        {
            currentCols = Mathf.RoundToInt(value);
            UpdateLabel();
        }               

        private void OnGameStarted()
        {
            gameCanvas?.SetActive(true);
            mainMenuPanel?.SetActive(false);
            if (resetButtonLabel != null)
                resetButtonLabel.text = "Reset";
        }

        private void OnRemainingPairsChanged(int remaining)
        {
            if (remainingPairsLabel != null)
            {
                remainingPairsLabel.text = $"Pairs Left: {remaining}";
            }
        }

        private void OnTimeUpdated(float time)
        {
            if (timeLabel != null)
            {               
                timeLabel.text = $"Time: {(int)time}s";
            }
        }

        private void OnGameReset()
        {
            gameCanvas?.SetActive(false);
            gameCompletePanel?.SetActive(false);
            mainMenuPanel?.SetActive(true);
            OnTimeUpdated(0);
        }

        private void OnGameCompleted(float completionTime)
        {
            ShowGameCompleteUI(completionTime);
        }

        private void ShowGameCompleteUI(float completionTime)
        {
           gameCompletePanel?.SetActive(true);
            OnTimeUpdated(completionTime);
            if(winLabel != null)
                winLabel.text = $"You Win!\nTime: {(int)completionTime}s";

            // Best time comparison
            string gridSize = $"{currentRows}x{currentCols}";
            float bestTime = scoreManager?.GetBestTime(gridSize) ?? 0f;

            if (bestTimeText != null)
            {
                if (bestTime > 0f)
                    bestTimeText.text = $"Best ({gridSize}): {(int)bestTime}s";
                else
                    bestTimeText.text = "First time!";
            }

            if (resetButtonLabel != null)
                resetButtonLabel.text = "Menu";
        }

        #endregion

        #region UI Updates
        private void UpdateLabel()
        {
            if (rowsLabel != null && colsLabel != null)
            {
                rowsLabel.text = $"Rows: {currentRows}";
                colsLabel.text = $"Cols: {currentCols}";
            }
        }
        #endregion

        #region Public API
        public (int rows, int cols) GetGridSize()
        {
            return (currentRows, currentCols);
        }
        #endregion
    }
}
