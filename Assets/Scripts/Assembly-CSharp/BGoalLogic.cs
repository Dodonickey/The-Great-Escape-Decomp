using UnityEngine;

public static class BGoalLogic
{
	public static void Initialize()
	{
		ChipmunkS.AddCollisionInterest(true, false, false, (ColliderType)20, (ColliderType)21, HandleBLOBtoGOAL);
	}

	private static void HandleBLOBtoGOAL(ChipmunkCollisionPair _collisionPair, ChipmunkCollisionList _collisionList)
	{
		ChipmunkC chipmunkC = ChipmunkS.m_components.m_array[_collisionPair.componentIndexA];
		ChipmunkC chipmunkC2 = ChipmunkS.m_components.m_array[_collisionPair.componentIndexB];
		if (chipmunkC.colliderType != (ColliderType)20)
		{
			chipmunkC = ChipmunkS.m_components.m_array[_collisionPair.componentIndexB];
			chipmunkC2 = ChipmunkS.m_components.m_array[_collisionPair.componentIndexA];
		}
		BlobC blobC = chipmunkC.customComponent as BlobC;
		BGoalC bGoalC = chipmunkC2.customComponent as BGoalC;
		if (bGoalC.blob == null)
		{
			bGoalC.blob = blobC;
			blobC.goal = bGoalC;
			for (int i = 0; i < blobC.feet.Count; i++)
			{
				ChipmunkWrapper.SetCustomBodyLinearDamp(blobC.feet[i].cpBodyPtr, Vector2.one * 0.5f);
			}
		}
	}

	public static void Update(BGoalC _c)
	{
		if (_c.blob != null)
		{
			BlobC blob = _c.blob;
			Vector2 vector = blob.TAC.TC.transform.position - _c.CMC.TC.transform.position;
			for (int i = 0; i < blob.feet.Count; i++)
			{
				ChipmunkWrapper.ApplyImpulse(blob.feet[i].cpBodyPtr, -vector, Vector2.zero, true);
			}
			if (blob.radius >= _c.radius && !_c.trigger.triggered)
			{
				_c.trigger.collidingCount++;
				GETriggerLogic.HandleBeginTriggerEvent(_c.trigger);
			}
		}
	}
}
