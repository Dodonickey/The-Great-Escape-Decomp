using System;
using System.Collections.Generic;
using UnityEngine;

public static class GETransformGizmoA
{
	public static int m_draggedOverEIC;

	public static GETransformGizmoC Assemble(bool _active)
	{
		m_draggedOverEIC = -1;
		float num = 1f;
		if (!_active)
		{
			num = 0.25f;
		}
		int count = EditorState.m_selection.Count;
		Vector3 zero = Vector3.zero;
		for (int i = 0; i < count; i++)
		{
			zero += EditorState.m_selection[i].uiTC.transform.position;
		}
		zero /= (float)count;
		EditorState.m_selectionOffsets = new Vector3[count];
		for (int j = 0; j < count; j++)
		{
			EditorState.m_selectionOffsets[j] = zero - EditorState.m_selection[j].uiTC.transform.position;
		}
		Vector3 rotation = Vector3.zero;
		if (EditorState.m_selection.Count == 1)
		{
			rotation = EditorState.m_selection[0].TC.transform.rotation.eulerAngles;
		}
		string[] tags = new string[1] { "transformGizmo" };
		TransformC transformC = EntityManager.AddEntityWithTC(tags);
		TransformS.SetPosition(transformC, zero);
		TransformS.SetRotation(transformC, rotation);
		TransformC transformC2 = TransformS.AddComponent(transformC.entityIndex);
		TransformS.ParentComponent(transformC2, transformC, Vector3.zero);
		transformC2.forceRotation = true;
		float num2 = 60f;
		float num3 = 100f;
		float roundRadius = 5f;
		TransformC transformC3 = TransformS.AddComponent(transformC.entityIndex);
		TransformC transformC4 = TransformS.AddComponent(transformC.entityIndex);
		TransformC transformC5 = TransformS.AddComponent(transformC.entityIndex);
		TransformS.ParentComponent(transformC3, transformC, Vector3.zero);
		TransformS.ParentComponent(transformC4, transformC, Vector3.zero);
		TransformS.ParentComponent(transformC5, transformC, Vector3.zero);
		TransformS.SetPosition(transformC3, Vector3.right * (num3 - 15f));
		TransformS.SetPosition(transformC4, Vector3.up * (num3 - 15f));
		TransformS.SetPosition(transformC5, new Vector3(num3 - 15f, num3 - 15f, 0f) * Mathf.Sin((float)Math.PI / 4f));
		TransformC transformC6 = TransformS.AddComponent(transformC2.entityIndex);
		TransformS.ParentComponent(transformC6, transformC2);
		TransformS.SetPosition(transformC6, new Vector3(0f, -130f, 0f));
		GETransformGizmoC gETransformGizmoC = GES.AddTransformGizmoComponent(transformC);
		gETransformGizmoC.originalPosition = new List<Vector3>(30);
		gETransformGizmoC.originalRotation = new List<Vector3>(30);
		gETransformGizmoC.originalScale = new List<Vector3>(30);
		Vector2[] circle = DebugDraw.GetCircle(num2, 36, Vector2.zero, false);
		Vector2[] circle2 = DebugDraw.GetCircle(num2 - 6f, 36, Vector2.zero, false);
		PrefabS.CreatePathPrefabComponentFromVectorArray(transformC, Vector3.forward * -10f, circle, 6f, new Color(1f, 1f, 1f, 1f * num), ResourceManager.GetMaterial("Line8"), Main.uiCamera, Position.Center, true);
		PrefabS.CreatePathPrefabComponentFromVectorArray(transformC, Vector3.forward * -10f, circle2, 8f, new Color(1f, 1f, 1f, 0.1f * num), ResourceManager.GetMaterial("Line8"), Main.uiCamera, Position.Inside, true);
		PrefabS.CreatePathPrefabComponentFromVectorArray(transformC, Vector3.forward * -10f, circle, 22f, new Color(1f, 1f, 1f, 0.1f * num), ResourceManager.GetMaterial("Line32"), Main.uiCamera, Position.Inside, true);
		if (EditorState.m_selection.Count == 1)
		{
			EIC eIC = EditorState.m_selection[0];
			if (eIC.connectionMode)
			{
				GEConnectionLogic.CreateInputAnchor(eIC);
				GEConnectionLogic.CreateOutputAnchor(eIC);
				GEConnectionLogic.CreateModifierAnchor(eIC);
			}
			else
			{
				if (eIC.isRotateable)
				{
					if (_active)
					{
						TouchAreaC touchAreaComponent = TouchAreaS.AddComponent(transformC, "rotateZ", num3, true, Main.uiCamera, gETransformGizmoC);
						TouchAreaS.AddTouchEventListener(touchAreaComponent, HandleTransformGizmoTouches);
					}
					if (eIC.isScaleable)
					{
						if (eIC.isScaleUnified)
						{
							DrawHandle(transformC, 67.5f, 22.5f, 2f, num2, num3, roundRadius, 0, num);
						}
						else
						{
							DrawHandle(transformC, 112.5f, 337.5f, 2f, num2, num3, roundRadius, 0, num);
						}
					}
					else
					{
						DrawHandle(transformC, 0f, 0f, 0f, num2, num3, roundRadius, 0, num);
					}
				}
				if (eIC.isScaleable)
				{
					if (!eIC.isScaleUnified)
					{
						DrawHandle(transformC3, -transformC3.transform.localPosition, -22.5f, 22.5f, 2f, num2, num3, roundRadius, 1, num);
						DrawHandle(transformC5, -transformC5.transform.localPosition, 22.5f, 67.5f, 2f, num2, num3, roundRadius, 2, num);
						DrawHandle(transformC4, -transformC4.transform.localPosition, 67.5f, 112.5f, 2f, num2, num3, roundRadius, 1, num);
						TouchAreaC touchAreaComponent2 = TouchAreaS.AddComponent(transformC3, "scaleX", 30f, true, Main.uiCamera, gETransformGizmoC);
						TouchAreaS.AddTouchEventListener(touchAreaComponent2, HandleTransformGizmoTouches);
						TouchAreaC touchAreaComponent3 = TouchAreaS.AddComponent(transformC4, "scaleY", 30f, true, Main.uiCamera, gETransformGizmoC);
						TouchAreaS.AddTouchEventListener(touchAreaComponent3, HandleTransformGizmoTouches);
						TouchAreaC touchAreaComponent4 = TouchAreaS.AddComponent(transformC5, "scale", 30f, true, Main.uiCamera, gETransformGizmoC);
						TouchAreaS.AddTouchEventListener(touchAreaComponent4, HandleTransformGizmoTouches);
					}
					else
					{
						DrawHandle(transformC5, -transformC5.transform.localPosition, 22.5f, 67.5f, 2f, num2, num3, roundRadius, 2, num);
						TouchAreaC touchAreaComponent5 = TouchAreaS.AddComponent(transformC5, "scale", 30f, true, Main.uiCamera, gETransformGizmoC);
						TouchAreaS.AddTouchEventListener(touchAreaComponent5, HandleTransformGizmoTouches);
					}
				}
			}
			if (_active)
			{
				gETransformGizmoC.moveTAC = TouchAreaS.AddComponent(transformC, "move", num2, true, Main.uiCamera, gETransformGizmoC);
				TouchAreaS.AddTouchEventListener(gETransformGizmoC.moveTAC, HandleTransformGizmoTouches);
				if (EditorState.m_selection[0].itemType != 0 && EditorState.m_selection[0].trigger != null)
				{
					TouchAreaC touchAreaComponent6 = TouchAreaS.AddComponent(transformC6, "mode", 20f, true, Main.uiCamera, gETransformGizmoC);
					TouchAreaS.AddTouchEventListener(touchAreaComponent6, HandleTransformGizmoTouches);
					SpriteC c = SpriteS.AddComponent(transformC6, new Frame(0f, 128f, 128f, 64f), GEState.editorUISheet);
					SpriteS.SetDimensionScale(c, 0.5f);
				}
			}
		}
		else if (_active)
		{
			gETransformGizmoC.moveTAC = TouchAreaS.AddComponent(transformC, "move", num2, true, Main.uiCamera, gETransformGizmoC);
			TouchAreaS.AddTouchEventListener(gETransformGizmoC.moveTAC, HandleTransformGizmoTouches);
		}
		return gETransformGizmoC;
	}

