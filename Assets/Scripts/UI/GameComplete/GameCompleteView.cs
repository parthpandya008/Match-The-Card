using System;
using TMPro;
using UnityEngine;

namespace CardMatch.UI
{
    public class GameCompleteView : MonoBehaviour, IGameCompleteView
    {
        #region Inspector References
        [Header("Panel")]
        [SerializeField] private GameObject gameCompletePanel;

        [Header("Labels")]
        [SerializeField] private TextMeshProUGUI winLabel;
        [SerializeField] private TextMeshProUGUI bestTimeText;               
        #endregion        

        #region IScreenController
        public void Show()
        {
            gameCompletePanel?.SetActive(true);
        }
        
        public void Hide()
        {
            gameCompletePanel?.SetActive(false);
        }            
        #endregion

        #region IGameCompleteView
        public void SetWinLabel(string value)
        {
            if (winLabel != null) 
                winLabel.text = value;
        }

        public void SetBestTimeLabel(string value)
        {
            if (bestTimeText != null) 
                bestTimeText.text = value;
        }        
        #endregion
    }
}
