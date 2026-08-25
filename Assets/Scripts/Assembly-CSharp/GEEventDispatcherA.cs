using UnityEngine;

public class GEEventDispatcherA
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
		GETriggerC gETriggerC = GES.AddTriggerComponent(_eic.camera, _data, TriggerType.EventDispatchTrigger, tc);
		gETriggerC.autoTrigger = true;
		gETriggerC.inputSlots = new ConnectionSlot[1];
		gETriggerC.inputSlots[0] = new ConnectionSlot(ConnectionSlotType.Input, 0);
		gETriggerC.outputSlots = new ConnectionSlot[0];
		gETriggerC.modifierSlots = new ConnectionSlot[3];
		gETriggerC.modifierSlots[0] = new ConnectionSlot(ConnectionSlotType.Activate, 0);
		gETriggerC.modifierSlots[1] = new ConnectionSlot(ConnectionSlotType.Deactivate, 1);
		gETriggerC.modifierSlots[2] = new ConnectionSlot(ConnectionSlotType.Destroy, 2);
		gETriggerC.data = _data;
		_eic.trigger = gETriggerC;
		if (!_data.active)
		{
			EntityManager.SetActivityOfEntity(gETriggerC.entityIndex, _data.active, true);
		}
		GETriggerLogic.AddBeganEventDelegate(gETriggerC, TriggerEventHandler);
		return gETriggerC;
	}

	private static void TriggerEventHandler(IControlledComponent _c)
	{
		string[] keys = new string[2] { "vector", "text" };
		object[] values = new object[2]
		{
			_c.inputSlots[0].m_value.vector,
			_c.inputSlots[0].m_value.text
		};
		GETriggerC gETriggerC = _c as GETriggerC;
		EventS.Dispatch(gETriggerC.data.eventTarget, keys, values, false);
		gETriggerC.dispatched = true;
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
		triggerData.eventTarget = "ShipEvent";
		triggerData.eventDispatchOnlyOnce = true;
		triggerData.eventDispatchDelay = 0f;
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
		UIS.AddToCanvasGrid(LabelA.Assemble(canvasCamera, "Event Dispatcher", tags), _propertyBar, true);
		UIC uIC = TextFieldA.Assemble(canvasCamera, 0, "Dispatch Target", HandleEventPropertyChange, null, tags);
		UIS.AddToCanvasGrid(uIC, _propertyBar, true);
		UIS.SetRelativeSize(uIC, 1f, 0.1f);
		DrawTextField(uIC, triggerData.eventTarget);
	}

	public static void HandleEventPropertyChange(EventC _c)
	{
		TriggerData triggerData = EditorState.m_selection[0].data as TriggerData;
		switch (_c.identifier)
		{
		case "Dispatch Target":
			triggerData.eventTarget = (string)_c.properties["value"];
			break;
		}
		EditorState.ResetEditorItem(EditorState.m_selection[0]);
	}

	public static void DrawTextField(UIC _textField, string _default)
	{
		Vector2[] roundedRect = DebugDraw.GetRoundedRect(_textField.width, _textField.height, 5f, 5, Vector2.zero);
		PrefabS.CreateFlatPrefabComponentsFromPolygon(_textField.TC, Vector3.forward * -10f, DebugDraw.Vector2ArrayToPolygon(roundedRect), DebugDraw.GetColor(230f, 230f, 230f), ResourceManager.GetMaterial("Solid"), _textField.parent.canvasCamera);
		TextS.SetStyle("body");
		_textField.textC = TextS.AddComponent(_textField.contentTC, _default, 1f, true, true, 0.5f, 0.5f, _textField.width, _textField.height, Align.Left, Align.Middle, 20f, 20f, 0f, 0f, 0f, 0f);
		SpriteS.SetColorByTransformComponent(_textField.textC.contentTC, DebugDraw.GetColor(0f, 0f, 0f), false, false);
		_textField.textPCs.AddRange(SpriteS.ConvertSpritesToPrefabComponent(_textField.textC.contentTC, _textField.parent.canvasCamera, true));
	}
}
