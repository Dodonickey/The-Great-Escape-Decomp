using UnityEngine;

public static class GEMenuAreaA
{
	private static UIC m_menuButton;

	public static bool m_isMenuOpen;

	public static UIC m_menuList;

	public static UIC Assemble(UIC _parent)
	{
		string[] tags = new string[1] { "MenuArea" };
		UIC uIC = CanvasA.Assemble(Main.uiCamera, 0, "MenuArea", null, tags);
		UIS.AddToCanvas(uIC, _parent, Vector3.zero);
		UIS.SetCanvasRelativeSize(uIC, 0.55f, 1f, 0f, 0f);
		UIS.SetRelativePosition(uIC, new Vector2(0f, 1f), 0);
		UIS.SetCanvasRelativeMarginAndSpacing(uIC, 0f, 0.015f);
		UIS.SetCanvasAlign(uIC, Align.Left, Align.Top);
		m_menuButton = ButtonA.Assemble(Main.uiCamera, 0, "Menu", HandleTouches, tags);
		UIC uIC2 = ButtonA.Assemble(Main.uiCamera, 2, "Test", HandleTouches, tags);
		UIS.AddToCanvasGrid(m_menuButton, uIC, false);
		UIS.AddToCanvasGrid(uIC2, uIC, false);
		UIS.SetRelativeSize(m_menuButton, 0.2f, 0.065f);
		UIS.SetRelativeSize(uIC2, 0.175f, 0.065f);
		Vector2[] roundedRect = DebugDraw.GetRoundedRect(m_menuButton.width, m_menuButton.height, 25f, 8, Vector2.zero, false);
		Polygon polygon = DebugDraw.Vector2ArrayToPolygon(roundedRect);
		Vector2[] roundedRect2 = DebugDraw.GetRoundedRect(uIC2.width, uIC2.height, 25f, 8, Vector2.zero, false);
		Polygon polygon2 = DebugDraw.Vector2ArrayToPolygon(roundedRect2);
		m_menuButton.outlinePCs.AddRange(PrefabS.CreatePathPrefabComponentFromPolygon(m_menuButton.TC, Vector3.forward * 0f, polygon, 6f, DebugDraw.GetColor(200f, 200f, 200f), ResourceManager.GetMaterial("Line6"), Main.uiCamera, Position.Center, true));
		m_menuButton.backgroundPCs.AddRange(PrefabS.CreateFlatPrefabComponentsFromPolygon(m_menuButton.TC, Vector3.forward * 10f, polygon, DebugDraw.GetColor(230f, 230f, 230f), ResourceManager.GetMaterial("Solid"), Main.uiCamera));
		uIC2.outlinePCs.AddRange(PrefabS.CreatePathPrefabComponentFromPolygon(uIC2.TC, Vector3.forward * 0f, polygon2, 6f, DebugDraw.GetColor(200f, 200f, 200f), ResourceManager.GetMaterial("Line6"), Main.uiCamera, Position.Center, true));
		uIC2.backgroundPCs.AddRange(PrefabS.CreateFlatPrefabComponentsFromPolygon(uIC2.TC, Vector3.forward * 10f, polygon2, DebugDraw.GetColor(230f, 230f, 230f), ResourceManager.GetMaterial("Solid"), Main.uiCamera));
		TextS.SetStyle("subheader");
		TextC textC = TextS.AddSingleLineComponent(m_menuButton.contentTC, m_menuButton.label, 1f, Align.Left, Align.Middle);
		SpriteS.SetColorByTransformComponent(textC.contentTC, DebugDraw.GetColor(0f, 0f, 0f), false, false);
		TransformS.Move(textC.contentTC, Vector3.right * (m_menuButton.width * -0.5f + 45f));
		SpriteS.ConvertSpritesToPrefabComponent(textC.contentTC, m_menuButton.canvasCamera, true);
		SpriteC spriteC = SpriteS.AddComponent(m_menuButton.TC, new Frame(192f, 192f, 64f, 64f), GEState.editorUISheet);
		SpriteS.SetDimensionScale(spriteC, 0.5f);
		SpriteS.SetOffset(spriteC, Vector3.right * (m_menuButton.width * -0.5f + 25f), 0f);
		textC = TextS.AddSingleLineComponent(uIC2.contentTC, uIC2.label, 1f, Align.Left, Align.Middle);
		SpriteS.SetColorByTransformComponent(textC.contentTC, DebugDraw.GetColor(0f, 0f, 0f), false, false);
		TransformS.Move(textC.contentTC, Vector3.right * (uIC2.width * -0.5f + 45f));
		SpriteS.ConvertSpritesToPrefabComponent(textC.contentTC, m_menuButton.canvasCamera, true);
		SpriteC spriteC2 = SpriteS.AddComponent(uIC2.TC, new Frame(320f, 192f, 64f, 64f), GEState.editorUISheet);
		SpriteS.SetDimensionScale(spriteC2, 0.5f);
		SpriteS.SetOffset(spriteC2, Vector3.right * (uIC2.width * -0.5f + 25f), 0f);
		UIC uIC3 = ButtonA.Assemble(Main.uiCamera, 3, "Undo", HandleTouches, tags);
		UIC uIC4 = ButtonA.Assemble(Main.uiCamera, 4, "Redo", HandleTouches, tags);
		UIS.AddToCanvasGrid(uIC3, uIC, false);
		UIS.AddToCanvasGrid(uIC4, uIC, false);
		UIS.SetRelativeSize(uIC3, 0.065f);
		UIS.SetRelativeSize(uIC4, 0.065f);
		SpriteC c = SpriteS.AddComponent(uIC3.TC, new Frame(128f, 128f, 64f, 64f), GEState.editorUISheet);
		SpriteC c2 = SpriteS.AddComponent(uIC4.TC, new Frame(192f, 128f, 64f, 64f), GEState.editorUISheet);
		SpriteS.SetDimensions(c, uIC3.width * 0.75f, uIC3.height * 0.75f);
		SpriteS.SetDimensions(c2, uIC3.width * 0.75f, uIC3.height * 0.75f);
		Vector2[] circle = DebugDraw.GetCircle(uIC3.height * 0.5f, 36, Vector2.zero);
		PrefabS.CreatePathPrefabComponentFromVectorArray(uIC3.TC, Vector3.forward * -10f, circle, 6f, new Color(1f, 1f, 1f), ResourceManager.GetMaterial("Line6"), Main.uiCamera, Position.Center, true);
		PrefabS.CreatePathPrefabComponentFromVectorArray(uIC4.TC, Vector3.forward * -10f, circle, 6f, new Color(1f, 1f, 1f), ResourceManager.GetMaterial("Line6"), Main.uiCamera, Position.Center, true);
		UIS.PlaceCanvasContent(uIC);
		return uIC;
	}

