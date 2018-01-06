using System;
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
                case State.Run:
                case State.Idle:
                    owner.ChangeHorizontalMove(newDirection);
                    if (newDirection == 0)
                    {
                        state = State.Idle;
                        break;
                    }
                    state = State.Run;
                    break;
                case State.Fall:
                case State.Jump:
                case State.Disable:
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
            if (!owner.transgressionState.CanTransgression())
            {
                Debug.Log("Can't transgression cause state");
                return;
            }

            if (owner.IsInside() != null || owner.otherPlayer.IsInside() != null)
            {
                var collider = owner.IsInside() != null ? owner.IsInside() : owner.otherPlayer.IsInside();
                var who = owner.IsInside() != null ? "my" : "other";
                Debug.Log(who);
                Debug.Log(collider.transform.position);
                Debug.DrawLine(owner.transform.position, collider.transform.position);
                return;
            }

            owner.transgressionState.Next();

            switch (state)
            {
                case State.Disable:
                    state = State.Idle;
                    owner.Turn(true);
                    break;
                default:
                    state = State.Disable;
                    owner.Turn(false);
                    ShiftCameraView();
                    break;
            }
        }

        private void ShiftCameraView()
        {
            Debug.Log("Start shift camera");
            switch (owner.gameObject.name == "Player")
            {
                case true:
//                    EventManager.TriggerEvent("OnViewportGoal", 1f);
                    EventManager.TriggerEvent("OnShadowSlide", true);
                    break;
                default:
//                    EventManager.TriggerEvent("OnViewportGoal", 0f);
                    EventManager.TriggerEvent("OnShadowSlide", false);
                    break;
            }
        }

        public string CurrentState()
        {
            return state.ToString();
        }

        public void Interact()
        {
            switch (state)
            {
                case State.Disable:
                    break;
                default:
                    owner.TryInteract();
                    break;
            }
        }

        public void Falling()
        {
            switch (state)
            {
                case State.Disable:
                    break;
                default:
                    state = State.Fall;
                    break;
            }
        }

        public void StopFalling()
        {
            switch (state)
            {
                case State.Fall:
                    state = State.Idle;
                    break;
                default:
                    break;
            }
        }
    }
}