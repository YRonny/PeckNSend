using System;

namespace PeckNSend.FSM
{
    public abstract class FiniteStateMachine
    {

        public event EventHandler CurrentStateChanged;
        private IState _currentState;
        public IState CurrentState
        {
            get { return _currentState; }
            set
            {
                if (_currentState == value) return;
                _currentState = value;
                OnStateChanged();
            }
        }
        protected virtual void OnStateChanged()
        {
            CurrentStateChanged?.Invoke(this, EventArgs.Empty);
        }
        public virtual void FixedUpdate(float fixedDeltaTime)
        {
            CurrentState.FixedUpdate(fixedDeltaTime);
        }
        public virtual void TransitionTo(IState newState)
        {
            if (newState == null) return;
            if (newState == CurrentState) return;
            if (CurrentState != null) CurrentState.OnExit();
            CurrentState = newState;
            CurrentState.OnEnter();
        }
    }
}

