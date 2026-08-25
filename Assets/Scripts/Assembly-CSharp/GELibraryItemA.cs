using System.Collections.Generic;
using UnityEngine;

public static class GELibraryItemA
{
	public static UIC Assemble(UIC _parent, string _label, int _iconIndex)
	{
		return Assemble(_parent, _label, _iconIndex, GEState.outlinerIconSheet);
	}

	public static UIC Assemble(UIC _parent, string _label, int _iconIndex, SpriteSheet _customSheet)
	{
		string[] tags = new string[1] { "LibraryItem" };
		UIC uIC = CanvasA.Assemble(_parent.canvasCamera, 0, _label, null, tags);
		UIS.AddToCanvasGrid(uIC, _parent, true);
		uIC.intent = _parent.intent + 1f;
		uIC.iconIndex = _iconIndex;
		int subItemCount = UIS.GetSubItemCount(uIC, 0);
		subItemCount++;
		UIS.SetCanvasAbsoluteSize(uIC, _parent.width - _parent.contentMargin * 2f, 20f * (float)subItemCount, 20f, 0f);
		UIS.SetCanvasAbsoluteMarginAndSpacing(uIC, 0f, 0f);
		UIS.SetCanvasAlign(uIC, Align.Left, Align.Top);
		UIS.SetCanvasExpandable(uIC, false, false);
		uIC.iconSheet = _customSheet;
		SpriteS.SetSpriteSheetCamera(uIC.iconSheet, _parent.canvasCamera);
		DrawItem(uIC, _parent.canvasCamera);
		if (!_parent.expanded)
		{
			EntityManager.SetActivityOfEntity(uIC.entityIndex, false, true);
		}
		return uIC;
	}

