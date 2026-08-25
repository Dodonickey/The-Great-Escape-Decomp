using System;
using UnityEngine;

public struct ChipmunkConstraintQueryInfo
{
	public IntPtr constraint;

	public ucpConstraintType type;

	public int unityComponentIndexA;

	public int unityComponentIndexB;

	public IntPtr bodyA;

	public IntPtr bodyB;

	public IntPtr bodyC;

	public Vector2 anchorA;

	public Vector2 anchorB;

	public float jnAcc;

	public float jnMax;

	public Vector2 jAcc;

	public float jMaxLen;

	public float dist;

	public float min;

	public float max;

	public Vector2 n;

	public float nMass;

	public Vector2 k1;

	public Vector2 k2;

	public Vector2 r1;

	public Vector2 r2;

	public float iSum;

	public float ratio;

	public float phase;
}
