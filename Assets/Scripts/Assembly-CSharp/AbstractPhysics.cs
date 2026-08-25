using UnityEngine;

public static class AbstractPhysics
{
	public static PhysicsEngine m_physicsEngine;

	private static IPhysicsSystem m_currentEngine;

	public static void Initialize(PhysicsEngine _engine, int _maxComponents)
	{
		ChipmunkS.Initialize(_maxComponents, 30);
	}

	public static void CreateWorld(Vector3 _gravity, int _steps, float _damping, float _sleepTreshold, float _collisionSlop)
	{
		ChipmunkWrapper.CreateWorld(_gravity, _steps, _damping, _sleepTreshold, _collisionSlop);
	}

	public static void Update(float _dt)
	{
		ChipmunkS.Update(_dt);
	}
}
