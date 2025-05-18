using System;
using Godot;

public partial class EnemyReturnState : EnemyState
{
    [Export(PropertyHint.Range, "0,20,0.1")]
    private float speed = 5;

    public override void _Ready()
    {
        base._Ready();

        destination = GetPointGlobalPosition(0);
    }

    protected override void EnterState()
    {
        characterNode.AnimPlayerNode.Play(GameConstants.ANIM_MOVE);

        characterNode.AgentNode.TargetPosition = destination;
        characterNode.ChaseAreaNode.BodyEntered += HandleChaseAreaBodyEntered;
    }

    protected override void ExitState()
    {
        characterNode.ChaseAreaNode.BodyEntered -= HandleChaseAreaBodyEntered;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (characterNode.AgentNode.IsNavigationFinished())
        {
            GD.Print("Reached Destination");
            characterNode.StateMachineNode.SwitchState<EnemyPatrolState>();
            return;
        }
        // GD.Print("Returning");
        Move();
    }
}
