using System;
using Godot;

public partial class EnemyAttackState : EnemyState
{
    protected override void EnterState()
    {
        characterNode.AnimPlayerNode.Play(GameConstants.ANIM_ATTACK);
    }
}
