using PeckNSend.Presenters;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBackInputHandler : MonoBehaviour
{
    [SerializeField] private InputActionReference _backAction;

    private void OnEnable()
    {
        _backAction.action.performed += OnBackPerformed;
        _backAction.action.Enable();
    }

    private void OnDisable()
    {
        _backAction.action.performed -= OnBackPerformed;
        _backAction.action.Disable();
    }

    private void OnBackPerformed(InputAction.CallbackContext context)
    {
        MainMenuScreenPresenter.Instance.OnBackButtonPressed();
    }
}