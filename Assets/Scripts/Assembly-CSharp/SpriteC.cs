using UnityEngine;

public class SpriteC : IComponent
{
	private bool _active;

	private int _index;

	private int _entityIndex;

	private ComponentType _componentType;

	public bool update;

	public bool visible;

	public bool isVisible;

	public Vector3 align;

	public Vector3 offset;

	public Vector3 offsetRight;

	public Vector3 offsetUp;

	public float width;

	public float height;

	public Vector3 scaledRelRight;

	public Vector3 scaledRelUp;

	public Vector3 scaledRelOffset;

	public Vector3 relRight;

	public Vector3 relUp;

	public Vector3 relOffset;

	public float wScale;

	public float hScale;

	public float dimensionScale;

	public float wDimension;

	public float hDimension;

	public TransformC p_TC;

	public SpriteSheet p_spriteSheet;

	public Color color;

	public Frame frame;

	public int meshIndex;

	public int vertDataIndex;

	public float sortValue;

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
