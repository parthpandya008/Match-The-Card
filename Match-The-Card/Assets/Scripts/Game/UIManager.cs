using System;
using CardMatch.GameEvent;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardMatch.UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] private GameObject gameCanvas;
        [SerializeField] private GameObject mainCanvas;

        [Header("Controls")]
        [SerializeField] private TextMeshProUGUI rowsLabel;
        [SerializeField] private Slider rowsSlider;
        [SerializeField] private TextMeshProUGUI colsLabel;
        [SerializeField] private Slider colsSlider;

        private GameEvents gameEvents;
        private int currentRows = 2;
        private int currentCols = 2;
        public void Initialize(GameEvents events)
        {
            gameEvents = events;

            // Setup sliders
            rowsSlider.onValueChanged.AddListener(OnRowsChanged);
            colsSlider.onValueChanged.AddListener(OnColsChanged);

            UpdateLabel();
            Debug.Log("UIManager initialized");
        }

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

        private void UpdateLabel()
        {
            if (rowsLabel != null && colsLabel != null)
            {
                rowsLabel.text = $"Rows: {currentRows}";
                colsLabel.text = $"Cols: {currentCols}";
            }            
        }

        public (int rows, int cols) GetGridSize()
        {
            return (currentRows, currentCols);
        }
    }
}
