using PeckNSend.Models;
using PeckNSend.Presenters;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PeckNSend.Debugging
{

    [DefaultExecutionOrder(-1000)]
    public class PlayerJoinSceneDirectEntryBootstrapper : MonoBehaviour
    {
        [SerializeField] private bool _enableDirectSceneBootstrap = true;
        [SerializeField] private bool _clearJoinedPlayersOnDirectEntry = true;
        //[SerializeField] private bool _seedDebugPlayersWhenSessionIsEmpty = false;
        //[SerializeField] private int _debugPlayerCount = 2;

        private void Awake()
        {
            if (!_enableDirectSceneBootstrap)
            {
                return;
            }

            EnsurePersistentPresenters();
            EnsureSceneManagerIsInPlayerJoinState();
            PreparePlayerJoinSession();
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

        private void EnsureSceneManagerIsInPlayerJoinState()
        {
            if (SceneManagerPresenter.Instance == null || SceneManagerPresenter.Instance.Model == null)
            {
                Debug.LogError("SceneManagerPresenter or SceneManager model could not be created.");
                return;
            }

            SceneManagerModel sceneManagerModel = SceneManagerPresenter.Instance.Model;

            if (sceneManagerModel.SceneFSM.CurrentState == null)
            {
                sceneManagerModel.SceneFSM.TransitionTo(sceneManagerModel.SceneFSM.PlayerJoinState);
                return;
            }

            if (sceneManagerModel.SceneFSM.CurrentState != sceneManagerModel.SceneFSM.PlayerJoinState)
            {
                sceneManagerModel.SceneFSM.TransitionTo(sceneManagerModel.SceneFSM.PlayerJoinState);
            }
        }

        private void PreparePlayerJoinSession()
        {
            if (GameSessionPresenter.Instance == null || GameSessionPresenter.Instance.Model == null)
            {
                Debug.LogError("GameSessionPresenter or GameSessionModel could not be created.");
                return;
            }

            GameSessionModel gameSessionModel = GameSessionPresenter.Instance.Model;

            if (_clearJoinedPlayersOnDirectEntry)
            {
                gameSessionModel.ClearJoinedPlayers();
            }

            //if (_seedDebugPlayersWhenSessionIsEmpty && gameSessionModel.JoinedPlayers.Count == 0)
            //{
            //    CreateDebugPlayers(gameSessionModel);
            //}
        }

        //private void CreateDebugPlayers(GameSessionModel gameSessionModel)
        //{
        //    int playerCount = Mathf.Clamp(_debugPlayerCount, 1, 4);

        //    for (int i = 0; i < playerCount; i++)
        //    {
        //        InputDevice device = GetDebugDevice(i);
        //        string controllerType = device != null ? device.displayName : $"Debug Controller {i + 1}";

        //        gameSessionModel.AddJoinedPlayer(controllerType, i, device);
        //    }
        //}

        //private InputDevice GetDebugDevice(int playerIndex)
        //{
        //    if (playerIndex == 0 && Keyboard.current != null)
        //    {
        //        return Keyboard.current;
        //    }

        //    int gamepadIndex = playerIndex - 1;
        //    if (gamepadIndex >= 0 && gamepadIndex < Gamepad.all.Count)
        //    {
        //        return Gamepad.all[gamepadIndex];
        //    }

        //    return null;
        //}
    }
}