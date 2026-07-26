using PeckNSend.Models;
using System.ComponentModel;
using UnityEngine;

namespace PeckNSend.Presenters
{
    public class GameSessionPresenter : PresenterBaseClass<GameSessionModel>
    {
        public static GameSessionPresenter Instance { get; private set; }

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
                Model = new GameSessionModel();
            }
        }

        private void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        protected override void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }
    }
}