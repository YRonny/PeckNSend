//using UnityEngine;
using PeckNSend.FSM;
using System.Collections.Generic;
using System.Linq;

namespace PeckNSend.Models
{
    public partial class SceneManagerModel
    {
        //states decide what
        //the context decides how (state triggers the action, model performs it).
        public abstract class SceneBaseState : IState
        {            
            public abstract void OnEnter();
            public abstract void OnExit();
            public abstract void FixedUpdate(float fixedDeltaTime);

            public SceneManagerFSM SceneFSM { get; private set; } // References the STATE MACHINE (watch out with naming it the same as the script)
            public SceneManagerModel Context { get => SceneFSM.Context; } // References the model
            public SceneBaseState(SceneManagerFSM fsm)
            {
                SceneFSM = fsm;
            }

            // nothing happens when these are called in the base state, but they can be overridden in the child states to trigger scene changes when buttons are pressed.
            public virtual void OnRequestMainMenuScene() { }
            public virtual void OnRequestPlayerJoinScene() { }
            public virtual void OnRequestPlayScene() { }
            public virtual void OnRequestResultScene() { }
            public virtual void OnRequestQuit() { }
        }
    }
}
