using System.Collections.Generic;
using CardMatch.Audio;
using CardMatch.GameEvent;
using CardMatch.Score;
using CardMatch.UI;
using CardMatch.UI.Events;
using UnityEngine;

namespace CardMatch.UI
{
    /* 
     - TODO:For more UI screen  Replace [SerializeField] View references.
     - With a ScreenRegistry ScriptableObject and  PresenterFactory     
     - Use Addressables to load/unload screens     
    */
    public class UIManager : MonoBehaviour
    {
        #region Inspector References
        [SerializeField] 
        private MainMenuView mainMenuView;
        [SerializeField] 
        private HUDView hudView;
        [SerializeField]
        private GameCompleteView gameCompleteView;
        #endregion

        #region Presenters References
        private MainMenuPresenter mainMenuPresenter;
        private HUDPresenter hudPresenter;
        private GameCompletePresenter gameCompletePresenter;
        #endregion

        private List<IScreenController> allScreens = new List<IScreenController>();

        #region Initialization
        public void Initialize(GameEvents gameEvents, UIEvents uiEvents,
                               ScoreManager scoreManager, IAudioService audioService, 
                               AudioConfig audioConfig)
        {
            
            // Build Presenters — MainMenu first 
            mainMenuPresenter = new MainMenuPresenter(mainMenuView, gameEvents, uiEvents,
                                                      audioService, audioConfig.UIButtonClickData);
            hudPresenter = new HUDPresenter(hudView, gameEvents, uiEvents,
                                                audioService, audioConfig.UIButtonClickData);
            gameCompletePresenter = new GameCompletePresenter(gameCompleteView, 
                                                              gameEvents, 
                                                              scoreManager,
                                                              () => mainMenuPresenter.GetGridSize());

            allScreens.Add(mainMenuView);

            HideAll();
            mainMenuView?.Show();

            Logger.Log("UIManager initialized", this);
        }
        #endregion

        #region Unity Lifecycle
        private void OnDestroy()
        {
            // Clean up all event subscriptions held inside Presenters
            mainMenuPresenter?.Dispose();
            hudPresenter?.Dispose();
            gameCompletePresenter?.Dispose();
        }
        #endregion

        private void HideAll()
        {
            foreach (var screen in allScreens)
                screen.Hide();
        }

        #region Public API
        public (int rows, int cols) GetGridSize()
        {
            if(mainMenuPresenter == null)
            {
                Logger.Log("MainMenuPresenter is null!");
                return (0,0);
            }
           return mainMenuPresenter.GetGridSize();
        }
        #endregion
    }
}
