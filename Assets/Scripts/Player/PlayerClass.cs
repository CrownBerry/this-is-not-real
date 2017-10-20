using System;
using System.Collections;
using Interfaces;
using JetBrains.Annotations;
using Player.FSM;
using Puzzles;
using UnityEngine;

namespace Player
{
    public class PlayerClass : MonoBehaviour
    {
        private float MaxSpeed = 5.0f;
        private float JumpSpeed = 6.0f;
        public float horizontalMoving;
        public bool lookRight;
        private float movingSpeed;
        public PlayerClass otherPlayer;

        public bool isInside;
        private Collider otherCollider;

        private Vector3 constantDeltaPosition;

        private new CapsuleCollider collider;
        private new Transform camera;
        private Rigidbody rigidBody;
        public PlayerStateMachine.State startingState;
        public PlayerStateMachine state;
        public CarryingStateMachine carryingState;
        public TransgressionStateMachine transgressionState;

        private IInteractable interactable;

        private void Awake()
        {
            carryingState = new CarryingStateMachine(this, CarryingStateMachine.State.None);
            transgressionState = new TransgressionStateMachine(this);
            state = new PlayerStateMachine(this, startingState);

            switch (gameObject.name == "Player")
            {
                case true:
                    camera = GameObject.Find("Main Camera").transform;
                    break;
                default:
                    camera = GameObject.Find("Second Camera").transform;
                    break;
            }

            collider = GetComponent<CapsuleCollider>();
            rigidBody = GetComponent<Rigidbody>();

            var players = GameObject.FindGameObjectsWithTag("Player");
            foreach (var player in players)
            {
                var playerScript = player.GetComponent<PlayerClass>();
                if (playerScript.Equals(this)) continue;

                otherPlayer = playerScript;
            }

            constantDeltaPosition = transform.position - otherPlayer.transform.position + new Vector3(0f, 0.0001f, 0f);

            otherCollider = new Collider();
            lookRight = true;
            horizontalMoving = 0f;
            movingSpeed = 5f;
        }

        private void OnEnable()
        {
            EventManager.StartListening("EndTransgression", OnTransgressionEnd);
        }

        private void OnDisable()
        {
            EventManager.StopListening("EndTransgression", OnTransgressionEnd);
        }

        private void OnDestroy()
        {
            EventManager.StopListening("EndTransgression", OnTransgressionEnd);
        }

        private void LateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                TryInteract();
            }
            carryingState.Rotate();
            camera.position = Vector3.Lerp(camera.position,
                new Vector3(transform.position.x, transform.position.y + 3, -7), Time.deltaTime * 4.0f);
        }

        public void TryInteract()
        {
            if (interactable == null) return;

            interactable.Interact();
        }

        private void FixedUpdate()
        {
            otherCollider = null;
            isInside = false;
            Move();
            if (otherPlayer != null)
            {
                otherPlayer.state.MoveDisabled();
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            var normal = other.contacts[0].normal;
            if (normal.y < 0f) return;
            state.Landing();
        }

        private void OnTriggerStay(Collider other)
        {
            otherCollider = other;
            isInside = true;
            StartCoroutine("WaitingFixedUpdate");
        }

        private IEnumerator WaitingFixedUpdate()
        {
            yield return new WaitForFixedUpdate();
        }

        private void OnTriggerEnter(Collider other)
        {
            otherCollider = other;
            isInside = true;
        }

        private void OnTriggerExit(Collider other)
        {
            otherCollider = null;
            isInside = false;
        }

        [CanBeNull]
        public Collider IsInside()
        {
            return otherCollider;
        }

        #region Moving

        public void ChangeHorizontalMove(float newMove)
        {
            horizontalMoving = newMove;
        }

        public void Jump()
        {
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, JumpSpeed, 0);
        }

        public void RotatePlayer()
        {
            if (lookRight && horizontalMoving < 0)
            {
                transform.Rotate(0, 180, 0);
                lookRight = !lookRight;
            }
            else if (!lookRight && horizontalMoving > 0)
            {
                transform.Rotate(0, 180, 0);
                lookRight = !lookRight;
            }
        }

        private void Move()
        {
            var currentVelocity = rigidBody.velocity.x;
            var currentVerticalVelocity = rigidBody.velocity.y;

            if (MaxSpeed - Math.Abs(currentVelocity) > 2.0f)
                rigidBody.AddForce(new Vector3(horizontalMoving * movingSpeed, 0f, 0f));

            if (Math.Abs(currentVelocity) > float.Epsilon && Math.Abs(horizontalMoving) < float.Epsilon)
                if (currentVelocity > 0)
                {
                    if (currentVelocity < 1)
                        rigidBody.velocity = new Vector3(0f, rigidBody.velocity.y, 0f);
                    else
                        rigidBody.AddForce(new Vector3(-movingSpeed * 2f, 0f, 0f));
                }
                else
                {
                    if (currentVelocity > -1)
                        rigidBody.velocity = new Vector3(0f, rigidBody.velocity.y, 0f);
                    else
                        rigidBody.AddForce(new Vector3(movingSpeed * 2f, 0f, 0f));
                }
            if (Math.Abs(currentVelocity) > float.Epsilon && Math.Abs(horizontalMoving) > float.Epsilon)
                rigidBody.velocity = horizontalMoving > 0
                    ? new Vector3(MaxSpeed, currentVerticalVelocity, 0f)
                    : new Vector3(-MaxSpeed, currentVerticalVelocity, 0f);
        }

        #endregion

        public void MoveDisabled()
        {
            transform.position = otherPlayer.transform.position + constantDeltaPosition;
        }

        public void CameraFollowMe()
        {
            camera.position = new Vector3(transform.position.x, transform.position.y + 3, -7);
        }

        public void Turn(bool turnOn)
        {
            rigidBody.isKinematic = !turnOn;
            collider.isTrigger = !turnOn;
        }

        public void SetMaximumSpeed(float newMax)
        {
            MaxSpeed = newMax;
        }

        private void OnTransgressionEnd(params object[] list)
        {
            Debug.Log("End transgression");
            otherPlayer.transgressionState.Next();
            transgressionState.Next();
        }

        public void GiveInteract(IInteractable obj)
        {
            interactable = obj;
        }

        public void RemoveInteract()
        {
            interactable = null;
        }
    }
}