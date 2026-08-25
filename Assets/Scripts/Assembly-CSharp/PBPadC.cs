using System;

public class PBPadC : BasicControlledComponent
{
	public ChipmunkC CMC;

	public IntPtr motorPtr;

	public float restAngle;

	public bool isLeft;

	public bool isTriggered;

	public bool noMoreForce;
}
