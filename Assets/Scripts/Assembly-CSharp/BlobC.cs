using System.Collections.Generic;
using UnityEngine;

public class BlobC : BasicComponent
{
	public BlobType blobType;

	public TouchAreaC TAC;

	public float radius;

	public List<ChipmunkC> feet;

	public PrefabC skin;

	public float touchStart;

	public TransformC aimTC;

	public bool aiming;

	public bool launched;

	public float aimStart;

	public float launchTime;

	public List<float> collidingUnitFirstTouched;

	public List<BlobC> collidingUnits;

	public List<int> collidingUnitTouchCounts;

	public int willMergeWithIndex;

	public bool merged;

	public bool doNotMerge;

	public Vector3 bornPos;

	public float friction;

	public float elasticy;

	public float minElasticy;

	public float shapeDamp;

	public float segmentLength;

	public BGoalC goal;
}
