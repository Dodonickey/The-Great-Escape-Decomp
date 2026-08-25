using System;
using UnityEngine;

public class GEBlockC : BasicComponent
{
	public float created;

	public ChipmunkC CMC;

	public Polygon originalShape;

	public Polygon modifiedShape;

	public float area;

	public GroundSettings groundSettings;

	public Vector2 linearDamp;

	public float angularDamp;

	public Vector2 gravity;

	public bool isOneway;

	public Vector2 oneWayDirection;

	public bool isBreakable;

	public float breakingImpulse;

	public int breakEvent;

	public float breakEventScale;

	public bool isPowerLane;

	public uint powerLaneType;

	public Vector2 powerLaneDirection;

	public float powerLaneForce;

	public IntPtr powerLaneShape;
}
