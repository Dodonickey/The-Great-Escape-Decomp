using UnityEngine;

public class CameraTargetC : IComponent
{
	private bool _active;

	private int _index;

	private int _entityIndex;

	private ComponentType _componentType;

	public TransformC TC;

	public Camera camera;

	public Vector3 offset;

	public Vector3 rotationOffset;

	public Vector3 prevPos;

	public Vector3 prevRot;

	public Vector3 prevVel;

	public float destinationSmooth;

	public float velocityDirectionSmooth;

	public Vector3 lowSpeed;

	public Vector3 highSpeed;

	public float lowSpeedDistance;

	public float highSpeedDistance;

	public float directionalOffset;

	public float maxDisplacement;

	public float shakeDuration;

	public float shakeBegin;

	public float shakeAmount;

	public float lastShake;

	public float shakeInterval;

	public float shakeFalloff;

	public Vector3 shake;

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
