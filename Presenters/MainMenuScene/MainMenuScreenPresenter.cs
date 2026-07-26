using PeckNSend.Models;
using System.ComponentModel;
using UnityEngine;

namespace PeckNSend.Presenters
{
    public class MainMenuScreenPresenter : PresenterBaseClass<MainMenuScreenModel>
    {
        public static MainMenuScreenPresenter Instance { get; private set; }

        [Header("Canvas References")]
        [SerializeField] private GameObject _settingsCanvas;
        [SerializeField] private GameObject _aboutCanvas;

        [Header("Controller Focus Points")]
        [SerializeField] private GameObject _homeFirstButton;     // e.g., Start Button
        [SerializeField] private GameObject _settingsFirstButton; // e.g., Volume Slider or Back Button
        [SerializeField] private GameObject _aboutFirstButton;    // e.g., Back Button

        // Add this reference to handle "Memory"
        private GameObject _lastSelectedHomeButton;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            if (Model == null)
                Model = new MainMenuScreenModel();
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }

        protected override void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MainMenuScreenModel.ActiveScreen))
                RefreshCanvases();
        }

        protected override void ModelSetInitialization(MainMenuScreenModel previousModel)
        {
            RefreshCanvases();
        }

        private void RefreshCanvases()
        {
            var screen = Model.ActiveScreen;

            _settingsCanvas?.SetActive(screen == MainMenuScreenModel.MenuScreen.Settings);
            _aboutCanvas?.SetActive(screen == MainMenuScreenModel.MenuScreen.About);



            // It is a best practice to clear selection before setting a new one
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);

            switch (screen)
            {
                case MainMenuScreenModel.MenuScreen.Home:
                    // If we are returning home, try to go back to the last button used, 
                    // otherwise use the default home button.
                    var toSelect = _lastSelectedHomeButton != null ? _lastSelectedHomeButton : _homeFirstButton;
                    UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(toSelect);
                    break;

                case MainMenuScreenModel.MenuScreen.Settings:
                    // Save which button we were on in the Home screen before we left
                    _lastSelectedHomeButton = UnityEngine.EventSystems.EventSystem.current.firstSelectedGameObject;
                    UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(_settingsFirstButton);
                    break;

                case MainMenuScreenModel.MenuScreen.About:
                    _lastSelectedHomeButton = UnityEngine.EventSystems.EventSystem.current.firstSelectedGameObject;
                    UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(_aboutFirstButton);
                    break;
            }
        }

        #region ----- UI Button Hooks -----
        // ── MAIN MENU UI Button hooks ────────────────────────────────────────────────
        public void OnStartButtonPressed() => SceneManagerPresenter.Instance.Model.RequestPlayerJoinScene();
        public void OnSettingsButtonPressed() => Model.RequestSettings();
        public void OnAboutButtonPressed() => Model.RequestAbout();
        public void OnHomeButtonPressed() => Model.RequestHome();
        public void OnExitButtonPressed() => Model.RequestExit();
        // canvas button hooks ─────────────────────────────────────────────────────────────
        public void OnBackButtonPressed() => Model.RequestHome();


        #endregion

    }
}