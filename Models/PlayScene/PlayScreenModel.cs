using PeckNSend.Presenters;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace PeckNSend.Models
{
    public partial class PlayScreenModel : UnityModelBaseClass
    {
        private PlayScreenFSM _fsm;

        private float _matchDuration;

        #region ------PROPERTIES------

        private float _matchTimeRemaining;
        public float MatchTimeRemaining
        {
            get => _matchTimeRemaining;
            private set
            {
                _matchTimeRemaining = value;
                OnPropertyChanged();
                MatchTimePercentage = _matchDuration > 0 ? _matchTimeRemaining / _matchDuration : 0f;
            }
        }

        private float _matchTimePercentage;
        public float MatchTimePercentage
        {
            get => _matchTimePercentage;
            private set
            {
                if (Mathf.Approximately(_matchTimePercentage, value)) return;
                _matchTimePercentage = value;
                OnPropertyChanged();
            }
        }

        private float _pregameCountdownRemaining;
        public float PregameCountdownRemaining
        {
            get { return _pregameCountdownRemaining; }
            private set
            {
                _pregameCountdownRemaining = value;
                OnPropertyChanged();
            }
        }

        private string _countdownText = string.Empty;
        public string CountdownText
        {
            get => _countdownText;
            private set
            {
                if (_countdownText == value) return;
                _countdownText = value;
                OnPropertyChanged();
            }
        }

        private string _matchTimeText = "00:00";
        public string MatchTimeText
        {
            get => _matchTimeText;
            private set
            {
                if (_matchTimeText == value) return;
                _matchTimeText = value;
                OnPropertyChanged();
            }
        }

        //CONSIDER: h        
        public enum PlayScreen { Play, Pause, CountDown, MatchFinished }

        private PlayScreen _activeScreen;
        public PlayScreen ActiveScreen
        {
            get => _activeScreen;
            set 
            {
                if (_activeScreen == value) return;
                _activeScreen = value; 
                OnPropertyChanged(); 
            }
        }
        #endregion

        public PlayScreenModel()
        {
            _fsm = new PlayScreenFSM(this);
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

        #region -----REQUESTS-----
        public void RequestBeginMatchFlow(float pregameCountdownSeconds, float matchDurationSeconds)
        {
            _matchDuration = matchDurationSeconds;
            _pregameCountdownRemaining = pregameCountdownSeconds;
            _matchTimeRemaining = matchDurationSeconds;
            MatchTimePercentage = 1f; // full at the start
            _fsm.TransitionTo(_fsm.CountDownState);
        }

        public void RequestPause()
        {
            _fsm.CurrentState?.OnRequestPause();
        }

        public void RequestResume()
        {
            _fsm.CurrentState?.OnRequestResume();
        }
        #endregion

        #region -----EXECUTES-----
        private void UpdateCountdownText()
        {
            int currentNumber = Mathf.CeilToInt(PregameCountdownRemaining);
            if (currentNumber < 0) currentNumber = 0;
            CountdownText = currentNumber.ToString();
        }

        private void UpdateMatchTimeText()
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(MatchTimeRemaining);
            MatchTimeText = $"{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
        }

        private void HandleMatchFinished()
        {
            SceneManagerPresenter.Instance.Model.RequestResultScene();
        }
        #endregion
    }
}
