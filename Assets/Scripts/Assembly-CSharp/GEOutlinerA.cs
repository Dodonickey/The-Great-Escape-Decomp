using UnityEngine;

public static class GEOutlinerA
{
	public static UIC m_level;

	public static UIC m_library;

	public static UIC m_properties;

	public static UIC Assemble(UIC _parent, bool _expanded)
	{
		string[] tags = new string[1] { "Outliner" };
		UIC uIC = CanvasA.Assemble(Main.uiCamera, -1, "Outliner", CanvasA.HandleTouches, tags);
		UIS.AddToCanvas(uIC, _parent, Vector3.zero);
		UIS.SetCanvasRelativeSize(uIC, 0.2f, 1f, 0.065f, 0.025f);
		UIS.SetRelativePosition(uIC, new Vector2(1f, 1f), 0);
		UIS.SetCanvasAbsoluteMarginAndSpacing(uIC, 0f, 0f);
		UIS.SetCanvasAlign(uIC, Align.Left, Align.Top);
		UIS.SetCanvasExpandable(uIC, true, _expanded);
		DrawCanvas(uIC, Main.uiCamera);
		m_level = GEOutlinerTabA.Assemble(uIC, "Current Level", false);
		m_library = GEOutlinerTabA.Assemble(uIC, "Item Library", true);
		m_properties = GEOutlinerTabA.Assemble(uIC, "Item Properties", false);
		EditorState.FillItemBar();
		UIS.PlaceCanvasContent(uIC);
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
			Polygon polygon = DebugDraw.Vector2ArrayToPolygon(roundedRect);
			PrefabS.CreatePathPrefabComponentFromPolygon(_uic.TC, Vector3.forward * -5f, polygon, 6f, DebugDraw.GetColor(220f, 220f, 220f), ResourceManager.GetMaterial("Line4"), camera, Position.Center, true);
			PrefabS.CreateFlatPrefabComponentsFromPolygon(_uic.TC, Vector3.forward * 0f, polygon, DebugDraw.GetColor(220f, 220f, 220f), ResourceManager.GetMaterial("Solid"), camera);
		}
		else
		{
			Vector2[] roundedRect2 = DebugDraw.GetRoundedRect(width, headerHeight, 8f, 8, Vector2.zero, false);
			PrefabS.CreatePathPrefabComponentFromVectorArray(_uic.TC, Vector3.up * (height * 0.5f - headerHeight * 0.5f) + Vector3.forward * -5f, roundedRect2, 6f, DebugDraw.GetColor(220f, 220f, 220f), ResourceManager.GetMaterial("Line4"), camera, Position.Center, true);
			PrefabS.CreateFlatPrefabComponentsFromVectorArray(_uic.TC, Vector3.up * (height * 0.5f - headerHeight * 0.5f) + Vector3.forward * 0f, roundedRect2, PrefabS.ColorToUInt(DebugDraw.GetColor(220f, 220f, 220f)), PrefabS.ColorToUInt(DebugDraw.GetColor(220f, 220f, 220f)), ResourceManager.GetMaterial("Solid"), camera, string.Empty);
		}
		Vector2[] roundedRect3 = DebugDraw.GetRoundedRect(width - 8f, headerHeight - 8f, 5f, 8, Vector2.zero, false);
		PrefabS.CreatePathPrefabComponentFromVectorArray(_uic.TC, Vector3.up * (height * 0.5f - headerHeight * 0.5f) + Vector3.forward * -10f, roundedRect3, 6f, DebugDraw.GetColor(92f, 156f, 50f), ResourceManager.GetMaterial("Line4"), camera, Position.Center, true);
		PrefabS.CreateFlatPrefabComponentsFromVectorArray(_uic.TC, Vector3.up * (height * 0.5f - headerHeight * 0.5f) + Vector3.forward * -5f, roundedRect3, PrefabS.ColorToUInt(DebugDraw.GetColor(92f, 156f, 50f)), PrefabS.ColorToUInt(DebugDraw.GetColor(112f, 176f, 70f)), ResourceManager.GetMaterial("Solid"), camera, string.Empty);
		TransformC transformC = TransformS.AddComponent(_uic.TC.entityIndex);
		TransformS.ParentComponent(transformC, _uic.TC, Vector3.up * (height * 0.5f - headerHeight * 0.5f));
		TextS.SetStyle("header");
		_uic.textC = TextS.AddSingleLineComponent(transformC, _uic.label, 1f, Align.Left, Align.Middle);
		SpriteS.SetColorByTransformComponent(_uic.textC.contentTC, DebugDraw.GetColor(255f, 255f, 255f), false, false);
		TransformS.Move(_uic.textC.contentTC, Vector3.right * (width * -0.5f + headerHeight) + Vector3.forward * -10f);
		SpriteS.ConvertSpritesToPrefabComponent(_uic.textC.TC, camera, true);
		if (_uic.expandable)
		{
			Vector2[] array = new Vector2[3];
			if (_uic.expanded)
			{
				array[0] = Vector2.up * -5f;
				array[1] = Vector2.right * -5f + Vector2.up * 5f;
				array[2] = Vector2.right * 5f + Vector2.up * 5f;
			}
			else
			{
				array[0] = Vector2.right * 5f;
				array[1] = Vector2.right * -5f + Vector2.up * 5f;
				array[2] = Vector2.right * -5f + Vector2.up * -5f;
			}
			TouchAreaC touchAreaComponent = TouchAreaS.AddComponent(transformC, "expand", width, headerHeight, true, Main.uiCamera, _uic);
			TouchAreaS.AddTouchEventListener(touchAreaComponent, HandleTouches);
			PrefabS.CreatePathPrefabComponentFromVectorArray(_uic.TC, Vector3.up * (height * 0.5f - headerHeight * 0.5f) + Vector3.forward * -10f + Vector3.right * (width * -0.5f + headerHeight * 0.5f), array, 8f, DebugDraw.GetColor(255f, 255f, 255f), ResourceManager.GetMaterial("Line4"), camera, Position.Center, true);
			PrefabS.CreateFlatPrefabComponentsFromVectorArray(_uic.TC, Vector3.up * (height * 0.5f - headerHeight * 0.5f) + Vector3.forward * -15f + Vector3.right * (width * -0.5f + headerHeight * 0.5f), array, PrefabS.ColorToUInt(DebugDraw.GetColor(255f, 255f, 255f)), PrefabS.ColorToUInt(DebugDraw.GetColor(255f, 255f, 255f)), ResourceManager.GetMaterial("Solid"), camera, string.Empty);
		}
	}

	private static void HandleTouches(TouchAreaC _c, int _i, bool _consumed)
	{
		UIC uIC = _c.customComponent as UIC;
		if (_c.touchEvent[_i] == TouchEvent.Release && _c.touchStartedInside[_i])
		{
			if (uIC.expanded)
			{
				Minimize();
			}
			else
			{
				Maximize();
			}
		}
	}

	public static void Minimize()
	{
		UIC outliner = EditorState.m_outliner;
		UIS.SetCanvasExpandable(outliner, true, false);
		UIS.SetActivityOfChildComponents(outliner, false);
		UIS.SetCanvasRelativeSize(outliner, 0.2f, 0.065f, 0.065f, 0f);
		UIS.SetRelativePosition(outliner, new Vector2(1f, 1f), 0);
		PrefabS.RemoveComponentsByEntityIndex(outliner.entityIndex);
		TouchAreaS.RemoveComponentsByTransformComponent(outliner.TC);
		for (int i = 0; i < outliner.TC.childs.Count; i++)
		{
			TouchAreaS.RemoveComponentsByTransformComponent(outliner.TC.childs[i]);
			if (outliner.TC.childs[i] != m_level.TC.parent)
			{
				TransformS.RemoveComponent(outliner.TC.childs[i]);
			}
		}
		TextS.RemoveComponent(outliner.textC);
		DrawCanvas(outliner, Main.uiCamera);
		UIS.ResetCursor(outliner);
		UIS.PlaceCanvasContent(outliner);
	}

	public static void Maximize()
	{
		UIC outliner = EditorState.m_outliner;
		UIS.SetCanvasExpandable(outliner, true, true);
		UIS.SetActivityOfChildComponents(outliner, true);
		UIS.SetCanvasRelativeSize(outliner, 0.2f, 1f, 0.065f, 0.025f);
		UIS.SetRelativePosition(outliner, new Vector2(1f, 1f), 0);
		PrefabS.RemoveComponentsByEntityIndex(outliner.entityIndex);
		TouchAreaS.RemoveComponentsByTransformComponent(outliner.TC);
		for (int i = 0; i < outliner.TC.childs.Count; i++)
		{
			TouchAreaS.RemoveComponentsByTransformComponent(outliner.TC.childs[i]);
			if (outliner.TC.childs[i] != m_level.TC.parent)
			{
				TransformS.RemoveComponent(outliner.TC.childs[i]);
			}
		}
		TextS.RemoveComponent(outliner.textC);
		DrawCanvas(outliner, Main.uiCamera);
		UIS.ResetCursor(outliner);
		UIS.PlaceCanvasContent(outliner);
	}
}
