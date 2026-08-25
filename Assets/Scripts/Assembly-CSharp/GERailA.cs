using System.Collections.Generic;
using UnityEngine;

public static class GERailA
{
	public static GEConstraintC Assemble(EIC _eic, ConstraintData _data)
	{
		List<EIC> subItems = _eic.subItems;
		string[] tags = new string[2]
		{
			LevelManager.m_currentLevel.name + ":GameEntity",
			LevelManager.m_currentLevel.name
		};
		Entity entity = EntityManager.AddEntity(tags);
		TransformC transformC = TransformS.AddComponent(entity);
		PrefabC prefabC = PrefabS.AddComponent(transformC, Vector3.zero);
		LineRenderer lineRenderer = prefabC.p_gameObject.AddComponent("LineRenderer") as LineRenderer;
		lineRenderer.SetWidth(10f, 10f);
		lineRenderer.material = GEState.constraintSheet.m_material;
		lineRenderer.material.shader = Shader.Find("GameEditor/VertexColorUnlit/Transparent");
		lineRenderer.material.SetTextureOffset("_MainTex", new Vector2(0f, 0.9375f));
		lineRenderer.material.SetTextureScale("_MainTex", new Vector2(1f, 2f / 33f));
		int num = 0;
		List<AnchorPointInfo> list = new List<AnchorPointInfo>();
		for (int i = 0; i < subItems.Count; i++)
		{
			EIC eIC = subItems[i];
			if (eIC.identifier == "RailPoint" && (eIC.itemType == 2 || eIC.itemType == 1))
			{
				AnchorPointInfo anchorPointInfo = new AnchorPointInfo(eIC.data.position.ToVector3(), (eIC.data as ConstraintPointData).anchorIndex, AnchorType.RailPoint);
				anchorPointInfo.anchorIndex = (eIC.data as ConstraintPointData).anchorIndex;
				anchorPointInfo.entryEasingType = (eIC.data as ConstraintPointData).entryEasingType;
				anchorPointInfo.exitEasingType = (eIC.data as ConstraintPointData).exitEasingType;
				anchorPointInfo.interpolationType = (eIC.data as ConstraintPointData).interpolationType;
				anchorPointInfo.velocityMultipler = (eIC.data as ConstraintPointData).velocityMultipler;
				anchorPointInfo.waitAtPoint = (eIC.data as ConstraintPointData).waitAtPoint;
				list.Add(anchorPointInfo);
				num++;
			}
		}
		if (_data.railClosed)
		{
			EIC eIC2 = subItems[0];
			AnchorPointInfo anchorPointInfo2 = new AnchorPointInfo(eIC2.data.position.ToVector3(), (eIC2.data as ConstraintPointData).anchorIndex, AnchorType.RailPoint);
			anchorPointInfo2.anchorIndex = (eIC2.data as ConstraintPointData).anchorIndex;
			anchorPointInfo2.entryEasingType = (eIC2.data as ConstraintPointData).entryEasingType;
			anchorPointInfo2.exitEasingType = (eIC2.data as ConstraintPointData).exitEasingType;
			anchorPointInfo2.interpolationType = (eIC2.data as ConstraintPointData).interpolationType;
			anchorPointInfo2.velocityMultipler = (eIC2.data as ConstraintPointData).velocityMultipler;
			anchorPointInfo2.waitAtPoint = (eIC2.data as ConstraintPointData).waitAtPoint;
			list.Add(anchorPointInfo2);
			num++;
		}
		if (_data.railInterpolationStyle == 1)
		{
			lineRenderer.SetVertexCount(num * 2);
			for (int j = 0; j < list.Count; j++)
			{
				lineRenderer.SetPosition(j * 2, list[j].position);
				Vector3 position = list[j].position;
				if (j > 0)
				{
					position = list[j].position + (list[j].position - list[j - 1].position).normalized * 0.01f;
				}
				lineRenderer.SetPosition(j * 2 + 1, position);
				if (j < list.Count - 1)
				{
					list[j].length = (list[j + 1].position - list[j].position).magnitude;
				}
				else
				{
					list[j].length = (list[0].position - list[j].position).magnitude;
				}
			}
		}
		else
		{
			int num2 = 10;
			lineRenderer.SetVertexCount((list.Count - 1) * num2 - (list.Count - 2));
			int num3 = 0;
			Vector2 zero = Vector2.zero;
			Vector2 zero2 = Vector2.zero;
			Vector2 zero3 = Vector2.zero;
			Vector2 zero4 = Vector2.zero;
			for (int k = 0; k < list.Count - 1; k++)
			{
				if (k == 0)
				{
					zero2 = list[k].position;
					zero3 = list[k + 1].position;
					zero4 = ((k + 2 < list.Count) ? ((Vector2)list[k + 2].position) : (zero3 + (zero3 - zero2)));
					zero = ((!_data.railClosed) ? (zero2 - (zero3 - zero2)) : ((Vector2)list[list.Count - 2].position));
				}
				else if (k == list.Count - 2)
				{
					zero = list[k - 1].position;
					zero2 = list[k].position;
					zero3 = list[k + 1].position;
					zero4 = ((!_data.railClosed) ? (zero3 + (zero3 - zero2)) : ((Vector2)list[1].position));
				}
				else
				{
					zero = list[k - 1].position;
					zero2 = list[k].position;
					zero3 = list[k + 1].position;
					zero4 = ((k + 2 < list.Count) ? ((Vector2)list[k + 2].position) : ((!_data.railClosed) ? (zero3 + (zero3 - zero2)) : ((Vector2)list[1].position)));
				}
				float num4 = 0f;
				Vector3 vector = zero2;
				for (int l = 0; l < num2; l++)
				{
					float t = (float)l / (float)(num2 - 1);
					Vector3 vector2 = ToolBox.PointOnSplineSegment(zero, zero2, zero3, zero4, t);
					num4 += (vector2 - vector).magnitude;
					vector = vector2;
					vector2.z = 100f;
					if (l > 0 || k == 0)
					{
						lineRenderer.SetPosition(num3, vector2);
						num3++;
					}
				}
				list[k].length = num4;
			}
		}
		if (_data.railClosed)
		{
			list.RemoveAt(list.Count - 1);
		}
		GEConstraintC gEConstraintC = GES.AddConstraintComponent(_data, transformC, list.ToArray());
		gEConstraintC.constraintType = ConstraintType.Rail;
		gEConstraintC.railInterpolationStyle = _data.railInterpolationStyle;
		gEConstraintC.railClosed = _data.railClosed;
		return gEConstraintC;
	}

