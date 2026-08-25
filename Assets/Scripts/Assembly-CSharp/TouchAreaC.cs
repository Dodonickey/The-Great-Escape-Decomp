using System.Collections.Generic;
using UnityEngine;

public class TouchAreaC : IComponent
{
	private bool _active;

	private int _index;

	private int _entityIndex;

	private ComponentType _componentType;

	public TransformC TC;

	public Vector3 offset;

	public Camera camera;

	public float radius;

	public float width;

	public float height;

	public bool consumeTouches;

	public bool scaleByCameraDistance;

	public bool scaleByTransformComponent;

	public bool isReserved;

	public int reservingFingerId;

	public bool reservingStartedInside;

	public string identifier;

	public IComponent customComponent;

	public int delegatedCount;

	public TouchEventDelegate touchEventDelegate;

	public List<TouchEvent> touchEvent;

	public List<Vector2> touchPos;

	public List<Vector2> touchStartPos;

	public List<int> touchIndex;

	public List<int> touchFingerId;

	public List<bool> touchStartedInside;

	public List<bool> touchWasInside;

	public List<bool> touchWasDragged;

	public bool clip;

	public float clipMinX;

	public float clipMaxX;

	public float clipMinY;

	public float clipMaxY;

	public int order;

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
