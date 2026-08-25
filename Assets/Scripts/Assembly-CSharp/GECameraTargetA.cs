using System.Collections.Generic;
using UnityEngine;

public static class GECameraTargetA
{
	public static CameraTargetC Assemble(EIC _eic, CameraData _cameraData)
	{
		Vector3 position = _cameraData.position.ToVector3();
		Entity entity = null;
		if (_eic.container != null && (_eic.container.identifier == "Elvis & Kart" || _eic.container.identifier == "Kevin & Kart" || _eic.container.identifier == "Elvis" || _eic.container.identifier == "Kevin"))
		{
			string[] tags = new string[4]
			{
				LevelManager.m_currentLevel.name + ":GameEntity",
				LevelManager.m_currentLevel.name,
				_eic.identifier,
				"PlayerTarget"
			};
			entity = EntityManager.AddEntity(tags);
		}
		else
		{
			string[] tags2 = new string[3]
			{
				LevelManager.m_currentLevel.name + ":GameEntity",
				LevelManager.m_currentLevel.name,
				_eic.identifier
			};
			entity = EntityManager.AddEntity(tags2);
		}
		TransformC transformC = TransformS.AddComponent(entity);
		transformC.forceRotation = true;
		if (_eic.container != null)
		{
			for (int i = 0; i < _eic.container.gameComponents.Count; i++)
			{
				IComponent component = _eic.container.gameComponents[i];
				if (component.componentType == (ComponentType)100)
				{
					GEBlockC gEBlockC = component as GEBlockC;
					TransformS.ParentComponent(transformC, gEBlockC.CMC.TC, Vector3.zero);
					break;
				}
				if (component.componentType == (ComponentType)102)
				{
					GECharacterC gECharacterC = component as GECharacterC;
					TransformS.ParentComponent(transformC, gECharacterC.rootNode.TC, Vector3.zero);
					break;
				}
				if (component.componentType == (ComponentType)113)
				{
					GEVehicleC gEVehicleC = component as GEVehicleC;
					TransformS.ParentComponent(transformC, gEVehicleC.rootNode.TC, Vector3.zero);
					break;
				}
				List<IComponent> componentsByEntityIndex = EntityManager.GetComponentsByEntityIndex(ComponentType.Chipmunk, component.entityIndex);
				if (componentsByEntityIndex.Count > 0)
				{
					TransformS.ParentComponent(transformC, (componentsByEntityIndex[0] as ChipmunkC).TC, Vector3.zero);
					break;
				}
				List<IComponent> componentsByEntityIndex2 = EntityManager.GetComponentsByEntityIndex(ComponentType.Transform, component.entityIndex);
				if (componentsByEntityIndex2.Count > 0)
				{
					TransformS.ParentComponent(transformC, componentsByEntityIndex2[0] as TransformC, Vector3.zero);
					break;
				}
			}
		}
		else
		{
			TransformS.SetGlobalPosition(transformC, position);
		}
		if (!GEState.editorMode && _cameraData.active)
		{
			CameraS.m_currentCameraPosition = _cameraData.position.ToVector3() + _cameraData.offset.ToVector3() + Vector3.forward * _cameraData.lowVelocityDistance;
			Main.camera.transform.position = CameraS.m_currentCameraPosition;
		}
		CameraTargetC cameraTargetC = CameraS.AddTargetComponent(_eic.camera, transformC, _cameraData.offset.ToVector3(), _cameraData.destinationSmooth, _cameraData.directionalSmooth, _cameraData.lowVelocity.ToVector3(), _cameraData.highVelocity.ToVector3(), _cameraData.lowVelocityDistance, _cameraData.highVelocityDistance, _cameraData.directionalOffset, _cameraData.maxDisplacement);
		cameraTargetC.rotationOffset = _cameraData.rotationOffset.ToVector3();
		if (GEState.editorMode)
		{
			cameraTargetC.active = false;
		}
		GETriggerC gETriggerC = GES.AddTriggerComponent(_eic.camera, _cameraData, TriggerType.CameraTargetTrigger, transformC);
		gETriggerC.inputSlots = new ConnectionSlot[0];
		gETriggerC.outputSlots = new ConnectionSlot[0];
		gETriggerC.modifierSlots = new ConnectionSlot[2];
		gETriggerC.modifierSlots = new ConnectionSlot[3];
		gETriggerC.modifierSlots[0] = new ConnectionSlot(ConnectionSlotType.Activate, 0);
		gETriggerC.modifierSlots[1] = new ConnectionSlot(ConnectionSlotType.Deactivate, 1);
		gETriggerC.modifierSlots[2] = new ConnectionSlot(ConnectionSlotType.Destroy, 2);
		_eic.trigger = gETriggerC;
		if (!_cameraData.active)
		{
			EntityManager.SetActivityOfEntity(gETriggerC.entityIndex, _cameraData.active, true);
		}
		return cameraTargetC;
	}

