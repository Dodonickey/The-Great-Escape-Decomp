using UnityEngine;

public class TweenC : IComponent
{
	private bool _active;

	private int _index;

	private int _entityIndex;

	private ComponentType _componentType;

	public TransformC p_TC;

	public TweenedProperty component;

	public bool mirrored;

	public int currentRepeat;

	public int repeats;

	public TweenStyle currentTweenStyle;

	public TweenStyle mirroredTweenStyle;

	public Vector3 startValue;

	public Vector3 endValue;

	public Vector3 currentValue;

	public float delay;

	public float duration;

	public float startTime;

	public int delegatedCount;

	public TweenEventDelegate tweenEventDelegate;

	public bool removeEntityAtFinish;

	public bool removeComponentAtFinish;

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
