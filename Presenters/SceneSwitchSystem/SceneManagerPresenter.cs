using PeckNSend.Models;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PeckNSend.Presenters
{
    public class SceneManagerPresenter : PresenterBaseClass<SceneManagerModel>
    {
        public static SceneManagerPresenter Instance { get; private set; }

        [SerializeField] private bool _autoStartBootFlow = true;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (Model == null)
            {
                Model = new SceneManagerModel();
            }
        }

        private void Start()
        {
            if (!_autoStartBootFlow)
            {
                return;
            }

            if (SceneManager.GetActiveScene().name == "BootScene")
            {
                Model.RequestBoot();
            }
        }

        protected override void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }
    }
}