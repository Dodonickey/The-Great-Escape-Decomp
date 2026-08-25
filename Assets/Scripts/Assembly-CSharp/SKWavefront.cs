public class SKWavefront
{
	public SKArc a1;

	public SKArc a2;

	public SKWavefront prev;

	public SKWavefront next;

	public SKWavefront(SKBase _sk, SKArc _a, SKArc _b)
	{
		a1 = _a;
		a2 = _b;
		_sk.wavefronts.Add(this);
	}

	public SKWavefront(SKBase _sk, SKArc _a, SKArc _b, int _index)
	{
		a1 = _a;
		a2 = _b;
		_sk.wavefronts.Insert(_index, this);
	}
}
