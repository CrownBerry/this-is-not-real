using System.Collections.Generic;
using Player;
using UnityEngine;

namespace System.Managers
{
    public class InputManager : MonoBehaviour
    {
        private Command jumpCommand,
            moveLeftCommand,
            moveRightCommand,
            notMoveCommand,
            transgressionCommand,
            pauseCommand,
            interactCommand,
            doNothingCommand;

        protected List<PlayerClass> playerObjectsList = new List<PlayerClass>();

        private IDictionary<string, Command> commands;
        private IDictionary<string, Command> commandsWithoutPlayer;

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

            commands = new Dictionary<string, Command>
            {
                {"Jump", jumpCommand},
                {"Teleport", transgressionCommand},
                {"Use", interactCommand}
            };

            commandsWithoutPlayer = new Dictionary<string, Command>
            {
                {"Exit", pauseCommand}
            };
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
            if (Input.GetAxisRaw("Horizontal") > 0)
                playerObjectsList.ForEach(_ => moveRightCommand.Execute(_));
            if (Input.GetAxisRaw("Horizontal") < 0)
                playerObjectsList.ForEach(_ => moveLeftCommand.Execute(_));
            if (Math.Abs(Input.GetAxisRaw("Horizontal")) < Mathf.Epsilon)
                playerObjectsList.ForEach(_ => notMoveCommand.Execute(_));
            foreach (var command in commands)
            {
                if (Input.GetButtonDown(command.Key))
                {
                    playerObjectsList.ForEach(_ => command.Value.Execute(_));
                }
            }
            foreach (var command in commandsWithoutPlayer)
            {
                if (Input.GetButtonDown(command.Key))
                {
                    command.Value.ExecuteWithoutPlayer();
                }
            }
        }
    }
}