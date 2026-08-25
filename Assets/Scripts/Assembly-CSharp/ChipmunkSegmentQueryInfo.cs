using System;
using UnityEngine;

public struct ChipmunkSegmentQueryInfo
{
	public IntPtr shape;

	public IntPtr body;

	public int unityComponentIndex;

	public float t;

	public Vector2 n;

	public Vector2 p;

	public float d;
}
