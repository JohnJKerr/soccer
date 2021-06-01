using Godot;
using System;
using System.Linq;

public class Player : KinematicBody2D
{
    private const int Speed = 80;
    private const int Touch = 40;
    private const int MaxKickPower = 1000;
    private Vector2 _movement = Vector2.Zero;
    private int _kickPower;
    private AnimatedSprite AnimatedSprite => GetNode<AnimatedSprite>(nameof(AnimatedSprite));
    private Area2D Legs => GetNode<Area2D>(nameof(Legs));
    private bool IsKicking => IsPressed(Action.Kick);
    private bool CanMove => !IsKicking || !InKickRange;
    private Node2D Pitch => GetTree().Root.GetNode<Node2D>(nameof(Pitch));
    private Ball Ball => Pitch.GetNode<Ball>(nameof(Ball));
    private bool InKickRange => Legs.GetOverlappingBodies().OfType<RigidBody2D>().FirstOrDefault() != default;
    
    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        _movement = GetMovement();
        _movement = MoveAndSlide(_movement);
        Animate();
        Kick();
    }

    private void Kick()
    {
        if (!InKickRange) return;
        _kickPower = Math.Max(Touch, _kickPower);
        if (IsKicking && _kickPower <= MaxKickPower)
        {
            _kickPower += 20;
            return;
        }
        var direction = Position.DirectionTo(Ball.Position).Normalized();
        Ball.ApplyCentralImpulse(direction * _kickPower);
        if(Ball.IsStopped) _kickPower = 0;
    }

    private Vector2 GetDirectionInput()
    {
        var vector = Vector2.Zero;
        if (IsPressed(Direction.Left)) vector.x -= 1;
        if (IsPressed(Direction.Right)) vector.x += 1;
        if (IsPressed(Direction.Up)) vector.y -= 1;
        if (IsPressed(Direction.Down)) vector.y += 1;
        return vector;
    }
    
    private Vector2 GetMovement()
    {
        var direction = !CanMove ? Vector2.Zero : GetDirectionInput();
        direction = direction.Normalized() * Speed;
        return direction;
    }

    private void Animate()
    {
        if (!_movement.Equals(Vector2.Zero)) Rotate();
        var animation = GetAnimation();
        AnimatedSprite.Animation = animation.ToString();
        AnimatedSprite.Play();
    }

    private void Rotate()
    {
        Rotation = _movement.Angle() + new Vector2(1, 0).Angle();
    }

    private Animation GetAnimation()
    {
        return _movement.Equals(Vector2.Zero) || IsKicking ? Animation.Default : Animation.Run;
    }

    private static bool IsPressed(Direction direction) => Input.IsActionPressed(direction.ToString());
    private static bool IsPressed(Action action) => Input.IsActionPressed(action.ToString());
    private static bool IsReleased(Action action) => Input.IsActionJustReleased(action.ToString());
}
