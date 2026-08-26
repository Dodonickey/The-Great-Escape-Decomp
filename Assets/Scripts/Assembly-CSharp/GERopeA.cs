using System;
using System.Collections.Generic;
using UnityEngine;

public static class GERopeA
{
	public static GEConstraintC Assemble(EIC _eic, ConstraintData _data)
	{
		List<EIC> editorItemsWithUniqueId = GES.GetEditorItemsWithUniqueId(_data.id);
		if (editorItemsWithUniqueId.Count > 1)
		{
			string[] tags = new string[2]
			{
				LevelManager.m_currentLevel.name + ":GameEntity",
				LevelManager.m_currentLevel.name
			};
			Entity entity = EntityManager.AddEntity(tags);
			TransformC tc = TransformS.AddComponent(entity);
			PrefabC prefabC = null;
			LineRenderer lineRenderer = null;
			IntPtr zero = IntPtr.Zero;
			IntPtr zero2 = IntPtr.Zero;
			IntPtr zero3 = IntPtr.Zero;
			IntPtr zero4 = IntPtr.Zero;
			IntPtr zero5 = IntPtr.Zero;
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			List<ChipmunkC> list = new List<ChipmunkC>();
			List<Vector2> list2 = new List<Vector2>();
			List<TransformC> list3 = new List<TransformC>();
			List<ChipmunkC> list4 = new List<ChipmunkC>();
			EIC eIC = editorItemsWithUniqueId[0];
			EIC eIC2 = editorItemsWithUniqueId[1];
			Vector2 vector = eIC.TC.transform.position;
			Vector2 vector2 = eIC2.TC.transform.position;
			Vector3 vector3 = vector;
			Vector3 vector4 = vector2;
			ChipmunkC chipmunkC = null;
			ChipmunkC chipmunkC2 = null;
			IntPtr intPtr3 = IntPtr.Zero;
			IntPtr intPtr4 = IntPtr.Zero;
			GEConstraintC gEConstraintC = null;
			GEConstraintC gEConstraintC2 = null;
			if (eIC.container != null)
			{
				if (eIC.container.identifier == "Block")
				{
					GEBlockC gEBlockC = eIC.container.gameComponents[0] as GEBlockC;
					chipmunkC = gEBlockC.CMC;
					intPtr3 = gEBlockC.CMC.cpBodyPtr;
					vector3 = ChipmunkWrapper.GetLocalPos(intPtr3, vector);
				}
				else if (eIC.container.identifier == "Rail Motor" && eIC.container.gameComponents.Count > 0)
				{
					gEConstraintC = eIC.container.gameComponents[0] as GEConstraintC;
				}
			}
			if (eIC2.container != null)
			{
				if (eIC2.container.identifier == "Block")
				{
					GEBlockC gEBlockC2 = eIC2.container.gameComponents[0] as GEBlockC;
					chipmunkC2 = gEBlockC2.CMC;
					intPtr4 = gEBlockC2.CMC.cpBodyPtr;
					vector4 = ChipmunkWrapper.GetLocalPos(intPtr4, vector2);
				}
				else if (eIC2.container.identifier == "Rail Motor" && eIC2.container.gameComponents.Count > 0)
				{
					gEConstraintC2 = eIC2.container.gameComponents[0] as GEConstraintC;
				}
			}
			float magnitude = (vector2 - vector).magnitude;
			if (intPtr3 == IntPtr.Zero && intPtr4 == IntPtr.Zero)
			{
				return null;
			}
			if (intPtr3 == IntPtr.Zero)
			{
				intPtr3 = ChipmunkWrapper.GetSpaceStaticBody();
			}
			else if (intPtr4 == IntPtr.Zero)
			{
				intPtr4 = ChipmunkWrapper.GetSpaceStaticBody();
			}
			if (intPtr3 != intPtr4)
			{
				if (_data.ropeIsRigid)
				{
					intPtr2 = ChipmunkWrapper.AddSlideJoint(intPtr3, intPtr4, vector3, vector4, magnitude, magnitude);
				}
				else if (_data.ropeHasLimits)
				{
					intPtr2 = ChipmunkWrapper.AddSlideJoint(intPtr3, intPtr4, vector3, vector4, magnitude * _data.ropeMinLength, magnitude * _data.ropeMaxLength);
				}
				if (_data.ropeIsFlexible)
				{
					intPtr = ((!(_data.ropeFlexRestLength > 1f)) ? ChipmunkWrapper.AddDampedSpring(intPtr3, intPtr4, vector3, vector4, magnitude * _data.ropeFlexRestLength, _data.ropeFlexForce * 19000f, _data.ropeFlexDamp * 190f) : ChipmunkWrapper.AddDampedSpring(intPtr3, intPtr4, vector3, vector4, magnitude * _data.ropeFlexRestLength, 0f, 0f));
				}
				if (gEConstraintC != null)
				{
					gEConstraintC.railedSlideJointAPtr = intPtr2;
					gEConstraintC.railedDampedSpringAPtr = intPtr;
				}
				if (gEConstraintC2 != null)
				{
					gEConstraintC2.railedSlideJointBPtr = intPtr2;
					gEConstraintC2.railedDampedSpringBPtr = intPtr;
				}
				list.Add(chipmunkC);
				list.Add(chipmunkC2);
				list2.Add(vector3);
				list2.Add(vector4);
				Vector2 vector5 = vector2 - vector;
				int b = Mathf.FloorToInt(magnitude * _data.ropeMaxLength / 25f);
				if (_data.ropeIsFlexible)
				{
					b = Mathf.FloorToInt(magnitude * _data.ropeFlexRestLength / 25f);
				}
				b = Mathf.Max(3, Mathf.Min(7, b));
				if (_data.ropeIsRigid)
				{
					b = 2;
				}
				Vector2 vector6 = vector5 / (b - 1);
				float magnitude2 = vector6.magnitude;
				ChipmunkC chipmunkC3 = null;
				for (int i = 0; i < b; i++)
				{
					TransformC transformC = TransformS.AddComponent(entity.index);
					PrefabC prefabC2 = null;
					PrefabC prefabC3 = null;
					ChipmunkC chipmunkC4 = null;
					bool flag = false;
					if (i == 0)
					{
						prefabC2 = PrefabS.AddComponent(transformC, Vector3.zero, ResourceManager.GetGameObject("Pin"));
						if (gEConstraintC != null)
						{
							gEConstraintC.railedSlideJointATC = transformC;
						}
						else if (chipmunkC != null)
						{
							TransformS.ParentComponent(transformC, chipmunkC.TC, vector3);
						}
						else
						{
							prefabC3 = PrefabS.AddComponent(transformC, Vector3.zero, ResourceManager.GetGameObject("Bar"));
							TransformC transformComponent = TransformS.AddComponent(entity);
							chipmunkC4 = ChipmunkS.AddInactiveComponent(transformComponent, false, ColliderType.Any, 0u, GEState.layer_all, true, false);
							ChipmunkS.ActivateChipmunkComponent(chipmunkC4, ChipmunkWrapper.AddCircleBody(chipmunkC4.isStatic, chipmunkC4.isRogue, vector, chipmunkC4.index, Vector2.zero, 0f, 8f, 0.5f, 0.5f, chipmunkC4.colliderGroup, chipmunkC4.colliderLayer, false, chipmunkC4.colliderType));
						}
						flag = true;
					}
					else if (i == b - 1)
					{
						prefabC2 = PrefabS.AddComponent(transformC, Vector3.zero, ResourceManager.GetGameObject("Pin"));
						if (gEConstraintC2 != null)
						{
							gEConstraintC2.railedSlideJointBTC = transformC;
						}
						else if (chipmunkC2 != null)
						{
							TransformS.ParentComponent(transformC, chipmunkC2.TC, vector4);
						}
						else
						{
							prefabC3 = PrefabS.AddComponent(transformC, Vector3.zero, ResourceManager.GetGameObject("Bar"));
							TransformC transformComponent2 = TransformS.AddComponent(entity);
							chipmunkC4 = ChipmunkS.AddInactiveComponent(transformComponent2, false, ColliderType.Any, 0u, GEState.layer_all, true, false);
							ChipmunkS.ActivateChipmunkComponent(chipmunkC4, ChipmunkWrapper.AddCircleBody(chipmunkC4.isStatic, chipmunkC4.isRogue, vector2, chipmunkC4.index, Vector2.zero, 0f, 8f, 0.5f, 0.5f, chipmunkC4.colliderGroup, chipmunkC4.colliderLayer, false, chipmunkC4.colliderType));
						}
						flag = true;
					}
					if (flag)
					{
					}
					if ((!flag || !GEState.editorMode) && GEState.editorMode)
					{
						continue;
					}
					ChipmunkC chipmunkC5 = ChipmunkS.AddInactiveComponent(transformC, flag, ColliderType.Any, 0u, 0u, false, flag);
					Vector2 position = vector + vector6 * i;
					ChipmunkS.ActivateChipmunkComponent(chipmunkC5, ChipmunkWrapper.AddCircleBody(chipmunkC5.isStatic, chipmunkC5.isRogue, position, chipmunkC5.index, Vector2.zero, 1f, 3f, 0f, 0f, chipmunkC5.colliderGroup, chipmunkC5.colliderLayer, true, chipmunkC5.colliderType));
					ChipmunkWrapper.SetCustomBodyLinearDamp(chipmunkC5.cpBodyPtr, Vector2.one * 0.9f);
					ChipmunkWrapper.SetCustomBodyGravity(chipmunkC5.cpBodyPtr, Vector2.up * -1000f);
					if (list4.Count > 0)
					{
						if (_data.ropeIsFlexible)
						{
							ChipmunkWrapper.AddDampedSpring(chipmunkC3.cpBodyPtr, chipmunkC5.cpBodyPtr, Vector2.zero, Vector2.zero, magnitude2 * _data.ropeFlexRestLength, 1000f, 100f);
						}
						else
						{
							ChipmunkWrapper.AddSlideJoint(chipmunkC3.cpBodyPtr, chipmunkC5.cpBodyPtr, Vector2.zero, Vector2.zero, 0f, magnitude2 * _data.ropeMaxLength);
						}
					}
					list4.Add(chipmunkC5);
					chipmunkC3 = chipmunkC5;
				}
				prefabC = PrefabS.AddComponent(list4[0].TC, Vector3.zero);
				lineRenderer = prefabC.p_gameObject.AddComponent<LineRenderer>() as LineRenderer;
				lineRenderer.SetWidth(3f, 3f);
				lineRenderer.SetVertexCount(2);
				lineRenderer.SetPosition(0, vector);
				lineRenderer.SetPosition(1, vector2);
				lineRenderer.material = new Material(ResourceManager.GetShader("ConstraintShader"));
				lineRenderer.material.mainTexture = ResourceManager.GetTexture("ConstraintDif");
				float num = 16f;
				num = (_data.ropeIsFlexible ? ((!_data.ropeIsCuttable) ? 14f : 11f) : (_data.ropeIsRigid ? ((!_data.ropeIsCuttable) ? 13f : 10f) : ((!_data.ropeIsCuttable) ? 15f : 12f)));
				lineRenderer.material.SetTextureOffset("_MainTex", new Vector2(0f, num / 16f - 0.0625f));
				lineRenderer.material.SetTextureScale("_MainTex", new Vector2(magnitude * _data.ropeFlexRestLength / 30f, 2f / 33f));
			}
			GEConstraintC gEConstraintC3 = GES.AddConstraintComponent(_data, tc, null);
			gEConstraintC3.slideJointPtr = intPtr2;
			gEConstraintC3.connectJointPtr = intPtr;
			gEConstraintC3.constraintType = ConstraintType.Rope;
			gEConstraintC3.connectedBodies = list.ToArray();
			gEConstraintC3.connectedBodyLocalAnchors = list2.ToArray();
			gEConstraintC3.ropeCMCs = list4.ToArray();
			if (_data.ropeIsFlexible)
			{
				gEConstraintC3.ropeLength = magnitude * _data.ropeFlexRestLength;
			}
			else
			{
				gEConstraintC3.ropeLength = magnitude * _data.ropeMaxLength;
			}
			gEConstraintC3.ropeMinLength = _data.ropeMinLength;
			gEConstraintC3.ropeMaxLength = _data.ropeMaxLength;
			gEConstraintC3.hasLimits = _data.ropeHasLimits;
			gEConstraintC3.isCuttable = _data.ropeIsCuttable;
			gEConstraintC3.isRigid = _data.ropeIsRigid;
			gEConstraintC3.isFlexible = _data.ropeIsFlexible;
			gEConstraintC3.flexDamp = _data.ropeFlexDamp * 190f;
			gEConstraintC3.flexForce = _data.ropeFlexForce * 19000f;
			gEConstraintC3.flexRestLength = _data.ropeFlexRestLength;
			gEConstraintC3.modifierSlots = new ConnectionSlot[1];
			gEConstraintC3.modifierSlots[0] = new ConnectionSlot(ConnectionSlotType.Destroy, 0);
			gEConstraintC3.triggerType = TriggerType.RopeConstraint;
			gEConstraintC3.autoTrigger = true;
			gEConstraintC3.energy = 1f;
			gEConstraintC3.PC = prefabC;
			gEConstraintC3.lineRenderer = lineRenderer;
			gEConstraintC3.camera = _eic.camera;
			_eic.trigger = gEConstraintC3;
			return gEConstraintC3;
		}
		return null;
	}

