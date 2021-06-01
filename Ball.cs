using Godot;
using System;

public class Ball : RigidBody2D
{
    public override void _IntegrateForces(Physics2DDirectBodyState state)
    {
        base._IntegrateForces(state);
        var transform = state.Transform.Rotated(IsStopped ? 0 : 0.1F);
        state.Transform = transform;
    }

    internal bool IsStopped => LinearVelocity.Abs() <= Vector2.One;
}
