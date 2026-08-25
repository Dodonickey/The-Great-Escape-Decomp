using UnityEngine;

public static class GESlingableControllerA
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
		gETriggerC.data = _data;
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
			PrefabS.CreatePathPrefabComponentFromPolygon(transformC, Vector3.zero, gEBlockC.modifiedShape, 6f, Color.magenta, ResourceManager.GetMaterial("Line6"), Main.camera, Position.Center, true);
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
		triggerData.eventDispatchDelay = 100f;
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

	public static void PopulatePropertyBar(EIC _eiC, UIC _propertyBar)
	{
		string[] tags = new string[1] { "propertyBar" };
		TriggerData triggerData = _eiC.data as TriggerData;
		Camera canvasCamera = _propertyBar.canvasCamera;
		UIC component = NumericFieldA.Assemble(Main.uiCamera, "Sling Strength", HandleEventPropertyChange, null, true, Align.Left, 80f, 1f, false, 0f, 9999f, triggerData.eventDispatchDelay, tags);
		UIS.AddToCanvasGrid(LabelA.Assemble(canvasCamera, "Slingable", tags), _propertyBar, true);
		UIS.AddToCanvasGrid(component, _propertyBar, true);
	}

	public static void HandleEventPropertyChange(EventC _c)
	{
		TriggerData triggerData = EditorState.m_selection[0].data as TriggerData;
		switch (_c.identifier)
		{
		case "Flick Strength":
			triggerData.eventDispatchDelay = (float)_c.properties["value"];
			break;
		}
		EditorState.ResetEditorItem(EditorState.m_selection[0]);
	}

	private static void HandleTouches(TouchAreaC _c, int _i, bool _consumed)
	{
		GETriggerC gETriggerC = _c.customComponent as GETriggerC;
		if (_c.touchStartedInside[_i] && _c.touchEvent[_i] == TouchEvent.ReleaseOutside && _c.touchWasDragged[_i])
		{
			Vector2 zero = Vector2.zero;
			Vector2 deltaPosition = InputManager.m_touches[_i].deltaPosition;
			zero = _c.touchStartPos[_i] - _c.touchPos[_i];
			zero *= gETriggerC.data.eventDispatchDelay;
			ChipmunkWrapper.ApplyImpulse(gETriggerC.fingerBC.CMC.cpBodyPtr, zero, Vector2.zero, true);
		}
	}
}
