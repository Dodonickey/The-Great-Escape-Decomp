using UnityEngine;

public class AnchorPointInfo
{
	public int anchorIndex;

	public AnchorType anchorType;

	public Vector3 position;

	public Vector3 rotation;

	public float velocityMultipler;

	public float waitAtPoint;

	public int entryEasingType;

	public int exitEasingType;

	public int interpolationType;

	public float length;

	public AnchorPointInfo(Vector3 _pos, int _index, AnchorType _anchorType)
	{
		anchorIndex = _index;
		anchorType = _anchorType;
		position = _pos;
		rotation = Vector3.zero;
		velocityMultipler = 1f;
		waitAtPoint = 0f;
		entryEasingType = 0;
		exitEasingType = 0;
		interpolationType = 0;
		length = 0f;
	}
}
