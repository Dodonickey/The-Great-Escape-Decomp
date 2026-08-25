using UnityEngine;

public static class GEDPadControllerA
{
	public static GETriggerC Assemble(EIC _eic, TriggerData _data)
	{
		string[] tags = new string[3]
		{
			LevelManager.m_currentLevel.name + ":Controller",
			LevelManager.m_currentLevel.name,
			_eic.identifier
		};
		TransformC transformC = EntityManager.AddEntityWithTC(tags);
		TransformS.SetPosition(transformC, _data.position.ToVector3());
		TransformC transformC2 = TransformS.AddComponent(transformC.entityIndex);
		TransformS.ParentComponent(transformC2, transformC, Vector3.zero);
		GETriggerC gETriggerC = GES.AddTriggerComponent(_eic.camera, _data, (TriggerType)_data.triggerType, transformC);
		gETriggerC.inputSlots = new ConnectionSlot[0];
		gETriggerC.outputSlots = new ConnectionSlot[1];
		gETriggerC.outputSlots[0] = new ConnectionSlot(ConnectionSlotType.Output, 0);
		gETriggerC.modifierSlots = new ConnectionSlot[2];
		gETriggerC.modifierSlots[0] = new ConnectionSlot(ConnectionSlotType.Activate, 0);
		gETriggerC.modifierSlots[1] = new ConnectionSlot(ConnectionSlotType.Deactivate, 1);
		gETriggerC.data = _data;
		if (_data.triggerType == 14 || _data.triggerType == 28)
		{
			Vector2[] rect = DebugDraw.GetRect(50f, 50f, new Vector2(0f, -60f));
			Vector2[] rect2 = DebugDraw.GetRect(50f, 50f, new Vector2(0f, 60f));
			PrefabS.CreatePathPrefabComponentFromVectorArray(transformC, Vector3.zero, rect, 6f, Color.white, ResourceManager.GetMaterial("Line6"), Main.uiCamera, Position.Center, true);
			PrefabS.CreatePathPrefabComponentFromVectorArray(transformC, Vector3.zero, rect2, 6f, Color.white, ResourceManager.GetMaterial("Line6"), Main.uiCamera, Position.Center, true);
		}
		if (_data.triggerType == 14 || _data.triggerType == 16)
		{
			Vector2[] rect3 = DebugDraw.GetRect(50f, 50f, new Vector2(-60f, 0f));
			Vector2[] rect4 = DebugDraw.GetRect(50f, 50f, new Vector2(60f, 0f));
			PrefabS.CreatePathPrefabComponentFromVectorArray(transformC, Vector3.zero, rect3, 6f, Color.white, ResourceManager.GetMaterial("Line6"), Main.uiCamera, Position.Center, true);
			PrefabS.CreatePathPrefabComponentFromVectorArray(transformC, Vector3.zero, rect4, 6f, Color.white, ResourceManager.GetMaterial("Line6"), Main.uiCamera, Position.Center, true);
		}
		if (!GEState.editorMode)
		{
			TouchAreaC touchAreaComponent = TouchAreaS.AddComponent(transformC, "stick", 150f, true, _eic.camera, gETriggerC);
			TouchAreaS.AddTouchEventListener(touchAreaComponent, HandleTouches);
		}
		Vector2[] circle = DebugDraw.GetCircle(100f, 36, Vector2.zero, false);
		PrefabS.CreatePathPrefabComponentFromVectorArray(transformC, Vector3.zero, circle, 6f, new Color(1f, 1f, 1f), ResourceManager.GetMaterial("Line6"), Main.uiCamera, Position.Center, true);
		Vector2[] circle2 = DebugDraw.GetCircle(10f, 8, Vector2.zero, false);
		PrefabS.CreatePathPrefabComponentFromVectorArray(transformC2, Vector3.zero, circle2, 6f, new Color(1f, 1f, 1f), ResourceManager.GetMaterial("Line6"), Main.uiCamera, Position.Center, true);
		if (!_data.active)
		{
			EntityManager.SetActivityOfEntity(gETriggerC.entityIndex, _data.active, true);
		}
		_eic.trigger = gETriggerC;
		return gETriggerC;
	}

