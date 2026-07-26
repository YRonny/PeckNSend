using PeckNSend.Models;
using System.ComponentModel;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PeckNSend.Presenters
{
    public class ResultScreenPresenter : PresenterBaseClass<ResultScreenModel>
    {
        public static ResultScreenPresenter Instance { get; private set; }

        [Header("Bird Images — index matches birdVariantIndex (0 = P1, 1 = P2, ...)")]
        [SerializeField] private Texture2D[] _birdImages = new Texture2D[4];

        [Header("contestant slots (1st ,2nd, 3rd, 4th)")]
        [SerializeField] private GameObject[] _contestantImageObjects = new GameObject[4];
        [SerializeField] private GameObject[] _contestantScoreObjects = new GameObject[4];

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
                Model = new ResultScreenModel();
            }

            //dissable all slots at start, they will be enabled as the results are revealed
            foreach (var obj in _contestantImageObjects)
            {
                obj.SetActive(false);
            }
            foreach (var obj in _contestantScoreObjects)
            {
                obj.SetActive(false);
            }
        }

        private void Start()
        {
            _gameSessionModel = GameSessionPresenter.Instance.Model;
            RefreshSlots();
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
            //if (e.PropertyName == nameof(ResultScreenModel.Results))
            //    RefreshSlots();
        }

        protected override void ModelSetInitialization(ResultScreenModel previousModel)
        {
            //RefreshSlots();
        }

        public void OnReplayPressed()
        {
            SceneManagerPresenter.Instance.Model.RequestPlayScene();
        }

        public void OnMainMenuPressed()
        {
            SceneManagerPresenter.Instance.Model.RequestMainMenuScene();
        }

        private void RefreshSlots()
        {
            //PlayerMatchStatsModel winner = _gameSessionModel.MatchStats.GetWinner();

            PlayerMatchStatsModel[] players = _gameSessionModel.MatchStats.GetOrderedPlayers().ToArray();

            for (int i = 0; i < players.Length; i++)
            {
                PlayerMatchStatsModel player = players[i];
                if (i < _contestantImageObjects.Length)
                {
                    _contestantImageObjects[i].GetComponent<RawImage>().texture = _birdImages[player.BirdVariantIndex];
                    _contestantImageObjects[i].SetActive(true);

                    _contestantScoreObjects[i].GetComponent<TMP_Text>().text = player.DeliveredMailCount.ToString();
                    _contestantScoreObjects[i].SetActive(true);
                }
            }
        }

        public void OnStartButtonPressed() => SceneManagerPresenter.Instance.Model.RequestPlayScene();

        public void OnMenuButtonPressed() => SceneManagerPresenter.Instance.Model.RequestMainMenuScene();

    }
}