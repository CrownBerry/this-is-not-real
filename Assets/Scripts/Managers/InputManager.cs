using System.Player;
using UnityEngine;

namespace System.Managers
{
    public class InputManager : MonoBehaviour
    {
        public PlayerObject playerObject;

        private Command jumpButton, moveLeft, moveRight, notMove, doNothing;

        private void Awake()
        {
            jumpButton = new JumpCommand();
            moveLeft = new MoveLeftCommand();
            moveRight = new MoveRightCommand();
            notMove = new NotMoveCommand();
            doNothing = new DoNothingCommand();

            playerObject = FindObjectOfType<PlayerObject>();
        }

        private void Update()
        {
            if (Input.GetButtonDown("Jump"))
            {
                jumpButton.Execute(playerObject);
            }
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                moveRight.Execute(playerObject);
            }
            if (Input.GetAxisRaw("Horizontal") < 0)
            {
                moveLeft.Execute(playerObject);
            }
            if (Math.Abs(Input.GetAxisRaw("Horizontal")) < Mathf.Epsilon)
            {
                notMove.Execute(playerObject);
            }
        }
    }
}