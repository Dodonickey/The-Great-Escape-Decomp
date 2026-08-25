using UnityEngine;

public class GETimerTriggerA
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
		GETriggerC gETriggerC = GES.AddTriggerComponent(_eic.camera, _data, TriggerType.TimerTrigger, tc);
		gETriggerC.inputSlots = new ConnectionSlot[1];
		gETriggerC.inputSlots[0] = new ConnectionSlot(ConnectionSlotType.Input, 0);
		gETriggerC.outputSlots = new ConnectionSlot[1];
		gETriggerC.outputSlots[0] = new ConnectionSlot(ConnectionSlotType.Output, 0);
		gETriggerC.modifierSlots = new ConnectionSlot[3];
		gETriggerC.modifierSlots[0] = new ConnectionSlot(ConnectionSlotType.Activate, 0);
		gETriggerC.modifierSlots[1] = new ConnectionSlot(ConnectionSlotType.Deactivate, 1);
		gETriggerC.modifierSlots[2] = new ConnectionSlot(ConnectionSlotType.Destroy, 2);
		gETriggerC.data = _data;
		_eic.trigger = gETriggerC;
		GETriggerLogic.AddBeganEventDelegate(gETriggerC, TriggerEventHandler);
		if (!_data.active)
		{
			EntityManager.SetActivityOfEntity(gETriggerC.entityIndex, _data.active, true);
		}
		return gETriggerC;
	}

	private static void TriggerEventHandler(IControlledComponent _c)
	{
		Debug.Log("lol");
	}

	public static EIC CreateNewEditorItem(EIC _container, string _identifier, Vector3 _pos, Vector3 _rot, Vector3 _sca)
	{
		TriggerData triggerData = new TriggerData();
		triggerData.position = new Vertex3(_pos);
		triggerData.rotation = new Vertex3(_rot);
		triggerData.scale = new Vertex3(_sca);
		triggerData.triggerType = 26u;
		triggerData.colliderType = 0u;
		triggerData.eventIdentifier = _identifier;
		triggerData.eventTarget = "ShipEvent";
		triggerData.eventDispatchOnlyOnce = true;
		triggerData.eventDispatchDelay = 0f;
		triggerData.active = true;
		triggerData.toggle = false;
		triggerData.triggerOnlyOnce = false;
		triggerData.triggerUntilOutOfEnergy = false;
		triggerData.triggerOnlyOnFullEnergy = true;
		triggerData.autoTrigger = false;
		triggerData.energy = 0f;
		triggerData.energyClips = 1;
		triggerData.energyGain = Time.deltaTime;
		triggerData.energyConsume = 1f;
		triggerData.gainInterval = 1f;
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
		UIC component = NumericFieldA.Assemble(canvasCamera, "Delay", HandleEventPropertyChange, null, true, Align.Left, 80f, 1f, false, 0f, 99999.9f, triggerData.eventDispatchDelay, tags);
		UIS.AddToCanvasGrid(LabelA.Assemble(canvasCamera, "Timer", tags), _propertyBar, true);
		UIS.AddToCanvasGrid(component, _propertyBar, true);
	}

	public static void HandleEventPropertyChange(EventC _c)
	{
		TriggerData triggerData = EditorState.m_selection[0].data as TriggerData;
		switch (_c.identifier)
		{
		case "Delay":
			triggerData.eventDispatchDelay = (float)_c.properties["value"];
			break;
		}
		EditorState.ResetEditorItem(EditorState.m_selection[0]);
	}
}
