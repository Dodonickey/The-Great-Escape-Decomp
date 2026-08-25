using UnityEngine;

public static class GECollectibleA
{
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
		DebugDraw.CreateCircle(_eic.camera, transformC, Vector2.zero, 5f, false);
		SpriteS.SetColorByTransformComponent(transformC, Color.yellow, false, false);
		ChipmunkC chipmunkC = ChipmunkS.AddInactiveComponent(transformC, true, (ColliderType)11, 0u, 17895697u, false, false);
		ChipmunkS.ActivateChipmunkComponent(chipmunkC, ChipmunkWrapper.AddBody(chipmunkC.isStatic, chipmunkC.isRogue, triggerData.position.ToVector2(), chipmunkC.index, chipmunkC.colliderType));
		ChipmunkWrapper.AddCircleShape(chipmunkC.cpBodyPtr, Vector2.zero, 1f, 5f, 0f, 0f, chipmunkC.colliderGroup, chipmunkC.colliderLayer, true);
		GETriggerC gETriggerC = (GETriggerC)(chipmunkC.customComponent = GES.AddTriggerComponent(_eic.camera, triggerData, chipmunkC));
		gETriggerC.collisionHandler = HandleCollectible;
		gETriggerC.triggerType = TriggerType.CollectibleTrigger;
		gETriggerC.inputSlots = new ConnectionSlot[1];
		gETriggerC.inputSlots[0] = new ConnectionSlot(ConnectionSlotType.Input, 0);
		gETriggerC.outputSlots = new ConnectionSlot[1];
		gETriggerC.outputSlots[0] = new ConnectionSlot(ConnectionSlotType.Output, 0);
		if (!triggerData.active)
		{
			EntityManager.SetActivityOfEntity(gETriggerC.entityIndex, triggerData.active, true);
		}
		return gETriggerC;
	}

	public static void HandleCollectible(GETriggerC _trigger, ChipmunkC _collidingCMC, ChipmunkCollisionPair _collisionPair, ChipmunkCollisionList _collisionList)
	{
		if (_collidingCMC.customComponent != null && _trigger.triggered && _collidingCMC.colliderType == (ColliderType)3 && _collisionList == ChipmunkCollisionList.BEGIN)
		{
			_trigger.collidingCount++;
			if (_trigger.collidingCount == 1)
			{
				GETriggerLogic.HandleBeginTriggerEvent(_trigger);
				EntityManager.SetVisibilityOfEntity(_trigger.entityIndex, false);
			}
		}
	}

	public static EIC CreateNewEditorItem(EIC _container, string _identifier, Vector3 _pos, Vector3 _rot, Vector3 _sca)
	{
		TriggerData triggerData = new TriggerData();
		triggerData.position = new Vertex3(_pos);
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
