using System;
using CardMatch.Audio;
using CardMatch.GameEvent;
using CardMatch.UI.Events;
using UnityEngine;

namespace CardMatch.UI
{    
    public class HUDPresenter 
    {
        private readonly IHUDView view;
        private readonly GameEvents gameEvents;
        private readonly UIEvents uiEvents;
        private readonly IAudioService audioService;
        private readonly AudioData buttonAudioData;

        public HUDPresenter(IHUDView view, GameEvents gameEvents, UIEvents uiEvents,
                            IAudioService audioService, AudioData buttonAudioData)
        {
            if (view == null || gameEvents == null || uiEvents == null)
            {
                Logger.LogError("HUDPresenter initialization failed due to null dependencies.");
                return;
            }
            this.view = view;
            this.gameEvents = gameEvents;
            this.uiEvents = uiEvents;

            this.audioService = audioService;
            this.buttonAudioData = buttonAudioData;

            this.gameEvents.OnGameStarted += OnGameStarted;
            this.gameEvents.OnGameReset += OnGameReset;
            this.gameEvents.OnRemainingPairsChanged += OnRemainingPairsChanged;
            this.gameEvents.OnTimeUpdated += OnTimeUpdated;

            this.view.OnMenuButtonClicked += OnMenuButtonClicked;

            view.SetTime(FormatTime(0f));
        }

        #region Game Event Handlers
        private void OnGameStarted()
        {
            view.Show();            
        }

        private void OnGameReset()
        {
            view.Hide();
            view.SetTime(FormatTime(0f));
        }

        private void OnRemainingPairsChanged(int remaining)
        {
            view.SetRemainingPairs($"Pairs Left: {remaining}");
        }
            
        private void OnTimeUpdated(float time)
        {           
            view.SetTime(FormatTime(time));
        }

        private void OnMenuButtonClicked()
        {
            uiEvents?.RaiseStopButtonClicked();
            audioService?.Play(buttonAudioData);
        }
        #endregion

        private string FormatTime(float time)
        {
            return  $"Time: {(int)time}s";
        }
        
        public void Dispose()
        {
            gameEvents.OnGameStarted -= OnGameStarted;
            gameEvents.OnGameReset -= OnGameReset;
            gameEvents.OnRemainingPairsChanged -= OnRemainingPairsChanged;
            gameEvents.OnTimeUpdated -= OnTimeUpdated;

            view.OnMenuButtonClicked -= OnMenuButtonClicked;
        }        
    }
}
