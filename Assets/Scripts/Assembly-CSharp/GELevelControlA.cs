using UnityEngine;

public class GELevelControlA
{
	public static GETriggerC Assemble(EIC _eic)
	{
		string[] tags = new string[2]
		{
			LevelManager.m_currentLevel.name + ":GameEntity",
			LevelManager.m_currentLevel.name
		};
		Entity entity = EntityManager.AddEntity(tags);
		TransformC transformC = TransformS.AddComponent(entity);
		TriggerData triggerData = _eic.data as TriggerData;
		TransformS.SetPosition(transformC, triggerData.position.ToVector3());
		GETriggerC gETriggerC = GES.AddTriggerComponent(_eic.camera, triggerData, TriggerType.LevelControlTrigger, transformC);
		gETriggerC.inputSlots = new ConnectionSlot[1];
		gETriggerC.inputSlots[0] = new ConnectionSlot(ConnectionSlotType.Input, 0);
		gETriggerC.outputSlots = new ConnectionSlot[1];
		gETriggerC.outputSlots[0] = new ConnectionSlot(ConnectionSlotType.Output, 0);
		gETriggerC.modifierSlots = new ConnectionSlot[3];
		gETriggerC.modifierSlots[0] = new ConnectionSlot(ConnectionSlotType.Activate, 0);
		gETriggerC.modifierSlots[1] = new ConnectionSlot(ConnectionSlotType.Deactivate, 1);
		gETriggerC.modifierSlots[2] = new ConnectionSlot(ConnectionSlotType.Destroy, 2);
		_eic.trigger = gETriggerC;
		if (!triggerData.active)
		{
			EntityManager.SetActivityOfEntity(gETriggerC.entityIndex, triggerData.active, true);
		}
		GETriggerLogic.AddBeganEventDelegate(gETriggerC, TriggerEventHandler);
		return gETriggerC;
	}

	public static void HandleExit(EventC _c)
	{
		string text = _c.properties["identifier"] as string;
		string levelName = _c.properties["target"] as string;
		switch (text)
		{
		case "Change Level":
			LevelManager.ChangeLevel(levelName, true);
			EntityManager.RemoveEntitiesByTag("EditorItem");
			break;
		case "Next Level":
		{
			uint currentLevelIndex = LevelManager.m_currentLevelIndex;
			uint currentChapterIndex = LevelManager.m_currentChapterIndex;
			LevelManager.m_currentLevelIndex++;
			if (!LevelManager.ChangeLevel(LevelManager.m_currentChapterIndex, LevelManager.m_currentLevelIndex, true))
			{
				LevelManager.m_currentChapterIndex++;
				LevelManager.m_currentLevelIndex = 1u;
				if (!LevelManager.ChangeLevel(LevelManager.m_currentChapterIndex, LevelManager.m_currentLevelIndex, true))
				{
					LevelManager.m_currentChapterIndex = 1u;
					LevelManager.m_currentLevelIndex = 1u;
					LevelManager.ChangeLevel(LevelManager.m_currentChapterIndex, LevelManager.m_currentLevelIndex, true);
				}
			}
			EntityManager.RemoveEntitiesByTag("EditorItem");
			break;
		}
		case "Reset All Levels":
			LevelManager.ResetAll(true);
			EntityManager.RemoveEntitiesByTag("EditorItem");
			break;
		case "Remove All And Reset Current":
			LevelManager.ChangeLevel(LevelManager.m_levels[0].name, true);
			EntityManager.RemoveEntitiesByTag("EditorItem");
			break;
		}
		EntityManager.Update();
	}

	private static void TriggerEventHandler(IControlledComponent _c)
	{
		GETriggerC gETriggerC = _c as GETriggerC;
		string eventIdentifier = gETriggerC.data.eventIdentifier;
		string eventTarget = gETriggerC.data.eventTarget;
		string[] keys = new string[2] { "identifier", "target" };
		object[] values = new object[2] { eventIdentifier, eventTarget };
		switch (eventIdentifier)
		{
		case "Change Level":
			Main.m_currentGame.GetCurrentScene().CreateLoadingScreen();
			EventS.AddEventListener("exitEvent", HandleExit, 0.01f, false, true, false, true);
			EventS.Dispatch("exitEvent", keys, values, true);
			break;
		case "Next Level":
			Main.m_currentGame.GetCurrentScene().CreateLoadingScreen();
			EventS.AddEventListener("exitEvent", HandleExit, 0.01f, false, true, false, true);
			EventS.Dispatch("exitEvent", keys, values, true);
			break;
		case "Append Level":
			LevelManager.AppendLevel(eventTarget, false);
			EntityManager.RemoveEntitiesByTag("EditorItem");
			break;
		case "Remove Level":
			if (eventTarget == string.Empty)
			{
				LevelManager.RemoveLevel(LevelManager.m_currentLevel);
			}
			else
			{
				LevelManager.RemoveLevel(eventTarget);
			}
			break;
		case "Reset All Levels":
			Main.m_currentGame.GetCurrentScene().CreateLoadingScreen();
			EventS.AddEventListener("exitEvent", HandleExit, 0.01f, false, true, false, true);
			EventS.Dispatch("exitEvent", keys, values, true);
			break;
		case "Remove All And Reset Current":
			Main.m_currentGame.GetCurrentScene().CreateLoadingScreen();
			EventS.AddEventListener("exitEvent", HandleExit, 0.01f, false, true, false, true);
			EventS.Dispatch("exitEvent", keys, values, true);
			break;
		case "Enable By Tag":
			EntityManager.SetActivityOfEntitiesWithTag(eventTarget, true, true);
			break;
		case "Disable By Tag":
			EntityManager.SetActivityOfEntitiesWithTag(eventTarget, false, true);
			break;
		}
		EntityManager.Update();
	}

	public static EIC CreateNewEditorItem(EIC _container, string _identifier, Vector3 _pos, Vector3 _rot, Vector3 _sca)
	{
		TriggerData triggerData = new TriggerData();
		triggerData.position = new Vertex3(_pos);
		triggerData.rotation = new Vertex3(_rot);
		triggerData.scale = new Vertex3(_sca);
		triggerData.triggerType = 2u;
		triggerData.colliderType = 0u;
		triggerData.eventIdentifier = _identifier;
		triggerData.eventTarget = string.Empty;
		triggerData.eventDispatchOnlyOnce = true;
		triggerData.eventDispatchDelay = 0f;
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
		GETriggerC gETriggerC = Assemble(_eic);
		_eic.gameComponents.Add(gETriggerC);
		TransformS.ParentComponent(gETriggerC.TC, _eic.TC, Vector3.zero);
	}

	private static void HandleEvent(EventC _c)
	{
	}

	public static void PopulatePropertyBar(EIC _eic, UIC _propertyBar)
	{
		string[] tags = new string[1] { "propertyBar" };
		TriggerData triggerData = _eic.data as TriggerData;
		Camera canvasCamera = _propertyBar.canvasCamera;
		UIC uIC = TextFieldA.Assemble(canvasCamera, 0, "Dispatch Target", GEEventDispatcherA.HandleEventPropertyChange, null, tags);
		UIS.AddToCanvasGrid(uIC, _propertyBar, true);
		UIS.SetRelativeSize(uIC, 1f, 0.1f);
		GEEventDispatcherA.DrawTextField(uIC, triggerData.eventTarget);
	}
}
