using System.Collections.Generic;
using UnityEngine;

public class GETriggerC : BasicControlledComponent
{
	public TriggerData data;

	public ChipmunkC CMC;

	public EventC EC;

	public int actionType;

	public ChipmunkC connectedCMC;

	public List<ColliderType> listenedColliderTypes;

	public bool running;

	public float currentTime;

	public float duration;

	public float waitBeforeStart;

	public TouchAreaC fingerTAC;

	public List<ChipmunkC> fingerCMC;

	public List<int> fingerTouchIndices;

	public bool dragging;

	public Vector3 touchOffset;

	public List<ChipmunkCollisionPair> fingerCollisions;

	public GEBlockC fingerBC;

	public bool fingerColliding;

	public ChipmunkC fingerTouchCMC;

	public Vector2 fingerPrevWorldPos;

	public Vector2 fingerLastNonCollidingPos;

	public float fingerStartAngle;

	public ChipmunkC fingerSensorCMC;

	public Vector2 fingerLocalPos;

	public bool selector;

	public int selectedIndex;

	public bool hideUnselected;

	public TriggerCollisionDelegate collisionHandler;

	public bool dispatched;

	public bool dispatchOnlyOnce;

	public TransformC tileTC;
}
