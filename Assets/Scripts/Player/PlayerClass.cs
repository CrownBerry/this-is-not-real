using System.Collections;
using System.FSM;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.VR.WSA;

namespace System.Player
{
    public class PlayerClass : MonoBehaviour
    {
        private const float maxSpeed = 5.0f;
        private const float jumpSpeed = 6.0f;
        private float horizontalMoving;
        private bool lookRight;
        private float movingSpeed;
        public PlayerClass otherPlayer;

        public bool isInside = false;
        private Collider otherCollider;

        private Vector3 savedPosition;

        public Vector3 oldPos;
        public Vector3 newPos;
        public Vector3 deltaPos;

        private CapsuleCollider _collider;
        private Rigidbody rigidBody;
        public PlayerStateMachine.State startingState;
        public PlayerStateMachine state;
        private Transform _camera;

        private void Awake()
        {
            otherCollider = new Collider();
            _collider = GetComponent<CapsuleCollider>();
            _camera = GameObject.Find("Main Camera").transform;
            savedPosition = transform.position;
            lookRight = true;
            horizontalMoving = 0f;
            movingSpeed = 5f;
            state = new PlayerStateMachine(this, startingState);
            rigidBody = GetComponent<Rigidbody>();

            var players = GameObject.FindGameObjectsWithTag("Player");
            foreach (var player in players)
            {
                var playerScript = player.GetComponent<PlayerClass>();
                if (!playerScript.Equals(this))
                {
                    otherPlayer = playerScript;
                }
            }
        }

        private void LateUpdate()
        {
            RotatePlayer();
            state.MoveCamera(_camera);
        }

        private void FixedUpdate()
        {
            otherCollider = null;
            isInside = false;
            Move();
            var newPosition = transform.position;
            var deltaPosition = newPosition - savedPosition;
            if (otherPlayer != null)
            {
                otherPlayer.state.MoveDisabled(deltaPosition);
            }
            savedPosition = transform.position;
        }

        private void OnCollisionEnter(Collision other)
        {
            state.Landing();
        }

        private void OnTriggerStay(Collider other)
        {
            otherCollider = other;
            isInside = true;
            StartCoroutine("WaitingFixedUpdate");
        }

        IEnumerator WaitingFixedUpdate()
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
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, jumpSpeed, 0);
        }

        private void RotatePlayer()
        {
            switch (lookRight)
            {
                case true:
                    if (horizontalMoving < 0)
                    {
                        transform.Rotate(0, 180, 0);
                        lookRight = !lookRight;
                    }
                    break;
                case false:
                    if (horizontalMoving > 0)
                    {
                        transform.Rotate(0, 180, 0);
                        lookRight = !lookRight;
                    }
                    break;
            }
        }

        private void Move()
        {
            var currentVelocity = rigidBody.velocity.x;
            var currentVerticalVelocity = rigidBody.velocity.y;

            if (maxSpeed - Math.Abs(currentVelocity) > 2.0f)
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
                    ? new Vector3(maxSpeed, currentVerticalVelocity, 0f)
                    : new Vector3(-maxSpeed, currentVerticalVelocity, 0f);
        }

        #endregion

        public void MoveDisabled(Vector3 deltaPosition)
        {
            var oldPosition = transform.position;
            var newPosition = oldPosition + deltaPosition;
            oldPos = oldPosition;
            newPos = newPosition;
            deltaPos = deltaPosition;
            transform.position = new Vector3(newPosition.x, newPosition.y, newPosition.z);
        }

        public void CameraFollowMe()
        {
            _camera.position = new Vector3(transform.position.x, transform.position.y + 3, -7);
        }

        public void Turn(bool turnOn)
        {
            rigidBody.isKinematic = !turnOn;
            _collider.isTrigger = !turnOn;
        }
    }
}