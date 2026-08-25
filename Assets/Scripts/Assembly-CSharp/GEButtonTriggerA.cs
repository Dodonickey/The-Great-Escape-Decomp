using UnityEngine;

public class GEButtonTriggerA
{
	public static GETriggerC Assemble(EIC _eic, TriggerData _data)
	{
		string[] tags = new string[2]
		{
			LevelManager.m_currentLevel.name + ":GameEntity",
			LevelManager.m_currentLevel.name
		};
		Entity entity = EntityManager.AddEntity(tags);
		TransformC transformC = TransformS.AddComponent(entity);
		TransformC transformC2 = TransformS.AddComponent(entity);
		TransformS.SetGlobalPosition(transformC, _data.position.ToVector3());
		TransformS.SetGlobalPosition(transformC2, _data.position.ToVector3());
		Vector2[] rect = DebugDraw.GetRect(45f, 15f, Vector2.zero);
		Vector2[] array = new Vector2[6]
		{
			new Vector2(-33f, -20f),
			new Vector2(-33f, -5f),
			new Vector2(-25f, 0f),
			new Vector2(25f, 0f),
			new Vector2(33f, -5f),
			new Vector2(33f, -20f)
		};
		PrefabC prefabC = PrefabS.AddComponent(transformC, Vector3.zero, ResourceManager.GetGameObject("ButtonBase"));
		PrefabC prefabC2 = PrefabS.AddComponent(transformC2, Vector3.up * 7.5f, ResourceManager.GetGameObject("ButtonTile"));
		ChipmunkC chipmunkC = ChipmunkS.AddInactiveComponent(transformC, false, (ColliderType)9, ChipmunkS.m_groundColliderGroup, 17895697u, true, false);
		ChipmunkS.ActivateChipmunkComponent(chipmunkC, ChipmunkWrapper.AddBody(chipmunkC.isStatic, chipmunkC.isRogue, _data.position.ToVector3(), chipmunkC.index, chipmunkC.colliderType));
		ChipmunkWrapper.AddPolyShape(chipmunkC.cpBodyPtr, Vector2.zero, 10f, array.Length, array, 0.5f, 0.5f, ChipmunkS.m_groundColliderGroup, 17895697u, false);
		ChipmunkC chipmunkC2 = ChipmunkS.AddInactiveComponent(transformC2, false, (ColliderType)11, ChipmunkS.m_groundColliderGroup, 17895697u, false, false);
		ChipmunkS.ActivateChipmunkComponent(chipmunkC2, ChipmunkWrapper.AddBoxBody(chipmunkC2.isStatic, chipmunkC2.isRogue, _data.position.ToVector2(), chipmunkC2.index, Vector2.zero, 5f, 45f, 15f, 0f, 1f, chipmunkC2.colliderGroup, chipmunkC2.colliderLayer, false, chipmunkC2.colliderType));
		chipmunkC2.dictateAngle = false;
		ChipmunkWrapper.AddDampedSpring(chipmunkC.cpBodyPtr, chipmunkC2.cpBodyPtr, new Vector2(0f, -10f), new Vector2(0f, 10f), 50f, 200f, 20f);
		ChipmunkWrapper.AddGrooveJoint(chipmunkC.cpBodyPtr, chipmunkC2.cpBodyPtr, Vector2.up * -8f, Vector2.zero, Vector2.zero);
		TransformS.SetRotation(chipmunkC.TC, _eic.data.rotation.ToVector3(), chipmunkC.cpBodyPtr);
		TransformS.SetRotation(chipmunkC2.TC, _eic.data.rotation.ToVector3(), chipmunkC2.cpBodyPtr);
		ChipmunkWrapper.AddRotaryLimitJoint(chipmunkC.cpBodyPtr, chipmunkC2.cpBodyPtr, 0f, 0f);
		ChipmunkWrapper.ReIndexBody(chipmunkC.cpBodyPtr);
		if (GEState.editorMode)
		{
			TransformS.ParentComponent(transformC2, _eic.TC);
		}
		else
		{
			transformC2.forceRotation = true;
			transformC2.forcedRotation = Quaternion.Euler(_eic.data.rotation.ToVector3());
			TransformS.SetRotation(transformC2, _eic.data.rotation.ToVector3());
		}
		GETriggerC gETriggerC = (GETriggerC)(chipmunkC2.customComponent = (chipmunkC.customComponent = GES.AddTriggerComponent(_eic.camera, _data, chipmunkC)));
		gETriggerC.collisionHandler = HandleSensor;
		gETriggerC.tileTC = transformC2;
		gETriggerC.inputSlots = new ConnectionSlot[1];
		gETriggerC.inputSlots[0] = new ConnectionSlot(ConnectionSlotType.Input, 0);
		gETriggerC.outputSlots = new ConnectionSlot[1];
		gETriggerC.outputSlots[0] = new ConnectionSlot(ConnectionSlotType.Output, 0);
		_eic.trigger = gETriggerC;
		return gETriggerC;
	}

	public static EIC CreateNewEditorItem(EIC _container, string _identifier, Vector3 _pos, Vector3 _rot, Vector3 _sca)
	{
		TriggerData triggerData = new TriggerData();
		triggerData.position = new Vertex3(_pos);
		triggerData.position.z = 50f;
		triggerData.rotation = new Vertex3(_rot);
		triggerData.scale = new Vertex3(_sca);
		triggerData.triggerType = 6u;
		uint uniqueId = GES.GetUniqueId();
		triggerData.Init(uniqueId, _identifier + uniqueId);
		EIC eIC = GEItemA.Assemble(_container, _identifier, triggerData, Main.camera);
		eIC.isRealtimeMovable = true;
		eIC.isDrawable = false;
		eIC.isRotateable = true;
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
		GETriggerC gETriggerC = Assemble(_eic, data);
		_eic.gameComponents.Add(gETriggerC);
		TransformS.ParentComponent(gETriggerC.TC, _eic.TC, Vector3.zero);
	}

