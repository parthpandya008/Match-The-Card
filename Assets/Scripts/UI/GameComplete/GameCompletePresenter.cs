using CardMatch.GameEvent;
using CardMatch.Score;
using CardMatch.UI;
using UnityEngine;
using System;

namespace CardMatch.UI
{
    public class GameCompletePresenter
    {
        private readonly IGameCompleteView view;
        private readonly GameEvents gameEvents;
        private readonly ScoreManager scoreManager;
        private readonly Func<(int rows, int cols)> getGridSize;

        public GameCompletePresenter(IGameCompleteView view,
                                        GameEvents gameEvents,
                                        ScoreManager scoreManager,
                                        Func<(int rows, int cols)> getGridSize)
        {
            if(view == null || gameEvents == null || scoreManager == null)
            {
                Logger.LogError("GameCompletePresenter initialization failed due to null dependencies.");
                return;
            }
            this.view = view;
            this.gameEvents = gameEvents;
            this.scoreManager = scoreManager;
            this.getGridSize = getGridSize;
           
            this.gameEvents.OnGameCompleted += OnGameCompleted;
            this.gameEvents.OnGameReset += OnGameReset;
        }       

        #region Game Event Handlers
        private void OnGameCompleted(float completionTime)
        {
            view.Show();
            view.SetWinLabel($"You Win!\nTime: {FormatTime(completionTime)}");
            view.SetBestTimeLabel(BuildBestTimeText(completionTime));           
        }        

        private void OnGameReset()
        {
            view.Hide();
        }
        
        #endregion

        #region Helpers
        private string BuildBestTimeText(float completionTime)
        {
            var (rows, cols) = getGridSize();
            string gridKey = $"{rows}x{cols}";
            float bestTime = scoreManager?.GetBestTime(gridKey) ?? 0f;

            return bestTime > 0f
                ? $"Best ({gridKey}): {FormatTime(bestTime)}"
                : "First time!";
        }
        
        private string FormatTime(float time)
        {
           return $"{(int)time}s";
        }
        #endregion

        public void Dispose()
        {
            gameEvents.OnGameCompleted -= OnGameCompleted;
            gameEvents.OnGameReset -= OnGameReset;
        }
    }
}
