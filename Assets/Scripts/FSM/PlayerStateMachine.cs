using System.Player;
using UnityEngine;

namespace System.FSM
{
    public class PlayerStateMachine
    {
        public enum State
        {
            Idle,
            Run,
            Jump,
            Fall,
            Disable
        }

        private readonly PlayerClass _owner;

        private State _state;

        public PlayerStateMachine(PlayerClass owner, State startingState)
        {
            _owner = owner;
            _state = startingState;
        }

        public void ChangeMoveDirection(float newDirection)
        {
            switch (_state)
            {
                case State.Idle:
                    _owner.ChangeHorizontalMove(newDirection);
                    _state = State.Run;
                    break;
                case State.Run:
                    _owner.ChangeHorizontalMove(newDirection);
                    break;
                case State.Jump:
                    _owner.ChangeHorizontalMove(newDirection);
                    break;
                case State.Fall:
                    _owner.ChangeHorizontalMove(newDirection);
                    break;
                case State.Disable:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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
                case State.Disable:
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
                case State.Disable:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void MoveDisabled(Vector3 transformPosition)
        {
            switch (_state)
            {
                case State.Idle:
                    break;
                case State.Run:
                    break;
                case State.Jump:
                    break;
                case State.Fall:
                    break;
                case State.Disable:
                    _owner.MoveDisabled(transformPosition);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}