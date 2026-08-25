using UnityEngine;

public class AShipC : BasicControlledComponent
{
	private int _plrIdx;

	private float _angle;

	private Vector2 _acceleration;

	private PlayerState _playerState;

	private ChipmunkC _CMC;

	private ShipData _data;

	private PrefabC _prefab;

	public int plrIdx
	{
		get
		{
			return _plrIdx;
		}
		set
		{
			_plrIdx = value;
		}
	}

	public float angle
	{
		get
		{
			return _angle;
		}
		set
		{
			_angle = value;
		}
	}

	public Vector2 acceleration
	{
		get
		{
			return _acceleration;
		}
		set
		{
			_acceleration = value;
		}
	}

	public PlayerState playerState
	{
		get
		{
			return _playerState;
		}
		set
		{
			_playerState = value;
		}
	}

	public ChipmunkC CMC
	{
		get
		{
			return _CMC;
		}
		set
		{
			_CMC = value;
		}
	}

	public ShipData data
	{
		get
		{
			return _data;
		}
		set
		{
			_data = value;
		}
	}

	public PrefabC prefab
	{
		get
		{
			return _prefab;
		}
		set
		{
			_prefab = value;
		}
	}
}
