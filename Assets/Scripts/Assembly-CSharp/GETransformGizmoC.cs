using System.Collections.Generic;
using UnityEngine;

public class GETransformGizmoC : IComponent
{
	private bool _active;

	private int _index;

	private int _entityIndex;

	private ComponentType _componentType;

	public TransformC gizmoTC;

	public TouchAreaC moveTAC;

	public List<Vector3> originalScale;

	public List<Vector3> originalRotation;

	public List<Vector3> originalPosition;

	public Vector3 rotateStart;

	public bool readyToMove;

	public Vector3 touchOffset;

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