	public static void DrawItem(UIC _uic, Camera _camera)
	{
		if (_uic.iconSheet == null)
		{
			_uic.iconSheet = GEState.outlinerIconSheet;
		}
		float width = _uic.width;
		float height = _uic.height;
		float headerHeight = _uic.headerHeight;
		Camera camera = _camera;
		if (_uic.parent != null && _uic.parent.separateRenderSpace)
		{
			camera = _uic.parent.canvasCamera;
		}
		TextS.SetStyle("body");
		_uic.textC = TextS.AddSingleLineComponent(_uic.TC, _uic.label, 1f, Align.Left, Align.Middle);
		SpriteS.SetColorByTransformComponent(_uic.textC.contentTC, DebugDraw.GetColor(0f, 0f, 0f), false, false);
		TransformS.Move(_uic.textC.contentTC, Vector3.up * (height * 0.5f - headerHeight * 0.5f) + Vector3.forward * -15f + Vector3.right * (width * -0.5f + headerHeight * (_uic.intent + 1f)));
		SpriteS.ConvertSpritesToPrefabComponent(_uic.textC.TC, camera, true);
		float num = 32 * _uic.iconIndex;
		float num2 = Mathf.Floor(num / (float)_uic.iconSheet.m_textureWidth);
		num -= num2 * 16f;
		int num3 = _uic.iconSheet.m_textureWidth / 32;
		int num4 = (int)Mathf.Floor(_uic.iconIndex / num3);
		int num5 = _uic.iconIndex - num4 * num3;
		SpriteC spriteC = SpriteS.AddComponent(_uic.TC, new Frame(num5 * 32, num4 * 32, 32f, 32f), _uic.iconSheet);
		SpriteS.SetOffset(spriteC, Vector3.forward * -15f + Vector3.up * (height * 0.5f - headerHeight * 0.5f) + Vector3.right * (width * -0.5f + headerHeight + 10f), 0f);
		SpriteS.SetDimensionScale(spriteC, 0.75f);
		TouchAreaC touchAreaC = TouchAreaS.AddComponent(_uic.TC, "select", width - headerHeight, headerHeight, true, _camera, _uic);
		TouchAreaS.SetNonRotatedOffset(touchAreaC, Vector3.up * (height * 0.5f - headerHeight * 0.5f) + Vector3.right * headerHeight);
		TouchAreaS.AddTouchEventListener(touchAreaC, HandleTouches);
		TouchAreaS.SetClip(touchAreaC, Mathf.RoundToInt(GEOutlinerA.m_library.TC.transform.position.x + (float)Screen.width * 0.5f - GEOutlinerA.m_library.width * 0.5f), Screen.width + 100, -100, Screen.height + 100);
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
			TouchAreaC touchAreaC2 = TouchAreaS.AddComponent(_uic.TC, "expand", headerHeight, headerHeight, true, _camera, _uic);
			TouchAreaS.SetNonRotatedOffset(touchAreaC2, Vector3.up * (height * 0.5f - headerHeight * 0.5f) + Vector3.right * (width * -0.5f + headerHeight * 0.5f));
			TouchAreaS.AddTouchEventListener(touchAreaC2, HandleTouches);
			PrefabS.CreatePathPrefabComponentFromVectorArray(_uic.TC, Vector3.up * (height * 0.5f - headerHeight * 0.5f) + Vector3.forward * -10f + Vector3.right * (width * -0.5f + headerHeight * 0.5f), array, 8f, DebugDraw.GetColor(220f, 220f, 220f), ResourceManager.GetMaterial("Line4"), camera, Position.Center, true);
			PrefabS.CreateFlatPrefabComponentsFromVectorArray(_uic.TC, Vector3.up * (height * 0.5f - headerHeight * 0.5f) + Vector3.forward * -15f + Vector3.right * (width * -0.5f + headerHeight * 0.5f), array, PrefabS.ColorToUInt(DebugDraw.GetColor(220f, 220f, 220f)), PrefabS.ColorToUInt(DebugDraw.GetColor(220f, 220f, 220f)), ResourceManager.GetMaterial("Solid"), camera, string.Empty);
		}
	}

	public static void HandleTouches(TouchAreaC _c, int _i, bool _consumed)
	{
		if (_consumed || EditorState.m_isSelectionLocked)
		{
			return;
		}
		UIC uIC = _c.customComponent as UIC;
		if (!(_c.identifier == "select"))
		{
			return;
		}
		if (_c.touchEvent[_i] == TouchEvent.Began)
		{
			Vector2[] rect = DebugDraw.GetRect(uIC.width, uIC.height, Vector2.zero);
			uIC.backgroundPCs.AddRange(PrefabS.CreateFlatPrefabComponentsFromVectorArray(uIC.TC, Vector3.forward * -10f, rect, PrefabS.ColorToUInt(DebugDraw.GetColor(144f, 199f, 71f)), PrefabS.ColorToUInt(DebugDraw.GetColor(144f, 199f, 71f)), ResourceManager.GetMaterial("Solid"), uIC.parent.canvasCamera, string.Empty));
		}
		else if (_c.touchEvent[_i] == TouchEvent.Release || _c.touchEvent[_i] == TouchEvent.ReleaseOutside)
		{
			while (uIC.backgroundPCs.Count > 0)
			{
				int index = uIC.backgroundPCs.Count - 1;
				PrefabS.RemoveComponent(uIC.backgroundPCs[index]);
				uIC.backgroundPCs.RemoveAt(index);
			}
		}
		else
		{
			if (_c.touchEvent[_i] != TouchEvent.RollOutOfClipArea)
			{
				return;
			}
			while (uIC.backgroundPCs.Count > 0)
			{
				int index2 = uIC.backgroundPCs.Count - 1;
				PrefabS.RemoveComponent(uIC.backgroundPCs[index2]);
				uIC.backgroundPCs.RemoveAt(index2);
			}
			TouchAreaS.ReleaseTouches(GEOutlinerA.m_library.TAC);
			List<EIC> list = EditorState.CreateNewEditorItem(null, uIC.label, TouchAreaS.GetTouchWorldPos(Main.camera, _c.touchPos[_i]), Vector3.zero, Vector3.one);
			UndoManager.AddStep(new CreateNewStep(list, uIC.label, TouchAreaS.GetTouchWorldPos(Main.camera, _c.touchPos[_i]), Vector3.zero, Vector3.one));
			EditorState.m_selection.Clear();
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i] != null)
				{
					if (list[i].camera == Main.uiCamera)
					{
						list[i].data.position = new Vertex3(_c.touchPos[_i]);
						TransformS.SetGlobalPosition(list[i].TC, _c.touchPos[_i] - new Vector2((float)Screen.width * 0.5f, (float)Screen.height * 0.5f));
						TransformS.SetGlobalPosition(list[i].uiTC, _c.touchPos[_i] - new Vector2((float)Screen.width * 0.5f, (float)Screen.height * 0.5f));
					}
					EditorState.FillEditorItemHierarchy(list[i]);
					UIC level = GEOutlinerA.m_level;
					if (list[i].container != null)
					{
						level = UIS.GetUIComponentByIdentifier(list[i].container.index);
						EditorState.AddItemToOutliner(level, list[i], Mathf.RoundToInt(level.intent + 1f));
					}
					else
					{
						EditorState.AddItemToOutliner(level, list[i], 1);
					}
					EditorState.m_selection.Add(list[i]);
				}
			}
			EditorState.UpdateSelection();
			if (EditorState.m_gizmo != null)
			{
				TLTouch t = InputManager.m_touches[_c.touchIndex[_i]];
				TouchAreaS.ReleaseTouches(_c);
				TouchAreaS.ForceTouch(EditorState.m_gizmo.moveTAC, t, _i, TouchEvent.Began, false);
				EditorState.m_gizmo.touchOffset = Vector3.zero;
			}
		}
	}
}