	public static void HandleTouches(TouchAreaC _c, int _i, bool _consumed)
	{
		if (_consumed)
		{
			return;
		}
		UIC uIC = _c.customComponent as UIC;
		TouchEvent touchEvent = _c.touchEvent[_i];
		if (_c.identifier == "Undo" || _c.identifier == "Redo")
		{
			if (touchEvent == TouchEvent.Release && _c.touchStartedInside[_i])
			{
				if (_c.identifier == "Undo")
				{
					UndoManager.Undo();
				}
				else if (_c.identifier == "Redo")
				{
					UndoManager.Redo();
				}
			}
			return;
		}
		if (touchEvent == TouchEvent.Began || (touchEvent == TouchEvent.RollIn && _c.touchStartedInside[_i]))
		{
			Vector2[] roundedRect = DebugDraw.GetRoundedRect(uIC.width, uIC.height, 25f, 8, Vector2.zero, false);
			Polygon polygon = DebugDraw.Vector2ArrayToPolygon(roundedRect);
			while (uIC.backgroundPCs.Count > 0)
			{
				int index = uIC.backgroundPCs.Count - 1;
				PrefabS.RemoveComponent(uIC.backgroundPCs[index]);
				uIC.backgroundPCs.RemoveAt(index);
			}
			uIC.backgroundPCs.AddRange(PrefabS.CreateFlatPrefabComponentsFromPolygon(uIC.TC, Vector3.forward * 10f, polygon, DebugDraw.GetColor(255f, 255f, 255f), ResourceManager.GetMaterial("Solid"), Main.uiCamera));
		}
		else if (touchEvent == TouchEvent.RollOut && _c.touchStartedInside[_i] && !m_isMenuOpen)
		{
			Vector2[] roundedRect2 = DebugDraw.GetRoundedRect(uIC.width, uIC.height, 25f, 8, Vector2.zero, false);
			Polygon polygon2 = DebugDraw.Vector2ArrayToPolygon(roundedRect2);
			while (uIC.backgroundPCs.Count > 0)
			{
				int index2 = uIC.backgroundPCs.Count - 1;
				PrefabS.RemoveComponent(uIC.backgroundPCs[index2]);
				uIC.backgroundPCs.RemoveAt(index2);
			}
			uIC.backgroundPCs.AddRange(PrefabS.CreateFlatPrefabComponentsFromPolygon(uIC.TC, Vector3.forward * 10f, polygon2, DebugDraw.GetColor(230f, 230f, 230f), ResourceManager.GetMaterial("Solid"), Main.uiCamera));
		}
		if (touchEvent != TouchEvent.Release || !_c.touchStartedInside[_i])
		{
			return;
		}
		if (_c.identifier == "Menu")
		{
			EditorState.m_selection.Clear();
			EditorState.UpdateSelection();
			if (!m_isMenuOpen)
			{
				if (m_menuList == null)
				{
					m_menuList = GEMenuListA.Assemble(EditorState.m_menuArea);
					m_isMenuOpen = true;
				}
			}
			else if (m_menuList != null)
			{
				CloseMenu();
				Vector2[] roundedRect3 = DebugDraw.GetRoundedRect(uIC.width, uIC.height, 30f, 8, Vector2.zero, false);
				Polygon polygon3 = DebugDraw.Vector2ArrayToPolygon(roundedRect3);
				uIC.backgroundPCs.AddRange(PrefabS.CreateFlatPrefabComponentsFromPolygon(uIC.TC, Vector3.forward * 10f, polygon3, DebugDraw.GetColor(230f, 230f, 230f), ResourceManager.GetMaterial("Solid"), Main.uiCamera));
			}
			UIS.PlaceCanvasContent(EditorState.m_menuArea);
		}
		else if (_c.identifier == "Test")
		{
			LevelManager.SaveLevelData(LevelManager.m_currentLevel, LevelManager.m_currentLevel.name);
			EditorState.p_parent.StateMachine.ChangeState(new TestState());
		}
	}

