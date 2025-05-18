using System;
using Godot;

public partial class EnemyPatrolState : EnemyState
{
    [Export]
    private Timer idleTimerNode;

    [Export(PropertyHint.Range, "0,20,0.1")]
    private float maxIdleTime = 4;

    private int pointIndex = 0;

    protected override void EnterState()
    {
        characterNode.AnimPlayerNode.Play(GameConstants.ANIM_MOVE);

        pointIndex = 1;

        destination = GetPointGlobalPosition(pointIndex);
        characterNode.AgentNode.TargetPosition = destination;

        // must unlisten to the subscription on exitstate
        characterNode.AgentNode.NavigationFinished += HandleNavigationFinished;
        idleTimerNode.Timeout += HandleTimeout;
        characterNode.ChaseAreaNode.BodyEntered += HandleChaseAreaBodyEntered;
    }

    protected override void ExitState()
    {
        // unsubscribe defined here
        characterNode.AgentNode.NavigationFinished -= HandleNavigationFinished;
        idleTimerNode.Timeout -= HandleTimeout;
        characterNode.ChaseAreaNode.BodyEntered -= HandleChaseAreaBodyEntered;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!idleTimerNode.IsStopped())
        {
            return;
        }
        // GD.Print("patrolling: ", pointIndex);
        Move();
    }

    private void HandleNavigationFinished()
    {
        characterNode.AnimPlayerNode.Play(GameConstants.ANIM_IDLE);

        RandomNumberGenerator rng = new();
        idleTimerNode.WaitTime = rng.RandfRange(0, maxIdleTime);
        // Emits Start timer
        idleTimerNode.Start();

        // Have to catch the event at EnterState()
    }

    private void HandleTimeout()
    {
        characterNode.AnimPlayerNode.Play(GameConstants.ANIM_MOVE);

        // incremental pathing
        // pointIndex = Mathf.Wrap(pointIndex + 1, 0, characterNode.PathNode.Curve.PointCount);

        //random pathing
        RandomNumberGenerator rng = new();
        pointIndex = rng.RandiRange(0, characterNode.PathNode.Curve.PointCount - 1);

        destination = GetPointGlobalPosition(pointIndex);
        characterNode.AgentNode.TargetPosition = destination;
    }
}
