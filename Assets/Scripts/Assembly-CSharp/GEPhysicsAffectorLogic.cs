using UnityEngine;

public static class GEPhysicsAffectorLogic
{
	public static void UpdatePhysicsAffector(GEPhysicsAffectorC _c)
	{
		if (_c.affectUntil <= 0f || Main.m_gameTime < _c.affectUntil)
		{
			int count = _c.cmcs.Count;
			if (count > 0)
			{
				if (_c.isForce)
				{
				}
				if (_c.isImpulse)
				{
					float num = _c.amount / (float)count;
					Vector2 j = num * _c.direction * 100f;
					for (int i = 0; i < count; i++)
					{
						ChipmunkWrapper.ApplyImpulse(_c.cmcs[i].cpBodyPtr, j, _c.point, _c.relative);
					}
				}
				if (_c.isVelocity)
				{
					Vector2 vel = _c.amount * _c.direction * 5f;
					for (int k = 0; k < count; k++)
					{
						ChipmunkWrapper.ActivateBody(_c.cmcs[k].cpBodyPtr);
						ChipmunkWrapper.SetVelocity(_c.cmcs[k].cpBodyPtr, vel);
					}
				}
				if (!_c.isAngularVelocity)
				{
				}
			}
			if (_c.affectUntil == 0f)
			{
				GES.RemovePhysicsAffectorComponent(_c);
			}
		}
		else
		{
			GES.RemovePhysicsAffectorComponent(_c);
		}
	}
}
