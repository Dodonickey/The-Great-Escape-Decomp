using System;
using System.Collections.Generic;
using UnityEngine;

public static class GEBlockA
{
	public static GEBlockC AssembleConvex(EIC _eic, ShapeData _shapeData, Polygon _modified)
	{
		Vector3 zero = Vector3.zero;
		Color grey = Color.grey;
		if (_shapeData.colliderLayer == GEState.layer_middle)
		{
			grey = DebugDraw.GetColor(128f, 128f, 128f);
		}
		else if (_shapeData.colliderLayer == GEState.layer_back)
		{
			grey = DebugDraw.GetColor(160f, 160f, 160f);
			zero.z = 25f;
		}
		else if (_shapeData.colliderLayer == GEState.layer_front)
		{
			grey = DebugDraw.GetColor(190f, 190f, 190f);
			zero.z = -25f;
		}
		_shapeData.groundSettings.hasBelt = false;
		Entity entity = null;
		TransformC transformC = null;
		ChipmunkC chipmunkC = null;
		string[] tags = new string[2]
		{
			LevelManager.m_currentLevel.name + ":GameEntity",
			LevelManager.m_currentLevel.name
		};
		entity = EntityManager.AddEntity(tags);
		transformC = TransformS.AddComponent(entity);
		float num = 0f;
		Vector2 zero2 = Vector2.zero;
		List<float> list = new List<float>();
		for (int i = 0; i < _modified.NofContours; i++)
		{
			Vector2 zero3 = Vector2.zero;
			float num2 = 0f;
			int nofVertices = _modified.Contour[i].NofVertices;
			for (int j = 0; j < nofVertices; j++)
			{
				Vector2 vector = _modified.Contour[i].Vertex[j];
				int num3 = j + 1;
				if (num3 == nofVertices)
				{
					num3 = 0;
				}
				Vector2 vector2 = _modified.Contour[i].Vertex[num3];
				num2 += vector.x * vector2.y - vector2.x * vector.y;
				zero3.x += (vector.x + vector2.x) * (vector.x * vector2.y - vector2.x * vector.y);
				zero3.y += (vector.y + vector2.y) * (vector.x * vector2.y - vector2.x * vector.y);
			}
			num2 *= -0.5f;
			zero3 *= 1f / (6f * (0f - num2));
			if (num2 > 25f)
			{
				num += num2;
				zero2 += zero3 * num2;
			}
			list.Add(num2);
		}
		zero2 /= num;
		TransformS.m_transformHelper.transform.rotation = Quaternion.identity;
		TransformS.m_transformHelper.transform.position = zero2;
		TransformS.m_transformHelper.transform.RotateAround(Vector3.zero, Vector3.forward, _eic.data.rotation.z);
		Vector2 vector3 = TransformS.m_transformHelper.transform.position;
		Vector2 vector4 = _shapeData.position.ToVector2();
		Vector2 position = vector4 + vector3;
		TransformS.SetGlobalPositionWithoutChildren(_eic.TC, new Vector3(position.x, position.y, _eic.TC.transform.position.z));
		_shapeData.position = new Vertex3(new Vector3(position.x, position.y, _eic.data.position.z));
		for (int k = 0; k < _shapeData.polygon.NofContours; k++)
		{
			for (int l = 0; l < _shapeData.polygon.Contour[k].Vertex.Length; l++)
			{
				_shapeData.polygon.Contour[k].Vertex[l] -= zero2;
			}
		}
		TransformS.SetGlobalRotation(transformC, _shapeData.rotation.ToVector3());
		chipmunkC = ChipmunkS.AddInactiveComponent(transformC, _shapeData.isStatic, (ColliderType)9, _shapeData.colliderGroup, _shapeData.colliderLayer, _shapeData.isStatic, false);
		ChipmunkS.ActivateChipmunkComponent(chipmunkC, ChipmunkWrapper.AddBodyWithCustomProperties(chipmunkC.isStatic, chipmunkC.isRogue, position, chipmunkC.index, chipmunkC.colliderType, _shapeData.groundSettings.linearDamp.ToVector2(), _shapeData.groundSettings.angularDamp, _shapeData.gravity.ToVector2()));
		IntPtr powerLaneShape = IntPtr.Zero;
		ChipmunkC chipmunkC2 = null;
		for (int m = 0; m < _modified.NofContours; m++)
		{
			if (list[m] > 25f)
			{
				Vector2[] vertex = _modified.Contour[m].Vertex;
				for (int n = 0; n < vertex.Length; n++)
				{
					vertex[n] -= zero2;
				}
				ChipmunkWrapper.AddPolyShape(chipmunkC.cpBodyPtr, Vector2.zero, list[m] * GEState.defaultChipmunkDensity * _shapeData.groundSettings.density, vertex.Length, vertex, _shapeData.groundSettings.elasticity, _shapeData.groundSettings.friction, chipmunkC.colliderGroup, chipmunkC.colliderLayer, false);
				if (_shapeData.isPowerLane)
				{
					chipmunkC2 = ChipmunkS.AddInactiveComponent(transformC, true, (ColliderType)9, 0u, 17895697u, false, false);
					ChipmunkS.ActivateChipmunkComponent(chipmunkC2, ChipmunkWrapper.AddBody(chipmunkC2.isStatic, chipmunkC2.isRogue, position, chipmunkC2.index, chipmunkC2.colliderType));
					powerLaneShape = ChipmunkWrapper.AddPolyShape(chipmunkC2.cpBodyPtr, Vector2.zero, 0.1f, vertex.Length, vertex, _shapeData.groundSettings.elasticity, _shapeData.groundSettings.friction, chipmunkC.colliderGroup, chipmunkC.colliderLayer, true);
				}
			}
		}
		GEBlockC gEBlockC = GES.AddBlockComponent(entity, chipmunkC, _shapeData, _modified, num);
		gEBlockC.powerLaneShape = powerLaneShape;
		gEBlockC.linearDamp = _shapeData.linearDamp.ToVector2();
		gEBlockC.angularDamp = _shapeData.angularDamp;
		gEBlockC.gravity = _shapeData.gravity.ToVector2();
		gEBlockC.isPowerLane = _shapeData.isPowerLane;
		gEBlockC.powerLaneDirection = _shapeData.powerLaneDirection.ToVector2();
		gEBlockC.powerLaneForce = _shapeData.powerLaneForce;
		gEBlockC.powerLaneType = _shapeData.powerLaneType;
		gEBlockC.groundSettings = _shapeData.groundSettings;
		Vector2 vector5 = _shapeData.groundSettings.surfaceVelocity.ToVector2();
		if (vector5 != Vector2.zero)
		{
			ChipmunkWrapper.SetBodySurfaceVelocity(chipmunkC.cpBodyPtr, vector5);
		}
		GELevelGenerator.GenerateBlock(_eic.camera, gEBlockC);
		if (_eic.container != null && !GEState.editorMode)
		{
			if (_eic.container.identifier == "Rail Motor")
			{
				if (_eic.container.gameComponents.Count > 0)
				{
					GEConstraintC gEConstraintC = _eic.container.gameComponents[0] as GEConstraintC;
					IntPtr railedPivotJointPtr = ChipmunkWrapper.AddPivotJoint2(ChipmunkWrapper.GetSpaceStaticBody(), chipmunkC.cpBodyPtr, _eic.container.data.position.ToVector2(), Vector2.zero);
					gEConstraintC.pivotOffset = _eic.data.position.ToVector3() - _eic.container.data.position.ToVector3();
					gEConstraintC.railedPivotJointPtr = railedPivotJointPtr;
				}
			}
			else if (_eic.container.identifier == "Bolt")
			{
				GEConstraintC gEConstraintC2 = _eic.container.gameComponents[0] as GEConstraintC;
				IntPtr bodyA = ChipmunkWrapper.GetSpaceStaticBody();
				if (gEConstraintC2.CMC != null)
				{
					bodyA = gEConstraintC2.CMC.cpBodyPtr;
				}
				ConstraintData constraintData = _eic.container.data as ConstraintData;
				if (gEConstraintC2.CMC != null)
				{
					ChipmunkWrapper.GetBodyValues(chipmunkC.cpBodyPtr, ref chipmunkC.ucpBodyStruct);
					if (constraintData.connectToWorld)
					{
						ChipmunkWrapper.AddPivotJoint(bodyA, chipmunkC.cpBodyPtr, constraintData.position.ToVector2());
						ChipmunkWrapper.AddRotaryLimitJoint(bodyA, chipmunkC.cpBodyPtr, chipmunkC.ucpBodyStruct.a - gEConstraintC2.CMC.ucpBodyStruct.a, chipmunkC.ucpBodyStruct.a - gEConstraintC2.CMC.ucpBodyStruct.a);
					}
					else
					{
						if (constraintData.softConnection)
						{
							gEConstraintC2.connectJointPtr = ChipmunkWrapper.AddDampedSpring2(gEConstraintC2.CMC.cpBodyPtr, chipmunkC.cpBodyPtr, constraintData.position.ToVector2(), 0f, constraintData.softConnectionStrength * 90000f, constraintData.softConnectionDamp * 900f);
						}
						else
						{
							gEConstraintC2.connectJointPtr = ChipmunkWrapper.AddPivotJoint(gEConstraintC2.CMC.cpBodyPtr, chipmunkC.cpBodyPtr, constraintData.position.ToVector2());
						}
						if (constraintData.rotaryStiffness > 0f)
						{
							gEConstraintC2.rotaryStiffnessPtr = ChipmunkWrapper.AddSimpleMotor(gEConstraintC2.CMC.cpBodyPtr, chipmunkC.cpBodyPtr, 0f, constraintData.rotaryStiffness * 90000000f);
						}
						if (constraintData.rotaryLimit)
						{
							gEConstraintC2.rotaryLimitJointPtr = ChipmunkWrapper.AddRotaryLimitJoint(gEConstraintC2.CMC.cpBodyPtr, chipmunkC.cpBodyPtr, chipmunkC.ucpBodyStruct.a - gEConstraintC2.CMC.ucpBodyStruct.a + constraintData.rotaryLimitMin * ((float)Math.PI / 180f), chipmunkC.ucpBodyStruct.a - gEConstraintC2.CMC.ucpBodyStruct.a + constraintData.rotaryLimitMax * ((float)Math.PI / 180f));
						}
						if (constraintData.rotarySpring)
						{
							gEConstraintC2.rotarySpringPtr = ChipmunkWrapper.AddDampedRotarySpring(gEConstraintC2.CMC.cpBodyPtr, chipmunkC.cpBodyPtr, 0f - chipmunkC.ucpBodyStruct.a - (0f - gEConstraintC2.CMC.ucpBodyStruct.a), constraintData.rotarySpringStrength * 90000000f, constraintData.rotarySpringDamp * 90000f);
						}
					}
					TransformS.ParentComponent(gEConstraintC2.TC, chipmunkC.TC, ChipmunkWrapper.GetLocalPos(chipmunkC.cpBodyPtr, _eic.container.data.position.ToVector2()));
					uint num4 = (uint)(_eic.container.index + 10000);
					ChipmunkQueryInfo[] array = new ChipmunkQueryInfo[100];
					int connectedBodies = ChipmunkWrapper.GetConnectedBodies(gEConstraintC2.CMC.cpBodyPtr, array);
					for (int num5 = 0; num5 < connectedBodies; num5++)
					{
						ChipmunkC chipmunkC3 = ChipmunkS.m_components.m_array[array[num5].componentIndex];
						ChipmunkWrapper.SetBodyGroup(chipmunkC3.cpBodyPtr, num4);
						chipmunkC3.colliderGroup = num4;
					}
				}
				else
				{
					ChipmunkWrapper.GetBodyValues(chipmunkC.cpBodyPtr, ref chipmunkC.ucpBodyStruct);
					if (constraintData.connectToWorld)
					{
						if (constraintData.softConnection)
						{
							gEConstraintC2.connectJointPtr = ChipmunkWrapper.AddDampedSpring2(ChipmunkWrapper.GetSpaceStaticBody(), chipmunkC.cpBodyPtr, constraintData.position.ToVector2(), 0f, constraintData.softConnectionStrength * 90000f, constraintData.softConnectionDamp * 900f);
						}
						else
						{
							gEConstraintC2.connectJointPtr = ChipmunkWrapper.AddPivotJoint(ChipmunkWrapper.GetSpaceStaticBody(), chipmunkC.cpBodyPtr, constraintData.position.ToVector2());
						}
						if (constraintData.rotaryStiffness > 0f)
						{
							gEConstraintC2.rotaryStiffnessPtr = ChipmunkWrapper.AddSimpleMotor(ChipmunkWrapper.GetSpaceStaticBody(), chipmunkC.cpBodyPtr, 0f, constraintData.rotaryStiffness * 90000000f);
						}
						if (constraintData.rotaryLimit)
						{
							gEConstraintC2.rotaryLimitJointPtr = ChipmunkWrapper.AddRotaryLimitJoint(ChipmunkWrapper.GetSpaceStaticBody(), chipmunkC.cpBodyPtr, chipmunkC.ucpBodyStruct.a + constraintData.rotaryLimitMin * ((float)Math.PI / 180f), chipmunkC.ucpBodyStruct.a + constraintData.rotaryLimitMax * ((float)Math.PI / 180f));
						}
						if (constraintData.rotarySpring)
						{
							gEConstraintC2.rotarySpringPtr = ChipmunkWrapper.AddDampedRotarySpring(ChipmunkWrapper.GetSpaceStaticBody(), chipmunkC.cpBodyPtr, 0f - chipmunkC.ucpBodyStruct.a, constraintData.rotarySpringStrength * 90000000f, constraintData.rotarySpringDamp * 90000f);
						}
					}
					gEConstraintC2.CMC = chipmunkC;
					TransformS.ParentComponent(gEConstraintC2.TC, gEConstraintC2.CMC.TC, ChipmunkWrapper.GetLocalPos(gEConstraintC2.CMC.cpBodyPtr, _eic.container.data.position.ToVector2()));
					uint num6 = (uint)(_eic.container.index + 10000);
					ChipmunkQueryInfo[] array2 = new ChipmunkQueryInfo[100];
					int connectedBodies2 = ChipmunkWrapper.GetConnectedBodies(gEConstraintC2.CMC.cpBodyPtr, array2);
					for (int num7 = 0; num7 < connectedBodies2; num7++)
					{
						ChipmunkC chipmunkC4 = ChipmunkS.m_components.m_array[array2[num7].componentIndex];
						ChipmunkWrapper.SetBodyGroup(chipmunkC4.cpBodyPtr, num6);
						chipmunkC4.colliderGroup = num6;
					}
				}
			}
		}
		TriggerData triggerData = new TriggerData();
		triggerData.Init(_eic.data.id, _eic.identifier);
		return gEBlockC;
	}

