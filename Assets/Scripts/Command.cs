using System.Player;

public class Command
{
    public virtual void Execute(PlayerClass actor)
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
    }
}

public class TransgressionCommand : Command
{
    public override void Execute(PlayerClass actor)
    {
        actor.state.Transgression();
    }
}

public class DoNothingCommand : Command
{
    public override void Execute(PlayerClass actor)
    {
    }
}