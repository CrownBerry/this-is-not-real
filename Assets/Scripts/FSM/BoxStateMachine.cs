using System;
using System.Objects;
using UnityEngine;

namespace Player.FSM
{
    public class BoxStateMachine
    {
        public enum State
        {
            Grabbed,
            None
        }

        private State state;
        private readonly BoxObject owner;

        public BoxStateMachine(BoxObject owner, State startingState)
        {
            this.owner = owner;
            state = startingState;
        }

        public State GetState()
        {
            return state;
        }

        public void Switch()
        {
            switch (state)
            {
                case State.None:
                    state = State.Grabbed;
                    break;
                default:
                    state = State.None;
                    break;
            }
        }

        public void Drop()
        {
            state = State.None;
        }

        public void Move(Vector3 pullDir, float pullF)
        {
            switch (state)
            {
                case State.Grabbed:
                    owner.Move(pullDir, pullF);
                    break;
                default:
                    break;
            }
        }
    }
}