	public static GEBlockC AssembleConcave(EIC _eic, ShapeData _data, Polygon _modified)
	{
		return null;
	}

	public static EIC CreateNewEditorItem(EIC _container, string _identifier, Vector3 _pos, Vector3 _rot, Vector3 _sca)
	{
		EIC eIC = null;
		ShapeData shapeData = new ShapeData();
		shapeData.position = new Vertex3(_pos);
		shapeData.rotation = new Vertex3(_rot);
		shapeData.scale = new Vertex3(_sca);
		shapeData.groundSettings = new GroundSettings(GroundType.Solid);
		shapeData.groundSettings.elasticity = 0.2f;
		shapeData.groundSettings.friction = 0.9f;
		shapeData.convex = true;
		shapeData.separate = false;
		shapeData.gravity = new Vertex3(Vector2.up * -450f);
		shapeData.linearDamp = new Vertex3(Vector2.one * 0.995f);
		shapeData.angularDamp = 0.99f;
		shapeData.isStatic = false;
		shapeData.colliderGroup = 0u;
		shapeData.colliderLayer = GEState.layer_all;
		shapeData.isOneWay = false;
		shapeData.oneWayDirection = new Vertex3(Vector2.up);
		shapeData.isBreakable = false;
		shapeData.breakEventType = 0u;
		shapeData.breakingImpulse = 10000f;
		shapeData.breakEventDirection = new Vertex3(Vector2.up);
		shapeData.breakEventForce = 500f;
		shapeData.isPowerLane = false;
		shapeData.powerLaneType = 0u;
		shapeData.powerLaneDirection = new Vertex3(Vector2.up);
		shapeData.powerLaneForce = 500f;
		Vector2[] rect = DebugDraw.GetRect(50f, 50f, Vector2.zero, false);
		shapeData.polygon = new Polygon();
		shapeData.polygon.AddContour(new VertexList(rect), false);
		uint uniqueId = GES.GetUniqueId();
		shapeData.Init(uniqueId, _identifier + uniqueId);
		eIC = GEItemA.Assemble(_container, _identifier, shapeData, Main.camera);
		eIC.isRealtimeMovable = true;
		eIC.isDrawable = true;
		eIC.isRotateable = true;
		eIC.isScaleable = true;
		eIC.isScaleUnified = false;
		return eIC;
	}

