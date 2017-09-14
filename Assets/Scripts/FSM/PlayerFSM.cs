using System.Player;

namespace System.FSM
{
    public class PlayerFSM
    {
        public enum State
        {
            Idle,
            Run,
            Jump,
            Fall
        }

        private readonly PlayerObject _owner;

        private State _state;

        public PlayerFSM(PlayerObject owner)
        {
            _owner = owner;
            _state = State.Idle;
        }

        public void Jump()
        {
            switch (_state)
            {
                case State.Idle:
                    _owner.Jump();
                    _state = State.Jump;
                    break;
                case State.Run:
                    _owner.Jump();
                    _state = State.Jump;
                    break;
                case State.Jump:
                    break;
                case State.Fall:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Landing()
        {
            switch (_state)
            {
                case State.Idle:
                    break;
                case State.Run:
                    break;
                case State.Jump:
                    _state = State.Idle;
                    break;
                case State.Fall:
                    _state = State.Idle;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}