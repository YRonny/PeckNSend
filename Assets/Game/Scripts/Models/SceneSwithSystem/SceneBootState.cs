using PeckNSend.Presenters;
using System;

namespace PeckNSend.Models
{
    public partial class SceneManagerModel
    {
        public class SceneBootState : SceneBaseState
        {
            private BootScreenPresenter _bootPresenter;

            public SceneBootState(SceneManagerFSM fsm) : base(fsm)
            {
            }

            public override void OnEnter()
            {
                _bootPresenter = BootScreenPresenter.Instance;

                _bootPresenter.Model.BeginCountdown(3f);
            }

            public override void OnExit()
            {
                if (_bootPresenter != null && _bootPresenter.Model != null)
                {
                    _bootPresenter.Model.Clear();
                }
            }

            public override void FixedUpdate(float fixedDeltaTime)
            {
                if (_bootPresenter.Model.IsFinished)
                {
                    SceneFSM.TransitionTo(SceneFSM.MainMenuState);
                }
            }

            public override string ToString()
            {
                return "BootState";
            }
        }
    }
}