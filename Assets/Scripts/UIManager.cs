using System;
using System.Collections;
using CardMatch.GameEvent;
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
        [SerializeField] private TextMeshProUGUI resetButtonLabel;
        [SerializeField] private GameObject gameCompletePanel;        
        #endregion

        private GameEvents gameEvents;
        private int currentRows = 2;
        private int currentCols = 2;
        public void Initialize(GameEvents events)
        {
            gameEvents = events;
            
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
                time = Mathf.FloorToInt(time);
                timeLabel.text = $"Time: {time}s";
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
            if(winLabel != null)
                winLabel.text = $"You Win!\nTime: {completionTime:F2} s";            
            if(resetButtonLabel != null)
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
