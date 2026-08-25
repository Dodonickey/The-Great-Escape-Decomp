using System.Collections.Generic;
using UnityEngine;

public static class GEActionAreaA
{
	private static int DRAW = 1;

	private static int COPY = 2;

	private static int TRASH = 3;

	public static UIC Assemble(UIC _parent)
	{
		string[] tags = new string[1] { "ActionBar" };
		UIC uIC = CanvasA.Assemble(Main.uiCamera, 0, "ActionBar", null, tags);
		UIS.AddToCanvas(uIC, _parent, Vector3.zero);
		UIS.SetCanvasAbsoluteSize(uIC, (float)Screen.width * 0.5f, 60f, 0f, 0f);
		UIS.SetRelativePosition(uIC, new Vector2(0.5f, 0f), 0);
		UIS.SetCanvasRelativeMarginAndSpacing(uIC, 0f, 0.15f);
		UIS.SetCanvasAlign(uIC, Align.Center, Align.Bottom);
		UIC uIC2 = ButtonA.Assemble(Main.uiCamera, DRAW, "Draw Mode", HandleTouches, tags);
		UIC uIC3 = ButtonA.Assemble(Main.uiCamera, COPY, "Copy", HandleTouches, tags);
		UIC uIC4 = ButtonA.Assemble(Main.uiCamera, TRASH, "Trash", HandleTouches, tags);
		UIS.AddToCanvasGrid(uIC2, uIC, false);
		UIS.AddToCanvasGrid(uIC3, uIC, false);
		UIS.AddToCanvasGrid(uIC4, uIC, false);
		UIS.SetRelativeSize(uIC2, 1f);
		UIS.SetRelativeSize(uIC3, 1f);
		UIS.SetRelativeSize(uIC4, 1f);
		SpriteC c = SpriteS.AddComponent(uIC2.TC, new Frame(0f, 0f, 128f, 128f), GEState.editorUISheet);
		SpriteC c2 = SpriteS.AddComponent(uIC3.TC, new Frame(128f, 0f, 128f, 128f), GEState.editorUISheet);
		SpriteC c3 = SpriteS.AddComponent(uIC4.TC, new Frame(256f, 0f, 128f, 128f), GEState.editorUISheet);
		SpriteS.SetDimensionScale(c, 0.5f);
		SpriteS.SetDimensionScale(c2, 0.5f);
		SpriteS.SetDimensionScale(c3, 0.5f);
		Vector2[] circle = DebugDraw.GetCircle(uIC.height * 0.5f, 36, Vector2.zero);
		PrefabS.CreatePathPrefabComponentFromVectorArray(uIC2.TC, Vector3.forward * -10f, circle, 6f, new Color(1f, 1f, 1f), ResourceManager.GetMaterial("Line6"), Main.uiCamera, Position.Center, true);
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
		switch (touchEvent)
		{
		case TouchEvent.Began:
			return;
		case TouchEvent.ReleaseOutside:
		case TouchEvent.RollOut:
			if (_c.touchStartedInside[_i])
			{
				return;
			}
			break;
		}
		if (touchEvent != TouchEvent.Release || !_c.touchStartedInside[_i])
		{
			return;
		}
		if (uIC.identifier == DRAW)
		{
			if (EditorState.m_selection.Count != 1)
			{
				return;
			}
			if (!EditorState.m_drawMode)
			{
				bool flag = false;
				if (EditorState.m_selection[0].identifier == "Block" || EditorState.m_selection[0].identifier == "Ground" || EditorState.m_selection[0].identifier == "Background" || EditorState.m_selection[0].identifier == "Landscape")
				{
					EditorState.SetHighlight(true, EditorState.m_selection[0]);
					flag = true;
				}
				else if (EditorState.m_selection[0].identifier == "Voxel Shape")
				{
					flag = true;
					EditorState.m_voxelDrawMode = true;
				}
				if (flag)
				{
					EditorState.m_isSelectionLocked = true;
					EditorState.m_drawMode = true;
					EntityManager.RemoveEntity(EditorState.m_gizmo.entityIndex);
					EditorState.m_gizmo = null;
					EntityManager.SetActivityOfEntitiesWithTag(LevelManager.m_currentLevel.name + ":EditorItem", false, true);
					EntityManager.SetActivityOfEntitiesWithTag(LevelManager.m_currentLevel.name + ":EditorAnchor", false, true);
					EntityManager.SetActivityOfEntitiesWithTag(LevelManager.m_currentLevel.name + ":EditorHandle", false, true);
					GEOutlinerA.Minimize();
					EntityManager.RemoveEntitiesByTag("DrawArea", true);
					GEDrawButtonsAreaA.Assemble(EditorState.m_mainCanvas, true);
				}
			}
			else
			{
				EditorState.m_isSelectionLocked = false;
				EditorState.m_drawMode = false;
				EditorState.m_voxelDrawMode = false;
				EditorState.UpdateSelection();
				EntityManager.SetActivityOfEntitiesWithTag(LevelManager.m_currentLevel.name + ":EditorItem", true, true);
				EntityManager.SetActivityOfEntitiesWithTag(LevelManager.m_currentLevel.name + ":EditorAnchor", true, true);
				EntityManager.SetActivityOfEntitiesWithTag(LevelManager.m_currentLevel.name + ":EditorHandle", true, true);
				GEOutlinerA.Maximize();
				if (EditorState.m_selection[0].identifier == "Block" || EditorState.m_selection[0].identifier == "Ground" || EditorState.m_selection[0].identifier == "Background" || EditorState.m_selection[0].identifier == "Landscape")
				{
					EditorState.SetHighlight(false, EditorState.m_selection[0]);
				}
				else if (EditorState.m_selection[0].identifier == "Voxel Shape")
				{
					VoxelData voxelData = EditorState.m_selection[0].data as VoxelData;
					GEVoxelShapeC gEVoxelShapeC = EditorState.m_selection[0].gameComponents[0] as GEVoxelShapeC;
					voxelData.map = gEVoxelShapeC.map;
					voxelData.colors = gEVoxelShapeC.colors;
				}
				EntityManager.RemoveEntitiesByTag("DrawArea", true);
				GEDrawButtonsAreaA.Assemble(EditorState.m_mainCanvas, false);
			}
		}
		else if (uIC.identifier == COPY)
		{
			if (EditorState.m_isSelectionLocked)
			{
				return;
			}
			List<EIC> list = new List<EIC>();
			List<EIC> list2 = new List<EIC>();
			for (int i = 0; i < EditorState.m_selection.Count; i++)
			{
				EIC eIC = EditorState.m_selection[i];
				if (eIC.itemType != 0 && eIC.container != null && eIC.container.data.id == eIC.data.id)
				{
					eIC = eIC.container;
				}
				if (!list2.Contains(eIC))
				{
					list2.Add(eIC);
					EIC item = GES.DublicateEditorItem(eIC, Vector3.right * 10f, list2);
					list.Add(item);
				}
			}
			EditorState.m_selection = list;
			EditorState.UpdateSelection();
		}
		else
		{
			if (uIC.identifier != TRASH || EditorState.m_isSelectionLocked)
			{
				return;
			}
			List<EIC> list3 = new List<EIC>();
			bool flag2 = false;
			bool flag3 = false;
			EIC eIC2 = null;
			UndoManager.AddStep(new DestroyStep(EditorState.m_selection));
			while (EditorState.m_selection.Count > 0)
			{
				int index = EditorState.m_selection.Count - 1;
				EIC eIC3 = EditorState.m_selection[index];
				if (eIC3.itemType != 0 && eIC3.container != null && eIC3.data.id == eIC3.container.data.id && eIC3.identifier != "RailPoint")
				{
					eIC3 = eIC3.container;
				}
				else if (eIC3.identifier == "RailPoint")
				{
					flag3 = true;
					eIC2 = eIC3.container;
				}
				if (!list3.Contains(eIC3))
				{
					list3.Add(eIC3);
					flag2 = EditorState.RemoveEditorItem(eIC3, list3);
				}
				if (eIC3.container != null)
				{
					GES.SetContainerPosition(eIC3.container, true);
				}
				EditorState.m_selection.RemoveAt(index);
			}
			EditorState.UpdateSelection();
			if (flag2)
			{
				GELevelGenerator.CreateShapes();
			}
			if (!flag3)
			{
				return;
			}
			int num = 0;
			for (int j = 0; j < eIC2.subItems.Count; j++)
			{
				if (eIC2.subItems[j].identifier == "RailPoint")
				{
					num++;
				}
			}
			if (num >= 2)
			{
				EditorState.ResetEditorItem(eIC2);
			}
			else
			{
				EditorState.RemoveEditorItem(eIC2);
			}
		}
	}
}
