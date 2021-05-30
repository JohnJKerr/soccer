using Godot;

public class Direction : Enumeration
{
	private Direction(int value, string name) : base(value, name.ToLower())
	{
	}
	
	public static Direction Up = new Direction(0, nameof(Up));
	public static Direction Left = new Direction(1, nameof(Left));
	public static Direction Right = new Direction(2, nameof(Right));
	public static Direction Down = new Direction(3, nameof(Down));
}