using UnityEngine;

public class PhysXC : IComponent, IPhysicsComponent
{
	private bool _active;

	private int _index;

	private int _entityIndex;

	private ComponentType _componentType;

	public TransformC TC;

	public GameObject GO;

	public Collider collider;

	public ColliderType colliderType;

	public bool transformComponentDictates;

	public bool dictatePosition;

	public bool dictateAngle;

	public bool isStatic;

	public bool isRogue;

	public uint colliderGroup;

	public uint colliderLayer;

	public IComponent customComponent;

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
