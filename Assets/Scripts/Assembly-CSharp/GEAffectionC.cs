public class GEAffectionC : BasicComponent
{
	public IComponent source;

	public GECreatureC affected;

	public GEAffectionType affectionType;

	public uint identifier;

	public string name;

	public float began;

	public float duration;

	public int stack;

	public int maxStack;

	public float tickInterval;

	public float lastTick;

	public GEEffect tickEffect;

	public GEEffect beganEffect;

	public GEEffect endEffect;

	public bool hasBegan;

	public bool hasEnded;
}