	public static List<EIC> CreateNewEditorItem(EIC _container, string _identifier, Vector3 _pos, Vector3 _rot, Vector3 _sca)
	{
		List<EIC> list = new List<EIC>();
		uint uniqueId = GES.GetUniqueId();
		ConstraintData constraintData = new ConstraintData();
		constraintData.position = new Vertex3(_pos);
		constraintData.rotation = new Vertex3(_rot);
		constraintData.scale = new Vertex3(_sca);
		constraintData.constraintType = 5u;
		if (_identifier == "Flexible Rope")
		{
			constraintData.ropeHasLimits = false;
			constraintData.ropeIsFlexible = true;
		}
		else
		{
			constraintData.ropeHasLimits = true;
			constraintData.ropeIsFlexible = false;
		}
		if (_identifier == "Bar")
		{
			constraintData.ropeIsRigid = true;
		}
		else
		{
			constraintData.ropeIsRigid = false;
		}
		constraintData.ropeMinLength = 0f;
		constraintData.ropeMinLength = 0f;
		constraintData.ropeMaxLength = 1f;
		constraintData.ropeFlexRestLength = 0.5f;
		constraintData.ropeFlexForce = 0.025f;
		constraintData.ropeFlexDamp = 0.1f;
		constraintData.Init(uniqueId, _identifier + uniqueId);
		EIC eIC = GEItemA.Assemble(_container, _identifier, constraintData, Main.camera);
		eIC.isRealtimeMovable = true;
		list.Add(eIC);
		ConstraintData constraintData2 = new ConstraintData();
		constraintData2.position = new Vertex3(_pos + Vector3.up * -50f);
		constraintData2.rotation = new Vertex3(_rot);
		constraintData2.scale = new Vertex3(_sca);
		constraintData2.constraintType = 5u;
		constraintData2.ropeIsFlexible = constraintData.ropeIsFlexible;
		constraintData2.ropeIsRigid = constraintData.ropeIsRigid;
		constraintData2.ropeMinLength = 0f;
		constraintData2.ropeMinLength = 0f;
		constraintData2.ropeMaxLength = 1f;
		constraintData2.ropeFlexRestLength = 0.5f;
		constraintData2.ropeFlexForce = 0.025f;
		constraintData2.ropeFlexDamp = 0.1f;
		constraintData2.ropeHasLimits = constraintData.ropeHasLimits;
		constraintData2.Init(uniqueId, _identifier + uniqueId);
		eIC = GEItemA.Assemble(_container, _identifier, constraintData2, Main.camera);
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
		ConstraintData constraintData = _eiC.data as ConstraintData;
		Camera canvasCamera = _propertyBar.canvasCamera;
		UIC component = CheckBoxA.Assemble(Main.uiCamera, "Cuttable", GEBoltA.HandleConstraintPropertyChange, null, true, Align.Right, 1f, constraintData.ropeIsCuttable, tags);
		UIC uIC = CheckBoxA.Assemble(Main.uiCamera, "Rigid", GEBoltA.HandleConstraintPropertyChange, null, true, Align.Right, 1f, constraintData.ropeIsRigid, tags);
		UIC uIC2 = CheckBoxA.Assemble(Main.uiCamera, "Has Limits", GEBoltA.HandleConstraintPropertyChange, null, true, Align.Right, 1f, constraintData.ropeHasLimits, tags);
		UIC uIC3 = NumericFieldA.Assemble(Main.uiCamera, "Min Length", GEBoltA.HandleConstraintPropertyChange, null, true, Align.Left, 80f, 1f, false, 0f, 10f, constraintData.ropeMinLength, tags);
		UIC uIC4 = NumericFieldA.Assemble(Main.uiCamera, "Max Length", GEBoltA.HandleConstraintPropertyChange, null, true, Align.Left, 80f, 1f, false, 0f, 10f, constraintData.ropeMaxLength, tags);
		UIC uIC5 = CheckBoxA.Assemble(Main.uiCamera, "Flexible", GEBoltA.HandleConstraintPropertyChange, null, true, Align.Right, 1f, constraintData.ropeIsFlexible, tags);
		UIC uIC6 = NumericFieldA.Assemble(Main.uiCamera, "Rest Length", GEBoltA.HandleConstraintPropertyChange, null, true, Align.Left, 80f, 1f, false, 0f, 10f, constraintData.ropeFlexRestLength, tags);
		UIC uIC7 = NumericFieldA.Assemble(Main.uiCamera, "Flex Force", GEBoltA.HandleConstraintPropertyChange, null, true, Align.Left, 80f, 1f, false, 0f, 1f, constraintData.ropeFlexForce, tags);
		UIC uIC8 = NumericFieldA.Assemble(Main.uiCamera, "Flex Damp", GEBoltA.HandleConstraintPropertyChange, null, true, Align.Left, 80f, 1f, false, 0f, 1f, constraintData.ropeFlexDamp, tags);
		UIS.SetController(uIC, uIC5, !constraintData.ropeIsRigid, true);
		UIS.SetController(uIC, uIC2, !constraintData.ropeIsRigid, true);
		UIS.SetController(uIC, uIC3, !constraintData.ropeIsRigid, true);
		UIS.SetController(uIC, uIC4, !constraintData.ropeIsRigid, true);
		UIS.SetController(uIC5, uIC, !constraintData.ropeIsFlexible, true);
		UIS.SetController(uIC5, uIC6, constraintData.ropeIsFlexible);
		UIS.SetController(uIC5, uIC7, constraintData.ropeIsFlexible);
		UIS.SetController(uIC5, uIC8, constraintData.ropeIsFlexible);
		UIS.AddToCanvasGrid(LabelA.Assemble(canvasCamera, "Limits", tags), _propertyBar, true);
		UIS.AddToCanvasGrid(uIC2, _propertyBar, true);
		UIS.AddToCanvasGrid(uIC3, _propertyBar, true);
		UIS.AddToCanvasGrid(uIC4, _propertyBar, false);
		UIS.MoveCursor(_propertyBar, 0f, -15f);
		UIS.AddToCanvasGrid(LabelA.Assemble(canvasCamera, "Properties", tags), _propertyBar, true);
		UIS.AddToCanvasGrid(component, _propertyBar, true);
		UIS.AddToCanvasGrid(uIC, _propertyBar, true);
		UIS.AddToCanvasGrid(uIC5, _propertyBar, true);
		UIS.AddToCanvasGrid(uIC7, _propertyBar, true);
		UIS.AddToCanvasGrid(uIC8, _propertyBar, false);
		UIS.AddToCanvasGrid(uIC6, _propertyBar, true);
		UIS.MoveCursor(_propertyBar, 0f, -15f);
	}

