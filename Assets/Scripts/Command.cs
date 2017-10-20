using System;
using Player;
using UnityEngine;

public class Command
{
    public virtual void Execute(PlayerClass actor)
    {
    }

    public virtual void ExecuteWithoutPlayer()
    {

    }
}

public class MoveLeftCommand : Command
{
    public override void Execute(PlayerClass actor)
    {
        actor.state.ChangeMoveDirection(-1f);
    }
}

public class MoveRightCommand : Command
{
    public override void Execute(PlayerClass actor)
    {
        actor.state.ChangeMoveDirection(1f);
    }
}

public class NotMoveCommand : Command
{
    public override void Execute(PlayerClass actor)
    {
        actor.state.ChangeMoveDirection(0f);
    }
}

public class JumpCommand : Command
{
    public override void Execute(PlayerClass actor)
    {
        actor.state.Jump();
        EventManager.TriggerEvent("DropBox");
    }
}

public class TransgressionCommand : Command
{
    public override void Execute(PlayerClass actor)
    {
        actor.state.Transgression();
        EventManager.TriggerEvent("DropBox");
    }
}

public class PauseCommand : Command
{
    private readonly UIManager manager;

    public PauseCommand()
    {
        manager = GameObject.Find("GameManager").GetComponent<UIManager>();
    }

    public override void ExecuteWithoutPlayer()
    {
        manager.SwitchPause();
    }
}

public class InteractCommand : Command
{
    public override void Execute(PlayerClass actor)
    {
        actor.state.Interact();
    }
}

public class DoNothingCommand : Command
{
    public override void Execute(PlayerClass actor)
    {
    }
}