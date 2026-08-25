public class AEmitterC : BasicComponent
{
	private int _numAsteroids;

	private int _asteroidSpeed;

	private int[] _asteroidSize;

	private AAsteroidC[] _asteroids;

	private BasicLevelData _data;

	public TransformC TC;

	public int numAsteroids
	{
		get
		{
			return _numAsteroids;
		}
		set
		{
			_numAsteroids = value;
		}
	}

	public int asteroidSpeed
	{
		get
		{
			return _asteroidSpeed;
		}
		set
		{
			_asteroidSpeed = value;
		}
	}

	public AAsteroidC[] asteroids
	{
		get
		{
			return _asteroids;
		}
		set
		{
			_asteroids = value;
		}
	}

	public BasicLevelData data
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
}
