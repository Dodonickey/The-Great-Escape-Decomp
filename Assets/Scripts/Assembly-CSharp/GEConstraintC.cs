using System;
using UnityEngine;

public class GEConstraintC : BasicControlledComponent
{
	public ConstraintType constraintType;

	public ChipmunkC CMC;

	public bool connectedToWorld;

	public IntPtr slideJointPtr;

	public IntPtr connectJointPtr;

	public IntPtr rotaryMotorPtr;

	public IntPtr rotarySpringPtr;

	public IntPtr rotaryLimitJointPtr;

	public IntPtr rotaryStiffnessPtr;

	public IntPtr railJointPtr;

	public ChipmunkC[] connectedBodies;

	public Vector2[] connectedBodyLocalAnchors;

	public AnchorPointInfo[] anchorPoints;

	public TweenStyle railTweenStyle;

	public int railInterpolationStyle;

	public bool railClosed;

	public bool updateRail;

	public int currentIndex;

	public float currentRailPos;

	public float moveFromPoint;

	public int currentRepeats;

	public int maxRepeats;

	public GEConstraintC rail;

	public IntPtr railedPivotJointPtr;

	public IntPtr railedSlideJointAPtr;

	public IntPtr railedDampedSpringAPtr;

	public TransformC railedSlideJointATC;

	public IntPtr railedSlideJointBPtr;

	public IntPtr railedDampedSpringBPtr;

	public TransformC railedSlideJointBTC;

	public Vector3 pivotOffset;

	public bool linearMotor;

	public bool linearMotorEnabled;

	public float linearMotorMaxForce;

	public float linearMotorRate;

	public int loopStyle;

	public int linearMotorDirection;

	public bool rotaryMotorEnabled;

	public float rotaryMotorRate;

	public float rotaryMotorMaxForce;

	public bool motorIsStiff;

	public ChipmunkC[] ropeCMCs;

	public PrefabC PC;

	public LineRenderer lineRenderer;

	public bool hasLimits;

	public float ropeMaxLength;

	public float ropeLength;

	public float ropeMinLength;

	public bool isCuttable;

	public bool isFlexible;

	public float flexForce;

	public float flexDamp;

	public bool flexDisabled;

	public float flexRestLength;

	public bool isRigid;

	public float ropeCutTime;
}
