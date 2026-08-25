public class GpcC : IComponent
{
	private bool _active;

	private int _index;

	private int _entityIndex;

	private ComponentType _componentType;

	public TransformC p_TC;

	public Polygon originalPolygon;

	public Polygon modifiedPolygon;

	public Polygon[] tiles;

	public float polyMinX;

	public float polyMaxX;

	public float polyMinY;

	public float polyMaxY;

	public float polyWidth;

	public float polyHeight;

	public int tileWidth;

	public int tileHeight;

	public int tileCountX;

	public int tileCountY;

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
