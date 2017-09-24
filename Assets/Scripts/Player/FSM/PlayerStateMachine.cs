using UnityEngine;

namespace Player.FSM
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

        private readonly PlayerClass owner;

        private State state;

        public PlayerStateMachine(PlayerClass owner, State startingState)
        {
            this.owner = owner;
            state = startingState;
        }

        public void ChangeMoveDirection(float newDirection)
        {
            switch (state)
            {
                case State.Idle:
                    owner.ChangeHorizontalMove(newDirection);
                    state = State.Run;
                    break;
                case State.Disable:
                    break;
                default:
                    owner.ChangeHorizontalMove(newDirection);
                    break;
            }
        }

        public void Jump()
        {
            switch (state)
            {
                case State.Idle:
                    owner.Jump();
                    state = State.Jump;
                    break;
                case State.Run:
                    owner.Jump();
                    state = State.Jump;
                    break;
                default:
                    break;
            }
        }

        public void Landing()
        {
            switch (state)
            {
                case State.Jump:
                    state = State.Idle;
                    break;
                case State.Fall:
                    state = State.Idle;
                    break;
                case State.Disable:
                    break;
                default:
                    break;
            }
        }

        public void MoveDisabled()
        {
            switch (state)
            {
                case State.Disable:
                    owner.MoveDisabled();
                    break;
                default:
                    break;
            }
        }

        public void MoveCamera(Transform camera)
        {
            switch (state)
            {
                case State.Disable:
                    break;
                default:
                    var ownerPosition = owner.transform.position;
                    camera.position = Vector3.Lerp(camera.position,
                        new Vector3(ownerPosition.x, ownerPosition.y + 3, -7), Time.deltaTime * 4.0f);
                    break;
            }
        }

        public void Transgression()
        {
            if (owner.IsInside() != null || owner.otherPlayer.IsInside() != null)
            {
                var collider = owner.IsInside() != null ? owner.IsInside() : owner.otherPlayer.IsInside();
                var who = owner.IsInside() != null ? "my" : "other";
                Debug.Log(who);
                Debug.Log(collider.transform.position);
                Debug.DrawLine(owner.transform.position, collider.transform.position);
                return;
            }

            switch (state)
            {
                case State.Disable:
                    state = State.Idle;
                    owner.CameraFollowMe();
                    owner.Turn(true);
                    break;
                default:
                    state = State.Disable;
                    owner.Turn(false);
                    break;
            }
        }
    }
}