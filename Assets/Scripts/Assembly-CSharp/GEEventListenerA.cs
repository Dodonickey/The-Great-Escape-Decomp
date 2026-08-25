using UnityEngine;

public class GEEventListenerA
{
	public static GETriggerC Assemble(EIC _eic, TriggerData _data)
	{
		string[] tags = new string[2]
		{
			LevelManager.m_currentLevel.name + ":GameEntity",
			LevelManager.m_currentLevel.name
		};
		Entity entity = EntityManager.AddEntity(tags);
		TransformC tc = TransformS.AddComponent(entity);
		Vector2[] array = new Vector2[3];
		EventC c = EventS.AddComponent(entity.index, _data.eventTarget, HandleTriggeredEvent, 0f, false, false, false, false);
		GETriggerC gETriggerC = GES.AddTriggerComponent(_eic.camera, _data, TriggerType.EventListenerTrigger, tc);
		gETriggerC.inputSlots = new ConnectionSlot[0];
		gETriggerC.outputSlots = new ConnectionSlot[1];
		gETriggerC.outputSlots[0] = new ConnectionSlot(ConnectionSlotType.Output, 0);
		gETriggerC.modifierSlots = new ConnectionSlot[3];
		gETriggerC.modifierSlots[0] = new ConnectionSlot(ConnectionSlotType.Activate, 0);
		gETriggerC.modifierSlots[1] = new ConnectionSlot(ConnectionSlotType.Deactivate, 1);
		gETriggerC.modifierSlots[2] = new ConnectionSlot(ConnectionSlotType.Destroy, 2);
		EventS.AddProperty(c, "trigger", gETriggerC);
		_eic.trigger = gETriggerC;
		if (!_data.active)
		{
			EntityManager.SetActivityOfEntity(gETriggerC.entityIndex, _data.active, true);
		}
		return gETriggerC;
	}

	private void TriggerEventHandler(IControlledComponent _c)
	{
	}

	public static EIC CreateNewEditorItem(EIC _container, string _identifier, Vector3 _pos, Vector3 _rot, Vector3 _sca)
	{
		TriggerData triggerData = new TriggerData();
		triggerData.position = new Vertex3(_pos);
		triggerData.rotation = new Vertex3(_rot);
		triggerData.scale = new Vertex3(_sca);
		triggerData.triggerType = 3u;
		triggerData.colliderType = 0u;
		triggerData.eventIdentifier = _identifier;
		triggerData.eventTarget = "GE_dead";
		triggerData.eventDispatchOnlyOnce = true;
		triggerData.eventDispatchDelay = 0f;
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
		GETriggerC gETriggerC = Assemble(_eic, data);
		_eic.gameComponents.Add(gETriggerC);
		TransformS.ParentComponent(gETriggerC.TC, _eic.TC, Vector3.zero);
	}

	public static void PopulatePropertyBar(EIC _eiC, UIC _propertyBar)
	{
		string[] tags = new string[1] { "propertyBar" };
		TriggerData triggerData = _eiC.data as TriggerData;
		Camera canvasCamera = _propertyBar.canvasCamera;
		UIS.AddToCanvasGrid(LabelA.Assemble(canvasCamera, "Event Listener", tags), _propertyBar, true);
		UIC uIC = TextFieldA.Assemble(canvasCamera, 0, "Listen To", HandleEventPropertyChange, null, tags);
		UIS.AddToCanvasGrid(uIC, _propertyBar, true);
		UIS.SetRelativeSize(uIC, 1f, 0.1f);
		GEEventDispatcherA.DrawTextField(uIC, triggerData.eventTarget);
	}

	public static void HandleEventPropertyChange(EventC _c)
	{
		TriggerData triggerData = EditorState.m_selection[0].data as TriggerData;
		switch (_c.identifier)
		{
		case "Listen To":
			triggerData.eventTarget = (string)_c.properties["value"];
			break;
		}
		EditorState.ResetEditorItem(EditorState.m_selection[0]);
	}

	public static void HandleTriggeredEvent(EventC _e)
	{
		GETriggerC gETriggerC = _e.properties["trigger"] as GETriggerC;
		if (gETriggerC != null)
		{
			gETriggerC.triggerCount++;
			gETriggerC.triggered = true;
			gETriggerC.began = true;
			gETriggerC.beganTime = Main.m_gameTime;
			GETriggerLogic.HandleBeginTriggerEvent(gETriggerC);
			GETriggerLogic.HandleEndTriggerEvent(gETriggerC);
			gETriggerC.triggerCount = 0;
			gETriggerC.triggered = false;
			gETriggerC.end = true;
			gETriggerC.endTime = Main.m_gameTime;
		}
	}
}
