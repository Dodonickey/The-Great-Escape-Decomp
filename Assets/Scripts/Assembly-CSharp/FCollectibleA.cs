using System;
using UnityEngine;

public static class FCollectibleA
{
	private static int m_lastCollectTime;

	private static float m_soundPitch;

	public static GETriggerC Assemble(EIC _eic)
	{
		TriggerData triggerData = _eic.data as TriggerData;
		string[] tags = new string[2]
		{
			LevelManager.m_currentLevel.name + ":GameEntity",
			LevelManager.m_currentLevel.name
		};
		Entity entity = EntityManager.AddEntity(tags);
		TransformC transformC = TransformS.AddComponent(entity);
		transformC.forceRotation = true;
		ChipmunkC chipmunkC = ChipmunkS.AddInactiveComponent(transformC, true, (ColliderType)11, 0u, 17895697u, false, false);
		ChipmunkS.ActivateChipmunkComponent(chipmunkC, ChipmunkWrapper.AddBody(chipmunkC.isStatic, chipmunkC.isRogue, triggerData.position.ToVector2(), chipmunkC.index, chipmunkC.colliderType));
		ChipmunkWrapper.AddCircleShape(chipmunkC.cpBodyPtr, Vector2.zero, 1f, 5f, 0f, 0f, chipmunkC.colliderGroup, chipmunkC.colliderLayer, true);
		TransformS.SetPosition(transformC, triggerData.position.ToVector3());
		GETriggerC gETriggerC = (GETriggerC)(chipmunkC.customComponent = GES.AddTriggerComponent(_eic.camera, triggerData, chipmunkC));
		gETriggerC.triggerType = TriggerType.CollectibleTrigger;
		gETriggerC.inputSlots = new ConnectionSlot[1];
		gETriggerC.inputSlots[0] = new ConnectionSlot(ConnectionSlotType.Input, 0);
		gETriggerC.outputSlots = new ConnectionSlot[1];
		gETriggerC.outputSlots[0] = new ConnectionSlot(ConnectionSlotType.Output, 0);
		PrefabC prefabC = null;
		Transform transform = null;
		if (_eic.identifier == "Carrot")
		{
			prefabC = PrefabS.AddComponent(transformC, Vector3.zero, ResourceManager.GetGameObject("Carrot"));
			gETriggerC.collisionHandler = HandleCarrot;
		}
		else if (_eic.identifier == "Radish")
		{
			prefabC = PrefabS.AddComponent(transformC, Vector3.zero, ResourceManager.GetGameObject("Radish"));
			gETriggerC.collisionHandler = HandleRadish;
		}
		transform = prefabC.p_gameObject.transform.Find("control2/carrot");
		float num = UnityEngine.Random.Range(0.75f, 1.25f);
		float y = UnityEngine.Random.Range(0.75f, 1.25f);
		if (transform != null)
		{
			transform.localScale = new Vector3(num, y, num);
		}
		prefabC.p_gameObject.GetComponent<Animation>()["Take 001"].time = UnityEngine.Random.Range(0f, 1f);
		if (_eic.container != null && _eic.container.identifier == "Block")
		{
			GEBlockC gEBlockC = _eic.container.gameComponents[0] as GEBlockC;
			TransformS.ParentComponent(transformC, gEBlockC.CMC.TC);
			TransformS.SetGlobalPosition(transformC, _eic.data.position.ToVector3());
		}
		if (!triggerData.active)
		{
			EntityManager.SetActivityOfEntity(gETriggerC.entityIndex, triggerData.active, true);
		}
		return gETriggerC;
	}

	public static void HandleCarrot(GETriggerC _trigger, ChipmunkC _collidingCMC, ChipmunkCollisionPair _collisionPair, ChipmunkCollisionList _collisionList)
	{
		HandleCollectible(_trigger, _collidingCMC, _collisionPair, _collisionList, "CarrotStem");
	}

