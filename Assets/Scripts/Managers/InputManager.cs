using System.Collections.Generic;
using System.Player;
using UnityEngine;

namespace System.Managers
{
    public class InputManager : MonoBehaviour
    {
        private Command jumpButton, moveLeft, moveRight, notMove, doNothing;
        protected List<PlayerClass> playerObjectsList = new List<PlayerClass>();

        private void Awake()
        {
            jumpButton = new JumpCommand();
            moveLeft = new MoveLeftCommand();
            moveRight = new MoveRightCommand();
            notMove = new NotMoveCommand();
            doNothing = new DoNothingCommand();
        }

        private void Start()
        {
            var players = GameObject.FindGameObjectsWithTag("Player");
            foreach (var player in players)
            {
                var playerScriptObject = player.GetComponent<PlayerClass>();
                playerObjectsList.Add(playerScriptObject);
            }
        }

        private void Update()
        {
            if (Input.GetButtonDown("Jump"))
                playerObjectsList.ForEach(_ => jumpButton.Execute(_));
            if (Input.GetAxisRaw("Horizontal") > 0)
                playerObjectsList.ForEach(_ => moveRight.Execute(_));
            if (Input.GetAxisRaw("Horizontal") < 0)
                playerObjectsList.ForEach(_ => moveLeft.Execute(_));
            if (Math.Abs(Input.GetAxisRaw("Horizontal")) < Mathf.Epsilon)
                playerObjectsList.ForEach(_ => notMove.Execute(_));
        }
    }
}