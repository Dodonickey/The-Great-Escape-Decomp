using UnityEngine;

public static class GEOutlinerTabA
{
	public static UIC Assemble(UIC _parent, string _label, bool _expanded)
	{
		string[] tags = new string[1] { "OutlinerTab" };
		UIC uIC = CanvasA.Assemble(Main.uiCamera, -1, _label, CanvasA.HandleTouches, tags);
		UIS.AddToCanvasGrid(uIC, _parent, true);
		if (_expanded)
		{
			UIS.SetCanvasRelativeSize(uIC, 1f, 0.87f, 0.065f, 0f);
		}
		else
		{
			UIS.SetCanvasRelativeSize(uIC, 1f, 0.065f, 0.065f, 0f);
		}
		UIS.SetCanvasSeparateRenderSpace(uIC);
		UIS.SetCanvasAbsoluteMarginAndSpacing(uIC, 0f, 0f);
		UIS.SetCanvasAlign(uIC, Align.Left, Align.Top);
		UIS.SetCanvasExpandable(uIC, true, _expanded);
		DrawCanvas(uIC, uIC.parent.canvasCamera);
		return uIC;
	}

	public static void DrawCanvas(UIC _uic, Camera _camera)
	{
		float width = _uic.width;
		float height = _uic.height;
		float headerHeight = _uic.headerHeight;
		float footerHeight = _uic.footerHeight;
		Camera camera = _camera;
		if (_uic.parent != null && _uic.parent.separateRenderSpace)
		{
			camera = _uic.parent.canvasCamera;
		}
		if (_uic.expanded)
		{
			Vector2[] roundedRect = DebugDraw.GetRoundedRect(width, height, 8f, 8, Vector2.zero, false);
			Vector2[] rect = DebugDraw.GetRect(width, headerHeight, Vector2.up * (height * 0.5f - headerHeight * 0.5f));
			Vector2[] rect2 = DebugDraw.GetRect(width, footerHeight, Vector2.up * ((0f - height) * 0.5f + footerHeight * 0.5f));
			Polygon polygon = DebugDraw.Vector2ArrayToPolygon(roundedRect);
			Polygon polygon2 = null;
			Polygon polygon3 = null;
			if (_uic.headerHeight > 0f)
			{
				polygon2 = DebugDraw.Vector2ArrayToPolygon(rect);
			}
			if (_uic.footerHeight > 0f)
			{
				if (polygon2 != null)
				{
					polygon2.AddContour(new VertexList(rect2), false);
				}
				else
				{
					polygon2 = DebugDraw.Vector2ArrayToPolygon(rect2);
				}
			}
			PrefabS.CreatePathPrefabComponentFromPolygon(_uic.TC, Vector3.forward * -5f, polygon, 6f, DebugDraw.GetColor(220f, 220f, 220f), ResourceManager.GetMaterial("Line4"), camera, Position.Center, true);
			if (polygon2 != null)
			{
				polygon2 = GpcWrapper.Clip(GpcOperation.Intersection, polygon, polygon2);
				polygon3 = GpcWrapper.Clip(GpcOperation.Difference, polygon, polygon2);
				PrefabS.CreateFlatPrefabComponentsFromPolygon(_uic.TC, Vector3.forward * 0f, polygon3, DebugDraw.GetColor(250f, 250f, 250f), ResourceManager.GetMaterial("Solid"), camera);
				PrefabS.CreateFlatPrefabComponentsFromPolygon(_uic.TC, Vector3.forward * 0f, polygon2, DebugDraw.GetColor(220f, 220f, 220f), ResourceManager.GetMaterial("Solid"), camera);
			}
			else
			{
				PrefabS.CreateFlatPrefabComponentsFromPolygon(_uic.TC, Vector3.forward * 0f, polygon, DebugDraw.GetColor(250f, 250f, 250f), ResourceManager.GetMaterial("Solid"), camera);
			}
		}
		else
		{
			Vector2[] roundedRect2 = DebugDraw.GetRoundedRect(width, headerHeight, 8f, 8, Vector2.zero, false);
			PrefabS.CreatePathPrefabComponentFromVectorArray(_uic.TC, Vector3.up * (height * 0.5f - headerHeight * 0.5f) + Vector3.forward * -5f, roundedRect2, 6f, DebugDraw.GetColor(220f, 220f, 220f), ResourceManager.GetMaterial("Line4"), camera, Position.Center, true);
			PrefabS.CreateFlatPrefabComponentsFromVectorArray(_uic.TC, Vector3.up * (height * 0.5f - headerHeight * 0.5f) + Vector3.forward * 0f, roundedRect2, PrefabS.ColorToUInt(DebugDraw.GetColor(220f, 220f, 220f)), PrefabS.ColorToUInt(DebugDraw.GetColor(220f, 220f, 220f)), ResourceManager.GetMaterial("Solid"), camera, string.Empty);
		}
		Vector2[] roundedRect3 = DebugDraw.GetRoundedRect(width - 8f, headerHeight - 8f, 5f, 8, Vector2.zero, false);
		PrefabS.CreatePathPrefabComponentFromVectorArray(_uic.TC, Vector3.up * (height * 0.5f - headerHeight * 0.5f) + Vector3.forward * -10f, roundedRect3, 6f, DebugDraw.GetColor(0f, 0f, 0f), ResourceManager.GetMaterial("Line4"), camera, Position.Center, true);
		PrefabS.CreateFlatPrefabComponentsFromVectorArray(_uic.TC, Vector3.up * (height * 0.5f - headerHeight * 0.5f) + Vector3.forward * -5f, roundedRect3, PrefabS.ColorToUInt(DebugDraw.GetColor(0f, 0f, 0f)), PrefabS.ColorToUInt(DebugDraw.GetColor(50f, 50f, 50f)), ResourceManager.GetMaterial("Solid"), camera, string.Empty);
		TransformC transformC = TransformS.AddComponent(_uic.TC.entityIndex);
		TransformS.ParentComponent(transformC, _uic.TC, Vector3.up * (height * 0.5f - headerHeight * 0.5f));
		TextS.SetStyle("subheader");
		_uic.textC = TextS.AddSingleLineComponent(transformC, _uic.label, 1f, Align.Left, Align.Middle);
		SpriteS.SetColorByTransformComponent(_uic.textC.contentTC, DebugDraw.GetColor(255f, 255f, 255f), false, false);
		TransformS.Move(_uic.textC.contentTC, Vector3.right * (width * -0.5f + headerHeight * 0.5f) + Vector3.forward * -10f);
		SpriteS.ConvertSpritesToPrefabComponent(_uic.textC.TC, camera, true);
		TouchAreaC touchAreaComponent = TouchAreaS.AddComponent(transformC, "expand", width, headerHeight, true, Main.uiCamera, _uic);
		TouchAreaS.AddTouchEventListener(touchAreaComponent, HandleAccordionTabTouches);
	}