	public static void HandleRadish(GETriggerC _trigger, ChipmunkC _collidingCMC, ChipmunkCollisionPair _collisionPair, ChipmunkCollisionList _collisionList)
	{
		HandleCollectible(_trigger, _collidingCMC, _collisionPair, _collisionList, "RadishStem");
	}

	public static void HandleCollectible(GETriggerC _trigger, ChipmunkC _collidingCMC, ChipmunkCollisionPair _collisionPair, ChipmunkCollisionList _collisionList, string _info)
	{
		if (!_trigger.triggered || _collisionList != ChipmunkCollisionList.BEGIN)
		{
			return;
		}
		_trigger.collidingCount++;
		if (_trigger.collidingCount == 1)
		{
			GETriggerLogic.HandleBeginTriggerEvent(_trigger);
			EntityManager.SetVisibilityOfEntity(_trigger.entityIndex, false);
			m_soundPitch += 0.2f;
			if (Main.m_gameTime - (float)m_lastCollectTime > 2f)
			{
				m_soundPitch = 1f;
			}
			m_lastCollectTime = (int)Main.m_gameTime;
			SoundS.PlaySound("SoundCollect", _collidingCMC.TC.transform.gameObject, 1f, false, null, 0, m_soundPitch);
			ChipmunkC chipmunkC = null;
			chipmunkC = FCollectibleDerbyA.Assemble(ResourceManager.GetGameObject(_info), _trigger.position + Vector3.up * 15f, new Vector3(UnityEngine.Random.Range(-90, 90), UnityEngine.Random.Range(-90, 90), UnityEngine.Random.Range(-90, 90)), _collidingCMC.ucpBodyStruct.v * 0.5f + new Vector2(UnityEngine.Random.Range(-25, 25), UnityEngine.Random.Range(100, 200)), (float)UnityEngine.Random.Range(-360, 360) * ((float)Math.PI / 180f), _collidingCMC.colliderGroup, _collidingCMC.colliderLayer);
		}
	}

	public static EIC CreateNewEditorItem(EIC _container, string _identifier, Vector3 _pos, Vector3 _rot, Vector3 _sca)
	{
		TriggerData triggerData = new TriggerData();
		triggerData.position = new Vertex3(_pos);
		triggerData.position.z = 50f;
		triggerData.rotation = new Vertex3(_rot);
		triggerData.scale = new Vertex3(_sca);
		triggerData.triggerType = 29u;
		triggerData.active = true;
		triggerData.toggle = false;
		triggerData.triggerOnlyOnce = true;
		triggerData.triggerUntilOutOfEnergy = false;
		triggerData.triggerOnlyOnFullEnergy = false;
		triggerData.autoTrigger = true;
		triggerData.energy = 1f;
		triggerData.energyClips = -1;
		triggerData.energyGain = 0f;
		triggerData.energyConsume = 0f;
		triggerData.gainInterval = 0f;
		triggerData.consumeInterval = 0f;
		triggerData.cooldown = 0f;
		triggerData.defaultNumericValue = new Vertex3(Vector3.one);
		triggerData.defaultTextualValue = string.Empty;
		uint uniqueId = GES.GetUniqueId();
		triggerData.Init(uniqueId, _identifier + uniqueId);
		EIC eIC = GEItemA.Assemble(_container, _identifier, triggerData, Main.camera);
		eIC.isRealtimeMovable = true;
		eIC.isDrawable = false;
		eIC.isRotateable = false;
		eIC.isScaleable = false;
		eIC.isScaleUnified = false;
		return eIC;
	}

	public static EIC CreateLoadedEditorItem(EIC _container, EIC _loadedItem)
	{
		return GEItemA.Assemble(_container, _loadedItem.identifier, _loadedItem.data, Main.camera);
	}

	public static void FillEditorItem(EIC _eic)
	{
		GETriggerC gETriggerC = Assemble(_eic);
		_eic.gameComponents.Add(gETriggerC);
		if (GEState.editorMode)
		{
			TransformS.ParentComponent(gETriggerC.TC, _eic.TC, Vector3.zero);
		}
	}
}
