using UnityEngine;

public class TLTouch
{
	public int fingerId;

	public Vector2 position;

	public Vector2 deltaPosition;

	public TouchPhase phase;

	public bool masked;

	public bool consumed;

	public TouchAreaC consumingTAC;

	public Vector2 startPosition;
}