	public static List<GEConstraintC> AssembleSlicedRope(GEConstraintC _c, int _cutIndex, Vector3 _cutPos, Vector3 _delta)
	{
		List<GEConstraintC> list = new List<GEConstraintC>(2);
		Vector3 vector = _c.ropeCMCs[_cutIndex].TC.transform.position - _c.ropeCMCs[_cutIndex - 1].TC.transform.position;
		float num = 1f - (_cutPos - _c.ropeCMCs[_cutIndex - 1].TC.transform.position).magnitude / vector.magnitude;
		if (_c.index != -1)
		{
			Vector2[] array = new Vector2[_cutIndex + 1];
			Vector2[] array2 = new Vector2[_c.ropeCMCs.Length - _cutIndex + 1];
			for (int i = 0; i < _c.ropeCMCs.Length + 1; i++)
			{
				if (i < _cutIndex)
				{
					array[i] = _c.ropeCMCs[i].TC.transform.position;
				}
				else if (i == _cutIndex)
				{
					array[i] = _cutPos;
					array2[array2.Length - (i - _cutIndex) - 1] = _cutPos;
				}
				else
				{
					array2[array2.Length - (i - _cutIndex) - 1] = _c.ropeCMCs[i - 1].TC.transform.position;
				}
			}
			float num2 = (float)_cutIndex / (float)_c.ropeCMCs.Length;
			string[] tags = new string[2]
			{
				LevelManager.m_currentLevel.name + ":GameEntity",
				LevelManager.m_currentLevel.name
			};
			Entity entity = EntityManager.AddEntity(tags);
			TransformC transformComponent = TransformS.AddComponent(entity);
			ChipmunkC chipmunkC = ChipmunkS.AddInactiveComponent(transformComponent, false, ColliderType.Any, 0u, 0u, false, false);
			ChipmunkS.ActivateChipmunkComponent(chipmunkC, ChipmunkWrapper.AddCircleBody(chipmunkC.isStatic, chipmunkC.isRogue, _cutPos, chipmunkC.index, Vector2.zero, 1f, 3f, 0f, 0f, chipmunkC.colliderGroup, chipmunkC.colliderLayer, true, chipmunkC.colliderType));
			ChipmunkWrapper.SetCustomBodyLinearDamp(chipmunkC.cpBodyPtr, Vector2.one * 0.98f);
			ChipmunkWrapper.SetVelocity(chipmunkC.cpBodyPtr, _delta * 10f);
			list.Add(AssembleRopeSplit(array, _c, _c.connectedBodies[0], chipmunkC, num2, num));
			num = 1f - num;
			entity = EntityManager.AddEntity(tags);
			transformComponent = TransformS.AddComponent(entity);
			chipmunkC = ChipmunkS.AddInactiveComponent(transformComponent, false, ColliderType.Any, 0u, 0u, false, false);
			ChipmunkS.ActivateChipmunkComponent(chipmunkC, ChipmunkWrapper.AddCircleBody(chipmunkC.isStatic, chipmunkC.isRogue, _cutPos, chipmunkC.index, Vector2.zero, 1f, 3f, 0f, 0f, chipmunkC.colliderGroup, chipmunkC.colliderLayer, true, chipmunkC.colliderType));
			ChipmunkWrapper.SetCustomBodyLinearDamp(chipmunkC.cpBodyPtr, Vector2.one * 0.98f);
			ChipmunkWrapper.SetVelocity(chipmunkC.cpBodyPtr, _delta * 10f);
			list.Add(AssembleRopeSplit(array2, _c, _c.connectedBodies[1], chipmunkC, 1f - num2, num));
		}
		return list;
	}

