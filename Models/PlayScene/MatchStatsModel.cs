using System.Collections.Generic;
using System.Linq;

namespace PeckNSend.Models
{
    /// <summary>
    /// Represents the match statistics for all players in one single match and not further. 
    /// </summary>
    /// <remarks>This model provides access to per-player match statistics and supports operations for
    /// resetting and updating player stats during a match. It is typically used to track and manage player performance
    /// data within a single match context.</remarks>
    public class MatchStatsModel : UnityModelBaseClass
    {
        private readonly List<PlayerMatchStatsModel> _players = new();

        public IReadOnlyList<PlayerMatchStatsModel> Players => _players;

        public void ResetFromJoinedPlayers(IReadOnlyList<JoinedPlayerData> joinedPlayers)
        {
            _players.Clear();

            foreach (JoinedPlayerData joinedPlayer in joinedPlayers)
            {
                _players.Add(new PlayerMatchStatsModel(
                    joinedPlayer.UnityPlayerIndex,
                    joinedPlayer.PlayerNumber,
                    joinedPlayer.ControllerType,
                    joinedPlayer.BirdVariantIndex));
            }

            OnPropertyChanged(nameof(Players));
        }

        public void RegisterDeliveredMail(int unityPlayerIndex, int amount)
        {
            PlayerMatchStatsModel player = _players.FirstOrDefault(p => p.UnityPlayerIndex == unityPlayerIndex);

            player.RegisterDeliveredMail(amount);
            OnPropertyChanged(nameof(Players));
        }

        public PlayerMatchStatsModel GetWinner()
        {
            return _players
                .OrderByDescending(p => p.DeliveredMailCount)
                .ThenByDescending(p => p.DeliveredMailCount)
                .FirstOrDefault();
        }

        public List<PlayerMatchStatsModel> GetOrderedPlayers()
        {
            return _players.OrderByDescending(p => p.DeliveredMailCount).ToList();
        }
    }
}