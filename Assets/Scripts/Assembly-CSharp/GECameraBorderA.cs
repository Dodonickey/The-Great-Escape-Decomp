using UnityEngine;

public static class GECameraBorderA
{
	public static GETriggerC Assemble(Camera _camera, BasicLevelData _data, Vector3 _pos)
	{
		string[] tags = new string[2]
		{
			LevelManager.m_currentLevel.name + ":GameEntity",
			LevelManager.m_currentLevel.name
		};
		Entity entity = EntityManager.AddEntity(tags);
		TransformC transformC = TransformS.AddComponent(entity);
		TransformS.SetPosition(transformC, _pos);
		CameraBorderC cameraBorderC = CameraS.AddBorderComponent(_camera, transformC, (_data as CameraData).border);
		GETriggerC gETriggerC = GES.AddTriggerComponent(_camera, _data as TriggerData, TriggerType.CameraTargetTrigger, transformC);
		gETriggerC.inputSlots = new ConnectionSlot[0];
		gETriggerC.outputSlots = new ConnectionSlot[0];
		gETriggerC.modifierSlots = new ConnectionSlot[1];
		gETriggerC.modifierSlots[0] = new ConnectionSlot(ConnectionSlotType.Activate, 0);
		if (!_data.active)
		{
			EntityManager.SetActivityOfEntity(gETriggerC.entityIndex, _data.active, true);
		}
		return gETriggerC;
	}

	public static void PopulatePropertyBar(EIC _eiC, UIC _propertyBar)
	{
		string[] tags = new string[1] { "propertyBar" };
		CameraData cameraData = _eiC.container.data as CameraData;
		Camera canvasCamera = _propertyBar.canvasCamera;
		UIC component = CheckBoxA.Assemble(Main.uiCamera, "Active", HandleCameraBorderPropertyChange, null, true, Align.Right, 1f, cameraData.active, tags);
		UIC component2 = RadioButtonA.Assemble(Main.uiCamera, "Inside", HandleCameraBorderPropertyChange, null, true, Align.Bottom, 1f, cameraData.keepInside, 1, 100, tags);
		UIC component3 = RadioButtonA.Assemble(Main.uiCamera, "Outside", HandleCameraBorderPropertyChange, null, true, Align.Bottom, 1f, !cameraData.keepInside, 0, 100, tags);
		UIC component4 = RadioButtonA.Assemble(Main.uiCamera, "Left", HandleCameraBorderPropertyChange, null, true, Align.Bottom, 1f, cameraData.border == 0, 0, 101, tags);
		UIC component5 = RadioButtonA.Assemble(Main.uiCamera, "Right", HandleCameraBorderPropertyChange, null, true, Align.Bottom, 1f, cameraData.border == 1, 1, 101, tags);
		UIC component6 = RadioButtonA.Assemble(Main.uiCamera, "Top", HandleCameraBorderPropertyChange, null, true, Align.Bottom, 1f, cameraData.border == 2, 2, 101, tags);
		UIC component7 = RadioButtonA.Assemble(Main.uiCamera, "Bottom", HandleCameraBorderPropertyChange, null, true, Align.Bottom, 1f, cameraData.border == 3, 3, 101, tags);
		UIS.AddToCanvasGrid(LabelA.Assemble(canvasCamera, "Trigger", tags), _propertyBar, true);
		UIS.AddToCanvasGrid(component, _propertyBar, true);
		UIS.MoveCursor(_propertyBar, 0f, -15f);
		UIS.AddToCanvasGrid(LabelA.Assemble(canvasCamera, "Keep Camera", tags), _propertyBar, true);
		UIS.AddToCanvasGrid(component2, _propertyBar, true);
		UIS.AddToCanvasGrid(component3, _propertyBar, false);
		UIS.MoveCursor(_propertyBar, 0f, -15f);
		UIS.AddToCanvasGrid(LabelA.Assemble(canvasCamera, "Border", tags), _propertyBar, true);
		UIS.AddToCanvasGrid(component4, _propertyBar, true);
		UIS.AddToCanvasGrid(component5, _propertyBar, false);
		UIS.AddToCanvasGrid(component6, _propertyBar, false);
		UIS.AddToCanvasGrid(component7, _propertyBar, false);
		UIS.MoveCursor(_propertyBar, 0f, -15f);
	}

	public static void HandleCameraBorderPropertyChange(EventC _c)
	{
	}
}
