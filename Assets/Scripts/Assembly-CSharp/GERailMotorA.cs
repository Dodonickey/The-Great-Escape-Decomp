using UnityEngine;

public static class GERailMotorA
{
	public static GEConstraintC Assemble(EIC _eic, ConstraintData _data)
	{
		string[] tags = new string[2]
		{
			LevelManager.m_currentLevel.name + ":GameEntity",
			LevelManager.m_currentLevel.name
		};
		Entity entity = EntityManager.AddEntity(tags);
		TransformC transformC = TransformS.AddComponent(entity);
		ChipmunkC chipmunkC = ChipmunkS.AddInactiveComponent(transformC, true, ColliderType.Any);
		ChipmunkS.ActivateChipmunkComponent(chipmunkC, ChipmunkWrapper.AddBody(false, false, _data.position.ToVector2(), chipmunkC.index, ColliderType.Any));
		ChipmunkWrapper.AddCircleShape(chipmunkC.cpBodyPtr, Vector2.zero, 5f, 10f, 0f, 0f, 0u, 0u, true);
		transformC.parentedToPhysics = true;
		if (_eic.container != null && _eic.container.identifier == "Rail")
		{
			GEConstraintC gEConstraintC = _eic.container.gameComponents[0] as GEConstraintC;
			GEConstraintC gEConstraintC2 = GES.AddConstraintComponent(_data, transformC, null);
			gEConstraintC2.constraintType = ConstraintType.RailMotor;
			gEConstraintC2.inputSlots = new ConnectionSlot[2];
			gEConstraintC2.inputSlots[0] = new ConnectionSlot(ConnectionSlotType.RailMotorEnabled, 0);
			gEConstraintC2.inputSlots[1] = new ConnectionSlot(ConnectionSlotType.RailMotorRate, 1);
			gEConstraintC2.outputSlots = new ConnectionSlot[1];
			gEConstraintC2.outputSlots[0] = new ConnectionSlot(ConnectionSlotType.Output, 0);
			gEConstraintC2.modifierSlots = new ConnectionSlot[1];
			gEConstraintC2.modifierSlots[0] = new ConnectionSlot(ConnectionSlotType.Destroy, 0);
			gEConstraintC2.triggerType = TriggerType.RailMotorConstraint;
			gEConstraintC2.autoTrigger = true;
			gEConstraintC2.energy = 1f;
			gEConstraintC2.camera = Main.camera;
			gEConstraintC2.loopStyle = (_data.linearMotorLoop ? 1 : 0);
			gEConstraintC2.rail = gEConstraintC;
			gEConstraintC2.CMC = chipmunkC;
			gEConstraintC2.linearMotorDirection = 1;
			gEConstraintC2.linearMotorEnabled = _data.linearMotorEnabled;
			gEConstraintC2.linearMotorRate = _data.linearMotorRate;
			gEConstraintC2.currentIndex = (int)_data.linearMotorStartIndex;
			if (gEConstraintC.anchorPoints != null && gEConstraintC2.currentIndex >= gEConstraintC.anchorPoints.Length)
			{
				gEConstraintC2.currentIndex = gEConstraintC.anchorPoints.Length - 1;
			}
			gEConstraintC2.currentRailPos = _data.linearMotorStartPos;
			_eic.trigger = gEConstraintC2;
			return gEConstraintC2;
		}
		return null;
	}

	public static EIC CreateNewEditorItem(EIC _container, string _identifier, Vector3 _pos, Vector3 _rot, Vector3 _sca)
	{
		uint uniqueId = GES.GetUniqueId();
		ConstraintData constraintData = new ConstraintData();
		constraintData.position = new Vertex3(_pos);
		constraintData.rotation = new Vertex3(_rot);
		constraintData.scale = new Vertex3(_sca);
		constraintData.constraintType = 7u;
		constraintData.linearMotor = true;
		constraintData.linearMotorEnabled = true;
		constraintData.linearMotorMaxForce = 0.1f;
		constraintData.linearMotorRate = 1f;
		constraintData.linearMotorStartIndex = 0u;
		constraintData.linearMotorStartPos = 0f;
		constraintData.linearMotorLoop = true;
		constraintData.Init(uniqueId, _identifier + uniqueId);
		EIC eIC = GEItemA.Assemble(_container, _identifier, constraintData, Main.camera);
		eIC.isRealtimeMovable = true;
		return eIC;
	}

	public static EIC CreateLoadedEditorItem(EIC _container, EIC _loadedItem)
	{
		return GEItemA.Assemble(_container, _loadedItem.identifier, _loadedItem.data, Main.camera);
	}

	public static void FillEditorItem(EIC _eic)
	{
		ConstraintData data = _eic.data as ConstraintData;
		GEConstraintC gEConstraintC = Assemble(_eic, data);
		if (gEConstraintC != null)
		{
			_eic.gameComponents.Add(gEConstraintC);
			if (GEState.editorMode)
			{
				TransformS.ParentComponent(gEConstraintC.TC, _eic.TC, Vector3.zero);
			}
		}
	}

	public static void PopulatePropertyBar(EIC _eiC, UIC _propertyBar)
	{
		string[] tags = new string[1] { "propertyBar" };
		Camera canvasCamera = _propertyBar.canvasCamera;
		if (_eiC.identifier == "Rail Motor")
		{
			ConstraintData constraintData = _eiC.data as ConstraintData;
			UIC component = CheckBoxA.Assemble(canvasCamera, "Rail Motor Enabled", GEBoltA.HandleConstraintPropertyChange, null, true, Align.Right, 1f, constraintData.linearMotorEnabled, tags);
			UIC component2 = NumericFieldA.Assemble(canvasCamera, "Rail Motor Max Force", GEBoltA.HandleConstraintPropertyChange, null, true, Align.Left, 80f, 1f, false, 0f, 1f, constraintData.linearMotorMaxForce, tags);
			UIC component3 = NumericFieldA.Assemble(canvasCamera, "Rail Motor Rate", GEBoltA.HandleConstraintPropertyChange, null, true, Align.Left, 80f, 1f, false, -720f, 720f, constraintData.linearMotorRate, tags);
			UIC component4 = NumericFieldA.Assemble(canvasCamera, "Rail Motor Start Index", GEBoltA.HandleConstraintPropertyChange, null, true, Align.Left, 80f, 1f, true, 0f, 999f, constraintData.linearMotorStartIndex, tags);
			UIC component5 = NumericFieldA.Assemble(canvasCamera, "Rail Motor Start Pos", GEBoltA.HandleConstraintPropertyChange, null, true, Align.Left, 80f, 1f, false, 0f, 1f, constraintData.linearMotorStartPos, tags);
			UIC component6 = CheckBoxA.Assemble(canvasCamera, "Rail Motor Loops", GEBoltA.HandleConstraintPropertyChange, null, true, Align.Right, 1f, constraintData.linearMotorLoop, tags);
			UIS.AddToCanvasGrid(LabelA.Assemble(canvasCamera, "Rail Motor", tags), _propertyBar, true);
			UIS.AddToCanvasGrid(component, _propertyBar, true);
			UIS.AddToCanvasGrid(component2, _propertyBar, true);
			UIS.AddToCanvasGrid(component3, _propertyBar, false);
			UIS.AddToCanvasGrid(component4, _propertyBar, true);
			UIS.AddToCanvasGrid(component5, _propertyBar, true);
			UIS.AddToCanvasGrid(component6, _propertyBar, true);
		}
	}
}