	public static List<EIC> CreateNewEditorItem(EIC _container, string _identifier, Vector3 _pos, Vector3 _rot, Vector3 _sca)
	{
		List<EIC> list = new List<EIC>();
		uint uniqueId = GES.GetUniqueId();
		ConstraintData constraintData = new ConstraintData();
		constraintData.position = new Vertex3(_pos + Vector3.forward * 80f);
		constraintData.rotation = new Vertex3(_rot);
		constraintData.scale = new Vertex3(_sca);
		constraintData.constraintType = 4u;
		constraintData.railInterpolationStyle = 1;
		constraintData.railClosed = false;
		constraintData.Init(uniqueId, _identifier + uniqueId);
		EIC eIC = GEContainerA.Assemble(_container, _identifier, constraintData);
		eIC.camera = Main.camera;
		eIC.isRealtimeMovable = true;
		list.Add(eIC);
		ConstraintPointData constraintPointData = new ConstraintPointData(AnchorType.RailPoint);
		constraintPointData.position = new Vertex3(_pos + Vector3.up * -25f + Vector3.forward * 80f);
		constraintPointData.rotation = new Vertex3(_rot);
		constraintPointData.scale = new Vertex3(_sca);
		constraintPointData.anchorIndex = 0;
		constraintPointData.entryEasingType = 0;
		constraintPointData.exitEasingType = 0;
		constraintPointData.interpolationType = 0;
		constraintPointData.velocityMultipler = 1f;
		constraintPointData.waitAtPoint = 0f;
		constraintPointData.Init(uniqueId, _identifier + "Point" + uniqueId);
		EIC eIC2 = GEItemA.Assemble(eIC, "RailPoint", constraintPointData, Main.camera);
		eIC2.camera = Main.camera;
		eIC2.isRealtimeMovable = true;
		constraintPointData = new ConstraintPointData(AnchorType.RailPoint);
		constraintPointData.position = new Vertex3(_pos + Vector3.up * 25f + Vector3.forward * 80f);
		constraintPointData.rotation = new Vertex3(_rot);
		constraintPointData.scale = new Vertex3(_sca);
		constraintPointData.anchorIndex = 1;
		constraintPointData.entryEasingType = 0;
		constraintPointData.exitEasingType = 0;
		constraintPointData.interpolationType = 0;
		constraintPointData.velocityMultipler = 1f;
		constraintPointData.waitAtPoint = 0f;
		constraintPointData.Init(uniqueId, _identifier + "Point" + uniqueId);
		eIC2 = GEItemA.Assemble(eIC, "RailPoint", constraintPointData, Main.camera);
		eIC2.camera = Main.camera;
		eIC2.isRealtimeMovable = true;
		eIC2 = GERailMotorA.CreateNewEditorItem(eIC, "Rail Motor", _pos + Vector3.up * -25f, Vector3.zero, Vector3.one);
		return list;
	}

