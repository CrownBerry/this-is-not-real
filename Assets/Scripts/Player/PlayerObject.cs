using System.FSM;
using System.Globalization;
using UnityEngine;

namespace System.Player
{
    public class PlayerObject : MonoBehaviour
    {
        private PlayerFSM _playerFsm;
        private Rigidbody rb;
        private float movingDirection = 0f;
        private float speed = 5f;
        private float maxSpeed = 5.0f;
        private bool lookRight = true;

        private const float jumpSpeed = 6.0f;

        private void Awake()
        {
            _playerFsm = new PlayerFSM(this);
            rb = GetComponent<Rigidbody>();
        }

        private void LateUpdate()
        {
            Move();
        }

        #region Moving

        public void Jump()
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpSpeed, 0);
        }

        private void Move()
        {
            var currentVelocity = rb.velocity.x;
            var currentVerticalVelocity = rb.velocity.y;

            if (maxSpeed - Math.Abs(currentVelocity) > 2.0f)
            {
                rb.AddForce (new Vector3 (movingDirection * speed, 0f, 0f));
            }

            if (Math.Abs(currentVelocity) > float.Epsilon && Math.Abs(movingDirection) < float.Epsilon) {
                if (currentVelocity > 0) {
                    if (currentVelocity < 1)
                        rb.velocity = new Vector3(0f,rb.velocity.y,0f);
                    else
                        rb.AddForce (new Vector3 (-speed * 2f, 0f, 0f));
                } else {
                    if (currentVelocity > -1)
                        rb.velocity = new Vector3(0f,rb.velocity.y,0f);
                    else
                        rb.AddForce (new Vector3 (speed * 2f, 0f, 0f));
                }
            }
            if (Math.Abs(currentVelocity) > float.Epsilon && Math.Abs(movingDirection) > float.Epsilon)
            {
                rb.velocity = movingDirection > 0 ?
                    new Vector3 (maxSpeed, currentVerticalVelocity, 0f)
                    : new Vector3 (-maxSpeed, currentVerticalVelocity, 0f);
            }
        }

        #endregion

        public void DoJump()
        {
            _playerFsm.Jump();
        }

        private void OnCollisionEnter(Collision other)
        {
            _playerFsm.Landing();
        }

        public void MoveLeft()
        {
            if (lookRight)
            {
                transform.Rotate(0,180,0);
            }
            lookRight = false;
            movingDirection = -1.0f;
        }

        public void MoveRight()
        {
            if (!lookRight)
            {
                transform.Rotate(0,180,0);
            }
            lookRight = true;
            movingDirection = 1.0f;
        }

        public void NotMove()
        {
            movingDirection = 0.0f;
        }

    }
}