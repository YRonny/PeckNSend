

namespace PeckNSend.Models
{
    /// <summary>
    /// Represents statistical data for 1 player during a match.
    /// information.
    /// </summary>
    /// <remarks>This class is typically used to track and report individual player performance metrics within
    /// a single match. The score is calculated based on the number of delivered mail items. Instances are intended to
    /// be created per player per match.</remarks>
    public class PlayerMatchStatsModel
    {
        // CONSIDER: instead of adding the values of
        // joinedplayer to this model, we could also just add the joined player data itself and
        // then access the data from there.
        // This would reduce the amount of data we need to pass around and keep the player data more centralized.
        // playernumber, unityplayerindex, controller type and birdvariantindex are all static values that do not change during
        // the match, so we can just reference the joined player data for those instead of copying them here.
        public int UnityPlayerIndex { get; }
        public int PlayerNumber { get; }
        public string ControllerType { get; }
        public int BirdVariantIndex { get; }
        public int DeliveredMailCount { get; private set; }

        public PlayerMatchStatsModel(int unityPlayerIndex, int playerNumber, string controllerType, int birdVariantIndex)
        {
            UnityPlayerIndex = unityPlayerIndex;
            PlayerNumber = playerNumber;
            ControllerType = controllerType;
            BirdVariantIndex = birdVariantIndex;
        }

        public void RegisterDeliveredMail(int amount ) 
        {
            DeliveredMailCount += amount;
        }
    }
}