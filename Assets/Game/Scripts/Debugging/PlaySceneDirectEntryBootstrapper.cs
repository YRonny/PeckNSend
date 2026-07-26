using PeckNSend.Models;
using PeckNSend.Presenters;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace PeckNSend.Debugging
{
    /// <summary>
    /// Initializes and configures the play scene for direct entry in the Unity Editor, ensuring required presenters and
    /// debug players are set up for development and testing purposes.
    /// </summary>
    /// <remarks>This bootstrapper is intended for use in development environments to facilitate rapid scene
    /// testing without requiring a full application startup sequence. It ensures that persistent presenters and scene
    /// state are established, and can optionally seed debug players if the session is empty. This behavior is
    /// controlled by serialized fields and is typically enabled only in editor or debug builds.</remarks>
    [DefaultExecutionOrder(-1000)] // Ensure this runs early in the scene lifecycle
    public class PlaySceneDirectEntryBootstrapper : MonoBehaviour
    {
        [Tooltip("Set to true to enable direct scene bootstrapping when the scene is loaded, which will ensure that necessary presenters are created and the scene manager is in the correct state for playtesting.")]
        [SerializeField] private bool _enableDirectSceneBootstrap = true;
        [Tooltip("Set to true to automatically add debug players if no players are currently joined in the session.\r\n")]
        [SerializeField] private bool _seedDebugPlayersWhenSessionIsEmpty = true;
        [SerializeField] private int _debugPlayerCount = 1;

        private void Awake()
        {
            if (!_enableDirectSceneBootstrap)
            {
                return;
            }

            EnsurePersistentPresenters();
            EnsureSceneManagerIsInPlayState();
            SeedDebugPlayersIfNeeded();
        }

        private void EnsurePersistentPresenters()
        {
            GameObject appRoot = GameObject.Find("__AppRoot");

            if (appRoot == null)
            {
                appRoot = new GameObject("__AppRoot");
            }

            if (SceneManagerPresenter.Instance == null)
            {
                appRoot.AddComponent<SceneManagerPresenter>();
            }

            if (GameSessionPresenter.Instance == null)
            {
                appRoot.AddComponent<GameSessionPresenter>();
            }
        }

        private void EnsureSceneManagerIsInPlayState()
        {
            if (SceneManagerPresenter.Instance == null || SceneManagerPresenter.Instance.Model == null)
            {
                Debug.LogError("SceneManagerPresenter or SceneManager model could not be created.");
                return;
            }

            SceneManagerModel sceneManagerModel = SceneManagerPresenter.Instance.Model;

            if (sceneManagerModel.SceneFSM.CurrentState == null)
            {
                sceneManagerModel.SceneFSM.TransitionTo(sceneManagerModel.SceneFSM.PlayState);
            }
        }

        private void SeedDebugPlayersIfNeeded()
        {
            if (!_seedDebugPlayersWhenSessionIsEmpty) return;

            if (GameSessionPresenter.Instance == null || GameSessionPresenter.Instance.Model == null)
            {
                Debug.LogError("GameSessionPresenter or GameSessionModel could not be created.");
                return;
            }

            GameSessionModel gameSessionModel = GameSessionPresenter.Instance.Model;

            if (gameSessionModel.JoinedPlayers.Count > 0) return;

            gameSessionModel.ClearJoinedPlayers();

            int playerCount = Mathf.Max(1, _debugPlayerCount);

            for (int i = 0; i < playerCount; i++)
            {
                InputDevice device = GetDebugDevice(i);
                string controllerType = device != null ? device.displayName : $"Debug Controller {i + 1}";
                int birdVariantIndex = GetDebugBirdIndex(i);
                gameSessionModel.AddJoinedPlayer(controllerType, i, device, birdVariantIndex);
            }
        }

        private int GetDebugBirdIndex(int i)
        {
            // Return a bird variant index based on the player index (0-3)
            return i % _debugPlayerCount; // Assuming there are 4 bird variants (0, 1, 2, 3)
        }

        /// <summary>
        /// Returns a real input device for debug players.
        /// Player 0 gets the keyboard, subsequent players get gamepads if available.
        /// </summary>
        private InputDevice GetDebugDevice(int playerIndex)
        {
            if (playerIndex == 0)
            {
                return Keyboard.current;
            }

            int gamepadIndex = playerIndex - 1;
            if (gamepadIndex < Gamepad.all.Count)
            {
                return Gamepad.all[gamepadIndex];
            }

            // Fallback: no device available for this slot
            Debug.LogWarning($"No input device available for debug player {playerIndex + 1}.");
            return null;
        }
    }
}