	public static EIC CreateNewEditorItem(EIC _container, string _identifier, Vector3 _pos, Vector3 _rot, Vector3 _sca)
	{
		EIC eIC = null;
		CameraData cameraData = new CameraData();
		cameraData.position = new Vertex3(_pos);
		cameraData.rotation = new Vertex3(_rot);
		cameraData.scale = new Vertex3(_sca);
		cameraData.active = true;
		cameraData.offset = new Vertex3(Vector3.up * 50f);
		cameraData.rotationOffset = new Vertex3(new Vector3(5f, 0f, 0f));
		cameraData.lowVelocity = new Vertex3(new Vector3(1f, 2f, 0f));
		cameraData.highVelocity = new Vertex3(new Vector3(6f, 8f, 0f));
		cameraData.destinationSmooth = 0.05f;
		cameraData.directionalSmooth = 0.05f;
		cameraData.lowVelocityDistance = -300f;
		cameraData.highVelocityDistance = -500f;
		cameraData.directionalOffset = 350f;
		cameraData.maxDisplacement = 150f;
		cameraData.keepDirOffsetUntilLowVelocity = true;
		cameraData.triggerType = 8u;
		cameraData.connect = false;
		cameraData.shapeType = 0;
		cameraData.active = true;
		cameraData.toggle = false;
		cameraData.triggerOnlyOnce = false;
		cameraData.triggerUntilOutOfEnergy = false;
		cameraData.triggerOnlyOnFullEnergy = false;
		cameraData.autoTrigger = false;
		cameraData.energy = 1f;
		cameraData.energyClips = -1;
		cameraData.energyGain = 0f;
		cameraData.energyConsume = 0f;
		cameraData.gainInterval = 0f;
		cameraData.consumeInterval = 0f;
		cameraData.cooldown = 0f;
		uint uniqueId = GES.GetUniqueId();
		cameraData.Init(uniqueId, _identifier + uniqueId);
		eIC = GEItemA.Assemble(_container, _identifier, cameraData, Main.camera);
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
		CameraData cameraData = _eic.data as CameraData;
		CameraTargetC item = Assemble(_eic, cameraData);
		_eic.gameComponents.Add(item);
	}

	public static void PopulatePropertyBar(EIC _eiC, UIC _propertyBar)
	{
		string[] tags = new string[1] { "propertyBar" };
		CameraData cameraData = _eiC.data as CameraData;
		Camera canvasCamera = _propertyBar.canvasCamera;
		UIC component = CheckBoxA.Assemble(Main.uiCamera, "Active", HandleCameraTargetPropertyChange, null, true, Align.Right, 1f, cameraData.active, tags);
		UIC component2 = NumericFieldA.Assemble(Main.uiCamera, "Offset X", HandleCameraTargetPropertyChange, null, true, Align.Left, 50f, 1f, false, -1000f, 1000f, cameraData.offset.x, tags);
		UIC component3 = NumericFieldA.Assemble(Main.uiCamera, "Offset Y", HandleCameraTargetPropertyChange, null, true, Align.Left, 50f, 1f, false, -1000f, 1000f, cameraData.offset.y, tags);
		UIC component4 = NumericFieldA.Assemble(Main.uiCamera, "Offset Z", HandleCameraTargetPropertyChange, null, true, Align.Left, 50f, 1f, false, -2000f, 0f, cameraData.offset.z, tags);
		UIC component5 = NumericFieldA.Assemble(Main.uiCamera, "Rotation X", HandleCameraTargetPropertyChange, null, true, Align.Left, 50f, 1f, false, -90f, 90f, cameraData.rotationOffset.x, tags);
		UIC component6 = NumericFieldA.Assemble(Main.uiCamera, "Rotation Y", HandleCameraTargetPropertyChange, null, true, Align.Left, 50f, 1f, false, -90f, 90f, cameraData.rotationOffset.y, tags);
		UIC component7 = NumericFieldA.Assemble(Main.uiCamera, "Rotation Z", HandleCameraTargetPropertyChange, null, true, Align.Left, 50f, 1f, false, -90f, 90f, cameraData.rotationOffset.z, tags);
		UIC component8 = NumericFieldA.Assemble(Main.uiCamera, "Low Vel X", HandleCameraTargetPropertyChange, null, true, Align.Left, 50f, 1f, false, 0f, 100f, cameraData.lowVelocity.x, tags);
		UIC component9 = NumericFieldA.Assemble(Main.uiCamera, "Low Vel Y", HandleCameraTargetPropertyChange, null, true, Align.Left, 50f, 1f, false, 0f, 100f, cameraData.lowVelocity.y, tags);
		UIC component10 = NumericFieldA.Assemble(Main.uiCamera, "High Vel X", HandleCameraTargetPropertyChange, null, true, Align.Left, 50f, 1f, false, 0f, 100f, cameraData.highVelocity.x, tags);
		UIC component11 = NumericFieldA.Assemble(Main.uiCamera, "High Vel Y", HandleCameraTargetPropertyChange, null, true, Align.Left, 50f, 1f, false, 0f, 100f, cameraData.highVelocity.y, tags);
		UIC component12 = NumericFieldA.Assemble(Main.uiCamera, "Des Smooth", HandleCameraTargetPropertyChange, null, true, Align.Left, 50f, 1f, false, 0f, 1f, cameraData.destinationSmooth, tags);
		UIC component13 = NumericFieldA.Assemble(Main.uiCamera, "Dir Smooth", HandleCameraTargetPropertyChange, null, true, Align.Left, 50f, 1f, false, 0f, 1f, cameraData.directionalSmooth, tags);
		UIC component14 = NumericFieldA.Assemble(Main.uiCamera, "Low Vel Dist", HandleCameraTargetPropertyChange, null, true, Align.Left, 50f, 1f, false, -2000f, 0f, cameraData.lowVelocityDistance, tags);
		UIC component15 = NumericFieldA.Assemble(Main.uiCamera, "High Vel Dist", HandleCameraTargetPropertyChange, null, true, Align.Left, 50f, 1f, false, -2000f, 0f, cameraData.highVelocityDistance, tags);
		UIC component16 = NumericFieldA.Assemble(Main.uiCamera, "Dir Offset", HandleCameraTargetPropertyChange, null, true, Align.Left, 50f, 1f, false, 0f, 1000f, cameraData.directionalOffset, tags);
		UIC component17 = NumericFieldA.Assemble(Main.uiCamera, "Max Offset", HandleCameraTargetPropertyChange, null, true, Align.Left, 50f, 1f, false, 0f, 1000f, cameraData.maxDisplacement, tags);
		UIS.AddToCanvasGrid(LabelA.Assemble(canvasCamera, "Trigger", tags), _propertyBar, true);
		UIS.AddToCanvasGrid(component, _propertyBar, true);
		UIS.MoveCursor(_propertyBar, 0f, -15f);
		UIS.AddToCanvasGrid(LabelA.Assemble(canvasCamera, "Target", tags), _propertyBar, true);
		UIS.AddToCanvasGrid(component2, _propertyBar, true);
		UIS.AddToCanvasGrid(component3, _propertyBar, false);
		UIS.AddToCanvasGrid(component4, _propertyBar, false);
		UIS.AddToCanvasGrid(component5, _propertyBar, true);
		UIS.AddToCanvasGrid(component6, _propertyBar, false);
		UIS.AddToCanvasGrid(component7, _propertyBar, false);
		UIS.AddToCanvasGrid(component16, _propertyBar, true);
		UIS.AddToCanvasGrid(component17, _propertyBar, false);
		UIS.AddToCanvasGrid(component8, _propertyBar, true);
		UIS.AddToCanvasGrid(component9, _propertyBar, false);
		UIS.AddToCanvasGrid(component10, _propertyBar, true);
		UIS.AddToCanvasGrid(component11, _propertyBar, false);
		UIS.AddToCanvasGrid(component14, _propertyBar, true);
		UIS.AddToCanvasGrid(component15, _propertyBar, false);
		UIS.AddToCanvasGrid(component12, _propertyBar, true);
		UIS.AddToCanvasGrid(component13, _propertyBar, false);
		UIS.MoveCursor(_propertyBar, 0f, -15f);
	}