	public static GEConstraintC AssembleRopeSplit(Vector2[] _rope, GEConstraintC _c, ChipmunkC _connectedBody0, ChipmunkC _connectedBody1, float _splitValue, float _cutVal)
	{
		string[] tags = new string[2]
		{
			LevelManager.m_currentLevel.name + ":GameEntity",
			LevelManager.m_currentLevel.name
		};
		Entity entity = EntityManager.AddEntity(tags);
		TransformC tc = TransformS.AddComponent(entity);
		PrefabC prefabC = null;
		LineRenderer lineRenderer = null;
		IntPtr zero = IntPtr.Zero;
		IntPtr zero2 = IntPtr.Zero;
		IntPtr zero3 = IntPtr.Zero;
		IntPtr zero4 = IntPtr.Zero;
		IntPtr zero5 = IntPtr.Zero;
		IntPtr intPtr = IntPtr.Zero;
		IntPtr intPtr2 = IntPtr.Zero;
		List<ChipmunkC> list = new List<ChipmunkC>();
		List<Vector2> list2 = new List<Vector2>();
		List<TransformC> list3 = new List<TransformC>();
		List<ChipmunkC> list4 = new List<ChipmunkC>();
		Vector2 vector = _rope[0];
		Vector2 vector2 = _rope[_rope.Length - 1];
		Vector3 vector3 = vector;
		Vector3 vector4 = vector2;
		IntPtr intPtr3 = IntPtr.Zero;
		IntPtr intPtr4 = IntPtr.Zero;
		GEConstraintC gEConstraintC = null;
		GEConstraintC gEConstraintC2 = null;
		if (_connectedBody0 != null)
		{
			intPtr3 = _connectedBody0.cpBodyPtr;
			vector3 = ChipmunkWrapper.GetLocalPos(intPtr3, vector);
		}
		if (_connectedBody1 != null)
		{
			intPtr4 = _connectedBody1.cpBodyPtr;
			vector4 = ChipmunkWrapper.GetLocalPos(intPtr4, vector2);
		}
		float magnitude = (vector2 - vector).magnitude;
		if (intPtr3 == IntPtr.Zero && intPtr4 == IntPtr.Zero)
		{
			return null;
		}
		if (intPtr3 == IntPtr.Zero)
		{
			intPtr3 = ChipmunkWrapper.GetSpaceStaticBody();
		}
		else if (intPtr4 == IntPtr.Zero)
		{
			intPtr4 = ChipmunkWrapper.GetSpaceStaticBody();
		}
		Vector2 vector5 = vector2 - vector;
		int b = _rope.Length;
		b = Mathf.Max(2, b);
		float num = _c.ropeLength / (float)(b - 1);
		float num2 = _c.ropeLength * _splitValue / (float)(b - 1);
		float num3 = num2;
		if (intPtr3 != intPtr4)
		{
			if (_c.isRigid)
			{
				intPtr2 = ChipmunkWrapper.AddSlideJoint(intPtr3, intPtr4, vector3, vector4, _c.ropeLength * (1f - _cutVal), _c.ropeLength * (1f - _cutVal));
			}
			else if (_c.hasLimits)
			{
				intPtr2 = ChipmunkWrapper.AddSlideJoint(intPtr3, intPtr4, vector3, vector4, _c.ropeLength * _c.ropeMinLength * _splitValue, _c.ropeLength * _splitValue - num3 * _cutVal);
			}
			if (_c.isFlexible)
			{
				intPtr = ((!(_c.flexRestLength > 1f)) ? ChipmunkWrapper.AddDampedSpring(intPtr3, intPtr4, vector3, vector4, _c.ropeLength * _splitValue * _c.flexRestLength * _splitValue, _c.flexForce, _c.flexDamp) : ChipmunkWrapper.AddDampedSpring(intPtr3, intPtr4, vector3, vector4, _c.ropeLength * _splitValue * _c.flexRestLength * _splitValue, 0f, 0f));
			}
			if (gEConstraintC != null)
			{
				gEConstraintC.railedSlideJointAPtr = intPtr2;
				gEConstraintC.railedDampedSpringAPtr = intPtr;
			}
			if (gEConstraintC2 != null)
			{
				gEConstraintC2.railedSlideJointBPtr = intPtr2;
				gEConstraintC2.railedDampedSpringBPtr = intPtr;
			}
			list.Add(_connectedBody0);
			list.Add(_connectedBody1);
			list2.Add(vector3);
			list2.Add(vector4);
			ChipmunkC chipmunkC = null;
			for (int i = 0; i < b; i++)
			{
				TransformC transformC = TransformS.AddComponent(entity);
				PrefabC prefabC2 = null;
				PrefabC prefabC3 = null;
				ChipmunkC chipmunkC2 = null;
				bool flag = false;
				if (i == 0)
				{
					prefabC2 = PrefabS.AddComponent(transformC, Vector3.zero, ResourceManager.GetGameObject("Pin"));
					if (gEConstraintC != null)
					{
						gEConstraintC.railedSlideJointATC = transformC;
					}
					else if (_connectedBody0 != null)
					{
						TransformS.ParentComponent(transformC, _connectedBody0.TC, vector3);
					}
					else
					{
						prefabC3 = PrefabS.AddComponent(transformC, Vector3.zero, ResourceManager.GetGameObject("Bar"));
						TransformC transformComponent = TransformS.AddComponent(entity);
						chipmunkC2 = ChipmunkS.AddInactiveComponent(transformComponent, false, ColliderType.Any, 0u, GEState.layer_all, true, false);
						ChipmunkS.ActivateChipmunkComponent(chipmunkC2, ChipmunkWrapper.AddCircleBody(chipmunkC2.isStatic, chipmunkC2.isRogue, vector, chipmunkC2.index, Vector2.zero, 0f, 8f, 0.5f, 0.5f, chipmunkC2.colliderGroup, chipmunkC2.colliderLayer, false, chipmunkC2.colliderType));
					}
					flag = true;
				}
				else if (i == b - 1)
				{
					if (gEConstraintC2 != null)
					{
						gEConstraintC2.railedSlideJointBTC = transformC;
					}
					else if (_connectedBody1 != null)
					{
						TransformS.ParentComponent(transformC, _connectedBody1.TC, vector4);
					}
					else
					{
						prefabC3 = PrefabS.AddComponent(transformC, Vector3.zero, ResourceManager.GetGameObject("Bar"));
						TransformC transformComponent2 = TransformS.AddComponent(entity);
						chipmunkC2 = ChipmunkS.AddInactiveComponent(transformComponent2, false, ColliderType.Any, 0u, GEState.layer_all, true, false);
						ChipmunkS.ActivateChipmunkComponent(chipmunkC2, ChipmunkWrapper.AddCircleBody(chipmunkC2.isStatic, chipmunkC2.isRogue, vector2, chipmunkC2.index, Vector2.zero, 0f, 8f, 0.5f, 0.5f, chipmunkC2.colliderGroup, chipmunkC2.colliderLayer, false, chipmunkC2.colliderType));
					}
					flag = true;
				}
				ChipmunkC chipmunkC3 = ChipmunkS.AddInactiveComponent(transformC, flag, ColliderType.Any, 0u, 0u, false, flag);
				Vector2 position = _rope[i];
				ChipmunkS.ActivateChipmunkComponent(chipmunkC3, ChipmunkWrapper.AddCircleBody(chipmunkC3.isStatic, chipmunkC3.isRogue, position, chipmunkC3.index, Vector2.zero, 1f, 3f, 0f, 0f, chipmunkC3.colliderGroup, chipmunkC3.colliderLayer, true, chipmunkC3.colliderType));
				ChipmunkWrapper.SetCustomBodyLinearDamp(chipmunkC3.cpBodyPtr, Vector2.one * 0.9f);
				ChipmunkWrapper.SetCustomBodyGravity(chipmunkC3.cpBodyPtr, Vector2.up * -1000f);
				if (list4.Count > 0)
				{
					if (_c.isFlexible)
					{
						ChipmunkWrapper.AddDampedSpring(chipmunkC.cpBodyPtr, chipmunkC3.cpBodyPtr, Vector2.zero, Vector2.zero, num3 * _c.flexRestLength, 1000f, 100f);
					}
					else if (i == b - 1)
					{
						ChipmunkWrapper.AddSlideJoint(chipmunkC.cpBodyPtr, chipmunkC3.cpBodyPtr, Vector2.zero, Vector2.zero, 0f, num3 * (1f - _cutVal));
					}
					else
					{
						ChipmunkWrapper.AddSlideJoint(chipmunkC.cpBodyPtr, chipmunkC3.cpBodyPtr, Vector2.zero, Vector2.zero, 0f, num3);
					}
				}
				list4.Add(chipmunkC3);
				chipmunkC = chipmunkC3;
			}
			prefabC = PrefabS.AddComponent(list4[0].TC, Vector3.zero);
			lineRenderer = prefabC.p_gameObject.AddComponent<LineRenderer>() as LineRenderer;
			lineRenderer.SetWidth(3f, 3f);
			lineRenderer.SetVertexCount(2);
			lineRenderer.SetPosition(0, vector);
			lineRenderer.SetPosition(1, vector2);
			lineRenderer.material = new Material(ResourceManager.GetShader("ConstraintShader"));
			lineRenderer.material.mainTexture = ResourceManager.GetTexture("ConstraintDif");
			float num4 = 16f;
			num4 = (_c.isFlexible ? 14f : ((!_c.isRigid) ? 15f : 13f));
			lineRenderer.material.SetTextureOffset("_MainTex", new Vector2(0f, num4 / 16f - 0.0625f));
			lineRenderer.material.SetTextureScale("_MainTex", new Vector2(_c.ropeLength * _c.flexRestLength * _splitValue / 30f, 2f / 33f));
		}
		GEConstraintC gEConstraintC3 = GES.AddConstraintComponent(null, tc, null);
		gEConstraintC3.slideJointPtr = intPtr2;
		gEConstraintC3.connectJointPtr = intPtr;
		gEConstraintC3.constraintType = ConstraintType.Rope;
		gEConstraintC3.connectedBodies = list.ToArray();
		gEConstraintC3.connectedBodyLocalAnchors = list2.ToArray();
		gEConstraintC3.ropeCMCs = list4.ToArray();
		gEConstraintC3.ropeLength = _c.ropeLength * _splitValue + num3 * _cutVal;
		gEConstraintC3.ropeMinLength = _c.ropeMinLength;
		gEConstraintC3.ropeMaxLength = _c.ropeMaxLength * _splitValue;
		gEConstraintC3.hasLimits = _c.hasLimits;
		gEConstraintC3.isCuttable = false;
		gEConstraintC3.isRigid = _c.isRigid;
		gEConstraintC3.isFlexible = _c.isFlexible;
		gEConstraintC3.flexDamp = _c.flexDamp;
		gEConstraintC3.flexForce = _c.flexForce;
		gEConstraintC3.flexRestLength = _c.flexRestLength;
		gEConstraintC3.triggerType = TriggerType.RopeConstraint;
		gEConstraintC3.autoTrigger = true;
		gEConstraintC3.energy = 1f;
		gEConstraintC3.PC = prefabC;
		gEConstraintC3.lineRenderer = lineRenderer;
		gEConstraintC3.camera = _c.camera;
		gEConstraintC3.ropeCutTime = Main.m_gameTime;
		return gEConstraintC3;
	}
}