	public static EIC CreateLoadedEditorItem(EIC _container, EIC _loadedItem)
	{
		EIC eIC = GEContainerA.Assemble(_container, _loadedItem.identifier, _loadedItem.data);
		eIC.camera = Main.camera;
		eIC.isRealtimeMovable = true;
		return eIC;
	}

	public static EIC CreateLoadedRailPointEditorItem(EIC _container, EIC _loadedItem)
	{
		EIC eIC = GEItemA.Assemble(_container, _loadedItem.identifier, _loadedItem.data, Main.camera);
		eIC.camera = Main.camera;
		eIC.isRealtimeMovable = true;
		return eIC;
	}

	public static void FillEditorItem(EIC _eic)
	{
		ConstraintData data = _eic.data as ConstraintData;
		GEConstraintC gEConstraintC = Assemble(_eic, data);
		if (gEConstraintC != null)
		{
			_eic.gameComponents.Add(gEConstraintC);
		}
	}

	public static void PopulatePropertyBar(EIC _eiC, UIC _propertyBar)
	{
		string[] tags = new string[1] { "propertyBar" };
		Camera canvasCamera = _propertyBar.canvasCamera;
		ConstraintData constraintData = _eiC.data as ConstraintData;
		UIC component = CheckBoxA.Assemble(canvasCamera, "Closed", HandleConstraintPropertyChange, null, true, Align.Right, 1f, constraintData.railClosed, tags);
		UIC component2 = CheckBoxA.Assemble(canvasCamera, "Smooth", HandleConstraintPropertyChange, null, true, Align.Right, 1f, constraintData.railInterpolationStyle == 0, tags);
		UIS.AddToCanvasGrid(component, _propertyBar, true);
		UIS.AddToCanvasGrid(component2, _propertyBar, true);
		UIS.MoveCursor(_propertyBar, 0f, -15f);
	}

	public static void HandleConstraintPropertyChange(EventC _c)
	{
		ConstraintData constraintData = null;
		if (EditorState.m_selection[0].data.dataType == 3)
		{
			constraintData = EditorState.m_selection[0].data as ConstraintData;
		}
		else if (EditorState.m_selection[0].data.dataType == 6)
		{
			return;
		}
		switch (_c.identifier)
		{
		case "Closed":
			if ((bool)_c.properties["checked"])
			{
				constraintData.railClosed = true;
			}
			else
			{
				constraintData.railClosed = false;
			}
			break;
		case "Smooth":
			if ((bool)_c.properties["checked"])
			{
				constraintData.railInterpolationStyle = 0;
			}
			else
			{
				constraintData.railInterpolationStyle = 1;
			}
			break;
		}
		EditorState.ResetEditorItem(EditorState.m_selection[0]);
	}

	public static void PopulatePointPropertyBar(EIC _eiC, UIC _propertyBar)
	{
		string[] tags = new string[1] { "propertyBar" };
		Camera canvasCamera = _propertyBar.canvasCamera;
		ConstraintPointData constraintPointData = _eiC.data as ConstraintPointData;
		UIC component = NumericFieldA.Assemble(canvasCamera, "Wait At Point", HandleConstraintPointPropertyChange, null, true, Align.Right, 60f, 1f, false, 0f, 100f, constraintPointData.waitAtPoint, tags);
		UIS.AddToCanvasGrid(component, _propertyBar, true);
		UIS.MoveCursor(_propertyBar, 0f, -15f);
	}

	public static void HandleConstraintPointPropertyChange(EventC _c)
	{
		ConstraintPointData constraintPointData = null;
		if (EditorState.m_selection[0].data.dataType == 6)
		{
			constraintPointData = EditorState.m_selection[0].data as ConstraintPointData;
		}
		else if (EditorState.m_selection[0].data.dataType == 3)
		{
			return;
		}
		switch (_c.identifier)
		{
		case "Wait At Point":
			constraintPointData.waitAtPoint = (float)_c.properties["value"];
			break;
		}
		EditorState.ResetEditorItem(EditorState.m_selection[0].container);
	}
}
