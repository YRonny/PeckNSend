using System;
using UnityEngine;

namespace PeckNSend.Models
{
    public partial class MainMenuScreenModel : UnityModelBaseClass
    {
        public enum MenuScreen { Home, Settings, About }

        private MenuScreen _activeScreen;
        public MenuScreen ActiveScreen
        {
            get => _activeScreen;
            set 
            { 
                if (_activeScreen == value) return;
                _activeScreen = value; 
                OnPropertyChanged();
            }
        }

        private MainMenuScreenFSM _fsm;

        public MainMenuScreenModel()
        {
            _fsm = new MainMenuScreenFSM(this);
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            _fsm.CurrentState?.Update(deltaTime);
        }

        public override void FixedUpdate(float fixedDeltaTime)
        {
            base.FixedUpdate(fixedDeltaTime);
            _fsm.CurrentState?.FixedUpdate(fixedDeltaTime);
        }

        #region ----REQUESTS-----
        public void RequestAbout() => _fsm.CurrentState?.OnRequestAbout();
        public void RequestSettings() => _fsm.CurrentState?.OnRequestSettings();
        public void RequestHome() => _fsm.CurrentState?.OnRequestHome();
        public void RequestExit() => _fsm.CurrentState?.OnRequestExit();
        #endregion

        #region ----EXECUTORS-----
        // HACK: using unityengine here, gotta get rid of this dependency somehow
        public void ExecuteExit()
        {
            #if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
        #endregion
    }
}