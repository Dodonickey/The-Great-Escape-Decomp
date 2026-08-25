using UnityEngine;

public static class GEMenuListA
{
	public static int m_openedMenuId;

	public static UIC m_openedMenu;

	public static UIC Assemble(UIC _parent)
	{
		string[] tags = new string[1] { "MenuList" };
		UIC uIC = CanvasA.Assemble(Main.uiCamera, -1, string.Empty, null, tags);
		uIC.headerHeight = 0f;
		uIC.footerHeight = 0f;
		UIS.AddToCanvasGrid(uIC, _parent, true);
		UIS.SetRelativeSize(uIC, 0.25f, 0.39f);
		UIS.SetCanvasRelativeMarginAndSpacing(uIC, 0f, 0f);
		UIS.SetCanvasAlign(uIC, Align.Left, Align.Top);
		UIC uIC2 = ButtonA.Assemble(Main.uiCamera, 5001, "New", HandleTouches, tags);
		UIC uIC3 = ButtonA.Assemble(Main.uiCamera, 5002, "Open", HandleTouches, tags);
		UIC uIC4 = ButtonA.Assemble(Main.uiCamera, 5003, "Append", HandleTouches, tags);
		UIC uIC5 = ButtonA.Assemble(Main.uiCamera, 5004, "Save", HandleTouches, tags);
		UIC uIC6 = ButtonA.Assemble(Main.uiCamera, 5005, "Save As", HandleTouches, tags);
		UIC uIC7 = ButtonA.Assemble(Main.uiCamera, 5006, "Upload", HandleTouches, tags);
		UIC uIC8 = ButtonA.Assemble(Main.uiCamera, 5007, "Download", HandleTouches, tags);
		UIC uIC9 = ButtonA.Assemble(Main.uiCamera, 5006, "Close", HandleTouches, tags);
		UIC uIC10 = ButtonA.Assemble(Main.uiCamera, 5007, "Close All", HandleTouches, tags);
		UIS.AddToCanvasGrid(uIC2, uIC, true);
		UIS.AddToCanvasGrid(uIC3, uIC, true);
		UIS.AddToCanvasGrid(uIC4, uIC, true);
		UIS.AddToCanvasGrid(uIC5, uIC, true);
		UIS.AddToCanvasGrid(uIC6, uIC, true);
		UIS.AddToCanvasGrid(uIC7, uIC, true);
		UIS.AddToCanvasGrid(uIC8, uIC, true);
		UIS.AddToCanvasGrid(uIC9, uIC, true);
		UIS.AddToCanvasGrid(uIC10, uIC, true);
		float num = 1f / 9f;
		UIS.SetRelativeSize(uIC2, 1f, num);
		UIS.SetRelativeSize(uIC3, 1f, num);
		UIS.SetRelativeSize(uIC4, 1f, num);
		UIS.SetRelativeSize(uIC5, 1f, num);
		UIS.SetRelativeSize(uIC6, 1f, num);
		UIS.SetRelativeSize(uIC7, 1f, num);
		UIS.SetRelativeSize(uIC8, 1f, num);
		UIS.SetRelativeSize(uIC9, 1f, num);
		UIS.SetRelativeSize(uIC10, 1f, num);
		UIS.PlaceCanvasContent(uIC);
		Vector2[] roundedRect = DebugDraw.GetRoundedRect(uIC.width, uIC.height, 25f, 8, Vector2.zero, false);
		Polygon polygon = DebugDraw.Vector2ArrayToPolygon(roundedRect);
		Vector2[] line = DebugDraw.GetLine(Vector2.right * uIC.width * -0.5f, Vector2.right * uIC.width * 0.5f, 0);
		uIC.outlinePCs.AddRange(PrefabS.CreatePathPrefabComponentFromPolygon(uIC.TC, Vector3.forward * 0f, polygon, 6f, DebugDraw.GetColor(200f, 200f, 200f), ResourceManager.GetMaterial("Line6"), Main.uiCamera, Position.Center, true));
		uIC.backgroundPCs.AddRange(PrefabS.CreateFlatPrefabComponentsFromPolygon(uIC.TC, Vector3.forward * 10f, polygon, DebugDraw.GetColor(230f, 230f, 230f), ResourceManager.GetMaterial("Solid"), Main.uiCamera));
		float num2 = uIC.height * num * 0.5f;
		TextS.SetStyle("subheader");
		TextC textC = TextS.AddSingleLineComponent(uIC2.contentTC, uIC2.label, 1f, Align.Center, Align.Middle);
		SpriteS.SetColorByTransformComponent(textC.contentTC, DebugDraw.GetColor(0f, 0f, 0f), false, false);
		SpriteS.ConvertSpritesToPrefabComponent(textC.contentTC, uIC.canvasCamera, true);
		PrefabS.CreateLinePrefabComponentFromVectorArray(uIC2.TC, Vector3.up * (0f - num2), line, 1f, DebugDraw.GetColor(200f, 200f, 200f), ResourceManager.GetMaterial("Solid"), Main.uiCamera, Position.Center);
		textC = TextS.AddSingleLineComponent(uIC3.contentTC, uIC3.label, 1f, Align.Center, Align.Middle);
		SpriteS.SetColorByTransformComponent(textC.contentTC, DebugDraw.GetColor(0f, 0f, 0f), false, false);
		SpriteS.ConvertSpritesToPrefabComponent(textC.contentTC, uIC.canvasCamera, true);
		PrefabS.CreateLinePrefabComponentFromVectorArray(uIC3.TC, Vector3.up * (0f - num2), line, 1f, DebugDraw.GetColor(200f, 200f, 200f), ResourceManager.GetMaterial("Solid"), Main.uiCamera, Position.Center);
		textC = TextS.AddSingleLineComponent(uIC4.contentTC, uIC4.label, 1f, Align.Center, Align.Middle);
		SpriteS.SetColorByTransformComponent(textC.contentTC, DebugDraw.GetColor(0f, 0f, 0f), false, false);
		SpriteS.ConvertSpritesToPrefabComponent(textC.contentTC, uIC.canvasCamera, true);
		PrefabS.CreateLinePrefabComponentFromVectorArray(uIC4.TC, Vector3.up * (0f - num2), line, 1f, DebugDraw.GetColor(200f, 200f, 200f), ResourceManager.GetMaterial("Solid"), Main.uiCamera, Position.Center);
		textC = TextS.AddSingleLineComponent(uIC5.contentTC, uIC5.label, 1f, Align.Center, Align.Middle);
		SpriteS.SetColorByTransformComponent(textC.contentTC, DebugDraw.GetColor(0f, 0f, 0f), false, false);
		SpriteS.ConvertSpritesToPrefabComponent(textC.contentTC, uIC.canvasCamera, true);
		PrefabS.CreateLinePrefabComponentFromVectorArray(uIC5.TC, Vector3.up * (0f - num2), line, 1f, DebugDraw.GetColor(200f, 200f, 200f), ResourceManager.GetMaterial("Solid"), Main.uiCamera, Position.Center);
		textC = TextS.AddSingleLineComponent(uIC6.contentTC, uIC6.label, 1f, Align.Center, Align.Middle);
		SpriteS.SetColorByTransformComponent(textC.contentTC, DebugDraw.GetColor(0f, 0f, 0f), false, false);
		SpriteS.ConvertSpritesToPrefabComponent(textC.contentTC, uIC.canvasCamera, true);
		PrefabS.CreateLinePrefabComponentFromVectorArray(uIC6.TC, Vector3.up * (0f - num2), line, 1f, DebugDraw.GetColor(200f, 200f, 200f), ResourceManager.GetMaterial("Solid"), Main.uiCamera, Position.Center);
		textC = TextS.AddSingleLineComponent(uIC7.contentTC, uIC7.label, 1f, Align.Center, Align.Middle);
		SpriteS.SetColorByTransformComponent(textC.contentTC, DebugDraw.GetColor(0f, 0f, 0f), false, false);
		SpriteS.ConvertSpritesToPrefabComponent(textC.contentTC, uIC.canvasCamera, true);
		PrefabS.CreateLinePrefabComponentFromVectorArray(uIC7.TC, Vector3.up * (0f - num2), line, 1f, DebugDraw.GetColor(200f, 200f, 200f), ResourceManager.GetMaterial("Solid"), Main.uiCamera, Position.Center);
		textC = TextS.AddSingleLineComponent(uIC8.contentTC, uIC8.label, 1f, Align.Center, Align.Middle);
		SpriteS.SetColorByTransformComponent(textC.contentTC, DebugDraw.GetColor(0f, 0f, 0f), false, false);
		SpriteS.ConvertSpritesToPrefabComponent(textC.contentTC, uIC.canvasCamera, true);
		PrefabS.CreateLinePrefabComponentFromVectorArray(uIC8.TC, Vector3.up * (0f - num2), line, 1f, DebugDraw.GetColor(200f, 200f, 200f), ResourceManager.GetMaterial("Solid"), Main.uiCamera, Position.Center);
		textC = TextS.AddSingleLineComponent(uIC9.contentTC, uIC9.label, 1f, Align.Center, Align.Middle);
		SpriteS.SetColorByTransformComponent(textC.contentTC, DebugDraw.GetColor(0f, 0f, 0f), false, false);
		SpriteS.ConvertSpritesToPrefabComponent(textC.contentTC, uIC.canvasCamera, true);
		PrefabS.CreateLinePrefabComponentFromVectorArray(uIC9.TC, Vector3.up * (0f - num2), line, 1f, DebugDraw.GetColor(200f, 200f, 200f), ResourceManager.GetMaterial("Solid"), Main.uiCamera, Position.Center);
		textC = TextS.AddSingleLineComponent(uIC10.contentTC, uIC10.label, 1f, Align.Center, Align.Middle);
		SpriteS.SetColorByTransformComponent(textC.contentTC, DebugDraw.GetColor(0f, 0f, 0f), false, false);
		SpriteS.ConvertSpritesToPrefabComponent(textC.contentTC, uIC.canvasCamera, true);
		return uIC;
	}

