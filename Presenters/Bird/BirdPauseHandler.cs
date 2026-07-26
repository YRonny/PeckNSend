using UnityEngine;
using UnityEngine.InputSystem;

namespace PeckNSend.Presenters
{
    public class BirdPauseHandler : MonoBehaviour
    {
        // Called automatically by Player Input (Send Messages behavior)
        public void OnPause(InputValue value)
        {
            if (!value.isPressed)
            {
                return;
            }

            PlayScreenPresenter.Instance.Model.RequestPause();

        }
    }
}