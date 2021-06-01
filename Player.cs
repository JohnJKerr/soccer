using Godot;
using System;

public class Player : KinematicBody2D
{
    private const int Speed = 80;
    private AnimatedSprite AnimatedSprite => GetNode<AnimatedSprite>(nameof(AnimatedSprite));
    
    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        var movement = GetMovement();
        movement = MoveAndSlide(movement);
        Animate(movement);
    }
    
    private Vector2 GetMovement()
    {
        var velocity = Vector2.Zero;
        if (IsPressed(Direction.Left)) velocity.x -= 1;
        if (IsPressed(Direction.Right)) velocity.x += 1;
        if (IsPressed(Direction.Up)) velocity.y -= 1;
        if (IsPressed(Direction.Down)) velocity.y += 1;
        var targetX = Position.x + velocity.x;
        var targetY = Position.y + velocity.y;
        velocity.x = targetX - Position.x;
        velocity.y = targetY - Position.y;
        velocity = velocity.Normalized() * Speed;
        return velocity;
    }

    private void Animate(Vector2 movement)
    {
        if (!movement.Equals(Vector2.Zero)) Rotate(movement);
        var animation = GetAnimation(movement);
        AnimatedSprite.Animation = animation.ToString();
        AnimatedSprite.Play();
    }

    private void Rotate(Vector2 movement)
    {
        Rotation = movement.Angle() + new Vector2(1, 0).Angle();
    }

    private static Animation GetAnimation(Vector2 movement)
    {
        return movement.Equals(Vector2.Zero) ? Animation.Default : Animation.Run;
    }

    private static bool IsPressed(Direction input) => Input.IsActionPressed(input.DisplayName);
}
