using System;
using UnityEngine;

public struct ChipmunkCollisionPair
{
	public IntPtr shapeA;

	public IntPtr shapeB;

	public IntPtr bodyA;

	public IntPtr bodyB;

	public Vector2 pos;

	public Vector2 normal;

	public Vector2 impulse;

	public float depth;

	public Vector2 velA;

	public Vector2 velB;

	public int componentIndexA;

	public int componentIndexB;
}
