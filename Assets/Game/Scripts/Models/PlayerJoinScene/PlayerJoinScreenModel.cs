using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace PeckNSend.Models
{
    public class PlayerJoinScreenModel : UnityModelBaseClass
    {

        #region ----PROPERTIES----
        private readonly Dictionary<int, bool> _playerReadyStates = new();
        public IReadOnlyDictionary<int, bool> PlayerReadyStates => _playerReadyStates;

        private bool _canStartGame;
        public bool CanStartGame
        {
            get => _canStartGame;
            private set
            {
                if (_canStartGame == value)
                {
                    return;
                }

                _canStartGame = value;
                OnPropertyChanged();
            }
        }

        private int _minimumPlayersToStart;
        public int MinimumPlayersToStart
        {
            get => _minimumPlayersToStart;
            set
            {
                if (_minimumPlayersToStart == value)
                {
                    return;
                }

                _minimumPlayersToStart = value;
                OnPropertyChanged();
            }
        }
        #endregion

        // TODO: check
        public PlayerJoinScreenModel()
        {
            PropertyChanged += OnModelPropertyChanged;
        }

        public void OnDestroy()
        {
            PropertyChanged -= OnModelPropertyChanged;
        }

        private void OnModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PlayerReadyStates) ||
                e.PropertyName == nameof(MinimumPlayersToStart))
            {
                EvaluateCanStartGame();
            }
        }

        public void ClearReadyStates()
        {
            _playerReadyStates.Clear();
            OnPropertyChanged(nameof(PlayerReadyStates));
        }

        public void AddPlayer(int unityPlayerIndex, bool isReady)
        {
            _playerReadyStates[unityPlayerIndex] = isReady;
            OnPropertyChanged(nameof(PlayerReadyStates));
        }

        public void RemovePlayer(int unityPlayerIndex)
        {
            _playerReadyStates.Remove(unityPlayerIndex);
            OnPropertyChanged(nameof(PlayerReadyStates));
        }

        public void TogglePlayerReady(int unityPlayerIndex)
        {
            bool currentReadyState = IsPlayerReady(unityPlayerIndex);
            _playerReadyStates[unityPlayerIndex] = !currentReadyState;
            OnPropertyChanged(nameof(PlayerReadyStates));
        }

        public bool IsPlayerReady(int unityPlayerIndex)
        {
            if (_playerReadyStates.TryGetValue(unityPlayerIndex, out bool isReady))
            {
                return isReady;
            }

            return false;
        }

        public void EvaluateCanStartGame()
        {
            if (_playerReadyStates.Count >= MinimumPlayersToStart && _playerReadyStates.Values.All(isReady => isReady))
            {
                CanStartGame = true;
            }
            else
            {
                CanStartGame = false;
            }
        }
    }
}