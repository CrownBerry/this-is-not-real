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
        }
    }
}