public class GEEffect
{
	public bool[] effectActive;

	public int[] effects;

	public float[] affectionChances;

	public uint[] possibleAffections;

	public GEEffect()
	{
		effectActive = new bool[12];
		effects = new int[12];
		effects[0] = 0;
		effects[1] = 0;
		effects[2] = 0;
		effects[3] = 0;
		effects[4] = 0;
		effects[5] = 0;
		effects[6] = 0;
		effects[8] = 0;
		effects[9] = 0;
		effects[7] = 0;
		effects[10] = 0;
		effects[11] = 0;
		affectionChances = new float[0];
		possibleAffections = new uint[0];
	}
}
