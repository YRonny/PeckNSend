using UnityEngine;
using UnityEngine.InputSystem;

namespace PeckNSend.Models
{
    /// <summary>
    /// Represents information about a player who has joined, including their player number, controller type,
    /// Unity player index, and the paired input device.
    /// </summary>
    /// 

    //CONSIDER: renaming to model
    public class JoinedPlayerData
    {
        public int PlayerNumber { get; }
        public string ControllerType { get; }
        public int UnityPlayerIndex { get; }
        public InputDevice Device { get; }
        public int BirdVariantIndex { get; }


        public JoinedPlayerData(int playerNumber, string controllerType, int unityPlayerIndex, InputDevice device, int birdVariantIndex)
        {
            PlayerNumber = playerNumber;
            ControllerType = controllerType;
            UnityPlayerIndex = unityPlayerIndex;
            Device = device;
            BirdVariantIndex = birdVariantIndex;
        }
    }
}