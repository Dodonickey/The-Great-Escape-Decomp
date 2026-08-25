using UnityEngine;

public class FSlingC : BasicControlledComponent
{
	public ChipmunkC rootCMC;

	public ChipmunkC CMC;

	public ChipmunkC touchCMC;

	public PrefabC PC;

	public GEVehicleC vehicle;

	public Vector3 restPos;

	public float maxRange;

	public bool launched;

	public bool armed;

	public float lastLaunch;

	public bool ready;

	public Vector2 launchPos;

	public SpriteC slingSC;

	public SpriteC knotSC;

	public TransformC slingTC;

	public GETriggerC triggerC;

	public bool isGoal;

	public ChipmunkC connectedCMC;
}