	public static void HandleTouches(TouchAreaC _c, int _i, bool _consumed)
	{
		UIC uIC = _c.customComponent as UIC;
		TouchEvent touchEvent = _c.touchEvent[_i];
		switch (touchEvent)
		{
		case TouchEvent.Began:
		case TouchEvent.RollIn:
		{
			if (_c.identifier == "background")
			{
				if (!_consumed)
				{
					UIS.RemoveFromCanvasGrid(GEMenuAreaA.m_menuList);
					EntityManager.RemoveEntitiesByTransformComponentHierarchy(GEMenuAreaA.m_menuList.TC, false);
					GEMenuAreaA.m_menuList = null;
					GEMenuAreaA.m_isMenuOpen = false;
				}
				break;
			}
			Vector2[] rect = DebugDraw.GetRect(uIC.width, uIC.height, uIC.TC.transform.localPosition, false);
			Vector2[] roundedRect2 = DebugDraw.GetRoundedRect(GEMenuAreaA.m_menuList.width, GEMenuAreaA.m_menuList.height, 30f, 8, Vector2.zero, false);
			Polygon polygon2 = DebugDraw.Vector2ArrayToPolygon(rect);
			Polygon polygon3 = DebugDraw.Vector2ArrayToPolygon(roundedRect2);
			polygon2 = polygon2.Clip(GpcOperation.Intersection, polygon3);
			while (uIC.backgroundPCs.Count > 0)
			{
				int index2 = uIC.backgroundPCs.Count - 1;
				PrefabS.RemoveComponent(uIC.backgroundPCs[index2]);
				uIC.backgroundPCs.RemoveAt(index2);
			}
			uIC.backgroundPCs.AddRange(PrefabS.CreateFlatPrefabComponentsFromPolygon(uIC.TC, Vector3.forward * 10f - uIC.TC.transform.localPosition, polygon2, DebugDraw.GetColor(255f, 255f, 255f), ResourceManager.GetMaterial("Solid"), Main.uiCamera));
			break;
		}
		case TouchEvent.RollOut:
		{
			Vector2[] roundedRect = DebugDraw.GetRoundedRect(uIC.width, uIC.height, 25f, 8, Vector2.zero, false);
			Polygon polygon = DebugDraw.Vector2ArrayToPolygon(roundedRect);
			while (uIC.backgroundPCs.Count > 0)
			{
				int index = uIC.backgroundPCs.Count - 1;
				PrefabS.RemoveComponent(uIC.backgroundPCs[index]);
				uIC.backgroundPCs.RemoveAt(index);
			}
			break;
		}
		}
		if (touchEvent != TouchEvent.Release || !(_c.identifier != "background"))
		{
			return;
		}
		int openedMenuId = m_openedMenuId;
		if (m_openedMenuId > 5000 && m_openedMenuId != uIC.identifier)
		{
			UIC uIComponentByIdentifier = UIS.GetUIComponentByIdentifier(m_openedMenuId);
			while (uIComponentByIdentifier.backgroundPCs.Count > 0)
			{
				int index3 = uIComponentByIdentifier.backgroundPCs.Count - 1;
				PrefabS.RemoveComponent(uIComponentByIdentifier.backgroundPCs[index3]);
				uIComponentByIdentifier.backgroundPCs.RemoveAt(index3);
			}
		}
		if (m_openedMenuId != uIC.identifier && m_openedMenu != null)
		{
			UIS.RemoveFromCanvasGrid(m_openedMenu);
			EntityManager.RemoveEntitiesByTransformComponentHierarchy(m_openedMenu.TC, false);
			m_openedMenu = null;
			m_openedMenuId = 0;
		}
		if (openedMenuId == uIC.identifier)
		{
			return;
		}
		m_openedMenuId = uIC.identifier;
		if (_c.identifier == "New")
		{
			LevelManager.CreateNewLevel();
			GEMenuAreaA.CloseMenu();
			EditorState.ResetOutliner();
			GES.m_uniqueId = 0u;
		}
		else if (_c.identifier == "Open")
		{
			m_openedMenu = GEOpenDialogA.Assemble(EditorState.m_menuArea);
		}
		else if (_c.identifier == "Append")
		{
			m_openedMenu = GEAppendDialogA.Assemble(EditorState.m_menuArea);
		}
		else if (_c.identifier == "Save")
		{
			if (LevelManager.m_currentLevel.name != "MyLevel")
			{
				LevelManager.SaveLevelData(LevelManager.m_currentLevel, LevelManager.m_currentLevel.name);
				GEMenuAreaA.CloseMenu();
			}
			else
			{
				m_openedMenu = GESaveDialogA.Assemble(EditorState.m_menuArea);
			}
		}
		else if (_c.identifier == "Save As")
		{
			m_openedMenu = GESaveDialogA.Assemble(EditorState.m_menuArea);
		}
		else if (_c.identifier == "Close")
		{
			LevelManager.RemoveLevel(LevelManager.m_currentLevel);
			GEMenuAreaA.CloseMenu();
		}
		else if (_c.identifier == "Close All")
		{
			LevelManager.RemoveLevels();
			GEMenuAreaA.CloseMenu();
		}
		else if (_c.identifier == "Upload")
		{
			m_openedMenu = GEUploadDialogA.Assemble(EditorState.m_menuArea);
		}
		else if (_c.identifier == "Download")
		{
			m_openedMenu = GEDownloadDialogA.Assemble(EditorState.m_menuArea);
		}
		if (m_openedMenu != null)
		{
			UIS.PlaceCanvasContent(EditorState.m_menuArea);
			UIS.ResetClipsForTouchAreasInSeparateRenderSpaces(m_openedMenu);
		}
	}
}
