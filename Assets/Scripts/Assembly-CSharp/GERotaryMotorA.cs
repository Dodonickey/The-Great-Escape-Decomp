using System;
using UnityEngine;

public static class GERotaryMotorA
{
	public static GEConstraintC Assemble(EIC _eic, ConstraintData _data)
	{
		string[] tags = new string[2]
		{
			LevelManager.m_currentLevel.name + ":GameEntity",
			LevelManager.m_currentLevel.name
		};
		Entity entity = EntityManager.AddEntity(tags);
		TransformC tc = TransformS.AddComponent(entity);
		IntPtr rotaryMotorPtr = IntPtr.Zero;
		ChipmunkC chipmunkC = null;
		if (_eic.container != null)
		{
			if (_eic.container.gameComponents.Count > 0 && _eic.container.gameComponents[0].componentType == ComponentType.Chipmunk)
			{
				PrefabC prefabC = null;
				chipmunkC = _eic.container.gameComponents[0] as ChipmunkC;
				ChipmunkWrapper.GetBodyValues(chipmunkC.cpBodyPtr, ref chipmunkC.ucpBodyStruct);
				IntPtr spaceStaticBody = ChipmunkWrapper.GetSpaceStaticBody();
				rotaryMotorPtr = (_data.rotaryMotorEnabled ? ChipmunkWrapper.AddSimpleMotor(spaceStaticBody, chipmunkC.cpBodyPtr, _data.rotaryMotorRate * ((float)Math.PI / 180f), _data.rotaryMotorMaxForce * 90000000f) : ((!_data.motorIsStiff) ? ChipmunkWrapper.AddSimpleMotor(spaceStaticBody, chipmunkC.cpBodyPtr, 0f, _data.rotaryMotorMaxForce * 90000000f) : ChipmunkWrapper.AddSimpleMotor(spaceStaticBody, chipmunkC.cpBodyPtr, 0f, 0f)));
			}
			else if (_eic.container.identifier == "Block")
			{
				PrefabC prefabC2 = null;
				GEBlockC gEBlockC = _eic.container.gameComponents[0] as GEBlockC;
				chipmunkC = gEBlockC.CMC;
				ChipmunkWrapper.GetBodyValues(chipmunkC.cpBodyPtr, ref chipmunkC.ucpBodyStruct);
				IntPtr bodyA = ChipmunkWrapper.GetSpaceStaticBody();
				if (_eic.container.container != null)
				{
					if (_eic.container.container.identifier == "Bolt")
					{
						if (_eic.container.container.container != null && _eic.container.container.container.identifier == "Block")
						{
							bodyA = (_eic.container.container.gameComponents[0] as GEConstraintC).CMC.cpBodyPtr;
						}
						prefabC2 = PrefabS.AddComponent(gEBlockC.CMC.TC, Vector3.zero, ResourceManager.GetGameObject("RotaryMotor"));
						prefabC2.p_gameObject.transform.position = _eic.container.container.data.position.ToVector3();
					}
					else
					{
						prefabC2 = PrefabS.AddComponent(gEBlockC.CMC.TC, Vector3.zero, ResourceManager.GetGameObject("RotaryMotor"));
					}
				}
				else
				{
					prefabC2 = PrefabS.AddComponent(gEBlockC.CMC.TC, Vector3.zero, ResourceManager.GetGameObject("RotaryMotor"));
				}
				rotaryMotorPtr = (_data.rotaryMotorEnabled ? ChipmunkWrapper.AddSimpleMotor(bodyA, chipmunkC.cpBodyPtr, _data.rotaryMotorRate * ((float)Math.PI / 180f), _data.rotaryMotorMaxForce * 90000000f) : (_data.motorIsStiff ? ChipmunkWrapper.AddSimpleMotor(bodyA, chipmunkC.cpBodyPtr, 0f, _data.rotaryMotorMaxForce * 90000000f) : ChipmunkWrapper.AddSimpleMotor(bodyA, chipmunkC.cpBodyPtr, 0f, 0f)));
			}
		}
		GEConstraintC gEConstraintC = GES.AddConstraintComponent(_data, tc, null);
		gEConstraintC.rotaryMotorPtr = rotaryMotorPtr;
		gEConstraintC.constraintType = ConstraintType.RotaryMotor;
		gEConstraintC.inputSlots = new ConnectionSlot[2];
		gEConstraintC.inputSlots[0] = new ConnectionSlot(ConnectionSlotType.RotaryMotorEnabled, 0);
		gEConstraintC.inputSlots[1] = new ConnectionSlot(ConnectionSlotType.RotaryMotorRate, 1);
		gEConstraintC.outputSlots = new ConnectionSlot[1];
		gEConstraintC.outputSlots[0] = new ConnectionSlot(ConnectionSlotType.Output, 0);
		gEConstraintC.modifierSlots = new ConnectionSlot[1];
		gEConstraintC.modifierSlots[0] = new ConnectionSlot(ConnectionSlotType.Destroy, 0);
		gEConstraintC.triggerType = TriggerType.RotaryMotorConstraint;
		gEConstraintC.autoTrigger = true;
		gEConstraintC.energy = 1f;
		gEConstraintC.updateRail = true;
		gEConstraintC.camera = Main.camera;
		gEConstraintC.rotaryMotorEnabled = _data.rotaryMotorEnabled;
		gEConstraintC.rotaryMotorRate = _data.rotaryMotorRate;
		gEConstraintC.motorIsStiff = _data.motorIsStiff;
		_eic.trigger = gEConstraintC;
		if (gEConstraintC.currentRailPos == 0f || gEConstraintC.currentRailPos == 1f)
		{
			gEConstraintC.moveFromPoint = Main.m_gameTime;
		}
		return gEConstraintC;
	}

