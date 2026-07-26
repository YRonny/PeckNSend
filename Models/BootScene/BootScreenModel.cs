using System;

namespace PeckNSend.Models
{
    public class BootScreenModel : UnityModelBaseClass
    {
        private string _countdownText = string.Empty;
        private bool _isFinished;
        private float _countdownRemaining;
        private bool _isRunning;

        public string CountdownText
        {
            get => _countdownText;
            private set
            {
                if (_countdownText == value)
                {
                    return;
                }

                _countdownText = value;
                OnPropertyChanged();
            }
        }

        public bool IsFinished
        {
            get => _isFinished;
            private set
            {
                if (_isFinished == value)
                {
                    return;
                }

                _isFinished = value;
                OnPropertyChanged();
            }
        }

        public void BeginCountdown(float startSeconds)
        {
            _countdownRemaining = startSeconds;
            _isRunning = true;
            IsFinished = false;

            UpdateCountdownText();
        }

        public void Clear()
        {
            _isRunning = false;
            _countdownRemaining = 0f;
            CountdownText = string.Empty;
            IsFinished = false;
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            if (!_isRunning || IsFinished)
            {
                return;
            }

            _countdownRemaining -= deltaTime;

            if (_countdownRemaining <= 0f)
            {
                _countdownRemaining = 0f;
                _isRunning = false;
                CountdownText = string.Empty;
                IsFinished = true;
                return;
            }

            UpdateCountdownText();
        }

        private void UpdateCountdownText()
        {
            int currentNumber = (int)MathF.Ceiling(_countdownRemaining);
            if (currentNumber < 0)
            {
                currentNumber = 0;
            }

            CountdownText = currentNumber.ToString();
        }
    }
}