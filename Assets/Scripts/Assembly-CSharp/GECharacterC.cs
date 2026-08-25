using System;

public class GECharacterC : GECreatureC
{
	public EmotionState emotionState;

	public float emotionStateChanged;

	public SpritePrefabNode[] eyes;

	public SpritePrefabNode[] legs;

	public SpritePrefabNode[] arms;

	public SpritePrefabNode[] heads;

	public GESpritePrefabC hatSPC;

	public float balanceAngle;

	public GEVehicleC vehicle;

	public IntPtr vehicleSpringPtr;

	public IntPtr vehicleConnectionPtr;

	public IntPtr vehicleRotarySpringPtr;

	public IntPtr[] headSprings;

	public IntPtr[] armSprings;

	public IntPtr[] legSprings;
}
