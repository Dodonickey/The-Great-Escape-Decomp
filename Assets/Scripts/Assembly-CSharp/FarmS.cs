using UnityEngine;

public static class FarmS
{
	private static int m_slingCount = 20;

	public static GenericArray<FSlingC> m_slingComponents;

	public static void Initialize()
	{
		m_slingComponents = new GenericArray<FSlingC>(m_slingCount);
		for (int i = 0; i < m_slingCount; i++)
		{
			m_slingComponents.m_array[i] = new FSlingC();
			m_slingComponents.m_array[i].entityIndex = -1;
			m_slingComponents.m_array[i].index = i;
			m_slingComponents.m_array[i].componentType = (ComponentType)40;
			m_slingComponents.m_array[i].BeganEventDelegate = GETriggerLogic.DefaultBeganTriggerHandler;
		}
		ChipmunkS.AddCollisionInterest(true, false, false, (ColliderType)12, (ColliderType)20, FSlingLogic.HandleVEHICLEtoSLINGCollisions);
	}

	public static FSlingC AddSlingComponent(Entity _e, ChipmunkC _cmc, ChipmunkC _rootCMC, Vector3 _pos, float _maxRange)
	{
		int num = m_slingComponents.AddItem();
		FSlingC fSlingC = m_slingComponents.m_array[num];
		fSlingC.active = true;
		fSlingC.entityIndex = _e.index;
		fSlingC.CMC = _cmc;
		fSlingC.rootCMC = _rootCMC;
		fSlingC.touchCMC = null;
		fSlingC.vehicle = null;
		fSlingC.restPos = _pos;
		fSlingC.maxRange = _maxRange;
		fSlingC.launched = false;
		_e.components.Add(fSlingC);
		return fSlingC;
	}

	public static void RemoveSlingComponent(IComponent _c)
	{
		FSlingC fSlingC = _c as FSlingC;
		fSlingC.active = false;
		fSlingC.CMC = null;
		fSlingC.rootCMC = null;
		fSlingC.touchCMC = null;
		fSlingC.vehicle = null;
		fSlingC.restPos = Vector3.zero;
		fSlingC.ready = false;
		fSlingC.launched = false;
		fSlingC.armed = false;
		fSlingC.slingSC = null;
		fSlingC.slingTC = null;
		fSlingC.knotSC = null;
		fSlingC.triggerC = null;
		fSlingC.PC = null;
		fSlingC.isGoal = false;
		m_slingComponents.RemoveItem(_c.index);
		EntityManager.m_entities.m_array[fSlingC.entityIndex].components.Remove(_c);
	}

	public static void Update()
	{
		int aliveCount = m_slingComponents.m_aliveCount;
		for (int i = 0; i < aliveCount; i++)
		{
			FSlingC fSlingC = m_slingComponents.m_array[m_slingComponents.m_aliveIndices[i]];
			if (fSlingC.active)
			{
				FSlingLogic.Update(fSlingC);
			}
		}
	}
}