	private static Vector2[] DrawHandle(TransformC _tc, float _startAngle, float _endAngle, float _spacing, float _startRadius, float _endRadius, float _roundRadius, int _fillStyle, float _alpha)
	{
		return DrawHandle(_tc, Vector3.zero, _startAngle, _endAngle, _spacing, _startRadius, _endRadius, _roundRadius, _fillStyle, _alpha);
	}

	private static Vector2[] DrawHandle(TransformC _tc, Vector3 _offset, float _startAngle, float _endAngle, float _spacing, float _startRadius, float _endRadius, float _roundRadius, int _fillStyle, float _alpha)
	{
		List<Vector2> list = new List<Vector2>();
		if (_startAngle != _endAngle)
		{
			float num = (_startAngle + _spacing) * ((float)Math.PI / 180f);
			float num2 = _spacing * ((float)Math.PI / 180f) * 0.5f;
			Vector2 item = new Vector2(Mathf.Cos(num + num2) * _startRadius, Mathf.Sin(num + num2) * _startRadius);
			Vector2 vector = new Vector2(Mathf.Cos(num) * (_endRadius - _roundRadius), Mathf.Sin(num) * (_endRadius - _roundRadius));
			Vector2 vector2 = new Vector2(Mathf.Cos(num + (float)Math.PI / 2f) * _roundRadius, Mathf.Sin(num + (float)Math.PI / 2f) * _roundRadius);
			float num3 = (_endAngle - _spacing) * ((float)Math.PI / 180f);
			Vector2 item2 = new Vector2(Mathf.Cos(num3 - num2) * _startRadius, Mathf.Sin(num3 - num2) * _startRadius);
			Vector2 vector3 = new Vector2(Mathf.Cos(num3) * (_endRadius - _roundRadius), Mathf.Sin(num3) * (_endRadius - _roundRadius));
			Vector2 vector4 = new Vector2(Mathf.Cos(num3 - (float)Math.PI / 2f) * _roundRadius, Mathf.Sin(num3 - (float)Math.PI / 2f) * _roundRadius);
			Vector2[] line = DebugDraw.GetLine(vector, vector + vector2, 0);
			Vector2[] line2 = DebugDraw.GetLine(vector3, vector3 + vector4, 0);
			float num4 = Mathf.Atan2(line[1].y, line[1].x) * 57.29578f;
			float num5 = Mathf.Atan2(line2[1].y, line2[1].x) * 57.29578f;
			Vector2[] arc = DebugDraw.GetArc(_roundRadius, 9, num4 - _startAngle + 90f, _startAngle - 90f, line[1]);
			Vector2[] arc2 = DebugDraw.GetArc(_roundRadius, 9, num4 - _startAngle + 90f, num5, line2[1]);
			float magnitude = arc[0].magnitude;
			float num6 = num5 - num4;
			if (num6 < 0f)
			{
				num6 = 360f + num6;
			}
			Vector2[] arc3 = DebugDraw.GetArc(magnitude, 36, num6, num4, Vector2.zero);
			list.Add(item2);
			list.AddRange(arc2);
			list.RemoveAt(list.Count - 1);
			list.AddRange(arc3);
			list.RemoveAt(list.Count - 1);
			list.AddRange(arc);
			list.Add(item);
			switch (_fillStyle)
			{
			case 0:
			{
				PrefabS.CreateLinePrefabComponentFromVectorArray(_tc, _offset + Vector3.forward * -10f, list.ToArray(), 4f, new Color(1f, 1f, 1f, 1f * _alpha), ResourceManager.GetMaterial("Line6"), Main.uiCamera, Position.Center);
				Polygon polygon7 = new Polygon();
				polygon7.AddContour(new VertexList(list.ToArray()), false);
				Vector2[] circle3 = DebugDraw.GetCircle(_endRadius - 12f, 36, Vector2.zero, false);
				Polygon polygon8 = new Polygon();
				polygon8.AddContour(new VertexList(circle3), false);
				Polygon polygon9 = polygon7.Clip(GpcOperation.Difference, polygon8);
				polygon7 = polygon7.Clip(GpcOperation.Difference, polygon8);
				float num7 = (float)Math.PI / 12f;
				float num8 = _endRadius - 6f;
				Vector2[] p3 = new Vector2[4]
				{
					new Vector2(-12f, 3f),
					new Vector2(12f, 7f),
					new Vector2(12f, -6f),
					new Vector2(-12f, -6f)
				};
				for (int i = 0; i < 24; i++)
				{
					polygon8 = new Polygon();
					polygon8.AddContour(new VertexList(p3), false);
					Vector2 pos = new Vector2(Mathf.Cos(num7 * (float)i) * num8, Mathf.Sin(num7 * (float)i) * num8);
					polygon8 = DebugDraw.TransformPolygon(polygon8, pos, 12 + i * 15);
					polygon9 = polygon9.Clip(GpcOperation.Difference, polygon8);
				}
				Polygon polygon10 = polygon7.Clip(GpcOperation.Difference, polygon9);
				PrefabS.CreateFlatPrefabComponentsFromPolygon(_tc, _offset + Vector3.forward * -5f, polygon9, new Color(1f, 1f, 1f, 0.2f * _alpha), ResourceManager.GetMaterial("Solid"), Main.uiCamera);
				PrefabS.CreateFlatPrefabComponentsFromPolygon(_tc, _offset + Vector3.forward * -5f, polygon10, new Color(0f, 0f, 0f, 0.15f * _alpha), ResourceManager.GetMaterial("Solid"), Main.uiCamera);
				break;
			}
			case 1:
			{
				PrefabS.CreateLinePrefabComponentFromVectorArray(_tc, _offset + Vector3.forward * -10f, list.ToArray(), 4f, new Color(1f, 1f, 1f, 1f * _alpha), ResourceManager.GetMaterial("Line6"), Main.uiCamera, Position.Center);
				Polygon polygon3 = new Polygon();
				polygon3.AddContour(new VertexList(list.ToArray()), false);
				Vector2[] circle = DebugDraw.GetCircle(_endRadius - 12f, 36, Vector2.zero, false);
				Polygon polygon4 = new Polygon();
				polygon4.AddContour(new VertexList(circle), false);
				Vector2[] circle2 = DebugDraw.GetCircle(_endRadius - 6f, 36, Vector2.zero, false);
				Polygon polygon5 = new Polygon();
				polygon5.AddContour(new VertexList(circle2), false);
				Polygon polygon6 = polygon3.Clip(GpcOperation.Difference, polygon5);
				polygon3 = polygon3.Clip(GpcOperation.Difference, polygon4);
				polygon3 = polygon3.Clip(GpcOperation.Intersection, polygon5);
				PrefabS.CreateFlatPrefabComponentsFromPolygon(_tc, _offset + Vector3.forward * -5f, polygon3, new Color(1f, 1f, 1f, 0.2f * _alpha), ResourceManager.GetMaterial("Solid"), Main.uiCamera);
				PrefabS.CreateFlatPrefabComponentsFromPolygon(_tc, _offset + Vector3.forward * -5f, polygon6, new Color(0f, 0f, 0f, 0.15f * _alpha), ResourceManager.GetMaterial("Solid"), Main.uiCamera);
				TransformS.SetRotation(_tc, Vector3.zero);
				break;
			}
			case 2:
			{
				PrefabS.CreateLinePrefabComponentFromVectorArray(_tc, _offset + Vector3.forward * -10f, list.ToArray(), 4f, new Color(1f, 1f, 1f, 1f * _alpha), ResourceManager.GetMaterial("Line6"), Main.uiCamera, Position.Center);
				Vector2[] p = DrawHandle(_tc, _startAngle, _endAngle, 6f, _startRadius, _endRadius - 6f, _roundRadius * 0.5f, -1, _alpha);
				Vector2[] p2 = DrawHandle(_tc, _startAngle, _endAngle, 10f, _startRadius, _endRadius - 12f, _roundRadius * 0.25f, -1, _alpha);
				Polygon polygon = new Polygon();
				polygon.AddContour(new VertexList(p), false);
				polygon.AddContour(new VertexList(p2), true);
				Polygon polygon2 = new Polygon();
				polygon2.AddContour(new VertexList(list.ToArray()), false);
				polygon2.AddContour(new VertexList(p), true);
				PrefabS.CreateFlatPrefabComponentsFromPolygon(_tc, _offset + Vector3.forward * -5f, polygon, new Color(1f, 1f, 1f, 0.2f * _alpha), ResourceManager.GetMaterial("Solid"), Main.uiCamera);
				PrefabS.CreateFlatPrefabComponentsFromPolygon(_tc, _offset + Vector3.forward * -5f, polygon2, new Color(0f, 0f, 0f, 0.15f * _alpha), ResourceManager.GetMaterial("Solid"), Main.uiCamera);
				TransformS.SetRotation(_tc, Vector3.zero);
				break;
			}
			}
		}
		else
		{
			Vector2[] circle4 = DebugDraw.GetCircle(_endRadius, 36, Vector2.zero, true);
			list.AddRange(circle4);
			PrefabS.CreateLinePrefabComponentFromVectorArray(_tc, _offset + Vector3.forward * -10f, list.ToArray(), 4f, new Color(1f, 1f, 1f, 1f * _alpha), ResourceManager.GetMaterial("Line6"), Main.uiCamera, Position.Center);
			Polygon polygon11 = new Polygon();
			polygon11.AddContour(new VertexList(list.ToArray()), false);
			Vector2[] circle5 = DebugDraw.GetCircle(_endRadius - 12f, 36, Vector2.zero, false);
			Polygon polygon12 = new Polygon();
			polygon12.AddContour(new VertexList(circle5), false);
			Polygon polygon13 = polygon11.Clip(GpcOperation.Difference, polygon12);
			polygon11 = polygon11.Clip(GpcOperation.Difference, polygon12);
			float num9 = (float)Math.PI / 12f;
			float num10 = _endRadius - 6f;
			Vector2[] p4 = new Vector2[4]
			{
				new Vector2(-12f, 3f),
				new Vector2(12f, 7f),
				new Vector2(12f, -6f),
				new Vector2(-12f, -6f)
			};
			for (int j = 0; j < 24; j++)
			{
				polygon12 = new Polygon();
				polygon12.AddContour(new VertexList(p4), false);
				Vector2 pos2 = new Vector2(Mathf.Cos(num9 * (float)j) * num10, Mathf.Sin(num9 * (float)j) * num10);
				polygon12 = DebugDraw.TransformPolygon(polygon12, pos2, 12 + j * 15);
				polygon13 = polygon13.Clip(GpcOperation.Difference, polygon12);
			}
			Polygon polygon14 = polygon11.Clip(GpcOperation.Difference, polygon13);
			PrefabS.CreateFlatPrefabComponentsFromPolygon(_tc, _offset + Vector3.forward * -5f, polygon13, new Color(1f, 1f, 1f, 0.2f * _alpha), ResourceManager.GetMaterial("Solid"), Main.uiCamera);
			PrefabS.CreateFlatPrefabComponentsFromPolygon(_tc, _offset + Vector3.forward * -5f, polygon14, new Color(0f, 0f, 0f, 0.15f * _alpha), ResourceManager.GetMaterial("Solid"), Main.uiCamera);
		}
		return list.ToArray();
	}

