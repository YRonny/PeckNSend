using PeckNSend.Models;
using PeckNSend.Presenters;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PeckNSend.Debugging
{
    /// <summary>
    /// Initializes the result scene for direct entry in the Unity Editor, ensuring required presenters and debug data
    /// are set up for testing or development purposes.
    /// </summary>
    /// <remarks>This bootstrapper is intended for use in development environments to facilitate rapid scene

    // OPTIMIZE: Alex vanden abeele suggested
    /*
      [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void StartTheGameAlready()
    {
       var bootstrapperObject = Instantiate(Resources.Load<GameObject>("PlaySceneDirectEntryBootstrapper"));
       bootstrapperObject.GetComponent<PlaySceneDirectEntryBootstrapper>();
       DontDestroyOnLoad(bootstrapperObject);
    }
    */
        [DefaultExecutionOrder(-1000)]
    public class ResultSceneDirectEntryBootstrapper : MonoBehaviour
    {
        [SerializeField] private bool _enableDirectSceneBootstrap = true;
        [SerializeField] private bool _seedDebugResultDataWhenSessionIsEmpty = true;
        [SerializeField] private int _debugPlayerCount = 2;

        [SerializeField] private int _debugPlayer1DeliveredMail = 7;
        [SerializeField] private int _debugPlayer2DeliveredMail = 6;
        [SerializeField] private int _debugPlayer3DeliveredMail = 5;
        [SerializeField] private int _debugPlayer4DeliveredMail = 0;

        private void Awake()
        {
            if (!_enableDirectSceneBootstrap) return;

            EnsurePersistentPresenters();
            EnsureSceneManagerIsInResultState();
            SeedDebugResultDataIfNeeded();
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

        private void EnsureSceneManagerIsInResultState()
        {
            if (SceneManagerPresenter.Instance == null || SceneManagerPresenter.Instance.Model == null)
            {
                Debug.LogError("SceneManagerPresenter or SceneManager model could not be created.");
                return;
            }

            SceneManagerModel sceneManagerModel = SceneManagerPresenter.Instance.Model;

            if (sceneManagerModel.SceneFSM.CurrentState == null)
            {
                sceneManagerModel.SceneFSM.TransitionTo(sceneManagerModel.SceneFSM.ResultState);
                return;
            }

            if (sceneManagerModel.SceneFSM.CurrentState != sceneManagerModel.SceneFSM.ResultState)
            {
                sceneManagerModel.SceneFSM.TransitionTo(sceneManagerModel.SceneFSM.ResultState);
            }
        }

        private void SeedDebugResultDataIfNeeded()
        {
            if (!_seedDebugResultDataWhenSessionIsEmpty) return;

            if (GameSessionPresenter.Instance == null || GameSessionPresenter.Instance.Model == null)
            {
                Debug.LogError("GameSessionPresenter or GameSessionModel could not be created.");
                return;
            }

            GameSessionModel gameSessionModel = GameSessionPresenter.Instance.Model;

            if (gameSessionModel.JoinedPlayers.Count == 0)
            {
                CreateDebugPlayers(gameSessionModel);
            }

            if (gameSessionModel.MatchStats.Players.Count == 0)
            {
                CreateDebugMatchStats(gameSessionModel);
            }
        }

        private void CreateDebugPlayers(GameSessionModel gameSessionModel)
        {
            gameSessionModel.ClearJoinedPlayers();

            int playerCount = Mathf.Clamp(_debugPlayerCount, 1, 4);

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

        private void CreateDebugMatchStats(GameSessionModel gameSessionModel)
        {
            gameSessionModel.StartNewMatch();

            int[] deliveredMailValues =
            {
                _debugPlayer1DeliveredMail,
                _debugPlayer2DeliveredMail,
                _debugPlayer3DeliveredMail,
                _debugPlayer4DeliveredMail
            };

            int playerCount = Mathf.Min(gameSessionModel.JoinedPlayers.Count, deliveredMailValues.Length);

            for (int i = 0; i < playerCount; i++)
            {
                int deliveredMail = Mathf.Max(0, deliveredMailValues[i]);
                if (deliveredMail <= 0) continue;

                int unityPlayerIndex = gameSessionModel.JoinedPlayers[i].UnityPlayerIndex;
                gameSessionModel.RegisterDeliveredMail(unityPlayerIndex, deliveredMail);
            }
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

            Debug.LogWarning($"No input device available for debug player {playerIndex + 1}.");
            return null;
        }
    }
}