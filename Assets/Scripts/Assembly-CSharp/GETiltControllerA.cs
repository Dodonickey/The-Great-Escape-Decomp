using UnityEngine;

public static class GETiltControllerA
{
	public static GETriggerC Assemble(EIC _eic, TriggerData _data)
	{
		string[] tags = new string[2]
		{
			LevelManager.m_currentLevel.name + ":Controller",
			LevelManager.m_currentLevel.name
		};
		TransformC transformC = EntityManager.AddEntityWithTC(tags);
		TransformS.SetPosition(transformC, _data.position.ToVector3());
		TransformC transformC2 = TransformS.AddComponent(transformC.entityIndex);
		TransformS.ParentComponent(transformC2, transformC, Vector3.zero);
		GETriggerC gETriggerC = GES.AddTriggerComponent(_eic.camera, _data, (TriggerType)_data.triggerType, transformC);
		gETriggerC.autoTrigger = true;
		gETriggerC.inputSlots = new ConnectionSlot[0];
		gETriggerC.outputSlots = new ConnectionSlot[1];
		gETriggerC.outputSlots[0] = new ConnectionSlot(ConnectionSlotType.Output, 0);
		gETriggerC.modifierSlots = new ConnectionSlot[2];
		gETriggerC.modifierSlots[0] = new ConnectionSlot(ConnectionSlotType.Activate, 0);
		gETriggerC.modifierSlots[1] = new ConnectionSlot(ConnectionSlotType.Deactivate, 1);
		gETriggerC.update = true;
		Vector2[] circle = DebugDraw.GetCircle(10f, 36, Vector2.zero, false);
		PrefabS.CreatePathPrefabComponentFromVectorArray(transformC2, Vector3.zero, circle, 6f, new Color(1f, 1f, 1f), ResourceManager.GetMaterial("Line6"), Main.uiCamera, Position.Center, true);
		circle = DebugDraw.GetCircle(100f, 36, Vector2.zero, false);
		PrefabS.CreatePathPrefabComponentFromVectorArray(transformC, Vector3.zero, circle, 6f, new Color(1f, 1f, 1f), ResourceManager.GetMaterial("Line6"), Main.uiCamera, Position.Center, true);
		if (!_data.active)
		{
			EntityManager.SetActivityOfEntity(gETriggerC.entityIndex, _data.active, true);
		}
		_eic.trigger = gETriggerC;
		if (!GEState.editorMode)
		{
			KeyboardInputManager.RegisterTiltController(gETriggerC);
		}
		return gETriggerC;
	}

	public static EIC CreateNewEditorItem(EIC _container, string _identifier, Vector3 _pos, Vector3 _rot, Vector3 _sca)
	{
		TriggerData triggerData = new TriggerData();
		triggerData.position = new Vertex3(_pos);
		triggerData.rotation = new Vertex3(_rot);
		triggerData.scale = new Vertex3(_sca);
		triggerData.triggerType = 25u;
		triggerData.active = true;
		triggerData.toggle = false;
		triggerData.triggerOnlyOnce = false;
		triggerData.triggerUntilOutOfEnergy = false;
		triggerData.triggerOnlyOnFullEnergy = false;
		triggerData.autoTrigger = false;
		triggerData.energy = 1f;
		triggerData.energyClips = -1;
		triggerData.energyGain = 0f;
		triggerData.energyConsume = 0f;
		triggerData.gainInterval = 0f;
		triggerData.consumeInterval = 0f;
		triggerData.cooldown = 0f;
		uint uniqueId = GES.GetUniqueId();
		triggerData.Init(uniqueId, _identifier + uniqueId);
		EIC eIC = GEItemA.Assemble(_container, _identifier, triggerData, Main.uiCamera);
		eIC.isRealtimeMovable = true;
		eIC.isDrawable = false;
		eIC.isRotateable = false;
		eIC.isScaleable = false;
		eIC.isScaleUnified = false;
		return eIC;
	}

	public static EIC CreateLoadedEditorItem(EIC _container, EIC _loadedItem)
	{
		return GEItemA.Assemble(_container, _loadedItem.identifier, _loadedItem.data, Main.uiCamera);
	}

	public static void FillEditorItem(EIC _eic)
	{
		TriggerData data = _eic.data as TriggerData;
		GETriggerC gETriggerC = Assemble(_eic, data);
		_eic.gameComponents.Add(gETriggerC);
		if (GEState.editorMode)
		{
			TransformS.ParentComponent(gETriggerC.TC, _eic.TC, Vector3.zero);
		}
	}
}
