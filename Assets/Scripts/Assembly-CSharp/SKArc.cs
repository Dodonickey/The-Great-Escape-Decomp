using UnityEngine;

public class SKArc
{
	public int index;

	public SKArc next;

	public SKNode nA;

	public SKNode nB;

	public SKWavefront w1;

	public SKWavefront w2;

	public Vector2 normal;

	public bool active;

	public int multiplier;

	public SKArc(SKBase _sk, SKNode _a, SKNode _b)
	{
		nA = _a;
		nB = _b;
		normal = (_b.pos - _a.pos).normalized;
		active = true;
		multiplier = 1;
		_sk.arcs.Add(this);
	}
}
