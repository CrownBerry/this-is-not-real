using System.FSM;
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

        private Vector3 savedPosition;

        private Rigidbody rigidBody;
        public PlayerStateMachine.State startingState;
        public PlayerStateMachine state;

        private void Awake()
        {
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
            transform.position = new Vector3(newPosition.x, newPosition.y, newPosition.z);
            Debug.Log(string.Format("newPosition is {0}", newPosition));
        }
    }
}