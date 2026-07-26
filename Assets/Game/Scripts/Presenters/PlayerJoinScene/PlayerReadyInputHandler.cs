using PeckNSend.Presenters;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerReadyInputHandler : MonoBehaviour
{
    private PlayerInput _playerInput;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
    }

    // Wire this to your "Ready" action in the PlayerInput's Unity Events
    public void OnReady()
    {
        PlayerJoinScreenPresenter.Instance.ToggleReadyForPlayer(_playerInput.playerIndex);
    }

    public void OnBack()
    {
        PlayerJoinScreenPresenter.Instance.OnBackToMenuRequested();
    }


}