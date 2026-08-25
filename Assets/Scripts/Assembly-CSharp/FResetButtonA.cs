using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class FResetButtonA
{
	[CompilerGenerated]
	private static Dictionary<string, int> _003C_003Ef__switch_0024mapF;

	public static TransformC Assemble(EIC _eic)
	{
		string[] tags = new string[2]
		{
			LevelManager.m_currentLevel.name + ":UI",
			LevelManager.m_currentLevel.name
		};
		TransformC transformC = EntityManager.AddEntityWithTC(tags);
		TransformS.SetPosition(transformC, _eic.data.position.ToVector3());
		GETriggerC gETriggerC = GES.AddTriggerComponent(_eic.camera, _eic.data as TriggerData, TriggerType.ButtonController, transformC);
		gETriggerC.inputSlots = new ConnectionSlot[0];
		gETriggerC.outputSlots = new ConnectionSlot[0];
		gETriggerC.modifierSlots = new ConnectionSlot[2];
		gETriggerC.modifierSlots[0] = new ConnectionSlot(ConnectionSlotType.Activate, 0);
		gETriggerC.modifierSlots[1] = new ConnectionSlot(ConnectionSlotType.Deactivate, 1);
		_eic.trigger = gETriggerC;
		if (!GEState.editorMode)
		{
			TouchAreaC touchAreaComponent = TouchAreaS.AddComponent(transformC, "Reset", 80f, true, _eic.camera, null);
			TouchAreaS.AddTouchEventListener(touchAreaComponent, HandleTouches);
		}
		SpriteC c = SpriteS.AddComponent(transformC, new Frame(512f, 0f, 128f, 128f), FarmState.menuSheet);
		SpriteS.SetDimensions(c, 90f, 90f);
		return transformC;
	}

	private static void HandleTouches(TouchAreaC _c, int _i, bool _consumed)
	{
		if (!_consumed && _c.touchEvent[_i] == TouchEvent.Began)
		{
			Main.m_currentGame.GetCurrentScene().CreateLoadingScreen();
			string[] keys = new string[2] { "identifier", "target" };
			object[] values = new object[2]
			{
				"Remove All And Reset Current",
				string.Empty
			};
			EventS.AddEventListener("exitEvent", GELevelControlA.HandleExit, 0.01f, false, true, false, true);
			EventS.Dispatch("exitEvent", keys, values, true);
		}
	}

	public static EIC CreateNewEditorItem(EIC _container, string _identifier, Vector3 _pos, Vector3 _rot, Vector3 _sca)
	{
		TriggerData triggerData = new TriggerData();
		triggerData.position = new Vertex3(_pos);
		triggerData.rotation = new Vertex3(_rot);
		triggerData.scale = new Vertex3(_sca);
		triggerData.triggerType = 23u;
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
		TransformC item = Assemble(_eic);
		_eic.gameComponents.Add(item);
	}

	public static void PopulatePropertyBar(EIC _eic, UIC _propertyBar)
	{
		string[] array = new string[1] { "propertyBar" };
		Camera canvasCamera = _propertyBar.canvasCamera;
		BasicLevelData basicLevelData = _eic.data as BasicLevelData;
	}

	public static void HandlePropertyChange(EventC _c)
	{
		BasicLevelData basicLevelData = EditorState.m_selection[0].data as BasicLevelData;
		string identifier = _c.identifier;
		if (identifier != null)
		{
			if (_003C_003Ef__switch_0024mapF == null)
			{
				_003C_003Ef__switch_0024mapF = new Dictionary<string, int>(0);
			}
			int value;
			if (!_003C_003Ef__switch_0024mapF.TryGetValue(identifier, out value))
			{
			}
		}
		EditorState.ResetEditorItem(EditorState.m_selection[0]);
	}
}
