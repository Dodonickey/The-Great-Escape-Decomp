using System;
using UnityEngine;

public static class PBSystem
{
	public static GenericArray<PBPadC> m_padComponents;

	public static int m_padCount = 20;

	public static void Initialize()
	{
		m_padComponents = new GenericArray<PBPadC>(m_padCount);
		for (int i = 0; i < m_padCount; i++)
		{
			m_padComponents.m_array[i] = new PBPadC();
			m_padComponents.m_array[i].entityIndex = -1;
			m_padComponents.m_array[i].index = i;
			m_padComponents.m_array[i].componentType = (ComponentType)70;
		}
		ChipmunkS.AddCollisionInterest(true, true, false, ColliderType.Any, (ColliderType)26, HandleROUNDBUMPER);
		ChipmunkS.AddCollisionInterest(true, true, false, ColliderType.Any, (ColliderType)27, HandleWALLBUMPER);
	}

	private static void HandleROUNDBUMPER(ChipmunkCollisionPair _collisionPair, ChipmunkCollisionList _collisionList)
	{
		ChipmunkC chipmunkC = ChipmunkS.m_components.m_array[_collisionPair.componentIndexA];
		ChipmunkC chipmunkC2 = ChipmunkS.m_components.m_array[_collisionPair.componentIndexB];
		if (chipmunkC2.colliderType != (ColliderType)26)
		{
			chipmunkC = ChipmunkS.m_components.m_array[_collisionPair.componentIndexB];
			chipmunkC2 = ChipmunkS.m_components.m_array[_collisionPair.componentIndexA];
		}
		ChipmunkWrapper.ApplyImpulse(chipmunkC.cpBodyPtr, _collisionPair.normal.normalized * -1500f, ChipmunkWrapper.GetLocalPos(chipmunkC.cpBodyPtr, _collisionPair.pos), true);
	}

	private static void HandleWALLBUMPER(ChipmunkCollisionPair _collisionPair, ChipmunkCollisionList _collisionList)
	{
		ChipmunkC chipmunkC = ChipmunkS.m_components.m_array[_collisionPair.componentIndexA];
		ChipmunkC chipmunkC2 = ChipmunkS.m_components.m_array[_collisionPair.componentIndexB];
		if (chipmunkC2.colliderType != (ColliderType)27)
		{
			chipmunkC = ChipmunkS.m_components.m_array[_collisionPair.componentIndexB];
			chipmunkC2 = ChipmunkS.m_components.m_array[_collisionPair.componentIndexA];
		}
		ChipmunkWrapper.ApplyImpulse(chipmunkC.cpBodyPtr, _collisionPair.normal.normalized * -1500f, ChipmunkWrapper.GetLocalPos(chipmunkC.cpBodyPtr, _collisionPair.pos), true);
	}

	public static PBPadC AddPadComponent(ChipmunkC _cmc, float _restAngle, bool _isLeft, IntPtr _rotaryMotorPtr)
	{
		int num = m_padComponents.AddItem();
		PBPadC pBPadC = m_padComponents.m_array[num];
		pBPadC.active = true;
		pBPadC.entityIndex = _cmc.entityIndex;
		pBPadC.CMC = _cmc;
		pBPadC.restAngle = _restAngle;
		pBPadC.isLeft = _isLeft;
		pBPadC.motorPtr = _rotaryMotorPtr;
		EntityManager.m_entities.m_array[pBPadC.entityIndex].components.Add(pBPadC);
		return pBPadC;
	}

	public static void RemovePadComponent(IComponent _c)
	{
		PBPadC pBPadC = _c as PBPadC;
		pBPadC.active = false;
		pBPadC.CMC = null;
		pBPadC.isTriggered = false;
		pBPadC.motorPtr = IntPtr.Zero;
		pBPadC.noMoreForce = false;
		m_padComponents.RemoveItem(_c.index);
		EntityManager.m_entities.m_array[pBPadC.entityIndex].components.Remove(_c);
	}

	public static IControlledComponent GetControlledComponentWithUniqueId(uint _id)
	{
		return null;
	}

	public static void Update()
	{
		int aliveCount = m_padComponents.m_aliveCount;
		for (int i = 0; i < aliveCount; i++)
		{
			PBPadC pBPadC = m_padComponents.m_array[m_padComponents.m_aliveIndices[i]];
			if (!pBPadC.active)
			{
				continue;
			}
			if (pBPadC.isTriggered)
			{
				if (pBPadC.noMoreForce)
				{
					continue;
				}
				if (Mathf.Abs(pBPadC.CMC.ucpBodyStruct.a - pBPadC.restAngle) < 1.2217305f)
				{
					ChipmunkWrapper.ActivateBody(pBPadC.CMC.cpBodyPtr);
					if (pBPadC.isLeft)
					{
						ChipmunkWrapper.SetMotorProperties(pBPadC.motorPtr, -17.453293f, 100000000f);
					}
					else
					{
						ChipmunkWrapper.SetMotorProperties(pBPadC.motorPtr, 17.453293f, 100000000f);
					}
					continue;
				}
				pBPadC.noMoreForce = true;
				if (pBPadC.isLeft)
				{
					ChipmunkWrapper.SetAngle(pBPadC.CMC.cpBodyPtr, pBPadC.restAngle + 1.2217305f);
				}
				else
				{
					ChipmunkWrapper.SetAngle(pBPadC.CMC.cpBodyPtr, pBPadC.restAngle - 1.2217305f);
				}
				ChipmunkWrapper.SetAngularVelocity(pBPadC.CMC.cpBodyPtr, 0f);
				ChipmunkWrapper.SetMotorProperties(pBPadC.motorPtr, 0f, 1E+09f);
				continue;
			}
			pBPadC.noMoreForce = false;
			if (pBPadC.isLeft)
			{
				float num = pBPadC.CMC.ucpBodyStruct.a - pBPadC.restAngle;
				if (num > 0f)
				{
					ChipmunkWrapper.SetMotorProperties(pBPadC.motorPtr, 8.726646f, 1000000f);
					continue;
				}
				if (pBPadC.CMC.ucpBodyStruct.a > pBPadC.restAngle)
				{
					ChipmunkWrapper.SetAngle(pBPadC.CMC.cpBodyPtr, pBPadC.restAngle);
				}
				ChipmunkWrapper.SetMotorProperties(pBPadC.motorPtr, 0f, 1000000f);
				ChipmunkWrapper.SetAngularVelocity(pBPadC.CMC.cpBodyPtr, 0f);
				continue;
			}
			float num2 = pBPadC.CMC.ucpBodyStruct.a - pBPadC.restAngle;
			if (num2 < 0f)
			{
				ChipmunkWrapper.SetMotorProperties(pBPadC.motorPtr, -8.726646f, 1000000f);
				continue;
			}
			if (pBPadC.CMC.ucpBodyStruct.a < pBPadC.restAngle)
			{
				ChipmunkWrapper.SetAngle(pBPadC.CMC.cpBodyPtr, pBPadC.restAngle);
			}
			ChipmunkWrapper.SetMotorProperties(pBPadC.motorPtr, 0f, 1000000f);
			ChipmunkWrapper.SetAngularVelocity(pBPadC.CMC.cpBodyPtr, 0f);
		}
	}
}
