using System;
using UnityEngine;

public struct ChipmunkQueryInfo
{
	public IntPtr shape;

	public IntPtr body;

	public Vector2 pos;

	public Vector2 vel;

	public int componentIndex;
}
