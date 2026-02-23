using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardMatch.UI
{
    public class HUDView : MonoBehaviour, IHUDView
    {
        #region Inspector References
        [Header("Panel")]
        [SerializeField] private GameObject gameCanvas;
        [SerializeField] private GameObject hudPanel;
        [SerializeField] private Button menuButton;

        [Header("Labels")]
        [SerializeField] private TextMeshProUGUI remainingPairsLabel;
        [SerializeField] private TextMeshProUGUI timeLabel;
        #endregion

        public event Action OnMenuButtonClicked;

        #region Unity Lifecycle
        private void Start()
        {
            menuButton?.onClick.AddListener(OnMenuClicked);
        }
        private void OnDestroy()
        {
            menuButton?.onClick.RemoveListener(OnMenuClicked);
        }
        #endregion

        private void OnMenuClicked()
        {
            OnMenuButtonClicked?.Invoke();
            Logger.Log("Menu button clicked!", this);
        }

        #region IScreenController
        public void Show()
        {
            gameCanvas?.SetActive(true);
            hudPanel?.SetActive(true);
        }
        public void Hide()
        {
            gameCanvas?.SetActive(false);
            hudPanel?.SetActive(false);
        }
        #endregion

        #region IHUDView
        public void SetRemainingPairs(string value)
        {
            if (remainingPairsLabel != null) 
                    remainingPairsLabel.text = value;
        }

        public void SetTime(string value)
        {
            if (timeLabel != null) 
                    timeLabel.text = value;
        }        
        #endregion
    }
}
