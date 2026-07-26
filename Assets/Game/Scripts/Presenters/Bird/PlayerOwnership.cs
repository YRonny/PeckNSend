using PeckNSend.Models;
using UnityEngine;

namespace PeckNSend.Presenters
{
    public class PlayerOwnership : MonoBehaviour
    {
        public int UnityPlayerIndex { get; private set; } = -1;
        public int PlayerNumber { get; private set; } = -1;
        public string ControllerType { get; private set; } = string.Empty;

        public void AssignJoinedPlayerData(JoinedPlayerData joinedPlayerData)
        {
            UnityPlayerIndex = joinedPlayerData.UnityPlayerIndex;
            PlayerNumber = joinedPlayerData.PlayerNumber;
            ControllerType = joinedPlayerData.ControllerType;
        }

        public void AssignUnityPlayerIndex(int unityPlayerIndex)
        {
            UnityPlayerIndex = unityPlayerIndex;
        }
    }
}