	public static void PopulatePropertyBar(EIC _eiC, UIC _propertyBar)
	{
		string[] tags = new string[1] { "propertyBar" };
		TriggerData triggerData = _eiC.data as TriggerData;
		Camera canvasCamera = _propertyBar.canvasCamera;
		UIC component = CheckBoxA.Assemble(Main.uiCamera, "Active", HandlePropertyChange, null, true, Align.Right, 1f, triggerData.active, tags);
		UIC component2 = CheckBoxA.Assemble(Main.uiCamera, "AutoTrigger", HandlePropertyChange, null, true, Align.Right, 1f, triggerData.autoTrigger, tags);
		UIC component3 = NumericFieldA.Assemble(Main.uiCamera, "StartEnergy", HandlePropertyChange, null, true, Align.Left, 50f, 1f, false, 0f, 1f, triggerData.energy, tags);
		UIC component4 = NumericFieldA.Assemble(Main.uiCamera, "Consume", HandlePropertyChange, null, true, Align.Left, 50f, 1f, false, -1f, 1f, triggerData.energyConsume, tags);
		UIC component5 = NumericFieldA.Assemble(Main.uiCamera, "ConsumeInterval", HandlePropertyChange, null, true, Align.Left, 50f, 1f, false, 0f, 10f, triggerData.consumeInterval, tags);
		UIC component6 = NumericFieldA.Assemble(Main.uiCamera, "Gain", HandlePropertyChange, null, true, Align.Left, 50f, 1f, false, -1f, 1f, triggerData.energyGain, tags);
		UIC component7 = NumericFieldA.Assemble(Main.uiCamera, "GainInterval", HandlePropertyChange, null, true, Align.Left, 50f, 1f, false, 0f, 1f, triggerData.gainInterval, tags);
		UIC component8 = NumericFieldA.Assemble(Main.uiCamera, "GainCooldown", HandlePropertyChange, null, true, Align.Left, 50f, 1f, false, 0f, 1f, triggerData.cooldown, tags);
		UIC component9 = NumericFieldA.Assemble(Main.uiCamera, "Clips", HandlePropertyChange, null, true, Align.Left, 50f, 1f, true, -1f, 999f, triggerData.energyClips, tags);
		UIC component10 = NumericFieldA.Assemble(Main.uiCamera, "ReloadCooldown", HandlePropertyChange, null, true, Align.Left, 50f, 1f, false, 0f, 1f, triggerData.reloadCooldown, tags);
		UIC component11 = NumericFieldA.Assemble(Main.uiCamera, "TriggerCooldown", HandlePropertyChange, null, true, Align.Left, 50f, 1f, false, 0f, 1f, triggerData.triggerCooldown, tags);
		UIC component12 = CheckBoxA.Assemble(Main.uiCamera, "Toggle", HandlePropertyChange, null, true, Align.Right, 1f, triggerData.toggle, tags);
		UIC component13 = CheckBoxA.Assemble(Main.uiCamera, "Trigger Only Once", HandlePropertyChange, null, true, Align.Right, 1f, triggerData.triggerOnlyOnce, tags);
		UIC component14 = CheckBoxA.Assemble(Main.uiCamera, "Trigger Only On Full Energy", HandlePropertyChange, null, true, Align.Right, 1f, triggerData.triggerOnlyOnFullEnergy, tags);
		UIC component15 = CheckBoxA.Assemble(Main.uiCamera, "Trigger Until Out Of Energy", HandlePropertyChange, null, true, Align.Right, 1f, triggerData.triggerUntilOutOfEnergy, tags);
		UIS.AddToCanvasGrid(LabelA.Assemble(canvasCamera, "Trigger", tags), _propertyBar, true);
		UIS.AddToCanvasGrid(component, _propertyBar, true);
		UIS.AddToCanvasGrid(component2, _propertyBar, true);
		UIS.AddToCanvasGrid(component12, _propertyBar, true);
		UIS.AddToCanvasGrid(component13, _propertyBar, true);
		UIS.AddToCanvasGrid(component14, _propertyBar, true);
		UIS.AddToCanvasGrid(component15, _propertyBar, true);
		UIS.MoveCursor(_propertyBar, 0f, -15f);
		UIS.AddToCanvasGrid(component3, _propertyBar, true);
		UIS.AddToCanvasGrid(component4, _propertyBar, true);
		UIS.AddToCanvasGrid(component5, _propertyBar, false);
		UIS.AddToCanvasGrid(component6, _propertyBar, true);
		UIS.AddToCanvasGrid(component7, _propertyBar, false);
		UIS.AddToCanvasGrid(component8, _propertyBar, false);
		UIS.AddToCanvasGrid(component9, _propertyBar, true);
		UIS.AddToCanvasGrid(component10, _propertyBar, false);
		UIS.AddToCanvasGrid(component11, _propertyBar, false);
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

	public static void HandleSensor(GETriggerC trigger, ChipmunkC _collidingCMC, ChipmunkCollisionPair _collisionPair, ChipmunkCollisionList _collisionList)
	{
		if (_collidingCMC.customComponent == trigger)
		{
			Debug.Log("lol");
		}
	}
}