	public static EIC CreateLoadedEditorItem(EIC _container, EIC _loadedItem)
	{
		return GEItemA.Assemble(_container, _loadedItem.identifier, _loadedItem.data, Main.camera);
	}

	public static void FillEditorItem(EIC _eic)
	{
		ShapeData shapeData = _eic.data as ShapeData;
		Polygon polygon = GpcS.CleanPolygon(shapeData.polygon, shapeData.groundSettings.minSegment, shapeData.groundSettings.minAngle, shapeData.groundSettings.maxSegment, shapeData.convex);
		polygon = GpcS.SmoothPolygon(polygon, shapeData.groundSettings.smooth);
		GEBlockC gEBlockC = null;
		gEBlockC = ((!shapeData.convex) ? AssembleConcave(_eic, shapeData, polygon) : AssembleConvex(_eic, shapeData, polygon));
		_eic.gameComponents.Add(gEBlockC);
		if (GEState.editorMode)
		{
			TransformS.ParentComponent(gEBlockC.CMC.TC, _eic.TC, Vector3.zero);
		}
	}

	public static void PopulatePropertyBar(EIC _eiC, UIC _propertyBar)
	{
		string[] tags = new string[1] { "propertyBar" };
		Camera canvasCamera = _propertyBar.canvasCamera;
		ShapeData shapeData = _eiC.data as ShapeData;
		UIS.AddToCanvasGrid(LabelA.Assemble(canvasCamera, "Material", tags), _propertyBar, true);
		for (int i = 0; i < GEState.blockMats.Count; i++)
		{
			GEMat gEMat = GEState.blockMats[i];
			UIC component = RadioButtonA.Assemble(canvasCamera, gEMat.name, HandleBlockPropertyChange, null, true, Align.Bottom, 1f, false, i, 1000, tags);
			UIS.AddToCanvasGrid(component, _propertyBar, i % 4 == 0);
		}
		UIS.MoveCursor(_propertyBar, 0f, -15f);
		UIC component2 = RadioButtonA.Assemble(canvasCamera, "Back", HandleBlockPropertyChange, null, true, Align.Bottom, 1f, shapeData.colliderLayer == GEState.layer_back, 0, 101, tags);
		UIC component3 = RadioButtonA.Assemble(canvasCamera, "Front", HandleBlockPropertyChange, null, true, Align.Bottom, 1f, shapeData.colliderLayer == GEState.layer_front, 1, 101, tags);
		UIC component4 = RadioButtonA.Assemble(canvasCamera, "Both", HandleBlockPropertyChange, null, true, Align.Bottom, 1f, shapeData.colliderLayer == GEState.layer_all, 2, 101, tags);
		UIC component5 = RadioButtonA.Assemble(canvasCamera, "None", HandleBlockPropertyChange, null, true, Align.Bottom, 1f, shapeData.colliderLayer == 0, 3, 101, tags);
		UIC component6 = NumericFieldA.Assemble(canvasCamera, "Group", HandleBlockPropertyChange, null, true, Align.Left, 40f, 1f, true, 0f, 100f, shapeData.colliderGroup, tags);
		UIC component7 = CheckBoxA.Assemble(canvasCamera, "Static", HandleBlockPropertyChange, null, true, Align.Right, 1f, shapeData.isStatic, tags);
		UIC component8 = CheckBoxA.Assemble(canvasCamera, "Force Convex", HandleBlockPropertyChange, null, true, Align.Right, 1f, shapeData.convex, tags);
		UIC uIC = CheckBoxA.Assemble(canvasCamera, "Separate Contours", HandleBlockPropertyChange, null, true, Align.Right, 1f, shapeData.separate, tags);
		UIC component9 = NumericFieldA.Assemble(canvasCamera, "Min Seg Len", HandleBlockPropertyChange, null, true, Align.Left, 80f, 1f, false, 1f, 5f, shapeData.groundSettings.minSegment, tags);
		UIC component10 = NumericFieldA.Assemble(canvasCamera, "Max Seg Len", HandleBlockPropertyChange, null, true, Align.Left, 80f, 1f, false, 10f, 200f, shapeData.groundSettings.maxSegment, tags);
		UIC component11 = NumericFieldA.Assemble(canvasCamera, "Min Angle", HandleBlockPropertyChange, null, true, Align.Left, 80f, 1f, false, 1f, 45f, shapeData.groundSettings.minAngle, tags);
		UIC component12 = NumericFieldA.Assemble(canvasCamera, "Smooth Mult", HandleBlockPropertyChange, null, true, Align.Left, 80f, 1f, true, 0f, 5f, shapeData.groundSettings.smooth, tags);
		UIC component13 = NumericFieldA.Assemble(canvasCamera, "Density", HandleBlockPropertyChange, null, true, Align.Left, 50f, 1f, false, 0.1f, 10f, shapeData.groundSettings.density, tags);
		UIC component14 = NumericFieldA.Assemble(canvasCamera, "Elasticity", HandleBlockPropertyChange, null, true, Align.Left, 50f, 1f, false, 0f, 1f, shapeData.groundSettings.elasticity, tags);
		UIC component15 = NumericFieldA.Assemble(canvasCamera, "Friction", HandleBlockPropertyChange, null, true, Align.Left, 50f, 1f, false, 0f, 5f, shapeData.groundSettings.friction, tags);
		UIC component16 = NumericFieldA.Assemble(canvasCamera, "Lin Damp X", HandleBlockPropertyChange, null, true, Align.Left, 50f, 1f, false, 0f, 1f, shapeData.groundSettings.linearDamp.x, tags);
		UIC component17 = NumericFieldA.Assemble(canvasCamera, "Lin Damp Y", HandleBlockPropertyChange, null, true, Align.Left, 50f, 1f, false, 0f, 1f, shapeData.groundSettings.linearDamp.y, tags);
		UIC component18 = NumericFieldA.Assemble(canvasCamera, "Ang Damp", HandleBlockPropertyChange, null, true, Align.Left, 50f, 1f, false, 0f, 1f, shapeData.groundSettings.angularDamp, tags);
		UIC component19 = NumericFieldA.Assemble(canvasCamera, "Gravity X", HandleBlockPropertyChange, null, true, Align.Left, 50f, 1f, false, -450f, 450f, shapeData.gravity.x, tags);
		UIC component20 = NumericFieldA.Assemble(canvasCamera, "Gravity Y", HandleBlockPropertyChange, null, true, Align.Left, 50f, 1f, false, -450f, 450f, shapeData.gravity.y, tags);
		UIC component21 = NumericFieldA.Assemble(canvasCamera, "Surface Vel X", HandleBlockPropertyChange, null, true, Align.Left, 50f, 1f, false, -1000f, 1000f, shapeData.groundSettings.surfaceVelocity.x, tags);
		UIC component22 = NumericFieldA.Assemble(canvasCamera, "Surface Vel Y", HandleBlockPropertyChange, null, true, Align.Left, 50f, 1f, false, -1000f, 1000f, shapeData.groundSettings.surfaceVelocity.y, tags);
		UIC uIC2 = CheckBoxA.Assemble(canvasCamera, "Directional Collision", HandleBlockPropertyChange, null, true, Align.Right, 1f, shapeData.isOneWay, tags);
		UIC uIC3 = NumericFieldA.Assemble(canvasCamera, "Direction X", HandleBlockPropertyChange, null, true, Align.Left, 80f, 1f, false, -1f, 1f, shapeData.oneWayDirection.x, tags);
		UIC uIC4 = NumericFieldA.Assemble(canvasCamera, "Direction Y", HandleBlockPropertyChange, null, true, Align.Left, 80f, 1f, false, -1f, 1f, shapeData.oneWayDirection.y, tags);
		UIC uIC5 = CheckBoxA.Assemble(canvasCamera, "Breakable", HandleBlockPropertyChange, null, true, Align.Right, 1f, shapeData.isBreakable, tags);
		UIC uIC6 = NumericFieldA.Assemble(canvasCamera, "BreakingImpulse", HandleBlockPropertyChange, null, true, Align.Left, 80f, 1f, false, 0f, 100000f, shapeData.breakingImpulse, tags);
		UIC uIC7 = RadioButtonA.Assemble(canvasCamera, "No", HandleBlockPropertyChange, null, true, Align.Bottom, 1f, shapeData.breakEventType == 0, 0, 104, tags);
		UIC uIC8 = NumericFieldA.Assemble(canvasCamera, "Event DirectionX", HandleBlockPropertyChange, null, true, Align.Left, 80f, 1f, false, -1f, 1f, shapeData.breakEventDirection.x, tags);
		UIC uIC9 = NumericFieldA.Assemble(canvasCamera, "Event DirectionY", HandleBlockPropertyChange, null, true, Align.Left, 80f, 1f, false, -1f, 1f, shapeData.breakEventDirection.y, tags);
		UIC uIC10 = NumericFieldA.Assemble(canvasCamera, "Event Force", HandleBlockPropertyChange, null, true, Align.Left, 80f, 1f, false, 0f, 10f, shapeData.breakEventForce, tags);
		UIC component23 = CheckBoxA.Assemble(canvasCamera, "Power Lane", HandleBlockPropertyChange, null, true, Align.Right, 1f, shapeData.isPowerLane, tags);
		UIC component24 = RadioButtonA.Assemble(canvasCamera, "PL Speed", HandleBlockPropertyChange, null, true, Align.Bottom, 1f, shapeData.powerLaneType == 0, 0, 105, tags);
		UIC component25 = NumericFieldA.Assemble(canvasCamera, "PL DirectionX", HandleBlockPropertyChange, null, true, Align.Left, 80f, 1f, false, -1f, 1f, shapeData.powerLaneDirection.x, tags);
		UIC component26 = NumericFieldA.Assemble(canvasCamera, "PL DirectionY", HandleBlockPropertyChange, null, true, Align.Left, 80f, 1f, false, -1f, 1f, shapeData.powerLaneDirection.y, tags);
		UIC component27 = NumericFieldA.Assemble(canvasCamera, "PL Force", HandleBlockPropertyChange, null, true, Align.Left, 80f, 1f, false, 0f, 1000f, shapeData.powerLaneForce, tags);
		UIS.SetController(uIC2, uIC3, uIC2.isChecked);
		UIS.SetController(uIC2, uIC4, uIC2.isChecked);
		UIS.SetController(uIC5, uIC6, uIC5.isChecked);
		UIS.SetController(uIC7, uIC10, !uIC7.isSelected, true);
		UIS.SetController(uIC7, uIC8, !uIC7.isSelected, true);
		UIS.SetController(uIC7, uIC9, !uIC7.isSelected, true);
		UIS.AddToCanvasGrid(LabelA.Assemble(canvasCamera, "Collision Layers", tags), _propertyBar, true);
		UIS.AddToCanvasGrid(component2, _propertyBar, true);
		UIS.AddToCanvasGrid(component3, _propertyBar, false);
		UIS.AddToCanvasGrid(component4, _propertyBar, false);
		UIS.AddToCanvasGrid(component5, _propertyBar, false);
		UIS.AddToCanvasGrid(component6, _propertyBar, true);
		UIS.AddToCanvasGrid(LabelA.Assemble(canvasCamera, "Shapes in same group do not collide", tags), _propertyBar, false);
		UIS.MoveCursor(_propertyBar, 0f, -15f);
		UIS.AddToCanvasGrid(LabelA.Assemble(canvasCamera, "Shape", tags), _propertyBar, true);
		UIS.AddToCanvasGrid(component7, _propertyBar, true);
		UIS.MoveCursor(_propertyBar, 0f, -15f);
		UIS.AddToCanvasGrid(component8, _propertyBar, true);
		UIS.AddToCanvasGrid(uIC, _propertyBar, true);
		UIS.Disable(uIC);
		UIS.AddToCanvasGrid(component9, _propertyBar, true);
		UIS.AddToCanvasGrid(component10, _propertyBar, false);
		UIS.AddToCanvasGrid(component11, _propertyBar, true);
		UIS.AddToCanvasGrid(component12, _propertyBar, false);
		UIS.MoveCursor(_propertyBar, 0f, -15f);
		UIS.AddToCanvasGrid(LabelA.Assemble(canvasCamera, "Material", tags), _propertyBar, true);
		UIS.MoveCursor(_propertyBar, 0f, -15f);
		UIS.AddToCanvasGrid(component13, _propertyBar, true);
		UIS.AddToCanvasGrid(component14, _propertyBar, false);
		UIS.AddToCanvasGrid(component15, _propertyBar, false);
		UIS.AddToCanvasGrid(component16, _propertyBar, true);
		UIS.AddToCanvasGrid(component17, _propertyBar, false);
		UIS.AddToCanvasGrid(component18, _propertyBar, false);
		UIS.AddToCanvasGrid(component19, _propertyBar, true);
		UIS.AddToCanvasGrid(component20, _propertyBar, false);
		UIS.AddToCanvasGrid(component21, _propertyBar, true);
		UIS.AddToCanvasGrid(component22, _propertyBar, false);
		UIS.MoveCursor(_propertyBar, 0f, -15f);
		UIS.AddToCanvasGrid(LabelA.Assemble(canvasCamera, "Special Collisions", tags), _propertyBar, true);
		UIS.AddToCanvasGrid(uIC2, _propertyBar, true);
		UIS.AddToCanvasGrid(uIC3, _propertyBar, true);
		UIS.AddToCanvasGrid(uIC4, _propertyBar, false);
		UIS.MoveCursor(_propertyBar, 0f, -15f);
		UIS.AddToCanvasGrid(LabelA.Assemble(canvasCamera, "Breaking", tags), _propertyBar, true);
		UIS.AddToCanvasGrid(uIC5, _propertyBar, true);
		UIS.AddToCanvasGrid(uIC6, _propertyBar, true);
		UIS.MoveCursor(_propertyBar, 0f, -15f);
		UIS.AddToCanvasGrid(LabelA.Assemble(canvasCamera, "Break Event", tags), _propertyBar, true);
		UIS.AddToCanvasGrid(uIC7, _propertyBar, true);
		UIS.AddToCanvasGrid(uIC8, _propertyBar, true);
		UIS.AddToCanvasGrid(uIC9, _propertyBar, false);
		UIS.AddToCanvasGrid(uIC10, _propertyBar, true);
		UIS.MoveCursor(_propertyBar, 0f, -15f);
		UIS.AddToCanvasGrid(LabelA.Assemble(canvasCamera, "Power Lane", tags), _propertyBar, true);
		UIS.AddToCanvasGrid(component23, _propertyBar, true);
		UIS.AddToCanvasGrid(component24, _propertyBar, true);
		UIS.AddToCanvasGrid(component25, _propertyBar, true);
		UIS.AddToCanvasGrid(component26, _propertyBar, false);
		UIS.AddToCanvasGrid(component27, _propertyBar, true);
		UIS.MoveCursor(_propertyBar, 0f, -15f);
	}

