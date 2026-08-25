using UnityEngine;

public class SKNode
{
	public Vector2 pos;

	public SKNode(SKBase _sk, Vector2 _pos)
	{
		pos = _pos;
		_sk.nodes.Add(this);
	}
}
