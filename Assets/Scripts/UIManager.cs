using System;
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

        [Header("Grid Size Controls")]
        [SerializeField] private TextMeshProUGUI rowsLabel;
        [SerializeField] private Slider rowsSlider;
        [SerializeField] private TextMeshProUGUI colsLabel;
        [SerializeField] private Slider colsSlider;
        #endregion

        private GameEvents gameEvents;
        private int currentRows = 2;
        private int currentCols = 2;
        public void Initialize(GameEvents events)
        {
            gameEvents = events;
            
            UpdateLabel();
            SubscribeToEvents();
            Debug.Log("UIManager initialized");
        }

        #region Unity Lifecycle

        private void Awake()
        {
            // Setup sliders
            rowsSlider?.onValueChanged.AddListener(OnRowsChanged);
            colsSlider?.onValueChanged.AddListener(OnColsChanged);
        }

        private void OnDestroy()
        {
            if (gameEvents != null)
            {
                gameEvents.OnGameStarted -= OnGameStarted;
            }
        }
        #endregion

        private void SubscribeToEvents()
        {
            if (gameEvents != null)
            {
                gameEvents.OnGameStarted += OnGameStarted;
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
            gameCanvas.SetActive(true);
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
