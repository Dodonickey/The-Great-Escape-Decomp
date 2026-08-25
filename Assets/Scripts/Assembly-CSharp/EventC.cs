using System.Collections;

public class EventC : IComponent
{
	private bool _active;

	private int _index;

	private int _entityIndex;

	private ComponentType _componentType;

	public bool dispatched;

	public float startTime;

	public float delay;

	public bool removeAfterDelegate;

	public bool delegateAtRemove;

	public bool delegateOnlyOnce;

	public int count;

	public string identifier;

	public Hashtable properties;

	public EventDelegate eventDelegate;

	public int delegatedCount;

	public bool active
	{
		get
		{
			return _active;
		}
		set
		{
			_active = value;
		}
	}

	public int index
	{
		get
		{
			return _index;
		}
		set
		{
			_index = value;
		}
	}

	public int entityIndex
	{
		get
		{
			return _entityIndex;
		}
		set
		{
			_entityIndex = value;
		}
	}

	public ComponentType componentType
	{
		get
		{
			return _componentType;
		}
		set
		{
			_componentType = value;
		}
	}
}
