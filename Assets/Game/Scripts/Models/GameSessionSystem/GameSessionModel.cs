using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PeckNSend.Models
{
    /// <summary>
    /// Represents the player management logic for a multiplayer game session, including joined players, match
    /// statistics, and game start conditions.
    /// Not to be confused with MatchStatsModel which is more focused on the stats of a single match, 
    /// this model is focused on the overall session and player management.
    /// </summary>
    /// <remarks>This model is intended for use in Unity-based games to coordinate player joining, enforce
    /// minimum player requirements, and track match statistics. It provides methods to add or remove players, clear the
    /// player list, and start a new match. The session enforces a minimum number of players before the game can be
    /// started, and exposes properties to observe the current session state. Thread safety is not guaranteed; access
    /// should be managed appropriately in multithreaded scenarios.</remarks>
    public class GameSessionModel : UnityModelBaseClass
    {       
        #region ---- PROPERTIES ----
        private readonly List<JoinedPlayerData> _joinedPlayers = new();
        public IReadOnlyList<JoinedPlayerData> JoinedPlayers => _joinedPlayers;

        public MatchStatsModel MatchStats { get; } = new MatchStatsModel();

        //private int _minimumPlayersToStart = 1;
        //public int MinimumPlayersToStart
        //{
        //    get => _minimumPlayersToStart;
        //    set
        //    {
        //        if (_minimumPlayersToStart == value) return;
        //        _minimumPlayersToStart = value;
        //        EvaluateCanStartGame(); // TODO: remove this evaluate shit cause this should be in joinscreenpresneter
        //        OnPropertyChanged();
        //    }
        //}

        //private bool _canStartGame;
        //public bool CanStartGame
        //{
        //    get => _canStartGame;
        //    private set
        //    {
        //        if (_canStartGame == value) return;
        //        _canStartGame = value;
        //        OnPropertyChanged();
        //    }
        //}
        #endregion

        public void ClearJoinedPlayers()
        {
            _joinedPlayers.Clear();
            //EvaluateCanStartGame(); // TODO: remove this evaluate shit cause this should be in joinscreenpresneter
            OnPropertyChanged(nameof(JoinedPlayers));
        }

        // HACK: using unityengine here is not ideal for model (gameobject, inputdevice), but no time for dis shit now
        public void AddJoinedPlayer(string controllerType, int unityPlayerIndex, InputDevice device, int birdVariantIndex)
        {
            bool alreadyExists = _joinedPlayers.Any(p => p.UnityPlayerIndex == unityPlayerIndex);
            if (alreadyExists) return;

            int playerNumber = _joinedPlayers.Count + 1;
            _joinedPlayers.Add(new JoinedPlayerData(playerNumber, controllerType, unityPlayerIndex, device, birdVariantIndex));

            //EvaluateCanStartGame(); // TODO: remove this evaluate shit cause this should be in joinscreenpresneter
            OnPropertyChanged(nameof(JoinedPlayers));
        }

        public void RemoveJoinedPlayer(int unityPlayerIndex)
        {
            JoinedPlayerData playerToRemove = _joinedPlayers.FirstOrDefault(p => p.UnityPlayerIndex == unityPlayerIndex);
            if (playerToRemove == null) return;

            _joinedPlayers.Remove(playerToRemove);

            // Reassign player numbers while preserving all other data (device, bird prefab)
            for (int i = 0; i < _joinedPlayers.Count; i++)
            {
                JoinedPlayerData current = _joinedPlayers[i];
                _joinedPlayers[i] = new JoinedPlayerData(i + 1, current.ControllerType, current.UnityPlayerIndex, current.Device, current.BirdVariantIndex);
            }

            //EvaluateCanStartGame(); // TODO: remove this evaluate shit cause this should be in joinscreenpresneter
            OnPropertyChanged(nameof(JoinedPlayers));
        }

        public void StartNewMatch()
        {
            MatchStats.ResetFromJoinedPlayers(JoinedPlayers);
        }

        public void RegisterDeliveredMail(int unityPlayerIndex, int amount )
        {
            MatchStats.RegisterDeliveredMail(unityPlayerIndex, amount);
        }

        //private void EvaluateCanStartGame()
        //{
        //    CanStartGame = JoinedPlayers.Count >= MinimumPlayersToStart;
        //}
    }
}