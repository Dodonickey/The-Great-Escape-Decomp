public class GEMat
{
	public string projectCode;

	public string name;

	public string fill;

	public string road;

	public float fillScale;

	public float roadScale;

	public float beltWidth;

	public float beltDepth;

	public int smooth;

	public bool hasRoad;

	public bool hasFill;

	public bool hasBelt;

	public GEMat(string _projectCode, string _name, string _fill)
	{
		Constructor(_projectCode, _name, _fill, 1f, string.Empty, 1f, 0f, 0f, 1);
	}

	public GEMat(string _projectCode, string _name, string _fill, float _fillScale)
	{
		Constructor(_projectCode, _name, _fill, _fillScale, string.Empty, 1f, 0f, 0f, 1);
	}

	public GEMat(string _projectCode, string _name, string _fill, float _fillScale, string _road)
	{
		Constructor(_projectCode, _name, _fill, _fillScale, _road, 1f, 0f, 0f, 1);
	}

	public GEMat(string _projectCode, string _name, string _fill, float _fillScale, string _road, float _roadScale)
	{
		Constructor(_projectCode, _name, _fill, _fillScale, _road, _roadScale, 0f, 0f, 1);
	}

	public GEMat(string _projectCode, string _name, string _fill, float _fillScale, string _road, float _roadScale, float _beltWidth, float _beltDepth)
	{
		Constructor(_projectCode, _name, _fill, _fillScale, _road, _roadScale, _beltWidth, _beltDepth, 1);
	}

	public GEMat(string _projectCode, string _name, string _fill, float _fillScale, string _road, float _roadScale, float _beltWidth, float _beltDepth, int _smooth)
	{
		Constructor(_projectCode, _name, _fill, _fillScale, _road, _roadScale, _beltWidth, _beltDepth, _smooth);
	}

	public void Constructor(string _projectCode, string _name, string _fill, float _fillScale, string _road, float _roadScale, float _beltWidth, float _beltDepth, int _smooth)
	{
		projectCode = _projectCode;
		name = _name;
		fill = _fill;
		road = _road;
		fillScale = _fillScale;
		roadScale = _roadScale;
		beltWidth = _beltWidth;
		beltDepth = _beltDepth;
		smooth = _smooth;
		hasRoad = true;
		hasFill = true;
		hasBelt = false;
	}
}
