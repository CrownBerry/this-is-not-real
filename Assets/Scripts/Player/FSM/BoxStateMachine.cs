using System;

namespace Player.FSM
{
    public class BoxStateMachine
    {
        public enum State
        {
            Grab,
            None
        }

        private State state;
        private PlayerClass owner;

        public BoxStateMachine(PlayerClass owner, State startingState)
        {
            this.owner = owner;
            state = startingState;
        }

        public void Grab()
        {
            switch (state)
            {
                case State.Grab:
                    break;
                case State.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}