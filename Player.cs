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
    private bool _hasPossession;
    private AnimatedSprite AnimatedSprite => GetNode<AnimatedSprite>(nameof(AnimatedSprite));
    private bool IsMoving => _movement != Vector2.Zero;
    private bool IsKicking => IsPressed(Action.Kick);
    private Node2D Pitch => GetTree().Root.GetNode<Node2D>(nameof(Pitch));
    private Ball Ball => Pitch.GetNode<Ball>(nameof(Ball));
    
    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        _movement = GetMovement();
        _movement = MoveAndSlide(_movement);
        Animate();
        Dribble();
        Kick();
    }

    public void OnLegsEntered(Node2D body)
    {
        if (!(body is Ball ball)) return;
        GainPossession();
    }

    public void OnLegsExited(Node2D body)
    {
        if (!(body is Ball ball)) return;
        LosePossession();
    }

    private void GainPossession()
    {
        _hasPossession = true;
        Ball.Sleeping = true;
    }

    private void LosePossession()
    {
        _hasPossession = false;
        _kickPower = 0;
    }

    private void Dribble()
    {
        if (!_hasPossession) return;
        var ballDistance = Position.DistanceTo(Ball.Position);
        if (_hasPossession && ballDistance > Touch)
        {
            var direction = Ball.Position.DirectionTo(Position);
            Ball.ApplyCentralImpulse(direction * ballDistance);
        }
        if(IsMoving) Ball.ApplyCentralImpulse(_movement.Normalized() * Touch);
    }

    private void Kick()
    {
        PowerUpKick();
        if (!IsReleased(Action.Kick)) return;
        var direction = Position.DirectionTo(Ball.Position).Normalized();
        Ball.ApplyCentralImpulse(direction * _kickPower);
        _kickPower = 0;
    }

    private void PowerUpKick()
    {
        if (!IsKicking || !_hasPossession) return;
        _kickPower = Math.Min(MaxKickPower, _kickPower + 20);
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
        var direction = GetDirectionInput();
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
