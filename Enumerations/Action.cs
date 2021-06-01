public class Action : Enumeration
{
	public Action(int value, string name) : base(value, name.ToLower())
	{
	}

	public static Action Kick = new Action(0, nameof(Kick));
}