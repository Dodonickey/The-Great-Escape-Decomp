using System;
using System.Collections.Generic;
using UnityEngine;

public class GEVehicleC : GECreatureC
{
	public VehicleType vehicleType;

	public IntPtr[] tireConstraints;

	public SpritePrefabNode[] tires;

	public SpritePrefabNode[] crawlers;

	public Entity[] tireEffects;

	public float[] tireEffectTimes;

	public bool hasBrakes;

	public bool braking;

	public List<ChipmunkC> touchingColliders;

	public float lastContact;

	public float firstContact;

	public float lastJump;

	public float jumpPower;

	public float flyPower;

	public Vector2 contactNormal;

	public List<GECharacterC> characters;

	public int characterLimit;

	public List<SpritePrefabNode> seats;

	public List<GECreatureC> seatsTaken;

	public ChipmunkC carriedCMC;

	public ChipmunkC focusCMC;

	public bool carrying;

	public bool dragging;

	public bool hanging;

	public uint currentLayer;

	public uint currentGroup;

	public bool backBlocked;

	public bool frontBlocked;

	public IntPtr balanceSpring;

	public float currentBalance;

	public float characterBalanceAngle;

	public float currentBalanceDif;

	public Vector2 currentLookDir;

	public Vector2 currentLookNormal;

	public float currentBrakeAmount;

	public PlayerState playerState;

	public float desiredZ;

	public bool updateCharacterDepth;

	public SoundC driveSoundLoop;
}
