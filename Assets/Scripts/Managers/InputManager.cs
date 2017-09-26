using System.Collections.Generic;
using Player;
using UnityEngine;

namespace System.Managers
{
    public class InputManager : MonoBehaviour
    {
        private Command jumpCommand, moveLeftCommand, moveRightCommand, notMoveCommand,
            transgressionCommand, pauseCommand, interactCommand, doNothingCommand;

        protected List<PlayerClass> playerObjectsList = new List<PlayerClass>();

        private void Awake()
        {
            Application.targetFrameRate = 30;
            jumpCommand = new JumpCommand();
            moveLeftCommand = new MoveLeftCommand();
            moveRightCommand = new MoveRightCommand();
            notMoveCommand = new NotMoveCommand();
            doNothingCommand = new DoNothingCommand();
            transgressionCommand = new TransgressionCommand();
            pauseCommand = new PauseCommand();
            interactCommand = new InteractCommand();
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
                playerObjectsList.ForEach(_ => jumpCommand.Execute(_));
            if (Input.GetAxisRaw("Horizontal") > 0)
                playerObjectsList.ForEach(_ => moveRightCommand.Execute(_));
            if (Input.GetAxisRaw("Horizontal") < 0)
                playerObjectsList.ForEach(_ => moveLeftCommand.Execute(_));
            if (Math.Abs(Input.GetAxisRaw("Horizontal")) < Mathf.Epsilon)
                playerObjectsList.ForEach(_ => notMoveCommand.Execute(_));
            if (Input.GetButtonDown("Teleport"))
                playerObjectsList.ForEach(_ => transgressionCommand.Execute(_));
            if (Input.GetButtonDown("Exit"))
                pauseCommand.ExecuteWithoutPlayer();
            if(Input.GetButtonDown("Use"))
                interactCommand.ExecuteWithoutPlayer();
        }
    }
}