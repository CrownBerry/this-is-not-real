using System;
using UnityEngine;
using UnityEngine.UI;

namespace Player.FSM
{
    public class CarryingStateMachine
    {
        public enum State
        {
            Carrying,
            None
        }

        private State state;
        private PlayerClass owner;

        public CarryingStateMachine(PlayerClass owner, State startingState)
        {
            this.owner = owner;
            state = startingState;
        }

        public void Rotate()
        {
            switch (state)
            {
                case State.Carrying:
                    break;
                case State.None:
                    owner.RotatePlayer();
//                    Debug.Log(string.Format("{0} call Rotate", owner.gameObject.name));
                    break;
                default:
                    break;
            }
        }

        public void Next()
        {
            switch (state)
            {
                case State.None:
                    state = State.Carrying;
                    break;
                default:
                    state = State.None;
                    break;
            }
//            Debug.Log(string.Format("Now carrying state is: {0}", state));
        }

        public string Now()
        {
            return state.ToString();
        }
    }
}