	public static EIC CreateNewEditorItem(EIC _container, string _identifier, Vector3 _pos, Vector3 _rot, Vector3 _sca)
	{
		TriggerData triggerData = new TriggerData();
		triggerData.position = new Vertex3(_pos);
		triggerData.rotation = new Vertex3(_rot);
		triggerData.scale = new Vertex3(_sca);
		triggerData.triggerType = 14u;
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

	public static void PopulatePropertyBar(EIC _eiC, UIC _propertyBar)
	{
		string[] tags = new string[1] { "propertyBar" };
		Camera canvasCamera = _propertyBar.canvasCamera;
		TriggerData triggerData = _eiC.data as TriggerData;
		UIC component = RadioButtonA.Assemble(canvasCamera, "Full", HandleDPadPropertyChange, null, true, Align.Right, 1f, triggerData.triggerType == 14, 0, 101, tags);
		UIC component2 = RadioButtonA.Assemble(canvasCamera, "Horizontal", HandleDPadPropertyChange, null, true, Align.Right, 1f, triggerData.triggerType == 16, 1, 101, tags);
		UIC component3 = RadioButtonA.Assemble(canvasCamera, "Vertical", HandleDPadPropertyChange, null, true, Align.Right, 1f, triggerData.triggerType == 28, 2, 101, tags);
		UIS.AddToCanvasGrid(LabelA.Assemble(canvasCamera, "DPad Style", tags), _propertyBar, true);
		UIS.AddToCanvasGrid(component, _propertyBar, true);
		UIS.AddToCanvasGrid(component2, _propertyBar, true);
		UIS.AddToCanvasGrid(component3, _propertyBar, true);
	}

	public static void HandleDPadPropertyChange(EventC _c)
	{
		TriggerData triggerData = EditorState.m_selection[0].data as TriggerData;
		switch (_c.identifier)
		{
		case "Full":
			triggerData.triggerType = 14u;
			break;
		case "Horizontal":
			triggerData.triggerType = 16u;
			break;
		case "Vertical":
			triggerData.triggerType = 28u;
			break;
		}
		EditorState.ResetEditorItem(EditorState.m_selection[0]);
	}

	private static void HandleTouches(TouchAreaC _c, int _i, bool _consumed)
	{
		if (_consumed)
		{
			return;
		}
		GETriggerC gETriggerC = _c.customComponent as GETriggerC;
		Vector2 vector = Vector2.zero;
		gETriggerC.triggered = false;
		if (_c.touchEvent[_i] == TouchEvent.Began)
		{
			gETriggerC.began = true;
			gETriggerC.beganTime = Main.m_gameTime;
			gETriggerC.triggered = true;
		}
		else if (_c.touchEvent[_i] == TouchEvent.Release || _c.touchEvent[_i] == TouchEvent.ReleaseOutside)
		{
			gETriggerC.triggered = false;
			gETriggerC.end = true;
			gETriggerC.endTime = Main.m_gameTime;
		}
		else if (_c.touchEvent[_i] != TouchEvent.Release && _c.touchEvent[_i] != TouchEvent.ReleaseOutside)
		{
			vector = _c.touchPos[_i] - (Vector2)gETriggerC.TC.transform.position - new Vector2((float)Screen.width * 0.5f, (float)Screen.height * 0.5f);
			gETriggerC.triggered = true;
		}
		if (vector.sqrMagnitude > 10000f)
		{
			vector = vector.normalized * 100f;
		}
		TransformS.SetPosition(gETriggerC.TC.childs[0], vector);
		vector /= 100f;
		if (Mathf.Abs(vector.x) >= 0.33f && gETriggerC.data.triggerType != 28)
		{
			gETriggerC.def.vector.x = vector.x / Mathf.Abs(vector.x);
		}
		else
		{
			gETriggerC.def.vector.x = 0f;
			if (Mathf.Abs(vector.y) >= 0.33f && gETriggerC.data.triggerType != 16)
			{
				gETriggerC.def.vector.y = vector.y / Mathf.Abs(vector.y);
			}
			else
			{
				gETriggerC.def.vector.y = 0f;
			}
		}
		if (Mathf.Abs(vector.x) > 0.33f && Mathf.Abs(vector.y) > 0.33f)
		{
			gETriggerC.def.vector = Vector3.zero;
		}
		gETriggerC.update = true;
		gETriggerC.output.vector.x = vector.x / 100f;
		gETriggerC.output.vector.y = vector.y / 100f;
		gETriggerC.outputSlots[0].m_value.vector = gETriggerC.output.vector;
		gETriggerC.outputSlots[0].m_triggered = gETriggerC.triggered;
	}
}
