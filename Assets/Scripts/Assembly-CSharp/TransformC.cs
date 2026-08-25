using System.Collections.Generic;
using UnityEngine;

public class TransformC : IComponent
{
	private bool _active;

	private int _index;

	private int _entityIndex;

	private ComponentType _componentType;

	public bool updatePosition;

	public bool updateRotation;

	public bool updateScale;

	public bool updatedPosition;

	public bool updatedRotation;

	public bool updatedScale;

	public TransformC parent;

	public List<TransformC> childs;

	public int level;

	public bool parentedToPhysics;

	public Vector3 lastPos;

	public Vector3 delta;

	public bool forceRotation;

	public Quaternion forcedRotation = Quaternion.identity;

	public bool forceScale;

	public Vector3 forcedScale = Vector3.one;

	public Transform transform;

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
