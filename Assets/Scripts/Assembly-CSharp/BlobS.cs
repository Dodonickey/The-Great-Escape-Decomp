using System.Collections.Generic;
using UnityEngine;

public static class BlobS
{
	public static GenericArray<BlobC> m_blobComponents;

	public static GenericArray<BGoalC> m_goalComponents;

	private static int m_blobCount = 40;

	private static int m_goalCount = 10;

	public static List<BlobC> m_mergeList = new List<BlobC>();

	public static void Initialize()
	{
		m_blobComponents = new GenericArray<BlobC>(m_blobCount);
		for (int i = 0; i < m_blobCount; i++)
		{
			m_blobComponents.m_array[i] = new BlobC();
			m_blobComponents.m_array[i].index = i;
			m_blobComponents.m_array[i].componentType = (ComponentType)60;
		}
		m_goalComponents = new GenericArray<BGoalC>(m_goalCount);
		for (int j = 0; j < m_goalCount; j++)
		{
			m_goalComponents.m_array[j] = new BGoalC();
			m_goalComponents.m_array[j].index = j;
			m_goalComponents.m_array[j].componentType = (ComponentType)61;
		}
		BlobLogic.Initialize();
		BGoalLogic.Initialize();
	}

	public static BlobC AddBlobComponent(Entity _e, Vector3 _pos)
	{
		int num = m_blobComponents.AddItem();
		BlobC blobC = m_blobComponents.m_array[num];
		blobC.active = true;
		blobC.entityIndex = _e.index;
		blobC.launchTime = Main.m_gameTime;
		blobC.feet = new List<ChipmunkC>();
		blobC.bornPos = _pos;
		blobC.collidingUnitFirstTouched = new List<float>();
		blobC.collidingUnits = new List<BlobC>();
		blobC.collidingUnitTouchCounts = new List<int>();
		blobC.willMergeWithIndex = -1;
		blobC.merged = false;
		blobC.doNotMerge = false;
		blobC.aiming = false;
		_e.components.Add(blobC);
		return blobC;
	}

	public static void RemoveBlobComponent(IComponent _c)
	{
		BlobC blobC = _c as BlobC;
		blobC.active = false;
		blobC.TAC = null;
		blobC.launched = false;
		blobC.collidingUnitFirstTouched = null;
		blobC.collidingUnits = null;
		blobC.collidingUnitTouchCounts = null;
		blobC.willMergeWithIndex = -1;
		blobC.merged = false;
		blobC.goal = null;
		if (blobC.aimTC != null)
		{
			EntityManager.RemoveEntity(blobC.aimTC.entityIndex);
			blobC.aimTC = null;
		}
		m_blobComponents.RemoveItem(_c.index);
		EntityManager.m_entities.m_array[blobC.entityIndex].components.Remove(_c);
		_c.entityIndex = -1;
	}

	public static BGoalC AddGoalComponent(ChipmunkC _cmc, float _radius)
	{
		int num = m_goalComponents.AddItem();
		BGoalC bGoalC = m_goalComponents.m_array[num];
		bGoalC.active = true;
		bGoalC.entityIndex = _cmc.entityIndex;
		bGoalC.radius = _radius;
		bGoalC.CMC = _cmc;
		bGoalC.CMC.customComponent = bGoalC;
		EntityManager.m_entities.m_array[bGoalC.entityIndex].components.Add(bGoalC);
		return bGoalC;
	}

	public static void RemoveGoalComponent(IComponent _c)
	{
		BGoalC bGoalC = _c as BGoalC;
		bGoalC.active = false;
		bGoalC.CMC = null;
		bGoalC.blob = null;
		bGoalC.trigger = null;
		m_goalComponents.RemoveItem(_c.index);
		EntityManager.m_entities.m_array[bGoalC.entityIndex].components.Remove(_c);
		_c.entityIndex = -1;
	}

	public static IControlledComponent GetControlledComponentWithUniqueId(uint _id)
	{
		return null;
	}

	public static void Update()
	{
		DebugDraw.Clear(Main.camera, BlobLogic.tempTC);
		int aliveCount = m_blobComponents.m_aliveCount;
		for (int i = 0; i < aliveCount; i++)
		{
			BlobC blobC = m_blobComponents.m_array[m_blobComponents.m_aliveIndices[i]];
			if (blobC.active)
			{
				BlobLogic.Update(blobC);
			}
		}
		while (m_mergeList.Count > 0)
		{
			BlobC blobC2 = m_mergeList[m_mergeList.Count - 1];
			if (blobC2.willMergeWithIndex != -1)
			{
				BlobLogic.MergeBlob(blobC2, blobC2.collidingUnits[blobC2.willMergeWithIndex]);
			}
			m_mergeList.RemoveAt(m_mergeList.Count - 1);
		}
		aliveCount = m_goalComponents.m_aliveCount;
		for (int j = 0; j < aliveCount; j++)
		{
			BGoalC bGoalC = m_goalComponents.m_array[m_goalComponents.m_aliveIndices[j]];
			if (bGoalC.active)
			{
				BGoalLogic.Update(bGoalC);
			}
		}
	}
}
