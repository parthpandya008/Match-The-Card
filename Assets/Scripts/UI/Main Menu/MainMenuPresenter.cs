using System;
using CardMatch.Audio;
using CardMatch.GameEvent;
using CardMatch.UI.Events;
using UnityEngine;

namespace CardMatch.UI
{
    public class MainMenuPresenter 
    {
        private readonly IMainMenuView view;
        private readonly GameEvents gameEvent;
        private readonly UIEvents uiEvents;
        private readonly IAudioService audioService;
        private readonly AudioData buttonAudioData;

        private int currentRows = 2;
        private int currentCols = 2;

        public MainMenuPresenter(IMainMenuView view, GameEvents gameEvent, UIEvents uiEvents, 
                                    IAudioService audioService, AudioData buttonAudioData)
        {
            if(view == null || gameEvent == null || uiEvents == null)
            {
                Logger.LogError("MainMenuPresenter initialization failed due to null dependencies.");
                return;
            }
            this.view = view;            
            this.gameEvent = gameEvent;
            this.uiEvents = uiEvents;
            this.audioService = audioService;
            this.buttonAudioData = buttonAudioData;                       

            this.view.OnRowsChanged += OnRowsChanged;
            this.view.OnColsChanged += OnColsChanged;
            this.view.OnStartButtonClicked += OnStartButtonClicked;

            gameEvent.OnGameStarted += OnGameStarted;
            gameEvent.OnGameReset += OnGameReset;

            RefreshLabels();
        }

        #region View Event 
        private void OnRowsChanged(int rows)
        {
            currentRows = rows;
            RefreshLabels();
        }

        private void OnColsChanged(int cols)
        {
            currentCols = cols;
            RefreshLabels();
        }
        private void OnStartButtonClicked()
        {
            uiEvents?.RaiseStartButtonClicked();
            audioService?.Play(buttonAudioData);
        }

        private void OnGameStarted()
        {
            view.Hide();
        }
        private void OnGameReset()
        {
            view.Show();
        }
        #endregion

        private void RefreshLabels()
        {
            view.SetRowsLabel($"Rows: {currentRows}");
            view.SetColsLabel($"Cols: {currentCols}");
        }

        public (int rows, int cols) GetGridSize()
        {
            return (currentRows, currentCols);
        }

        public void Dispose()
        {
            view.OnRowsChanged -= OnRowsChanged;
            view.OnColsChanged -= OnColsChanged;
            view.OnStartButtonClicked -= OnStartButtonClicked;

            gameEvent.OnGameStarted -= OnGameStarted;
            gameEvent.OnGameReset -= OnGameReset;
        }        
    }
}
