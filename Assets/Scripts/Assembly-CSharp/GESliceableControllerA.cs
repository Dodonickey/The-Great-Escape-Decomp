using UnityEngine;

public static class GESliceableControllerA
{
	public static GETriggerC Assemble(EIC _eic, TriggerData _data)
	{
		string[] tags = new string[2]
		{
			LevelManager.m_currentLevel.name + ":Controller",
			LevelManager.m_currentLevel.name
		};
		Entity entity = EntityManager.AddEntity(tags);
		TransformC transformC = TransformS.AddComponent(entity);
		TransformS.SetPosition(transformC, _data.position.ToVector3());
		GETriggerC gETriggerC = GES.AddTriggerComponent(_eic.camera, _data, TriggerType.FingerController, transformC);
		gETriggerC.inputSlots = new ConnectionSlot[0];
		gETriggerC.outputSlots = new ConnectionSlot[1];
		gETriggerC.outputSlots[0] = new ConnectionSlot(ConnectionSlotType.Output, 0);
		gETriggerC.modifierSlots = new ConnectionSlot[3];
		gETriggerC.modifierSlots[0] = new ConnectionSlot(ConnectionSlotType.Activate, 0);
		gETriggerC.modifierSlots[1] = new ConnectionSlot(ConnectionSlotType.Deactivate, 1);
		gETriggerC.modifierSlots[2] = new ConnectionSlot(ConnectionSlotType.Destroy, 2);
		_eic.trigger = gETriggerC;
		TouchAreaC touchAreaC = null;
		ChipmunkC chipmunkC = null;
		if (_eic.container != null && _eic.container.identifier == "Block")
		{
			GEBlockC gEBlockC = _eic.container.gameComponents[0] as GEBlockC;
			float num = 999999f;
			float num2 = -999999f;
			float num3 = 999999f;
			float num4 = -999999f;
			for (int i = 0; i < gEBlockC.modifiedShape.Contour.Length; i++)
			{
				VertexList vertexList = gEBlockC.modifiedShape.Contour[i];
				for (int j = 0; j < vertexList.NofVertices; j++)
				{
					num = Mathf.Min(vertexList.Vertex[j].x, num);
					num2 = Mathf.Max(vertexList.Vertex[j].x, num2);
					num3 = Mathf.Min(vertexList.Vertex[j].y, num3);
					num4 = Mathf.Max(vertexList.Vertex[j].y, num4);
				}
			}
			float width = num2 - num;
			float height = num4 - num3;
			if (!GEState.editorMode)
			{
				touchAreaC = TouchAreaS.AddComponent(transformC, "finger", width, height, true, _eic.camera, gETriggerC);
				touchAreaC.scaleByCameraDistance = true;
				TouchAreaS.AddTouchEventListener(touchAreaC, HandleTouches);
				gETriggerC.fingerTAC = touchAreaC;
				gETriggerC.fingerBC = gEBlockC;
			}
			TransformS.ParentComponent(transformC, gEBlockC.CMC.TC, Vector3.zero);
			TransformS.SetRotation(transformC, Vector3.zero);
			PrefabS.CreatePathPrefabComponentFromPolygon(transformC, Vector3.zero, gEBlockC.modifiedShape, 6f, Color.red, ResourceManager.GetMaterial("Line6"), Main.camera, Position.Center, true);
		}
		return gETriggerC;
	}

	public static EIC CreateNewEditorItem(EIC _container, string _identifier, Vector3 _pos, Vector3 _rot, Vector3 _sca)
	{
		TriggerData triggerData = new TriggerData();
		triggerData.position = new Vertex3(_pos);
		triggerData.rotation = new Vertex3(_rot);
		triggerData.scale = new Vertex3(_sca);
		triggerData.triggerType = 13u;
		triggerData.active = true;
		triggerData.toggle = false;
		triggerData.triggerOnlyOnce = false;
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
		TriggerData data = _eic.data as TriggerData;
		GETriggerC item = Assemble(_eic, data);
		_eic.gameComponents.Add(item);
	}

	private static void HandleTouches(TouchAreaC _c, int _i, bool _consumed)
	{
		if (_c.touchEvent[_i] == TouchEvent.Slice)
		{
			RemoveChildren(_c.TC.parent);
		}
	}

	private static void RemoveChildren(TransformC _tc)
	{
		while (_tc.childs.Count > 0)
		{
			RemoveChildren(_tc.childs[0]);
			_tc.childs.RemoveAt(0);
		}
		EntityManager.RemoveEntity(_tc.entityIndex);
	}
}
