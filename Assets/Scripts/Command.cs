using System;
using System.Player;
using UnityEngine;

public abstract class Command
{
    public abstract void Execute(PlayerObject actor);
}

public class MoveLeftCommand : Command
{
    public override void Execute(PlayerObject actor)
    {
        Debug.Log("left");
        actor.MoveLeft();
    }
}

public class MoveRightCommand : Command
{
    public override void Execute(PlayerObject actor)
    {
        Debug.Log("right");
        actor.MoveRight();
    }
}

public class NotMoveCommand : Command
{
    public override void Execute(PlayerObject actor)
    {
        actor.NotMove();
    }
}

public class JumpCommand : Command
{
    public override void Execute(PlayerObject actor)
    {
        actor.DoJump();
    }
}

public class DoNothingCommand : Command
{
    public override void Execute(PlayerObject actor)
    {
    }
}