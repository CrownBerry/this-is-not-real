using System;
using UnityEngine;

namespace Player.FSM
{
    public class TransgressionStateMachine
    {
        public enum State
        {
            InTransgressionProcess,
            None
        }

        private State state;
        private PlayerClass owner;

        public TransgressionStateMachine(PlayerClass owner)
        {
            this.owner = owner;
            state = State.None;
        }


        public bool CanTransgression()
        {
            return state == State.None;
        }

        public void Next()
        {
            switch (state)
            {
                case State.InTransgressionProcess:
                    state = State.None;
                    break;
                case State.None:
                    state = State.InTransgressionProcess;
                    break;
                default:
                    state = State.None;
                    break;
            }
        }
    }
}