using System.Collections.Generic;
using UnityEngine;

public static class GEFingerControllerAreaA
{
	public static GETriggerC Assemble(EIC _eic, TriggerData _data)
	{
		string[] tags = new string[2]
		{
			LevelManager.m_currentLevel.name + ":Controller",
			LevelManager.m_currentLevel.name
		};
		TransformC transformC = EntityManager.AddEntityWithTC(tags);
		TransformS.SetGlobalPosition(transformC, _data.position.ToVector3());
		ChipmunkC chipmunkC = ChipmunkS.AddInactiveComponent(transformC, true, (ColliderType)11, 0u, 17895697u, false, false);
		ChipmunkS.ActivateChipmunkComponent(chipmunkC, ChipmunkWrapper.AddBody(chipmunkC.isStatic, chipmunkC.isRogue, _data.position.ToVector2(), chipmunkC.index, chipmunkC.colliderType));
		ChipmunkWrapper.AddCircleShape(chipmunkC.cpBodyPtr, Vector2.zero, 1f, _data.eventDispatchDelay, 0f, 0f, chipmunkC.colliderGroup, chipmunkC.colliderLayer, true);
		GETriggerC gETriggerC = GES.AddTriggerComponent(_eic.camera, _data, TriggerType.FingerControllerAreaController, transformC);
		gETriggerC.collisionHandler = HandleSensor;
		gETriggerC.inputSlots = new ConnectionSlot[1];
		gETriggerC.inputSlots[0] = new ConnectionSlot(ConnectionSlotType.Input, 0);
		gETriggerC.outputSlots = new ConnectionSlot[1];
		gETriggerC.outputSlots[0] = new ConnectionSlot(ConnectionSlotType.Output, 0);
		gETriggerC.modifierSlots = new ConnectionSlot[3];
		gETriggerC.modifierSlots[0] = new ConnectionSlot(ConnectionSlotType.Activate, 0);
		gETriggerC.modifierSlots[1] = new ConnectionSlot(ConnectionSlotType.Deactivate, 1);
		gETriggerC.modifierSlots[2] = new ConnectionSlot(ConnectionSlotType.Destroy, 2);
		chipmunkC.customComponent = gETriggerC;
		_eic.trigger = gETriggerC;
		gETriggerC.autoTrigger = true;
		gETriggerC.data = _data;
		if (GEState.editorMode || (!GEState.editorMode && _data.eventDispatchOnlyOnce))
		{
			Vector2[] circle = DebugDraw.GetCircle(_data.eventDispatchDelay, 36, Vector2.zero, false);
			PrefabS.CreatePathPrefabComponentFromVectorArray(transformC, Vector3.zero, circle, 4f, new Color(0.9f, 1f, 0.9f), ResourceManager.GetMaterial("Line4"), Main.camera, Position.Center, true);
		}
		if (!_data.active)
		{
			EntityManager.SetActivityOfEntity(gETriggerC.entityIndex, _data.active, true);
		}
		if (_eic.container != null && _eic.container.identifier == "Block")
		{
			GEBlockC gEBlockC = _eic.container.gameComponents[0] as GEBlockC;
			if (!gEBlockC.CMC.isStatic && !GEState.editorMode)
			{
				TransformS.ParentComponent(gETriggerC.TC, gEBlockC.CMC.TC);
				gETriggerC.connectedCMC = gEBlockC.CMC;
				for (int i = 0; i < gEBlockC.CMC.TC.childs.Count; i++)
				{
					List<IComponent> componentsByEntityIndex = EntityManager.GetComponentsByEntityIndex((ComponentType)112, gEBlockC.CMC.TC.childs[i].entityIndex);
					for (int j = 0; j < componentsByEntityIndex.Count; j++)
					{
						GETriggerC gETriggerC2 = componentsByEntityIndex[j] as GETriggerC;
						if (gETriggerC2.active && gETriggerC2.triggerType == TriggerType.FingerController)
						{
							EntityManager.SetActivityOfEntity(componentsByEntityIndex[j].entityIndex, true, true);
							gETriggerC2.collidingCount++;
						}
					}
				}
			}
		}
		return gETriggerC;
	}

	public static EIC CreateNewEditorItem(EIC _container, string _identifier, Vector3 _pos, Vector3 _rot, Vector3 _sca)
	{
		TriggerData triggerData = new TriggerData();
		triggerData.position = new Vertex3(_pos);
		triggerData.rotation = new Vertex3(_rot);
		triggerData.scale = new Vertex3(_sca);
		triggerData.triggerType = 1u;
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
		triggerData.eventDispatchDelay = 200f;
		triggerData.eventDispatchOnlyOnce = true;
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
		if (GEState.editorMode)
		{
			TransformS.ParentComponent(gETriggerC.TC, _eic.TC, Vector3.zero);
		}
	}

