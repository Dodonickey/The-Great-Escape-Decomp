using UnityEngine;

public class TextC : IComponent
{
	private bool _active;

	private int _index;

	private int _entityIndex;

	private ComponentType _componentType;

	public string text;

	public float fontSize;

	public bool update;

	public bool isDynamic;

	public bool isMultiline;

	public float textWidth;

	public float textHeight;

	public float textAreaWidth;

	public float textAreaHeight;

	public float textAreaOffsetX;

	public float textAreaOffsetY;

	public Align textVerticalAlign;

	public Align textHorizontalAlign;

	public float textAreaAlignX;

	public float textAreaAlignY;

	public float marginLeft;

	public float marginRight;

	public float marginTop;

	public float marginBottom;

	public GameObject gameObject;

	public TransformC TC;

	public TransformC contentTC;

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