	public static void HandleAccordionTabTouches(TouchAreaC _c, int _i, bool _consumed)
	{
		UIC uIC = _c.customComponent as UIC;
		if (_c.touchEvent[_i] == TouchEvent.Release && _c.touchStartedInside[_i] && !uIC.expanded)
		{
			Maximize(uIC);
			if (uIC.label == "Item Properties" && EditorState.m_selection.Count > 0)
			{
				EditorState.UpdatePropertyBar(EditorState.m_selection[0]);
			}
		}
	}

	public static void Minimize(UIC _item)
	{
		UIS.SetCanvasExpandable(_item, true, false);
		UIS.SetActivityOfChildComponents(_item, false);
		UIS.SetCanvasRelativeSize(_item, 1f, 0.065f, 0.065f, 0f);
		PrefabS.RemoveComponentsByEntityIndex(_item.entityIndex);
		TouchAreaS.RemoveComponentsByTransformComponent(_item.TC);
		for (int i = 0; i < _item.TC.childs.Count; i++)
		{
			TouchAreaS.RemoveComponentsByTransformComponent(_item.TC.childs[i]);
		}
		TextS.RemoveComponent(_item.textC);
		DrawCanvas(_item, Main.uiCamera);
	}

	public static void Maximize(UIC _item)
	{
		UIS.SetCanvasExpandable(_item, true, true);
		UIS.SetActivityOfChildComponents(_item, true);
		UIS.SetCanvasRelativeSize(_item, 1f, 0.87f, 0.065f, 0f);
		PrefabS.RemoveComponentsByEntityIndex(_item.entityIndex);
		TouchAreaS.RemoveComponentsByTransformComponent(_item.TC);
		for (int i = 0; i < _item.TC.childs.Count; i++)
		{
			TouchAreaS.RemoveComponentsByTransformComponent(_item.TC.childs[i]);
		}
		TextS.RemoveComponent(_item.textC);
		DrawCanvas(_item, Main.uiCamera);
		if (_item != GEOutlinerA.m_level)
		{
			Minimize(GEOutlinerA.m_level);
		}
		if (_item != GEOutlinerA.m_library)
		{
			Minimize(GEOutlinerA.m_library);
		}
		if (_item != GEOutlinerA.m_properties)
		{
			Minimize(GEOutlinerA.m_properties);
		}
		UIS.PlaceCanvasContent(_item.parent);
		UIS.ResetCursor(_item);
		UIS.PlaceCanvasContent(_item);
	}
}
