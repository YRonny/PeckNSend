using PeckNSend.Models;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace PeckNSend.Presenters
{
    public class PlayerJoinScreenPresenter : PresenterBaseClass<PlayerJoinScreenModel>
    {
        [Serializable]
        public class PlayerSlotUI
        {
            public GameObject PlayerSlotObject;
            public Texture2D ConnectedVisual;
            public Texture2D DisconnectedVisual;
            public Texture2D ReadyVisual;
        }

        public static PlayerJoinScreenPresenter Instance { get; private set; }

        [Header("UI")]
        [SerializeField] private Button _startButton;
        [SerializeField] private GameObject _fallbackFocusObject;

        [Header("Settings")]
        [SerializeField] private int _minimumPlayersToStart = 1;

        [Header("Player Slot UI (P1, P2, P3, P4)")]
        [SerializeField] private PlayerSlotUI[] _playerSlots = new PlayerSlotUI[4];

        private GameSessionModel _gameSessionModel;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            if (Model == null)
            {
                Model = new PlayerJoinScreenModel();
            }
        }

        private void Start()
        {
            _gameSessionModel = GameSessionPresenter.Instance.Model;
            _gameSessionModel.ClearJoinedPlayers();

            Model.MinimumPlayersToStart = _minimumPlayersToStart;
            Model.ClearReadyStates();


        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        protected override void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PlayerJoinScreenModel.CanStartGame))
                RefreshStartButton();

            if (e.PropertyName == nameof(PlayerJoinScreenModel.PlayerReadyStates))
                RefreshAllSlots();
        }

        protected override void ModelSetInitialization(PlayerJoinScreenModel previousModel)
        {
            RefreshStartButton();
        }

        public void OnPlayerJoined(PlayerInput playerInput)
        {
            InputDevice device = playerInput.devices.Count > 0 ? playerInput.devices[0] : null;
            string controllerType = GetControllerType(playerInput);

            int birdVariantIndex = _gameSessionModel.JoinedPlayers.Count;
            _gameSessionModel.AddJoinedPlayer(controllerType, playerInput.playerIndex, device, birdVariantIndex);

            Model.AddPlayer(playerInput.playerIndex, false); // TODO: do this with event from game session model instead of manually adding here
        }

        // FIXME: This method is currently not used, but it may be needed in the future if we want to handle player leaving events.
        public void OnPlayerLeft(PlayerInput playerInput)
        {
            //watch out with this 
            //when you switch scenes it may destroy the playerinput calling this and then it will try to remove a player
            //then in the playscene your left without a player

            //_gameSessionModel.RemoveJoinedPlayer(playerInput.playerIndex);

            //Model.RemovePlayerReadyState(playerInput.playerIndex);

            //RefreshFromGameSession();
        }

        public void ToggleReadyForPlayer(int unityPlayerIndex)
        {
            Model.TogglePlayerReady(unityPlayerIndex);
            RefreshSlot(unityPlayerIndex);
        }

        public void OnStartGamePressed()
        {
            SceneManagerPresenter.Instance.Model.RequestPlayScene();
        }

        public void OnBackToMenuRequested()
        {
            SceneManagerPresenter.Instance.Model.RequestMainMenuScene();
        }

        private string GetControllerType(PlayerInput playerInput)
        {
            if (playerInput.devices.Count > 0)
            {
                return playerInput.devices[0].displayName;
            }

            if (!string.IsNullOrWhiteSpace(playerInput.currentControlScheme))
            {
                return playerInput.currentControlScheme;
            }

            return "Unknown Controller";
        }

        /// <summary>
        /// Refreshes the visual for a single player slot based on current join/ready state.
        /// playerIndex is 0-based (Unity player index).
        /// </summary>
        private void RefreshSlot(int unityPlayerIndex)
        {
            if (unityPlayerIndex < 0 || unityPlayerIndex >= _playerSlots.Length) return;

            var slot = _playerSlots[unityPlayerIndex];
            if (slot == null) return;

            bool isJoined = _gameSessionModel.JoinedPlayers
                .Any(p => p.UnityPlayerIndex == unityPlayerIndex); // check if actually joined

            RawImage slotRawImage = slot.PlayerSlotObject.GetComponent<RawImage>();
            if (!isJoined)
            {
                slotRawImage.texture = slot.DisconnectedVisual;
                return;
            }

            if (Model.IsPlayerReady(unityPlayerIndex))
                slotRawImage.texture = slot.ReadyVisual;
            else
                slotRawImage.texture = slot.ConnectedVisual;
        }

        private void RefreshAllSlots()
        {
            for (int i = 0; i < _playerSlots.Length; i++)
                RefreshSlot(i);
        }

        private void RefreshStartButton()
        {
            _startButton.interactable = Model.CanStartGame;

            if (!Model.CanStartGame)
            {
                // Prevent EventSystem from dropping into the void
                UnityEngine.EventSystems.EventSystem.current
                    .SetSelectedGameObject(_fallbackFocusObject);
            }
        }
    }
}