using System.Collections.Generic;
using UnityEngine;

public class GEPhysicsAffectorC : BasicComponent
{
	public bool isImpulse;

	public bool isForce;

	public bool isVelocity;

	public bool isAngularVelocity;

	public Vector2 point;

	public Vector2 direction;

	public float amount;

	public float affectUntil;

	public bool relative;

	public List<ChipmunkC> cmcs;
}
