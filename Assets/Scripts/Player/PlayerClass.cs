using System;
using System.Collections;
using JetBrains.Annotations;
using Player.FSM;
using UnityEngine;

namespace Player
{
    public class PlayerClass : MonoBehaviour
    {
        private float MaxSpeed = 5.0f;
        private float JumpSpeed = 6.0f;
        private float horizontalMoving;
        private bool lookRight;
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

        private void Awake()
        {
            carryingState = new CarryingStateMachine(this, CarryingStateMachine.State.None);
            state = new PlayerStateMachine(this, startingState);
            camera = GameObject.Find("Main Camera").transform;
            collider = GetComponent<CapsuleCollider>();
            rigidBody = GetComponent<Rigidbody>();

            var players = GameObject.FindGameObjectsWithTag("Player");
            foreach (var player in players)
            {
                var playerScript = player.GetComponent<PlayerClass>();
                if (playerScript.Equals(this)) continue;

                otherPlayer = playerScript;
            }

            constantDeltaPosition = transform.position - otherPlayer.transform.position + new Vector3(0f,0.0001f,0f);
            otherCollider = new Collider();
            lookRight = true;
            horizontalMoving = 0f;
            movingSpeed = 5f;
        }

        private void LateUpdate()
        {
//            RotatePlayer();
            carryingState.Rotate();
            state.MoveCamera(camera);
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

        [CanBeNull] public Collider IsInside()
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
            switch (lookRight)
            {
                case true:
                    if (horizontalMoving < 0)
                    {
                        transform.Rotate(0, 180, 0);
                        otherPlayer.transform.Rotate(0,180,0);
                        otherPlayer.lookRight = !otherPlayer.lookRight;
                        lookRight = !lookRight;
                    }
                    break;
                case false:
                    if (horizontalMoving > 0)
                    {
                        transform.Rotate(0, 180, 0);
                        otherPlayer.transform.Rotate(0,180,0);
                        otherPlayer.lookRight = !otherPlayer.lookRight;
                        lookRight = !lookRight;
                    }
                    break;
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
    }
}