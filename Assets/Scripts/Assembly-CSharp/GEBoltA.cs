using System;
using System.Collections.Generic;
using UnityEngine;

public static class GEBoltA
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
		TransformC transformC2 = TransformS.AddComponent(entity);
		transformC2.forceRotation = true;
		TransformS.ParentComponent(transformC2, transformC, Vector3.zero);
		TransformS.SetGlobalPosition(transformC, _data.position.ToVector3());
		PrefabC prefabC = null;
		if (_data.rotaryLimit || _data.rotaryStiffness > 0f)
		{
			prefabC = PrefabS.AddComponent(transformC, Vector3.zero, ResourceManager.GetGameObject("BoltNut"));
		}
		else
		{
			prefabC = PrefabS.AddComponent(transformC, Vector3.zero, ResourceManager.GetGameObject("Bolt"));
		}
		IntPtr rotaryStiffnessPtr = IntPtr.Zero;
		IntPtr rotaryLimitJointPtr = IntPtr.Zero;
		IntPtr rotarySpringPtr = IntPtr.Zero;
		IntPtr zero = IntPtr.Zero;
		IntPtr connectJointPtr = IntPtr.Zero;
		List<ChipmunkC> list = new List<ChipmunkC>();
		List<Vector2> list2 = new List<Vector2>();
		ChipmunkC chipmunkC = null;
		if (_eic.container != null)
		{
			if (_eic.container.gameComponents.Count > 0 && _eic.container.gameComponents[0].componentType == ComponentType.Chipmunk)
			{
				chipmunkC = _eic.container.gameComponents[0] as ChipmunkC;
				ChipmunkWrapper.GetBodyValues(chipmunkC.cpBodyPtr, ref chipmunkC.ucpBodyStruct);
				if (_data.connectToWorld)
				{
					connectJointPtr = ((!_data.softConnection) ? ChipmunkWrapper.AddPivotJoint(ChipmunkWrapper.GetSpaceStaticBody(), chipmunkC.cpBodyPtr, _eic.data.position.ToVector2()) : ChipmunkWrapper.AddDampedSpring2(ChipmunkWrapper.GetSpaceStaticBody(), chipmunkC.cpBodyPtr, _eic.data.position.ToVector2(), 0f, _data.softConnectionStrength * 90000f, _data.softConnectionDamp * 900f));
				}
				if (_data.rotaryStiffness > 0f)
				{
					rotaryStiffnessPtr = ChipmunkWrapper.AddSimpleMotor(ChipmunkWrapper.GetSpaceStaticBody(), chipmunkC.cpBodyPtr, 0f, _data.rotaryStiffness * 90000000f);
				}
				if (_data.rotaryLimit)
				{
					rotaryLimitJointPtr = ChipmunkWrapper.AddRotaryLimitJoint(ChipmunkWrapper.GetSpaceStaticBody(), chipmunkC.cpBodyPtr, chipmunkC.ucpBodyStruct.a + _data.rotaryLimitMin * ((float)Math.PI / 180f), chipmunkC.ucpBodyStruct.a + _data.rotaryLimitMax * ((float)Math.PI / 180f));
				}
				if (_data.rotarySpring)
				{
					rotarySpringPtr = ChipmunkWrapper.AddDampedRotarySpring(ChipmunkWrapper.GetSpaceStaticBody(), chipmunkC.cpBodyPtr, 0f - chipmunkC.ucpBodyStruct.a, _data.rotarySpringStrength * 90000000f, _data.rotarySpringDamp * 90000f);
				}
				TransformS.ParentComponent(transformC, chipmunkC.TC, ChipmunkWrapper.GetLocalPos(chipmunkC.cpBodyPtr, _eic.data.position.ToVector2()));
				uint num = (uint)(_eic.index + 10000);
				ChipmunkQueryInfo[] array = new ChipmunkQueryInfo[100];
				int connectedBodies = ChipmunkWrapper.GetConnectedBodies(chipmunkC.cpBodyPtr, array);
				for (int i = 0; i < connectedBodies; i++)
				{
					ChipmunkC chipmunkC2 = ChipmunkS.m_components.m_array[array[i].componentIndex];
					ChipmunkWrapper.SetBodyGroup(chipmunkC2.cpBodyPtr, num);
					chipmunkC2.colliderGroup = num;
				}
			}
			else if (_eic.container.identifier == "Block")
			{
				GEBlockC gEBlockC = _eic.container.gameComponents[0] as GEBlockC;
				chipmunkC = gEBlockC.CMC;
				ChipmunkWrapper.GetBodyValues(chipmunkC.cpBodyPtr, ref chipmunkC.ucpBodyStruct);
				if (_data.connectToWorld)
				{
					connectJointPtr = ((!_data.softConnection) ? ChipmunkWrapper.AddPivotJoint(ChipmunkWrapper.GetSpaceStaticBody(), chipmunkC.cpBodyPtr, _eic.data.position.ToVector2()) : ChipmunkWrapper.AddDampedSpring2(ChipmunkWrapper.GetSpaceStaticBody(), chipmunkC.cpBodyPtr, _eic.data.position.ToVector2(), 0f, _data.softConnectionStrength * 90000f, _data.softConnectionDamp * 900f));
				}
				if (_data.rotaryStiffness > 0f)
				{
					rotaryStiffnessPtr = ChipmunkWrapper.AddSimpleMotor(ChipmunkWrapper.GetSpaceStaticBody(), chipmunkC.cpBodyPtr, 0f, _data.rotaryStiffness * 90000000f);
				}
				if (_data.rotaryLimit)
				{
					rotaryLimitJointPtr = ChipmunkWrapper.AddRotaryLimitJoint(ChipmunkWrapper.GetSpaceStaticBody(), chipmunkC.cpBodyPtr, chipmunkC.ucpBodyStruct.a + _data.rotaryLimitMin * ((float)Math.PI / 180f), chipmunkC.ucpBodyStruct.a + _data.rotaryLimitMax * ((float)Math.PI / 180f));
				}
				if (_data.rotarySpring)
				{
					rotarySpringPtr = ChipmunkWrapper.AddDampedRotarySpring(ChipmunkWrapper.GetSpaceStaticBody(), chipmunkC.cpBodyPtr, 0f - chipmunkC.ucpBodyStruct.a, _data.rotarySpringStrength * 90000000f, _data.rotarySpringDamp * 90000f);
				}
				TransformS.ParentComponent(transformC, chipmunkC.TC, ChipmunkWrapper.GetLocalPos(chipmunkC.cpBodyPtr, _eic.data.position.ToVector2()));
				uint num2 = (uint)(_eic.index + 10000);
				ChipmunkQueryInfo[] array2 = new ChipmunkQueryInfo[100];
				int connectedBodies2 = ChipmunkWrapper.GetConnectedBodies(chipmunkC.cpBodyPtr, array2);
				for (int j = 0; j < connectedBodies2; j++)
				{
					ChipmunkC chipmunkC3 = ChipmunkS.m_components.m_array[array2[j].componentIndex];
					ChipmunkWrapper.SetBodyGroup(chipmunkC3.cpBodyPtr, num2);
					chipmunkC3.colliderGroup = num2;
				}
			}
			else if (_eic.container.identifier == "Rail Motor" && _eic.container.gameComponents.Count > 0)
			{
				GEConstraintC gEConstraintC = _eic.container.gameComponents[0] as GEConstraintC;
				chipmunkC = gEConstraintC.CMC;
				connectJointPtr = (gEConstraintC.railedPivotJointPtr = ChipmunkWrapper.AddPivotJoint(ChipmunkWrapper.GetSpaceStaticBody(), chipmunkC.cpBodyPtr, _eic.data.position.ToVector2()));
				if (_data.rotaryStiffness > 0f)
				{
					rotaryStiffnessPtr = ChipmunkWrapper.AddSimpleMotor(ChipmunkWrapper.GetSpaceStaticBody(), chipmunkC.cpBodyPtr, 0f, _data.rotaryStiffness * 90000000f);
				}
				if (_data.rotaryLimit)
				{
					rotaryLimitJointPtr = ChipmunkWrapper.AddRotaryLimitJoint(ChipmunkWrapper.GetSpaceStaticBody(), chipmunkC.cpBodyPtr, chipmunkC.ucpBodyStruct.a + _data.rotaryLimitMin * ((float)Math.PI / 180f), chipmunkC.ucpBodyStruct.a + _data.rotaryLimitMax * ((float)Math.PI / 180f));
				}
				if (_data.rotarySpring)
				{
					rotarySpringPtr = ChipmunkWrapper.AddDampedRotarySpring(ChipmunkWrapper.GetSpaceStaticBody(), chipmunkC.cpBodyPtr, 0f - chipmunkC.ucpBodyStruct.a, _data.rotarySpringStrength * 90000000f, _data.rotarySpringDamp * 90000f);
				}
				uint num3 = (uint)(_eic.index + 10000);
				ChipmunkQueryInfo[] array3 = new ChipmunkQueryInfo[100];
				int connectedBodies3 = ChipmunkWrapper.GetConnectedBodies(chipmunkC.cpBodyPtr, array3);
				for (int k = 0; k < connectedBodies3; k++)
				{
					ChipmunkC chipmunkC4 = ChipmunkS.m_components.m_array[array3[k].componentIndex];
					ChipmunkWrapper.SetBodyGroup(chipmunkC4.cpBodyPtr, num3);
					chipmunkC4.colliderGroup = num3;
				}
			}
		}
		GEConstraintC gEConstraintC2 = GES.AddConstraintComponent(_data, transformC, null);
		gEConstraintC2.CMC = chipmunkC;
		gEConstraintC2.connectJointPtr = connectJointPtr;
		gEConstraintC2.rotaryLimitJointPtr = rotaryLimitJointPtr;
		gEConstraintC2.rotarySpringPtr = rotarySpringPtr;
		gEConstraintC2.rotaryMotorPtr = zero;
		gEConstraintC2.rotaryStiffnessPtr = rotaryStiffnessPtr;
		gEConstraintC2.connectedBodies = list.ToArray();
		gEConstraintC2.connectedBodyLocalAnchors = list2.ToArray();
		gEConstraintC2.constraintType = ConstraintType.Bolt;
		gEConstraintC2.inputSlots = new ConnectionSlot[0];
		gEConstraintC2.outputSlots = new ConnectionSlot[0];
		gEConstraintC2.modifierSlots = new ConnectionSlot[1];
		gEConstraintC2.modifierSlots[0] = new ConnectionSlot(ConnectionSlotType.Destroy, 0);
		gEConstraintC2.triggerType = TriggerType.BoltConstraint;
		gEConstraintC2.autoTrigger = true;
		gEConstraintC2.energy = 1f;
		gEConstraintC2.updateRail = true;
		gEConstraintC2.camera = Main.camera;
		if (gEConstraintC2.currentRailPos == 0f || gEConstraintC2.currentRailPos == 1f)
		{
			gEConstraintC2.moveFromPoint = Main.m_gameTime;
		}
		return gEConstraintC2;
	}

	public static List<EIC> CreateNewEditorItem(EIC _container, string _identifier, Vector3 _pos, Vector3 _rot, Vector3 _sca)
	{
		List<EIC> list = new List<EIC>();
		uint uniqueId = GES.GetUniqueId();
		ConstraintData constraintData = new ConstraintData();
		constraintData.position = new Vertex3(_pos);
		constraintData.rotation = new Vertex3(_rot);
		constraintData.scale = new Vertex3(_sca);
		constraintData.constraintType = 3u;
		constraintData.softConnection = false;
		constraintData.softConnectionStrength = 0.1f;
		constraintData.softConnectionDamp = 0.1f;
		constraintData.rotaryStiffness = 0f;
		constraintData.rotaryLimit = false;
		constraintData.rotaryLimitMin = 0f;
		constraintData.rotaryLimitMax = 0f;
		constraintData.rotarySpring = false;
		constraintData.rotarySpringStrength = 0.1f;
		constraintData.rotarySpringDamp = 0.1f;
		constraintData.Init(uniqueId, _identifier + uniqueId);
		EIC eIC = GEItemA.Assemble(_container, _identifier, constraintData, Main.camera);
		eIC.isRealtimeMovable = true;
		list.Add(eIC);
		return list;
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
		if (_eiC.identifier == "RailPointAnchor")
		{
			ConstraintPointData constraintPointData = _eiC.data as ConstraintPointData;
			UIC component = NumericFieldA.Assemble(canvasCamera, "Vel Multipler", HandleConstraintPropertyChange, null, true, Align.Left, 80f, 1f, false, -10f, 10f, constraintPointData.velocityMultipler, tags);
			UIC component2 = NumericFieldA.Assemble(canvasCamera, "Wait At Point", HandleConstraintPropertyChange, null, true, Align.Left, 80f, 1f, false, 0f, 10f, constraintPointData.waitAtPoint, tags);
			UIC component3 = RadioButtonA.Assemble(canvasCamera, "Linear", HandleConstraintPropertyChange, null, true, Align.Bottom, 1f, constraintPointData.entryEasingType == 0, 0, 200, tags);
			UIC component4 = RadioButtonA.Assemble(canvasCamera, "Quad", HandleConstraintPropertyChange, null, true, Align.Bottom, 1f, constraintPointData.entryEasingType == 1, 1, 200, tags);
			UIC component5 = RadioButtonA.Assemble(canvasCamera, "Elastic", HandleConstraintPropertyChange, null, true, Align.Bottom, 1f, constraintPointData.entryEasingType == 2, 2, 200, tags);
			UIC component6 = RadioButtonA.Assemble(canvasCamera, "Bounce", HandleConstraintPropertyChange, null, true, Align.Bottom, 1f, constraintPointData.entryEasingType == 3, 3, 200, tags);
			UIC component7 = RadioButtonA.Assemble(canvasCamera, "Linear", HandleConstraintPropertyChange, null, true, Align.Bottom, 1f, constraintPointData.exitEasingType == 0, 0, 201, tags);
			UIC component8 = RadioButtonA.Assemble(canvasCamera, "Quad", HandleConstraintPropertyChange, null, true, Align.Bottom, 1f, constraintPointData.exitEasingType == 1, 1, 201, tags);
			UIS.AddToCanvasGrid(LabelA.Assemble(canvasCamera, "Rail Point", tags), _propertyBar, true);
			UIS.AddToCanvasGrid(component, _propertyBar, true);
			UIS.AddToCanvasGrid(component2, _propertyBar, false);
			UIS.MoveCursor(_propertyBar, 0f, -15f);
			UIS.AddToCanvasGrid(LabelA.Assemble(canvasCamera, "Entry Easing", tags), _propertyBar, true);
			UIS.AddToCanvasGrid(component3, _propertyBar, true);
			UIS.AddToCanvasGrid(component4, _propertyBar, false);
			UIS.AddToCanvasGrid(component5, _propertyBar, false);
			UIS.AddToCanvasGrid(component6, _propertyBar, false);
			UIS.MoveCursor(_propertyBar, 0f, -15f);
			UIS.AddToCanvasGrid(LabelA.Assemble(canvasCamera, "Exit Easing", tags), _propertyBar, true);
			UIS.AddToCanvasGrid(component7, _propertyBar, true);
			UIS.AddToCanvasGrid(component8, _propertyBar, false);
		}
		else if (_eiC.identifier == "Bolt")
		{
			ConstraintData constraintData = _eiC.data as ConstraintData;
			UIC component9 = CheckBoxA.Assemble(canvasCamera, "Connect", HandleConstraintPropertyChange, null, true, Align.Right, 1f, constraintData.connectToWorld, tags);
			UIC component10 = CheckBoxA.Assemble(canvasCamera, "Soft Connection", HandleConstraintPropertyChange, null, true, Align.Right, 1f, constraintData.softConnection, tags);
			UIC component11 = NumericFieldA.Assemble(canvasCamera, "Soft Strength", HandleConstraintPropertyChange, null, true, Align.Left, 80f, 1f, false, 0f, 1f, constraintData.softConnectionStrength, tags);
			UIC component12 = NumericFieldA.Assemble(canvasCamera, "Soft Damping", HandleConstraintPropertyChange, null, true, Align.Left, 80f, 1f, false, 0f, 1f, constraintData.softConnectionDamp, tags);
			UIC component13 = NumericFieldA.Assemble(canvasCamera, "Stiffness", HandleConstraintPropertyChange, null, true, Align.Left, 80f, 1f, false, 0f, 1f, constraintData.rotaryStiffness, tags);
			UIC component14 = CheckBoxA.Assemble(canvasCamera, "Rotary Limit", HandleConstraintPropertyChange, null, true, Align.Right, 1f, constraintData.rotaryLimit, tags);
			UIC component15 = NumericFieldA.Assemble(canvasCamera, "Min", HandleConstraintPropertyChange, null, true, Align.Left, 80f, 1f, false, -720f, 0f, constraintData.rotaryLimitMin, tags);
			UIC component16 = NumericFieldA.Assemble(canvasCamera, "Max", HandleConstraintPropertyChange, null, true, Align.Left, 80f, 1f, false, 0f, 720f, constraintData.rotaryLimitMax, tags);
			UIC uIC = CheckBoxA.Assemble(canvasCamera, "Rotary Spring", HandleConstraintPropertyChange, null, true, Align.Right, 1f, constraintData.rotarySpring, tags);
			UIC uIC2 = NumericFieldA.Assemble(canvasCamera, "Strength", HandleConstraintPropertyChange, null, true, Align.Left, 80f, 1f, false, 0f, 1f, constraintData.rotarySpringStrength, tags);
			UIC uIC3 = NumericFieldA.Assemble(canvasCamera, "Damping", HandleConstraintPropertyChange, null, true, Align.Left, 80f, 1f, false, 0f, 1f, constraintData.rotarySpringDamp, tags);
			UIS.SetController(uIC, uIC2, constraintData.rotarySpring);
			UIS.SetController(uIC, uIC3, constraintData.rotarySpring);
			UIS.AddToCanvasGrid(LabelA.Assemble(canvasCamera, "Connect To World", tags), _propertyBar, true);
			UIS.AddToCanvasGrid(component9, _propertyBar, true);
			UIS.AddToCanvasGrid(component10, _propertyBar, true);
			UIS.AddToCanvasGrid(component11, _propertyBar, true);
			UIS.AddToCanvasGrid(component12, _propertyBar, false);
			UIS.MoveCursor(_propertyBar, 0f, -15f);
			UIS.AddToCanvasGrid(LabelA.Assemble(canvasCamera, "Bolt Stiffness", tags), _propertyBar, true);
			UIS.AddToCanvasGrid(component13, _propertyBar, true);
			UIS.MoveCursor(_propertyBar, 0f, -15f);
			UIS.AddToCanvasGrid(LabelA.Assemble(canvasCamera, "Bolt Limits", tags), _propertyBar, true);
			UIS.AddToCanvasGrid(component14, _propertyBar, true);
			UIS.AddToCanvasGrid(component15, _propertyBar, true);
			UIS.AddToCanvasGrid(component16, _propertyBar, false);
			UIS.MoveCursor(_propertyBar, 0f, -15f);
			UIS.AddToCanvasGrid(LabelA.Assemble(canvasCamera, "Bolt Spring", tags), _propertyBar, true);
			UIS.AddToCanvasGrid(uIC, _propertyBar, true);
			UIS.AddToCanvasGrid(uIC2, _propertyBar, true);
			UIS.AddToCanvasGrid(uIC3, _propertyBar, false);
			UIS.MoveCursor(_propertyBar, 0f, -15f);
		}
	}

	public static void HandleConstraintPropertyChange(EventC _c)
	{
		ConstraintData constraintData = EditorState.m_selection[0].data as ConstraintData;
		switch (_c.identifier)
		{
		case "Cuttable":
			if ((bool)_c.properties["checked"])
			{
				constraintData.ropeIsCuttable = true;
			}
			else
			{
				constraintData.ropeIsCuttable = false;
			}
			break;
		case "Has Limits":
			if ((bool)_c.properties["checked"])
			{
				constraintData.ropeHasLimits = true;
			}
			else
			{
				constraintData.ropeHasLimits = false;
			}
			break;
		case "Rigid":
			if ((bool)_c.properties["checked"])
			{
				constraintData.ropeIsRigid = true;
			}
			else
			{
				constraintData.ropeIsRigid = false;
			}
			break;
		case "Flexible":
			if ((bool)_c.properties["checked"])
			{
				constraintData.ropeIsFlexible = true;
			}
			else
			{
				constraintData.ropeIsFlexible = false;
			}
			break;
		case "Min Length":
			constraintData.ropeMinLength = (float)_c.properties["value"];
			break;
		case "Max Length":
			constraintData.ropeMaxLength = (float)_c.properties["value"];
			break;
		case "Rest Length":
			constraintData.ropeFlexRestLength = (float)_c.properties["value"];
			break;
		case "Flex Force":
			constraintData.ropeFlexForce = (float)_c.properties["value"];
			break;
		case "Flex Damp":
			constraintData.ropeFlexDamp = (float)_c.properties["value"];
			break;
		case "Connect":
			if ((bool)_c.properties["checked"])
			{
				constraintData.connectToWorld = true;
			}
			else
			{
				constraintData.connectToWorld = false;
			}
			break;
		case "Soft Connection":
			if ((bool)_c.properties["checked"])
			{
				constraintData.softConnection = true;
			}
			else
			{
				constraintData.softConnection = false;
			}
			break;
		case "Soft Strength":
			constraintData.softConnectionStrength = (float)_c.properties["value"];
			break;
		case "Soft Damping":
			constraintData.softConnectionDamp = (float)_c.properties["value"];
			break;
		case "Stiffness":
			constraintData.rotaryStiffness = (float)_c.properties["value"];
			break;
		case "Rotary Limit":
			if ((bool)_c.properties["checked"])
			{
				constraintData.rotaryLimit = true;
			}
			else
			{
				constraintData.rotaryLimit = false;
			}
			break;
		case "Min":
			constraintData.rotaryLimitMin = (float)_c.properties["value"];
			break;
		case "Max":
			constraintData.rotaryLimitMax = (float)_c.properties["value"];
			break;
		case "Rotary Spring":
			if ((bool)_c.properties["checked"])
			{
				constraintData.rotarySpring = true;
			}
			else
			{
				constraintData.rotarySpring = false;
			}
			break;
		case "Strength":
			constraintData.rotarySpringStrength = (float)_c.properties["value"];
			break;
		case "Damping":
			constraintData.rotarySpringDamp = (float)_c.properties["value"];
			break;
		case "Rotary Motor":
			if ((bool)_c.properties["checked"])
			{
				constraintData.rotaryMotor = true;
			}
			else
			{
				constraintData.rotaryMotor = false;
			}
			break;
		case "Rotary Motor Enabled":
			if ((bool)_c.properties["checked"])
			{
				constraintData.rotaryMotorEnabled = true;
			}
			else
			{
				constraintData.rotaryMotorEnabled = false;
			}
			break;
		case "Rotary MotorMax Force":
			constraintData.rotaryMotorMaxForce = (float)_c.properties["value"];
			break;
		case "Rotary Motor Rate":
			constraintData.rotaryMotorRate = (float)_c.properties["value"];
			break;
		case "Rotary Motor Start Angle":
			constraintData.rotaryMotorStartAngle = (float)_c.properties["value"];
			break;
		case "Rotary Motor Max Force":
			constraintData.rotaryMotorMaxForce = (float)_c.properties["value"];
			break;
		case "Motor Is Stiff":
			if ((bool)_c.properties["checked"])
			{
				constraintData.motorIsStiff = true;
			}
			else
			{
				constraintData.motorIsStiff = false;
			}
			break;
		case "Rail Motor Enabled":
			if ((bool)_c.properties["checked"])
			{
				constraintData.linearMotorEnabled = true;
			}
			else
			{
				constraintData.linearMotorEnabled = false;
			}
			break;
		case "Rail Motor Rate":
			constraintData.linearMotorRate = (float)_c.properties["value"];
			break;
		case "Rail Motor Max Force":
			constraintData.linearMotorMaxForce = (float)_c.properties["value"];
			break;
		case "Rail Motor Start Index":
			constraintData.linearMotorStartIndex = uint.Parse(_c.properties["value"].ToString());
			break;
		case "Rail Motor Start Pos":
			constraintData.linearMotorStartPos = (float)_c.properties["value"];
			break;
		case "Rail Motor Loops":
			if ((bool)_c.properties["checked"])
			{
				constraintData.linearMotorLoop = true;
			}
			else
			{
				constraintData.linearMotorLoop = false;
			}
			break;
		case "Rail Motor Repeats":
			constraintData.railRepeats = (int)_c.properties["value"];
			break;
		case "Rail Motor Wait At Points":
			constraintData.waitAtPoints = (float)_c.properties["value"];
			break;
		}
		if (EditorState.m_selection[0].identifier == "Rope")
		{
			List<EIC> editorItemsWithUniqueId = GES.GetEditorItemsWithUniqueId(constraintData.id);
			for (int i = 0; i < editorItemsWithUniqueId.Count; i++)
			{
				ConstraintData constraintData2 = editorItemsWithUniqueId[i].data as ConstraintData;
				if (constraintData != constraintData2)
				{
					constraintData2.ropeIsFlexible = constraintData.ropeIsFlexible;
					constraintData2.ropeIsRigid = constraintData.ropeIsRigid;
					constraintData2.ropeMaxLength = constraintData.ropeMaxLength;
					constraintData2.ropeMinLength = constraintData.ropeMinLength;
					constraintData2.ropeHasLimits = constraintData.ropeHasLimits;
					constraintData2.ropeFlexRestLength = constraintData.ropeFlexRestLength;
					constraintData2.ropeFlexForce = constraintData.ropeFlexForce;
					constraintData2.ropeFlexDamp = constraintData.ropeFlexDamp;
					constraintData2.ropeIsCuttable = constraintData.ropeIsCuttable;
				}
				EditorState.ResetEditorItem(editorItemsWithUniqueId[i]);
			}
		}
		else
		{
			EditorState.ResetEditorItem(EditorState.m_selection[0]);
		}
	}
}
