using System.Collections;

public class GESpritePrefabC : IComponent
{
	private bool _active;

	private int _index;

	private int _entityIndex;

	private ComponentType _componentType;

	public Hashtable animations;

	public SpritePrefabAnimation animation;

	public int currentFrame;

	public SpritePrefabNode rootNode;

	public SpritePrefabNode[] nodes;

	public Hashtable nodeTable;

	public int flipX;

	public IComponent customComponent;

	public bool animatePhysics;

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
