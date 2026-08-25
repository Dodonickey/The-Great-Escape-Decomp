using System;
using UnityEngine;

public class SpritePrefabNode
{
	public string name;

	public int index;

	public int parentIndex;

	public int isProp;

	public int notVisual;

	public int visibility;

	public int isEye;

	public int isLeg;

	public int isArm;

	public int isHead;

	public int isTire;

	public int isCrawler;

	public int hasPhysics;

	public int colliderShape;

	public int isStatic;

	public int isSensor;

	public int transformDictates;

	public int dictateRotation;

	public float dictatedRotation;

	public float mass;

	public float elasticity;

	public float friction;

	public Vector3 linearDamp;

	public float angularDamp;

	public Vector3 gravity;

	public int hasMotor;

	public float motorStrength;

	public float motorRate;

	public int hasSuspension;

	public float suspensionStrength;

	public float suspensionDamp;

	public float suspensionDepth;

	public int hasRotarySpring;

	public float rotarySpringStrength;

	public float rotarySpringDamp;

	public int reactToWind;

	public int hasRotaryLimits;

	public float minRotaryLimit;

	public float maxRotaryLimit;

	public float maxAngularVelocity;

	public Vector3 localPosition;

	public Vector3 localRotation;

	public Vector3 localScale;

	public Vector3 globalPosition;

	public Vector3 globalRotation;

	public Vector3 localCenter;

	public Vector3 globalCenter;

	public Vector3[] vertices;

	public Vector2[] uvs;

	public float width;

	public float height;

	public float uvX;

	public float uvY;

	public float uvWidth;

	public float uvHeight;

	public float sortValue;

	public Vector3[] collisionVertices;

	public TransformC TC;

	public ChipmunkC CMC;

	public SpriteC SC;

	public SpriteSheet spriteSheet;

	public TiledSpriteSheet tiledSpriteSheet;

	public IntPtr pivotJoint;

	public IntPtr rotarySpring;

	public IntPtr rotaryLimitJoint;

	public IntPtr motor;

	public IntPtr dampedSpring;

	public IntPtr grooveJoint;
}
