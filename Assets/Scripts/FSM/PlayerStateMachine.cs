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
                case State.Disable:
                    break;
                default:
                    _owner.ChangeHorizontalMove(newDirection);
                    break;
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
                default:
                    break;
            }
        }

        public void Landing()
        {
            switch (_state)
            {
                case State.Jump:
                    _state = State.Idle;
                    break;
                case State.Fall:
                    _state = State.Idle;
                    break;
                case State.Disable:
                    break;
                default:
                    break;
            }
        }

        public void MoveDisabled(Vector3 transformPosition)
        {
            switch (_state)
            {
                case State.Disable:
                    _owner.MoveDisabled(transformPosition);
                    break;
                default:
                    break;
            }
        }

        public void MoveCamera(Transform camera)
        {
            switch (_state)
            {
                case State.Disable:
                    break;
                default:
                    var ownerPosition = _owner.transform.position;
                    camera.position = Vector3.Lerp(camera.position,
                        new Vector3(ownerPosition.x, ownerPosition.y + 3, -7), Time.deltaTime * 4.0f);
                    break;
            }
        }

        public void Transgression()
        {
            if (_owner.IsInside() != null || _owner.otherPlayer.IsInside() != null)
            {
                var _collider = _owner.IsInside() != null ? _owner.IsInside() : _owner.otherPlayer.IsInside();
                var who = _owner.IsInside() != null ? "my" : "other";
                Debug.Log(who);
                Debug.Log(_collider.transform.position);
                return;
            }

            switch (_state)
            {
                case State.Disable:
                    _state = State.Idle;
                    _owner.CameraFollowMe();
                    _owner.Turn(true);
                    break;
                default:
                    _state = State.Disable;
                    _owner.Turn(false);
                    break;
            }
        }
    }
}