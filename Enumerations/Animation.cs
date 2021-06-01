public class Animation : Enumeration
{
	private Animation(int value, string name) : base(value, name.ToLower())
	{
	}

	public static Animation Default = new Animation(0, nameof(Default));
	public static Animation Run = new Animation(1, nameof(Run));
}