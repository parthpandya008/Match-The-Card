using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardMatch.UI
{
    public class MainMenuView : MonoBehaviour, IMainMenuView
    {
        #region Inspector References
        [SerializeField] private GameObject mainMenuPanel;
       
        [SerializeField] private Slider rowsSlider;
        [SerializeField] private Slider colsSlider;
        [SerializeField] private TextMeshProUGUI rowsLabel;
        [SerializeField] private TextMeshProUGUI colsLabel;
        [SerializeField] private Button startButton;
        #endregion

        public event Action<int> OnRowsChanged;
        public event Action<int> OnColsChanged;
        public event Action OnStartButtonClicked;

        #region Unity Lifecycle
        private void Start()
        {
            rowsSlider?.onValueChanged.AddListener(RowSliderUpdate);
            colsSlider?.onValueChanged.AddListener(ColSliderUpdate);
            startButton?.onClick.AddListener(OnStartClicked);
        }

        private void OnDestroy()
        {
            rowsSlider?.onValueChanged.RemoveListener(RowSliderUpdate);
            colsSlider?.onValueChanged.RemoveListener(ColSliderUpdate);
            startButton?.onClick.RemoveListener(OnStartClicked);
        }
        #endregion

        #region UI Event Handlers
        private void RowSliderUpdate(float value)
        {
            OnRowsChanged?.Invoke(Mathf.RoundToInt(value));
        }

        private void ColSliderUpdate(float value)
        {
            OnColsChanged?.Invoke(Mathf.RoundToInt(value));
        }

        private void OnStartClicked()
        {
            OnStartButtonClicked?.Invoke();
            Logger.Log("Start button clicked!", this);
        }
        #endregion

        #region IMainMenuView
        public void SetRowsLabel(string value)
        {
            if (rowsLabel != null) rowsLabel.text = value;
        }

        public void SetColsLabel(string value)
        {
            if (colsLabel != null) colsLabel.text = value;
        }
        #endregion

        #region IScreenController
        public void Show()
        {
            mainMenuPanel?.SetActive(true);
        }
        public void Hide()
        {
            mainMenuPanel?.SetActive(true);
        }
        #endregion
    }
}
