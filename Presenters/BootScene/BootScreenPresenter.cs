using PeckNSend.Models;
using System.ComponentModel;
using TMPro;
using UnityEngine;

namespace PeckNSend.Presenters
{
    public class BootScreenPresenter : PresenterBaseClass<BootScreenModel>
    {
        public static BootScreenPresenter Instance { get; private set; }

        [SerializeField] private TMP_Text _countdownText;

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
                Model = new BootScreenModel();
            }
        }

        private void Start()
        {
            RefreshCountdownText();
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
            if (e.PropertyName == nameof(BootScreenModel.CountdownText))
            {
                RefreshCountdownText();
            }
        }

        protected override void ModelSetInitialization(BootScreenModel previousModel)
        {
            RefreshCountdownText();
        }

        private void RefreshCountdownText()
        {
            if (_countdownText == null || Model == null)
            {
                return;
            }

            _countdownText.text = Model.CountdownText;
        }
    }
}