	public static void HandleTransformGizmoTouches(TouchAreaC _c, int _i, bool _consumed)
	{
		if (_consumed)
		{
			return;
		}
		GETransformGizmoC gETransformGizmoC = _c.customComponent as GETransformGizmoC;
		Vector2 vector = _c.touchPos[_i] - _c.touchStartPos[_i];
		TLTouch tLTouch = InputManager.m_touches[_c.touchIndex[_i]];
		Vector2 position = tLTouch.position;
		Vector3 vector2 = new Vector3(position.x - (float)Screen.width * 0.5f, position.y - (float)Screen.height * 0.5f, 0f);
		if (EditorState.m_selection[0].camera == Main.camera)
		{
			vector *= Main.m_gameCameraDistanceMultipler;
		}
		if (!_c.touchStartedInside[_i])
		{
			return;
		}
		if (_c.touchEvent[_i] == TouchEvent.Began)
		{
			if (!gETransformGizmoC.readyToMove)
			{
				gETransformGizmoC.readyToMove = true;
			}
			for (int i = 0; i < EditorState.m_selection.Count; i++)
			{
				EIC eIC = EditorState.m_selection[i];
				gETransformGizmoC.originalScale.Add(eIC.TC.transform.localScale);
				gETransformGizmoC.originalRotation.Add(eIC.TC.transform.rotation.eulerAngles);
				gETransformGizmoC.originalPosition.Add(eIC.TC.transform.position);
			}
			if (_c.identifier == "rotateZ")
			{
				Vector3 vector3 = _c.TC.transform.position - vector2;
				float z = Mathf.Atan2(vector3.y, vector3.x) * 57.29578f;
				gETransformGizmoC.rotateStart.z = z;
			}
			else if (!(_c.identifier == "scale") && !(_c.identifier == "scaleX") && !(_c.identifier == "scaleY") && _c.identifier == "move")
			{
				gETransformGizmoC.touchOffset = _c.TC.transform.position - vector2;
			}
		}
		else if (_c.touchEvent[_i] == TouchEvent.Drag || _c.touchEvent[_i] == TouchEvent.Down)
		{
			if (gETransformGizmoC.originalPosition.Count == 0)
			{
				for (int j = 0; j < EditorState.m_selection.Count; j++)
				{
					EIC eIC2 = EditorState.m_selection[j];
					gETransformGizmoC.originalScale.Add(eIC2.TC.transform.localScale);
					gETransformGizmoC.originalRotation.Add(eIC2.TC.transform.rotation.eulerAngles);
					gETransformGizmoC.originalPosition.Add(eIC2.TC.transform.position);
				}
			}
			if (_c.identifier == "rotateZ")
			{
				Vector3 vector4 = _c.TC.transform.position - vector2;
				float num = Mathf.Atan2(vector4.y, vector4.x) * 57.29578f;
				float num2 = gETransformGizmoC.rotateStart.z - num;
				for (int k = 0; k < EditorState.m_selection.Count; k++)
				{
					EIC eIC3 = EditorState.m_selection[k];
					TransformS.SetGlobalRotation(eIC3.TC, Vector3.forward * (gETransformGizmoC.originalRotation[k].z - num2));
					TransformS.SetGlobalRotation(gETransformGizmoC.gizmoTC, Vector3.forward * (gETransformGizmoC.originalRotation[k].z - num2));
				}
			}
			else if (_c.identifier == "scale")
			{
				float num3 = Mathf.Max(vector.x, vector.y);
				for (int l = 0; l < EditorState.m_selection.Count; l++)
				{
					if (gETransformGizmoC.originalScale[l].x + ((num3 + 120f) / 120f - 1f) * gETransformGizmoC.originalScale[l].x > 0f && gETransformGizmoC.originalScale[l].y + ((num3 + 85f) / 85f - 1f) * gETransformGizmoC.originalScale[l].y > 0f)
					{
						TransformS.SetScale(EditorState.m_selection[l].TC, gETransformGizmoC.originalScale[l] + new Vector3(((num3 + 85f) / 85f - 1f) * gETransformGizmoC.originalScale[l].x, ((num3 + 85f) / 85f - 1f) * gETransformGizmoC.originalScale[l].y, 0f));
					}
				}
			}
			else if (_c.identifier == "scaleX")
			{
				for (int m = 0; m < EditorState.m_selection.Count; m++)
				{
					if (gETransformGizmoC.originalScale[m].x + ((vector.x + 120f) / 120f - 1f) * gETransformGizmoC.originalScale[m].x > 0.1f)
					{
						TransformS.SetScale(EditorState.m_selection[m].TC, gETransformGizmoC.originalScale[m] + new Vector3(((vector.x + 85f) / 85f - 1f) * gETransformGizmoC.originalScale[m].x, 0f, 0f));
					}
				}
			}
			else if (_c.identifier == "scaleY")
			{
				for (int n = 0; n < EditorState.m_selection.Count; n++)
				{
					if (gETransformGizmoC.originalScale[n].y + ((vector.y + 120f) / 120f - 1f) * gETransformGizmoC.originalScale[n].y > 0.1f)
					{
						TransformS.SetScale(EditorState.m_selection[n].TC, gETransformGizmoC.originalScale[n] + new Vector3(0f, ((vector.y + 85f) / 85f - 1f) * gETransformGizmoC.originalScale[n].y, 0f));
					}
				}
			}
			else if (_c.identifier == "move" && gETransformGizmoC.readyToMove)
			{
				Vector3 vector5 = new Vector3(vector2.x, vector2.y, 0f);
				Vector3 step = vector5 - gETransformGizmoC.gizmoTC.transform.position + gETransformGizmoC.touchOffset;
				step *= 0.25f;
				bool flag = false;
				if (GEState.m_specialDown)
				{
					flag = true;
				}
				if (!flag)
				{
					TransformS.Move(gETransformGizmoC.gizmoTC, step);
				}
				for (int num4 = 0; num4 < EditorState.m_selection.Count; num4++)
				{
					EIC eIC4 = EditorState.m_selection[num4];
					if (!flag)
					{
						Vector3 vector6 = EditorState.m_selectionOffsets[num4];
						Vector3 vector7 = eIC4.uiTC.transform.position + vector6 - gETransformGizmoC.touchOffset;
						Vector3 step2 = vector5 - vector7;
						step2 *= 0.25f;
						TransformS.GlobalMove(eIC4.uiTC, step2);
						if (eIC4.camera == Main.camera)
						{
							Vector3 vector8 = Main.camera.ScreenToWorldPoint(eIC4.uiTC.transform.position + new Vector3((float)Screen.width * 0.5f, (float)Screen.height * 0.5f, Main.camera.transform.position.z) - Vector3.forward * eIC4.data.position.z) - Main.camera.transform.position * 2f;
							vector8.z = 0f;
							TransformS.SetGlobalPosition(eIC4.TC, -vector8 + Vector3.forward * eIC4.data.position.z);
						}
						else
						{
							TransformS.SetGlobalPosition(eIC4.TC, eIC4.uiTC.transform.position);
						}
					}
					else
					{
						TransformS.SetGlobalPosition(eIC4.TC, eIC4.TC.transform.position + Vector3.forward * tLTouch.deltaPosition.y * 5f);
						Vector3 position2 = eIC4.TC.transform.position;
						Vector3 position3 = Main.camera.transform.position;
						position3.z = 0f;
						Vector3 position4 = Main.camera.WorldToScreenPoint(position2) - new Vector3((float)Screen.width * 0.5f, (float)Screen.height * 0.5f, 0f);
						position4.z = 0f;
						TransformS.SetGlobalPosition(gETransformGizmoC.gizmoTC, position4);
					}
				}
			}
			if (!gETransformGizmoC.readyToMove && vector.sqrMagnitude > 100f)
			{
				gETransformGizmoC.readyToMove = true;
				gETransformGizmoC.touchOffset = (Vector2)_c.TC.transform.position - (_c.touchStartPos[_i] - new Vector2((float)Screen.width * 0.5f, (float)Screen.height * 0.5f));
			}
		}
		else if ((!_c.touchWasDragged[_i] || _c.identifier == "mode") && _c.touchEvent[_i] == TouchEvent.Release && _c.touchStartedInside[_i])
		{
			if (_c.identifier == "mode")
			{
				for (int num5 = 0; num5 < EditorState.m_selection.Count; num5++)
				{
					EIC eIC5 = EditorState.m_selection[num5];
					if (eIC5.connectionMode)
					{
						eIC5.connectionMode = false;
					}
					else
					{
						eIC5.connectionMode = true;
					}
				}
				EditorState.UpdateSelection();
			}
			else if (EditorState.m_selection.Count == 1 && gETransformGizmoC.readyToMove)
			{
				EditorState.m_selection.Clear();
				EditorState.UpdateSelection();
			}
		}
		else
		{
			if (_c.touchEvent[_i] != TouchEvent.Release && _c.touchEvent[_i] != TouchEvent.ReleaseOutside)
			{
				return;
			}
			bool flag2 = false;
			List<Vector3> list = new List<Vector3>(EditorState.m_selection.Count);
			for (int num6 = 0; num6 < EditorState.m_selection.Count; num6++)
			{
				EIC eIC6 = EditorState.m_selection[num6];
				Vector3 localScale = eIC6.TC.transform.localScale;
				list.Add(localScale);
				Vector3 eulerAngles = eIC6.TC.transform.rotation.eulerAngles;
				if (_c.identifier == "rotateZ")
				{
					if (eIC6.data != null)
					{
						eIC6.data.rotation = new Vertex3(eIC6.TC.transform.rotation.eulerAngles);
					}
				}
				else if (_c.identifier == "scale" || _c.identifier == "scaleX" || _c.identifier == "scaleY")
				{
					bool flag3 = false;
					for (int num7 = 0; num7 < eIC6.gameComponents.Count; num7++)
					{
						IComponent component = eIC6.gameComponents[num7];
						if (component.componentType == (ComponentType)100)
						{
							flag3 = true;
							GEBlockC gEBlockC = component as GEBlockC;
							if (gEBlockC.originalShape != null)
							{
								DebugDraw.ScalePolygon(gEBlockC.originalShape, localScale);
							}
						}
					}
					if (flag3)
					{
						TransformS.SetScale(eIC6.TC, Vector3.one);
						gETransformGizmoC.originalScale[num6] = Vector3.one;
					}
					eIC6.data.scale = new Vertex3(eIC6.TC.transform.localScale);
					EditorState.ResetEditorItem(eIC6);
				}
				else if (_c.identifier == "move" && eIC6.data != null)
				{
					if (eIC6.container != null)
					{
						GES.SetContainerPosition(eIC6.container, true);
						SetChildrenTransformsToData(eIC6.container);
					}
					else
					{
						GES.SetContainerPosition(eIC6, true);
						SetChildrenTransformsToData(eIC6);
					}
				}
			}
			if (!gETransformGizmoC.originalPosition.Contains(Vector3.zero))
			{
				UndoManager.AddStep(new TransformStep(EditorState.m_selection, _c, list));
			}
			else if ((flag2 & (UndoManager.current != null)) && UndoManager.current.type == UndoStepType.Create)
			{
				CreateNewStep createNewStep = UndoManager.current as CreateNewStep;
				for (int num8 = 0; num8 < EditorState.m_selection.Count; num8++)
				{
					EIC eIC7 = EditorState.m_selection[num8];
					createNewStep.pos = eIC7.TC.transform.position;
					createNewStep.scale = list[num8];
					createNewStep.rot = eIC7.TC.transform.rotation.eulerAngles;
				}
			}
			gETransformGizmoC.originalPosition.Clear();
			gETransformGizmoC.originalRotation.Clear();
			gETransformGizmoC.originalScale.Clear();
			foreach (EIC item in EditorState.m_selection)
			{
				EditorState.ResetEditorItem(item);
			}
		}
	}

	private static void SetChildrenTransformsToData(EIC _eic)
	{
		if (_eic.data != null)
		{
			_eic.data.position = new Vertex3(_eic.TC.transform.position);
			if (_eic.camera == Main.uiCamera)
			{
				_eic.referenceHeight = Screen.height;
				_eic.referenceWidth = Screen.width;
			}
		}
		if (_eic.subItems == null)
		{
			return;
		}
		for (int i = 0; i < _eic.subItems.Count; i++)
		{
			if (_eic.subItems[i].itemType == 2 || _eic.subItems[i].itemType == 1 || _eic.subItems[i].itemType == 0)
			{
				SetChildrenTransformsToData(_eic.subItems[i]);
			}
		}
	}
}
