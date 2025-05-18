using Godot;

public abstract partial class EnemyState : CharacterState
{
    protected Vector3 destination;

    protected Vector3 GetPointGlobalPosition(int index)
    {
        Vector3 localpos = characterNode.PathNode.Curve.GetPointPosition(index);
        Vector3 globalpos = characterNode.PathNode.GlobalPosition;

        return localpos + globalpos;
    }

    protected void Move()
    {
        characterNode.AgentNode.GetNextPathPosition();
        characterNode.Velocity = characterNode.GlobalPosition.DirectionTo(destination);

        characterNode.MoveAndSlide();
        characterNode.Flip();
    }

    // Chase player when area is entered
    protected void HandleChaseAreaBodyEntered(Node3D body)
    {
        characterNode.StateMachineNode.SwitchState<EnemyChaseState>();
    }
}