	public static void PopulatePropertyBar(EIC _eiC, UIC _propertyBar)
	{
		string[] tags = new string[1] { "propertyBar" };
		TriggerData triggerData = _eiC.data as TriggerData;
		Camera canvasCamera = _propertyBar.canvasCamera;
		UIC component = NumericFieldA.Assemble(Main.uiCamera, "Area Size", HandlePropertyChange, null, true, Align.Left, 50f, 1f, false, 0f, 99999f, triggerData.eventDispatchDelay, tags);
		UIC component2 = CheckBoxA.Assemble(Main.uiCamera, "Display Area In Game", HandlePropertyChange, null, true, Align.Right, 1f, triggerData.eventDispatchOnlyOnce, tags);
		UIS.AddToCanvasGrid(LabelA.Assemble(canvasCamera, "Finger Controller Area", tags), _propertyBar, true);
		UIS.AddToCanvasGrid(component, _propertyBar, true);
		UIS.AddToCanvasGrid(component2, _propertyBar, true);
	}

	public static void HandlePropertyChange(EventC _c)
	{
		TriggerData triggerData = EditorState.m_selection[0].data as TriggerData;
		if (_c.identifier == "Area Size")
		{
			triggerData.eventDispatchDelay = (float)_c.properties["value"];
		}
		else if (_c.identifier == "Display Area In Game")
		{
			triggerData.eventDispatchOnlyOnce = (((bool)_c.properties["checked"]) ? true : false);
		}
		EditorState.ResetEditorItem(EditorState.m_selection[0]);
	}

	public static void HandleSensor(GETriggerC trigger, ChipmunkC _collidingCMC, ChipmunkCollisionPair _collisionPair, ChipmunkCollisionList _collisionList)
	{
		if (_collidingCMC.isStatic || (trigger.triggerOnlyOnce && trigger.beganTime != 0f))
		{
			return;
		}
		switch (_collisionList)
		{
		case ChipmunkCollisionList.BEGIN:
		{
			for (int l = 0; l < _collidingCMC.TC.childs.Count; l++)
			{
				List<IComponent> componentsByEntityIndex2 = EntityManager.GetComponentsByEntityIndex((ComponentType)112, _collidingCMC.TC.childs[l].entityIndex);
				for (int m = 0; m < componentsByEntityIndex2.Count; m++)
				{
					GETriggerC gETriggerC2 = componentsByEntityIndex2[m] as GETriggerC;
					if (!gETriggerC2.active && gETriggerC2.triggerType == TriggerType.FingerController)
					{
						EntityManager.SetActivityOfEntity(componentsByEntityIndex2[m].entityIndex, true, true);
					}
					gETriggerC2.collidingCount++;
				}
			}
			break;
		}
		case ChipmunkCollisionList.SEPARATE:
		{
			for (int i = 0; i < _collidingCMC.TC.childs.Count; i++)
			{
				List<IComponent> componentsByEntityIndex = EntityManager.GetComponentsByEntityIndex((ComponentType)112, _collidingCMC.TC.childs[i].entityIndex);
				for (int j = 0; j < componentsByEntityIndex.Count; j++)
				{
					GETriggerC gETriggerC = componentsByEntityIndex[j] as GETriggerC;
					if (!gETriggerC.active || gETriggerC.triggerType != TriggerType.FingerController)
					{
						continue;
					}
					gETriggerC.collidingCount--;
					if (gETriggerC.collidingCount > 0)
					{
						continue;
					}
					if (gETriggerC.dragging)
					{
						gETriggerC.dragging = false;
						for (int k = 0; k < gETriggerC.fingerCMC.Count; k++)
						{
							if (gETriggerC.fingerCMC != null && gETriggerC.fingerCMC[k].entityIndex > -1)
							{
								ChipmunkWrapper.SetCustomBodyProperties(gETriggerC.fingerBC.CMC.cpBodyPtr, gETriggerC.fingerBC.linearDamp, gETriggerC.fingerBC.angularDamp, gETriggerC.fingerBC.gravity);
								EntityManager.RemoveEntity(gETriggerC.fingerCMC[k].entityIndex, true);
								gETriggerC.fingerCMC = null;
							}
						}
					}
					EntityManager.SetActivityOfEntity(componentsByEntityIndex[j].entityIndex, false, true);
					gETriggerC.collidingCount = 0;
				}
			}
			break;
		}
		}
	}
}
