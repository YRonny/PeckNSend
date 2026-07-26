using UnityEngine;

namespace PeckNSend.FSM
{
    public interface IState
    {
        public void OnEnter();
        public void OnExit();
        public void FixedUpdate(float fixedDeltaTime);
    }
}