	public static EIC CreateNewEditorItem(EIC _container, string _identifier, Vector3 _pos, Vector3 _rot, Vector3 _sca)
	{
		uint uniqueId = GES.GetUniqueId();
		ConstraintData constraintData = new ConstraintData();
		constraintData.position = new Vertex3(_pos);
		constraintData.rotation = new Vertex3(_rot);
		constraintData.scale = new Vertex3(_sca);
		constraintData.constraintType = 6u;
		constraintData.rotaryMotor = true;
		constraintData.rotaryMotorEnabled = true;
		constraintData.rotaryMotorMaxForce = 0.1f;
		constraintData.rotaryMotorRate = 90f;
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
		if (_eiC.identifier == "Motor")
		{
			ConstraintData constraintData = _eiC.data as ConstraintData;
			UIC component = CheckBoxA.Assemble(canvasCamera, "Rotary Motor", GEBoltA.HandleConstraintPropertyChange, null, true, Align.Right, 1f, constraintData.rotaryMotor, tags);
			UIC component2 = CheckBoxA.Assemble(canvasCamera, "Rotary Motor Enabled", GEBoltA.HandleConstraintPropertyChange, null, true, Align.Right, 1f, constraintData.rotaryMotorEnabled, tags);
			UIC component3 = NumericFieldA.Assemble(canvasCamera, "Rotary Motor Max Force", GEBoltA.HandleConstraintPropertyChange, null, true, Align.Left, 80f, 1f, false, 0f, 1f, constraintData.rotaryMotorMaxForce, tags);
			UIC component4 = NumericFieldA.Assemble(canvasCamera, "Rotary Motor Rate", GEBoltA.HandleConstraintPropertyChange, null, true, Align.Left, 80f, 1f, false, -720f, 720f, constraintData.rotaryMotorRate, tags);
			UIC component5 = CheckBoxA.Assemble(canvasCamera, "Motor Is Stiff", GEBoltA.HandleConstraintPropertyChange, null, true, Align.Right, 1f, constraintData.motorIsStiff, tags);
			UIS.AddToCanvasGrid(LabelA.Assemble(canvasCamera, "Motor", tags), _propertyBar, true);
			UIS.AddToCanvasGrid(component, _propertyBar, true);
			UIS.AddToCanvasGrid(component2, _propertyBar, true);
			UIS.AddToCanvasGrid(component3, _propertyBar, true);
			UIS.AddToCanvasGrid(component4, _propertyBar, false);
			UIS.AddToCanvasGrid(component5, _propertyBar, true);
		}
	}
}
