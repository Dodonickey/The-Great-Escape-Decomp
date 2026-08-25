using System.Collections.Generic;

public class SKPolygon
{
	public List<SKNode> nodes;

	public SKPolygon(SKBase _sk)
	{
		nodes = new List<SKNode>();
		_sk.polygons.Add(this);
	}
}
