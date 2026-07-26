using PeckNSend.Models;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Linq;

namespace PeckNSend.Presenters
{
    public class PlayScreenPresenter : PresenterBaseClass<PlayScreenModel>
    {
        public static PlayScreenPresenter Instance { get; private set; }

        [Header("HUD")]
        [SerializeField] private TMP_Text _countdownText;
        [SerializeField] private TMP_Text _matchTimerText;
        [SerializeField] private GameObject[] _contestantImageObject = new GameObject[4];
        [SerializeField] private GameObject[] _contestantScoreObject = new GameObject[4];
        [SerializeField] private Slider _matchTimerSlider;

        [Header("Pause")]
        [SerializeField] private GameObject _pauseCanvas;

        [Header("Birds")]
        [SerializeField] private GameObject[] _birdPrefabs = new GameObject[4];
        [SerializeField] private Transform[] _spawnPoints;

        [Header("Match Settings")]
        [SerializeField] private float _pregameCountdownSeconds = 3f;
        [SerializeField] private float _matchDurationSeconds = 300f;

        private readonly List<PlayerInput> _spawnedBirdInputs = new();
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
                Model = new PlayScreenModel();
            }
        }

        private void Start()
        {
            _gameSessionModel = GameSessionPresenter.Instance.Model;
            _gameSessionModel.StartNewMatch();
            _gameSessionModel.MatchStats.PropertyChanged += OnMatchStatsChanged; 


            SpawnBirdsForJoinedPlayers();

            Model.RequestBeginMatchFlow(_pregameCountdownSeconds, _matchDurationSeconds);

            RefreshCountdownText();
            RefreshMatchTimerSlider();
            //RefreshPauseCanvas();
            RefreshScores();
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }

            if (_gameSessionModel != null)
                _gameSessionModel.MatchStats.PropertyChanged -= OnMatchStatsChanged; 
        }
        protected override void ModelSetInitialization(PlayScreenModel previousModel)
        {
            RefreshCountdownText();
            RefreshMatchTimerSlider();
            //RefreshPauseCanvas();
        }

        protected override void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PlayScreenModel.CountdownText))
            {
                RefreshCountdownText();
            }
            else if (e.PropertyName == nameof(PlayScreenModel.MatchTimePercentage))
            {
                RefreshMatchTimerSlider();
            }
            //else if (e.PropertyName == nameof(PlayScreenModel.ActiveScreen))
            //{        
            //    RefreshPauseCanvas();
            //}
        }

        private void OnMatchStatsChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MatchStatsModel.Players))
                RefreshScores();
        }

        private void RefreshScores()
        {
            PlayerMatchStatsModel[] players = _gameSessionModel.MatchStats.Players.ToArray();

            for (int i = 0; i < players.Length; i++)
            {
                PlayerMatchStatsModel player = players[i];

                //use the birdvarantindex to determine which bird image to show for the player, and show the score next to it.
                int playerBirdVariantIndex = player.BirdVariantIndex;
                if (playerBirdVariantIndex < _contestantImageObject.Length)
                {
                    _contestantImageObject[playerBirdVariantIndex].SetActive(true);

                    _contestantScoreObject[playerBirdVariantIndex].GetComponent<TMP_Text>().text = player.DeliveredMailCount.ToString();
                    _contestantScoreObject[playerBirdVariantIndex].SetActive(true);
                }
            }
        }

        public void RegisterDeliveredMail(int unityPlayerIndex, int amount = 1)
        {
            _gameSessionModel.RegisterDeliveredMail(unityPlayerIndex, amount);
        }

        public void OnResumeButtonClicked()
        {
            Model.RequestResume();
        }

        public void OnBackToMenuButtonClicked()
        {
            SceneManagerPresenter.Instance.Model.RequestMainMenuScene();
        }

        private void SpawnBirdsForJoinedPlayers()
        {
            IReadOnlyList<JoinedPlayerData> joinedPlayers = _gameSessionModel.JoinedPlayers;

            for (int i = 0; i < joinedPlayers.Count; i++)
            {
                JoinedPlayerData player = joinedPlayers[i];
                Transform spawnPoint = _spawnPoints[i % _spawnPoints.Length];

                PlayerInput playerInput = PlayerInput.Instantiate(
                    prefab: _birdPrefabs[player.BirdVariantIndex],
                    playerIndex: player.UnityPlayerIndex,
                    pairWithDevice: player.Device
                );

                GameObject birdObject = playerInput.gameObject;
                birdObject.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);

                _spawnedBirdInputs.Add(playerInput);

                //Give the bird his identity
                //This is way with sendmessage, but it is shit. keeping it in as a lesson
                //birdObject.SendMessage("AssignJoinedPlayerData", player, SendMessageOptions.DontRequireReceiver); 
                birdObject.GetComponent<PlayerOwnership>().AssignJoinedPlayerData(player);
            }
        }

        private void RefreshCountdownText()
        {
            _countdownText.text = Model.CountdownText;
        }

        private void RefreshMatchTimerSlider()
        {
            _matchTimerSlider.value = 1f - Model.MatchTimePercentage;

        }

        private void RefreshPauseCanvas()
        {
            if (Model.ActiveScreen == PlayScreenModel.PlayScreen.Pause)
                _pauseCanvas.SetActive(true);
            else
                _pauseCanvas.SetActive(false);
        }
    }
}