	public static void HandleCameraTargetPropertyChange(EventC _c)
	{
		CameraData cameraData = EditorState.m_selection[0].data as CameraData;
		switch (_c.identifier)
		{
		case "Active":
			cameraData.active = (bool)_c.properties["checked"];
			break;
		case "Offset X":
			cameraData.offset.x = (float)_c.properties["value"];
			break;
		case "Offset Y":
			cameraData.offset.y = (float)_c.properties["value"];
			break;
		case "Offset Z":
			cameraData.offset.z = (float)_c.properties["value"];
			break;
		case "Rotation X":
			cameraData.rotationOffset.x = (float)_c.properties["value"];
			break;
		case "Rotation Y":
			cameraData.rotationOffset.y = (float)_c.properties["value"];
			break;
		case "Rotation Z":
			cameraData.rotationOffset.z = (float)_c.properties["value"];
			break;
		case "Low Vel X":
			cameraData.lowVelocity.x = (float)_c.properties["value"];
			break;
		case "Low Vel Y":
			cameraData.lowVelocity.y = (float)_c.properties["value"];
			break;
		case "High Vel X":
			cameraData.highVelocity.x = (float)_c.properties["value"];
			break;
		case "High Vel Y":
			cameraData.highVelocity.y = (float)_c.properties["value"];
			break;
		case "Des Smooth":
			cameraData.destinationSmooth = (float)_c.properties["value"];
			break;
		case "Dir Smooth":
			cameraData.directionalSmooth = (float)_c.properties["value"];
			break;
		case "Low Vel Dist":
			cameraData.lowVelocityDistance = (float)_c.properties["value"];
			break;
		case "High Vel Dist":
			cameraData.highVelocityDistance = (float)_c.properties["value"];
			break;
		case "Dir Offset":
			cameraData.directionalOffset = (float)_c.properties["value"];
			break;
		case "Max Offset":
			cameraData.maxDisplacement = (float)_c.properties["value"];
			break;
		}
		EditorState.ResetEditorItem(EditorState.m_selection[0]);
	}
}