	public static void HandleBlockPropertyChange(EventC _c)
	{
		ShapeData shapeData = EditorState.m_selection[0].data as ShapeData;
		int valueFromRadioButtonGroup = UIS.GetValueFromRadioButtonGroup(1000);
		if (valueFromRadioButtonGroup > -1)
		{
			GEMat gEMat = GEState.blockMats[valueFromRadioButtonGroup];
			shapeData.groundSettings.fillMaterialResourceIdentifier = gEMat.fill;
			shapeData.groundSettings.roadMaterialResourceIdentifier = gEMat.road;
			shapeData.groundSettings.fillScale = gEMat.fillScale;
			shapeData.groundSettings.roadScale = gEMat.roadScale;
			shapeData.groundSettings.beltWidth = gEMat.beltWidth;
			shapeData.groundSettings.beltDepth = gEMat.beltDepth;
			shapeData.groundSettings.smooth = gEMat.smooth;
			shapeData.groundSettings.hasRoad = gEMat.hasRoad;
			shapeData.groundSettings.hasFill = gEMat.hasFill;
			shapeData.groundSettings.hasBelt = gEMat.hasBelt;
		}
		switch (_c.identifier)
		{
		case "Force Convex":
			shapeData.convex = (bool)_c.properties["checked"];
			break;
		case "Separate Contours":
			shapeData.separate = (bool)_c.properties["checked"];
			break;
		case "Min Seg Len":
			shapeData.groundSettings.minSegment = (float)_c.properties["value"];
			break;
		case "Max Seg Len":
			shapeData.groundSettings.maxSegment = (float)_c.properties["value"];
			break;
		case "Min Angle":
			shapeData.groundSettings.minAngle = (float)_c.properties["value"];
			break;
		case "Smooth Mult":
			shapeData.groundSettings.smooth = int.Parse(_c.properties["value"].ToString());
			break;
		case "Static":
			shapeData.isStatic = (bool)_c.properties["checked"];
			break;
		case "Back":
			shapeData.colliderLayer = GEState.layer_back;
			break;
		case "Front":
			shapeData.colliderLayer = GEState.layer_front;
			break;
		case "Both":
			shapeData.colliderLayer = GEState.layer_all;
			break;
		case "None":
			shapeData.colliderLayer = 0u;
			break;
		case "Group":
			shapeData.colliderGroup = uint.Parse(_c.properties["value"].ToString());
			break;
		case "Density":
			shapeData.groundSettings.density = (float)_c.properties["value"];
			break;
		case "Elasticity":
			shapeData.groundSettings.elasticity = (float)_c.properties["value"];
			break;
		case "Friction":
			shapeData.groundSettings.friction = (float)_c.properties["value"];
			break;
		case "Lin Damp X":
			shapeData.groundSettings.linearDamp.x = (float)_c.properties["value"];
			break;
		case "Lin Damp Y":
			shapeData.groundSettings.linearDamp.y = (float)_c.properties["value"];
			break;
		case "Ang Damp":
			shapeData.groundSettings.angularDamp = (float)_c.properties["value"];
			break;
		case "Gravity X":
			shapeData.gravity.x = (float)_c.properties["value"];
			break;
		case "Gravity Y":
			shapeData.gravity.y = (float)_c.properties["value"];
			break;
		case "Surface Vel X":
			shapeData.groundSettings.surfaceVelocity.x = (float)_c.properties["value"];
			break;
		case "Surface Vel Y":
			shapeData.groundSettings.surfaceVelocity.y = (float)_c.properties["value"];
			break;
		case "Directional Collision":
			shapeData.isOneWay = (bool)_c.properties["checked"];
			break;
		case "Direction X":
			shapeData.oneWayDirection.x = (float)_c.properties["value"];
			break;
		case "Direction Y":
			shapeData.oneWayDirection.y = (float)_c.properties["value"];
			break;
		case "Breakable":
			shapeData.isBreakable = (bool)_c.properties["checked"];
			break;
		case "BreakingImpulse":
			shapeData.breakingImpulse = (float)_c.properties["value"];
			break;
		case "No":
		case "Explosion":
			shapeData.breakEventType = uint.Parse(_c.properties["value"].ToString());
			break;
		case "Event DirectionX":
			shapeData.breakEventDirection.x = (float)_c.properties["value"];
			break;
		case "Event DirectionY":
			shapeData.breakEventDirection.y = (float)_c.properties["value"];
			break;
		case "Event Force":
			shapeData.breakEventForce = (float)_c.properties["value"];
			break;
		case "Power Lane":
			shapeData.isPowerLane = (bool)_c.properties["checked"];
			break;
		case "PL Speed":
			shapeData.powerLaneType = uint.Parse(_c.properties["value"].ToString());
			break;
		case "PL DirectionX":
			shapeData.powerLaneDirection.x = (float)_c.properties["value"];
			break;
		case "PL DirectionY":
			shapeData.powerLaneDirection.y = (float)_c.properties["value"];
			break;
		case "PL Force":
			shapeData.powerLaneForce = (float)_c.properties["value"];
			break;
		}
		EditorState.ResetEditorItem(EditorState.m_selection[0]);
	}
}