	public static void CloseMenu()
	{
		if (m_isMenuOpen)
		{
			if (GEMenuListA.m_openedMenu != null)
			{
				UIS.RemoveFromCanvasGrid(GEMenuListA.m_openedMenu);
				EntityManager.RemoveEntitiesByTransformComponentHierarchy(GEMenuListA.m_openedMenu.TC, false);
				GEMenuListA.m_openedMenu = null;
				GEMenuListA.m_openedMenuId = 0;
			}
			UIS.RemoveFromCanvasGrid(m_menuList);
			EntityManager.RemoveEntitiesByTransformComponentHierarchy(m_menuList.TC, false);
			m_isMenuOpen = false;
			m_menuList = null;
			while (m_menuButton.backgroundPCs.Count > 0)
			{
				int index = m_menuButton.backgroundPCs.Count - 1;
				PrefabS.RemoveComponent(m_menuButton.backgroundPCs[index]);
				m_menuButton.backgroundPCs.RemoveAt(index);
			}
			Vector2[] roundedRect = DebugDraw.GetRoundedRect(m_menuButton.width, m_menuButton.height, 30f, 8, Vector2.zero, false);
			Polygon polygon = DebugDraw.Vector2ArrayToPolygon(roundedRect);
			m_menuButton.backgroundPCs.AddRange(PrefabS.CreateFlatPrefabComponentsFromPolygon(m_menuButton.TC, Vector3.forward * 10f, polygon, DebugDraw.GetColor(230f, 230f, 230f), ResourceManager.GetMaterial("Solid"), Main.uiCamera));
		}
	}
}
