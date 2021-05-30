using Godot;
using System;

public class Player : KinematicBody2D
{
    private const int Speed = 80;
    
    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        var movement = GetMovement();
        movement = MoveAndSlide(movement);
        if (movement != Vector2.Zero) Animate(movement);
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
        Rotation = movement.Angle() + new Vector2(1, 0).Angle();
    }
    
    private static bool IsPressed(Direction input) => Input.IsActionPressed(input.DisplayName);
}
