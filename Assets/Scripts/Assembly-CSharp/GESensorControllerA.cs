using UnityEngine;

public static class GESensorControllerA
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
		TransformS.SetGlobalPosition(transformC, _data.position.ToVector3());
		GETriggerC gETriggerC = GES.AddTriggerComponent(_eic.camera, _data, TriggerType.SensorController, transformC);
		gETriggerC.inputSlots = new ConnectionSlot[1];
		gETriggerC.inputSlots[0] = new ConnectionSlot(ConnectionSlotType.Input, 0);
		gETriggerC.outputSlots = new ConnectionSlot[1];
		gETriggerC.outputSlots[0] = new ConnectionSlot(ConnectionSlotType.Output, 0);
		gETriggerC.modifierSlots = new ConnectionSlot[3];
		gETriggerC.modifierSlots[0] = new ConnectionSlot(ConnectionSlotType.Activate, 0);
		gETriggerC.modifierSlots[1] = new ConnectionSlot(ConnectionSlotType.Destroy, 1);
		gETriggerC.modifierSlots[2] = new ConnectionSlot(ConnectionSlotType.Modifier, 2);
		_eic.trigger = gETriggerC;
		if (!GEState.editorMode)
		{
			TouchAreaC touchAreaC = TouchAreaS.AddComponent(transformC, "sensor", 60f, true, _eic.camera, gETriggerC);
			TouchAreaS.AddTouchEventListener(touchAreaC, HandleTouches);
			TouchAreaS.SetOrder(touchAreaC, 200);
            // Register for keyboard input
            KeyboardInputManager.RegisterSensorButton(gETriggerC);
        }
		Color color = DebugDraw.GetColor(27f, 21f, 17f, 140f);
		Vector2[] circle = DebugDraw.GetCircle(80f, 50, Vector2.zero);
		DebugDraw.AddRadialRandom(circle, 5f);
		Polygon polygon = DebugDraw.Vector2ArrayToPolygon(circle);
		polygon = GpcS.CleanPolygon(polygon, 5f, 0f, 20f, true);
		polygon = GpcS.SmoothPolygon(polygon, 5);
		DebugDraw.AddRadialRandom(circle, 5f);
		Polygon polygon2 = DebugDraw.Vector2ArrayToPolygon(circle);
		DebugDraw.ScalePolygon(polygon2, Vector2.one * 0.9f);
		polygon2 = GpcS.CleanPolygon(polygon2, 10f, 0f, 20f, true);
		polygon2 = GpcS.SmoothPolygon(polygon2, 5);
		PrefabS.CreatePathPrefabComponentFromPolygon(transformC, Vector3.forward * 0f, polygon, 8f, DebugDraw.GetColor(255f, 255f, 255f), ResourceManager.GetMaterial("Line8"), _eic.camera, Position.Center, true);
		PrefabS.CreatePathPrefabComponentFromPolygon(transformC, Vector3.forward * 5f, polygon2, 10f, DebugDraw.GetColor(255f, 255f, 255f, 64f), ResourceManager.GetMaterial("Line8"), _eic.camera, Position.Inside, true);
		PrefabS.CreateFlatPrefabComponentsFromPolygon(transformC, Vector3.forward * 10f, polygon, color, ResourceManager.GetMaterial("Solid"), _eic.camera);
		if (!_data.active)
		{
			EntityManager.SetActivityOfEntity(gETriggerC.entityIndex, _data.active, true);
		}
		return gETriggerC;
	}

	public static EIC CreateNewEditorItem(EIC _container, string _identifier, Vector3 _pos, Vector3 _rot, Vector3 _sca)
	{
		TriggerData triggerData = new TriggerData();
		triggerData.position = new Vertex3(_pos);
		triggerData.rotation = new Vertex3(_rot);
		triggerData.scale = new Vertex3(_sca);
		triggerData.triggerType = 7u;
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

	private static void HandleTouches(TouchAreaC _c, int _i, bool _consumed)
	{
		if (_consumed)
		{
			return;
		}
		GETriggerC gETriggerC = _c.customComponent as GETriggerC;
		if (!gETriggerC.triggerOnlyOnce || gETriggerC.beganTime == 0f)
		{
			if (_c.touchEvent[_i] == TouchEvent.Began || (_c.touchEvent[_i] == TouchEvent.RollIn && _c.touchStartedInside[_i]))
			{
				gETriggerC.collidingCount++;
				GETriggerLogic.HandleBeginTriggerEvent(gETriggerC);
			}
			else if ((_c.touchEvent[_i] == TouchEvent.Release || _c.touchEvent[_i] == TouchEvent.RollOut) && _c.touchStartedInside[_i])
			{
				gETriggerC.collidingCount--;
				GETriggerLogic.HandleEndTriggerEvent(gETriggerC);
			}
		}
		if (_c.touchEvent[_i] == TouchEvent.Began || (_c.touchEvent[_i] == TouchEvent.RollIn && _c.touchStartedInside[_i]))
		{
			TweenS.AddTransformTween(gETriggerC.TC, TweenedProperty.Scale, TweenStyle.CubicOut, Vector3.one * 1.05f, 0.1f, 0f);
		}
		else if (_c.touchEvent[_i] == TouchEvent.RollOut && _c.touchStartedInside[_i])
		{
			TweenS.AddTransformTween(gETriggerC.TC, TweenedProperty.Scale, TweenStyle.CubicOut, Vector3.one, 0.2f, 0f);
		}
		else if (_c.touchEvent[_i] == TouchEvent.Release)
		{
			TweenS.AddTransformTween(gETriggerC.TC, TweenedProperty.Scale, TweenStyle.CubicOut, Vector3.one, 0.2f, 0f);
		}
	}

	public static void PopulatePropertyBar(EIC _eiC, UIC _propertyBar)
	{
		string[] tags = new string[1] { "propertyBar" };
		TriggerData triggerData = _eiC.data as TriggerData;
		Camera canvasCamera = _propertyBar.canvasCamera;
		UIC component = CheckBoxA.Assemble(canvasCamera, "Active", HandlePropertyChange, null, true, Align.Right, 1f, triggerData.active, tags);
		UIC component2 = CheckBoxA.Assemble(canvasCamera, "AutoTrigger", HandlePropertyChange, null, true, Align.Right, 1f, triggerData.autoTrigger, tags);
		UIC component3 = NumericFieldA.Assemble(canvasCamera, "DefaultX", HandlePropertyChange, null, true, Align.Left, 50f, 1f, false, -99999f, 99999f, triggerData.defaultNumericValue.x, tags);
		UIC component4 = NumericFieldA.Assemble(canvasCamera, "DefaultY", HandlePropertyChange, null, true, Align.Left, 50f, 1f, false, -99999f, 99999f, triggerData.defaultNumericValue.y, tags);
		UIC component5 = NumericFieldA.Assemble(canvasCamera, "DefaultZ", HandlePropertyChange, null, true, Align.Left, 50f, 1f, false, -99999f, 99999f, triggerData.defaultNumericValue.z, tags);
		UIC component6 = NumericFieldA.Assemble(canvasCamera, "StartEnergy", HandlePropertyChange, null, true, Align.Left, 50f, 1f, false, 0f, 1f, triggerData.energy, tags);
		UIC component7 = NumericFieldA.Assemble(canvasCamera, "Consume", HandlePropertyChange, null, true, Align.Left, 50f, 1f, false, -1f, 1f, triggerData.energyConsume, tags);
		UIC component8 = NumericFieldA.Assemble(canvasCamera, "ConsumeInterval", HandlePropertyChange, null, true, Align.Left, 50f, 1f, false, 0f, 10f, triggerData.consumeInterval, tags);
		UIC component9 = NumericFieldA.Assemble(canvasCamera, "Gain", HandlePropertyChange, null, true, Align.Left, 50f, 1f, false, -1f, 1f, triggerData.energyGain, tags);
		UIC component10 = NumericFieldA.Assemble(canvasCamera, "GainInterval", HandlePropertyChange, null, true, Align.Left, 50f, 1f, false, 0f, 1f, triggerData.gainInterval, tags);
		UIC component11 = NumericFieldA.Assemble(canvasCamera, "GainCooldown", HandlePropertyChange, null, true, Align.Left, 50f, 1f, false, 0f, 1f, triggerData.cooldown, tags);
		UIC component12 = NumericFieldA.Assemble(canvasCamera, "Clips", HandlePropertyChange, null, true, Align.Left, 50f, 1f, true, -1f, 999f, triggerData.energyClips, tags);
		UIC component13 = NumericFieldA.Assemble(canvasCamera, "ReloadCooldown", HandlePropertyChange, null, true, Align.Left, 50f, 1f, false, 0f, 1f, triggerData.reloadCooldown, tags);
		UIC component14 = NumericFieldA.Assemble(canvasCamera, "TriggerCooldown", HandlePropertyChange, null, true, Align.Left, 50f, 1f, false, 0f, 1f, triggerData.triggerCooldown, tags);
		UIC component15 = CheckBoxA.Assemble(canvasCamera, "Toggle", HandlePropertyChange, null, true, Align.Right, 1f, triggerData.toggle, tags);
		UIC component16 = CheckBoxA.Assemble(canvasCamera, "Trigger Only Once", HandlePropertyChange, null, true, Align.Right, 1f, triggerData.triggerOnlyOnce, tags);
		UIC component17 = CheckBoxA.Assemble(canvasCamera, "Trigger Only On Full Energy", HandlePropertyChange, null, true, Align.Right, 1f, triggerData.triggerOnlyOnFullEnergy, tags);
		UIC component18 = CheckBoxA.Assemble(canvasCamera, "Trigger Until Out Of Energy", HandlePropertyChange, null, true, Align.Right, 1f, triggerData.triggerUntilOutOfEnergy, tags);
		UIS.AddToCanvasGrid(LabelA.Assemble(canvasCamera, "Trigger", tags), _propertyBar, true);
		UIS.AddToCanvasGrid(component3, _propertyBar, true);
		UIS.AddToCanvasGrid(component4, _propertyBar, false);
		UIS.AddToCanvasGrid(component5, _propertyBar, false);
		UIS.AddToCanvasGrid(component, _propertyBar, true);
		UIS.AddToCanvasGrid(component2, _propertyBar, true);
		UIS.AddToCanvasGrid(component15, _propertyBar, true);
		UIS.AddToCanvasGrid(component16, _propertyBar, true);
		UIS.AddToCanvasGrid(component17, _propertyBar, true);
		UIS.AddToCanvasGrid(component18, _propertyBar, true);
		UIS.MoveCursor(_propertyBar, 0f, -15f);
		UIS.AddToCanvasGrid(component6, _propertyBar, true);
		UIS.AddToCanvasGrid(component7, _propertyBar, true);
		UIS.AddToCanvasGrid(component8, _propertyBar, false);
		UIS.AddToCanvasGrid(component9, _propertyBar, true);
		UIS.AddToCanvasGrid(component10, _propertyBar, false);
		UIS.AddToCanvasGrid(component11, _propertyBar, false);
		UIS.AddToCanvasGrid(component12, _propertyBar, true);
		UIS.AddToCanvasGrid(component13, _propertyBar, false);
		UIS.AddToCanvasGrid(component14, _propertyBar, false);
		UIS.MoveCursor(_propertyBar, 0f, -15f);
	}

	public static void HandlePropertyChange(EventC _c)
	{
		TriggerData triggerData = EditorState.m_selection[0].data as TriggerData;
		switch (_c.identifier)
		{
		case "Active":
			triggerData.active = (bool)_c.properties["checked"];
			break;
		case "AutoTrigger":
			triggerData.autoTrigger = (bool)_c.properties["checked"];
			break;
		case "Toggle":
			triggerData.toggle = (bool)_c.properties["checked"];
			break;
		case "DefaultX":
		case "DefaultY":
		case "DefaultZ":
		{
			Vertex3 defaultNumericValue = triggerData.defaultNumericValue;
			if (_c.identifier == "DefaultX")
			{
				defaultNumericValue.x = (float)_c.properties["value"];
			}
			else if (_c.identifier == "DefaultY")
			{
				defaultNumericValue.y = (float)_c.properties["value"];
			}
			else if (_c.identifier == "DefaultZ")
			{
				defaultNumericValue.z = (float)_c.properties["value"];
			}
			triggerData.defaultNumericValue = defaultNumericValue;
			break;
		}
		case "Trigger Only On Full Energy":
			triggerData.triggerOnlyOnFullEnergy = (bool)_c.properties["checked"];
			break;
		case "Trigger Until Out Of Energy":
			triggerData.triggerUntilOutOfEnergy = (bool)_c.properties["checked"];
			break;
		case "Trigger Only Once":
			triggerData.triggerOnlyOnce = (bool)_c.properties["checked"];
			break;
		case "StartEnergy":
			triggerData.energy = (float)_c.properties["value"];
			break;
		case "Consume":
			triggerData.energyConsume = (float)_c.properties["value"];
			break;
		case "ConsumeInterval":
			triggerData.consumeInterval = (float)_c.properties["value"];
			break;
		case "Gain":
			triggerData.energyGain = (float)_c.properties["value"];
			break;
		case "GainInterval":
			triggerData.gainInterval = (float)_c.properties["value"];
			break;
		case "GainCooldown":
			triggerData.cooldown = (float)_c.properties["value"];
			break;
		case "Clips":
			triggerData.energyClips = Mathf.RoundToInt((float)_c.properties["value"]);
			break;
		case "ReloadCooldown":
			triggerData.reloadCooldown = (float)_c.properties["value"];
			break;
		case "TriggerCooldown":
			triggerData.triggerCooldown = (float)_c.properties["value"];
			break;
		}
		EditorState.ResetEditorItem(EditorState.m_selection[0]);
	}
}
