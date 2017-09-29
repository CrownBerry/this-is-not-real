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


    }
}