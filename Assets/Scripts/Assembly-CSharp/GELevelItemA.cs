using UnityEngine;

public static class GELevelItemA
{
	private static int m_draggedOverIndex;

	private static TransformC m_dragMarkerTC;

	public static UIC Assemble(UIC _parent, bool _expanded, EIC _eic, int _intent)
	{
		string[] tags = new string[1] { "LevelItem" };
		UIC uIC = CanvasA.Assemble(_parent.canvasCamera, _eic.index, _eic.data.name, null, tags);
		UIS.AddToCanvasGrid(uIC, _parent, true);
		uIC.intent = _intent;
		SpriteSheet spriteSheet = GEState.outlinerIconSheet;
		if (_eic.identifier == "Target")
		{
			uIC.iconIndex = 0;
		}
		else if (_eic.identifier == "Safe Frame")
		{
			uIC.iconIndex = 1;
		}
		else if (_eic.identifier == "Block")
		{
			uIC.iconIndex = 2;
		}
		else if (_eic.identifier == "Ground")
		{
			uIC.iconIndex = 3;
		}
		else if (_eic.identifier == "Background")
		{
			uIC.iconIndex = 4;
		}
		else if (_eic.identifier == "Landscape")
		{
			uIC.iconIndex = 5;
		}
		else if (_eic.identifier == "Bolt")
		{
			uIC.iconIndex = 6;
		}
		else if (_eic.identifier == "Motor Bolt")
		{
			uIC.iconIndex = 7;
		}
		else if (_eic.identifier == "Rail")
		{
			uIC.iconIndex = 8;
		}
		else if (_eic.identifier == "RailPoint")
		{
			uIC.iconIndex = 8;
		}
		else if (_eic.identifier == "Bar")
		{
			uIC.iconIndex = 9;
		}
		else if (_eic.identifier == "Rope")
		{
			uIC.iconIndex = 10;
		}
		else if (_eic.identifier == "Flexible Rope")
		{
			uIC.iconIndex = 11;
		}
		else if (_eic.identifier == "Area")
		{
			uIC.iconIndex = 12;
		}
		else if (_eic.identifier == "Button")
		{
			uIC.iconIndex = 30;
		}
		else if (_eic.identifier == "Switch")
		{
			uIC.iconIndex = 31;
		}
		else if (_eic.identifier == "Collectible")
		{
			uIC.iconIndex = 15;
		}
		else if (_eic.identifier == "Timer")
		{
			uIC.iconIndex = 16;
		}
		else if (_eic.identifier == "Counter")
		{
			uIC.iconIndex = 17;
		}
		else if (_eic.identifier == "Text")
		{
			uIC.iconIndex = 18;
		}
		else if (_eic.identifier == "Number")
		{
			uIC.iconIndex = 19;
		}
		else if (_eic.identifier == "Boolean")
		{
			uIC.iconIndex = 24;
		}
		else if (_eic.identifier == "Tilt")
		{
			uIC.iconIndex = 41;
		}
		else if (_eic.identifier == "Accelerometer")
		{
			uIC.iconIndex = 41;
		}
		else if (_eic.identifier == "Draggable")
		{
			uIC.iconIndex = 42;
		}
		else if (_eic.identifier == "Flickable")
		{
			uIC.iconIndex = 43;
		}
		else if (_eic.identifier == "Sliceable")
		{
			uIC.iconIndex = 44;
		}
		else if (_eic.identifier == "Sensor")
		{
			uIC.iconIndex = 33;
		}
		else if (_eic.identifier == "Window")
		{
			uIC.iconIndex = 60;
		}
		else if (_eic.identifier == "Textfield")
		{
			uIC.iconIndex = 61;
		}
		else if (_eic.identifier == "Numeric Field")
		{
			uIC.iconIndex = 62;
		}
		else if (_eic.identifier == "Textbox")
		{
			uIC.iconIndex = 63;
		}
		else if (_eic.identifier == "Label")
		{
			uIC.iconIndex = 59;
		}
		else if (_eic.identifier == "Checkbox")
		{
			uIC.iconIndex = 65;
		}
		else if (_eic.identifier == "Dropdown Menu")
		{
			uIC.iconIndex = 64;
		}
		else if (_eic.identifier == "Pause Button")
		{
			uIC.iconIndex = 66;
		}
		else if (_eic.identifier == "Reset Button")
		{
			uIC.iconIndex = 67;
		}
		else if (_eic.identifier == "Change")
		{
			uIC.iconIndex = 34;
		}
		else if (_eic.identifier == "Reset")
		{
			uIC.iconIndex = 35;
		}
		else if (_eic.identifier == "Append")
		{
			uIC.iconIndex = 36;
		}
		else if (_eic.identifier == "Remove")
		{
			uIC.iconIndex = 37;
		}
		else if (_eic.identifier == "Remove All")
		{
			uIC.iconIndex = 38;
		}
		else if (_eic.identifier == "Save Value")
		{
			uIC.iconIndex = 39;
		}
		else if (_eic.identifier == "Load Value")
		{
			uIC.iconIndex = 40;
		}
		else if (_eic.identifier == "Start")
		{
			uIC.iconIndex = 49;
		}
		else if (_eic.identifier == "Checkpoint")
		{
			uIC.iconIndex = 51;
		}
		else if (_eic.identifier == "Goal")
		{
			uIC.iconIndex = 50;
		}
		else if (_eic.identifier == "Onion")
		{
			uIC.iconIndex = 52;
		}
		else if (_eic.identifier == "Radish")
		{
			uIC.iconIndex = 53;
		}
		else if (_eic.identifier == "Cabbage")
		{
			uIC.iconIndex = 55;
		}
		else if (_eic.identifier == "Tomato")
		{
			uIC.iconIndex = 56;
		}
		else if (_eic.identifier == "Apple")
		{
			uIC.iconIndex = 57;
		}
		else if (_eic.identifier == "Tree")
		{
			uIC.iconIndex = 48;
		}
		else if (_eic.identifier == "DPad")
		{
			uIC.iconIndex = 20;
		}
		else if (_eic.identifier == "Joystick")
		{
			uIC.iconIndex = 22;
		}
		else if (_eic.identifier == "Control Scheme")
		{
			uIC.iconIndex = 23;
		}
		else if (_eic.identifier == "Event Listener")
		{
			uIC.iconIndex = 33;
		}
		else if (_eic.identifier == "Event Dispatcher")
		{
			uIC.iconIndex = 32;
		}
		else if (_eic.identifier == "Event")
		{
			uIC.iconIndex = 32;
		}
		else if (_eic.identifier == "Slingable")
		{
			uIC.iconIndex = 43;
		}
		else
		{
			uIC.iconIndex = 0;
			GEPlugin[] plugins = GEState.plugins;
			foreach (GEPlugin gEPlugin in plugins)
			{
				uIC.iconIndex = gEPlugin.GetIconIndex(_eic.identifier);
				if (uIC.iconIndex != 0)
				{
					spriteSheet = gEPlugin.GetIconSheet();
					break;
				}
			}
			if (spriteSheet == null)
			{
				spriteSheet = GEState.outlinerIconSheet;
			}
		}
		uIC.iconSheet = spriteSheet;
		int subItemCount = UIS.GetSubItemCount(uIC, 0);
		subItemCount++;
		UIS.SetCanvasAbsoluteSize(uIC, _parent.width - _parent.contentMargin * 2f, 26f * (float)subItemCount, 26f, 0f);
		UIS.SetCanvasAbsoluteMarginAndSpacing(uIC, 0f, 0f);
		UIS.SetCanvasAlign(uIC, Align.Left, Align.Top);
		UIS.SetCanvasExpandable(uIC, _eic.subItems.Count > 0, _expanded);
		DrawItem(uIC, _parent.canvasCamera);
		if (!_parent.expanded)
		{
			EntityManager.SetActivityOfEntity(uIC.entityIndex, false, true);
		}
		m_draggedOverIndex = 0;
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
		TransformS.Move(_uic.textC.contentTC, Vector3.forward * -15f + Vector3.up * (height * 0.5f - headerHeight * 0.5f) + Vector3.right * (width * -0.5f + headerHeight * (_uic.intent + 1f)));
		_uic.textPCs.AddRange(SpriteS.ConvertSpritesToPrefabComponent(_uic.textC.TC, camera, true));
		float num = 32 * _uic.iconIndex;
		float num2 = Mathf.Floor(num / (float)_uic.iconSheet.m_textureWidth);
		num -= num2 * 32f * 16f;
		int num3 = _uic.iconSheet.m_textureWidth / 32;
		int num4 = (int)Mathf.Floor(_uic.iconIndex / num3);
		int num5 = _uic.iconIndex - num4 * num3;
		SpriteC spriteC = SpriteS.AddComponent(_uic.TC, new Frame(num5 * 32, num4 * 32, 32f, 32f), _uic.iconSheet);
		SpriteS.SetOffset(spriteC, Vector3.forward * -15f + Vector3.up * (height * 0.5f - headerHeight * 0.5f) + Vector3.right * (width * -0.5f + headerHeight + 10f), 0f);
		SpriteS.SetDimensionScale(spriteC, 0.75f);
		_uic.TAC = TouchAreaS.AddComponent(_uic.TC, "selectOutlinerItem", width - headerHeight, headerHeight, true, _camera, _uic);
		TouchAreaS.SetNonRotatedOffset(_uic.TAC, Vector3.up * (height * 0.5f - headerHeight * 0.5f) + Vector3.right * headerHeight);
		TouchAreaS.AddTouchEventListener(_uic.TAC, HandleTouches);
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
			TouchAreaC touchAreaC = TouchAreaS.AddComponent(_uic.TC, "expand", headerHeight, headerHeight, true, _camera, _uic);
			TouchAreaS.SetNonRotatedOffset(touchAreaC, Vector3.up * (height * 0.5f - headerHeight * 0.5f) + Vector3.right * (width * -0.5f + headerHeight * 0.5f));
			TouchAreaS.AddTouchEventListener(touchAreaC, HandleTouches);
			_uic.outlinePCs.Add(PrefabS.CreatePathPrefabComponentFromVectorArray(_uic.TC, Vector3.up * (height * 0.5f - headerHeight * 0.5f) + Vector3.forward * -10f + Vector3.right * (width * -0.5f + headerHeight * 0.5f), array, 8f, DebugDraw.GetColor(220f, 220f, 220f), ResourceManager.GetMaterial("Line4"), camera, Position.Center, true));
			_uic.foregroundPCs.AddRange(PrefabS.CreateFlatPrefabComponentsFromVectorArray(_uic.TC, Vector3.up * (height * 0.5f - headerHeight * 0.5f) + Vector3.forward * -15f + Vector3.right * (width * -0.5f + headerHeight * 0.5f), array, PrefabS.ColorToUInt(DebugDraw.GetColor(220f, 220f, 220f)), PrefabS.ColorToUInt(DebugDraw.GetColor(220f, 220f, 220f)), ResourceManager.GetMaterial("Solid"), camera, string.Empty));
		}
	}

	public static void HandleTouches(TouchAreaC _c, int _i, bool _consumed)
	{
		UIC uIC = _c.customComponent as UIC;
		if (EditorState.m_isSelectionLocked)
		{
			return;
		}
		if (_c.identifier == "selectOutlinerItem")
		{
			if (_consumed)
			{
				if (_c.touchEvent[_i] == TouchEvent.RollIn)
				{
					m_draggedOverIndex = uIC.index;
				}
			}
			else if (_c.touchEvent[_i] == TouchEvent.Began)
			{
				m_draggedOverIndex = 0;
				if (EditorState.m_selection.Contains(GES.m_editorItemComponents.m_array[uIC.identifier]))
				{
					if (!GEState.m_addDown && !GEState.m_subDown)
					{
						uIC.isDragged = true;
						TouchAreaS.ReleaseTouches(GEOutlinerA.m_level.TAC);
					}
					else
					{
						EditorState.SelectEditorItem(uIC.identifier);
					}
				}
				else
				{
					EditorState.SelectEditorItem(uIC.identifier);
				}
			}
			else if (_c.touchEvent[_i] == TouchEvent.Release && _c.touchStartedInside[_i])
			{
				if (uIC.isDragged)
				{
					uIC.isDragged = false;
					if (m_dragMarkerTC != null)
					{
						EntityManager.RemoveEntity(m_dragMarkerTC.entityIndex);
					}
					m_dragMarkerTC = null;
				}
			}
			else if (_c.touchEvent[_i] == TouchEvent.Drag)
			{
				if (!uIC.isDragged || m_draggedOverIndex <= 0)
				{
					return;
				}
				UIC uIComponentByIndex = UIS.GetUIComponentByIndex(m_draggedOverIndex);
				EIC eIC = GES.m_editorItemComponents.m_array[uIComponentByIndex.identifier];
				EIC eIC2 = GES.m_editorItemComponents.m_array[uIC.identifier];
				if (m_dragMarkerTC == null)
				{
					m_dragMarkerTC = EntityManager.AddEntityWithTC();
					Vector2[] rect = DebugDraw.GetRect(uIComponentByIndex.width, uIComponentByIndex.headerHeight * 0.25f, Vector2.zero);
					PrefabS.CreateFlatPrefabComponentsFromVectorArray(m_dragMarkerTC, Vector3.forward * -5f, rect, PrefabS.ColorToUInt(DebugDraw.GetColor(0f, 0f, 0f)), PrefabS.ColorToUInt(DebugDraw.GetColor(0f, 0f, 0f)), ResourceManager.GetMaterial("Solid"), uIComponentByIndex.canvasCamera, string.Empty);
				}
				Vector3 vector = uIComponentByIndex.TC.transform.position + Vector3.up * (uIComponentByIndex.height * 0.5f - uIComponentByIndex.headerHeight * 0.5f) + new Vector3((float)Screen.width * 0.5f, (float)Screen.height * 0.5f, -100f);
				if (_c.touchPos[_i].y > vector.y + uIComponentByIndex.headerHeight * 0.25f)
				{
					if (eIC.container != null)
					{
						TransformS.SetPosition(m_dragMarkerTC, vector + Vector3.up * uIComponentByIndex.headerHeight * 0.5f - new Vector3((float)Screen.width * 0.5f, (float)Screen.height * 0.5f, -100f));
					}
					else
					{
						TransformS.SetPosition(m_dragMarkerTC, vector + Vector3.up * uIComponentByIndex.headerHeight * 0.5f - new Vector3((float)Screen.width * 0.5f, (float)Screen.height * 0.5f, -100f));
					}
				}
				else if (_c.touchPos[_i].y < vector.y - uIComponentByIndex.headerHeight * 0.25f)
				{
					if (!uIComponentByIndex.expanded)
					{
						if (eIC.container != null)
						{
							TransformS.SetPosition(m_dragMarkerTC, vector - Vector3.up * uIComponentByIndex.headerHeight * 0.5f - new Vector3((float)Screen.width * 0.5f, (float)Screen.height * 0.5f, -100f));
						}
						else
						{
							TransformS.SetPosition(m_dragMarkerTC, vector - Vector3.up * uIComponentByIndex.headerHeight * 0.5f - new Vector3((float)Screen.width * 0.5f, (float)Screen.height * 0.5f, -100f));
						}
					}
					else
					{
						TransformS.SetPosition(m_dragMarkerTC, vector - Vector3.up * uIComponentByIndex.headerHeight * 0.5f - new Vector3((float)Screen.width * 0.5f, (float)Screen.height * 0.5f, -100f));
					}
				}
				else
				{
					TransformS.SetPosition(m_dragMarkerTC, vector - new Vector3((float)Screen.width * 0.5f, (float)Screen.height * 0.5f, -100f));
				}
			}
			else
			{
				if (_c.touchEvent[_i] != TouchEvent.ReleaseOutside || !_c.touchStartedInside[_i] || !uIC.isDragged)
				{
					return;
				}
				uIC.isDragged = false;
				if (m_dragMarkerTC != null)
				{
					EntityManager.RemoveEntity(m_dragMarkerTC.entityIndex);
				}
				m_dragMarkerTC = null;
				UIC uIComponentByIndex2 = UIS.GetUIComponentByIndex(m_draggedOverIndex);
				EIC eIC3 = GES.m_editorItemComponents.m_array[uIComponentByIndex2.identifier];
				EIC eIC4 = GES.m_editorItemComponents.m_array[uIC.identifier];
				if (IsItemChild(eIC3, eIC4) || m_draggedOverIndex <= 0)
				{
					return;
				}
				Vector3 vector2 = uIComponentByIndex2.TC.transform.position + Vector3.up * (uIComponentByIndex2.height * 0.5f - uIComponentByIndex2.headerHeight * 0.5f) + new Vector3((float)Screen.width * 0.5f, (float)Screen.height * 0.5f, 0f);
				int rowIndex = UIS.GetRowIndex(uIComponentByIndex2);
				UIC parent = uIC.parent;
				int num = 0;
				if (eIC4.container == eIC3.container && UIS.GetRowIndex(uIC) < rowIndex)
				{
					num = -1;
				}
				EditorState.RemoveEditorItemFromContainer(eIC4);
				GELevel gELevel = LevelManager.m_currentLevel as GELevel;
				if (_c.touchPos[_i].y > vector2.y + uIComponentByIndex2.headerHeight * 0.25f)
				{
					if (eIC3.container != null)
					{
						EditorState.AddEditorItemToContainer(eIC4, eIC3.container, rowIndex + num);
						UIS.RemoveFromCanvasGrid(uIC);
						UIS.AddToCanvasGrid(uIC, uIComponentByIndex2.parent, rowIndex + num, 0, true);
					}
					else
					{
						EditorState.AddEditorItemToContainer(eIC4, null, rowIndex + num);
						if (eIC4.identifier == "Ground" || eIC4.identifier == "Background" || eIC4.identifier == "Landscape")
						{
							GELevelGenerator.CreateShapes();
						}
						UIS.RemoveFromCanvasGrid(uIC);
						UIS.AddToCanvasGrid(uIC, uIComponentByIndex2.parent, rowIndex + num, 0, true);
					}
				}
				else if (_c.touchPos[_i].y < vector2.y - uIComponentByIndex2.headerHeight * 0.25f)
				{
					if (!uIComponentByIndex2.expanded)
					{
						if (eIC3.container != null)
						{
							EditorState.AddEditorItemToContainer(eIC4, eIC3.container, rowIndex + 1 + num);
							UIS.RemoveFromCanvasGrid(uIC);
							UIS.AddToCanvasGrid(uIC, uIComponentByIndex2.parent, rowIndex + 1 + num, 0, true);
						}
						else
						{
							EditorState.AddEditorItemToContainer(eIC4, null, rowIndex + 1 + num);
							if (eIC4.identifier == "Ground" || eIC4.identifier == "Background" || eIC4.identifier == "Landscape")
							{
								GELevelGenerator.CreateShapes();
							}
							UIS.RemoveFromCanvasGrid(uIC);
							UIS.AddToCanvasGrid(uIC, GEOutlinerA.m_level, rowIndex + 1 + num, 0, true);
						}
					}
					else
					{
						EditorState.AddEditorItemToContainer(eIC4, eIC3, 0);
						UIS.RemoveFromCanvasGrid(uIC);
						UIS.AddToCanvasGrid(uIC, uIComponentByIndex2, 0, 0, true);
					}
				}
				else
				{
					EditorState.AddEditorItemToContainer(eIC4, eIC3, eIC3.subItems.Count);
					if (!uIComponentByIndex2.expandable)
					{
						UIS.SetCanvasExpandable(uIComponentByIndex2, true, false);
					}
					UIS.RemoveFromCanvasGrid(uIC);
					UIS.AddToCanvasGrid(uIC, uIComponentByIndex2, true);
				}
				if (parent.canvasComponents.Count == 0)
				{
					UIS.SetCanvasExpandable(parent, false, false);
				}
				UpdateCanvasSizes(parent);
				SetItemIntents(uIC);
				EditorState.MarkSelectedOutlinerItems(GEOutlinerA.m_level);
			}
		}
		else if (_c.identifier == "expand" && _c.touchEvent[_i] == TouchEvent.Began)
		{
			if (uIC.expanded)
			{
				UIS.SetCanvasExpandable(uIC, true, false);
				UIS.SetActivityOfChildComponents(uIC, false);
			}
			else
			{
				UIS.SetCanvasExpandable(uIC, true, true);
				UIS.SetActivityOfChildComponents(uIC, true);
			}
			UpdateCanvasSizes(uIC);
			EditorState.MarkSelectedOutlinerItems(GEOutlinerA.m_level);
		}
	}

	public static void UpdateCanvasSizes(UIC _item)
	{
		while (_item != null)
		{
			if (_item.parent != null && _item.identifier > -1)
			{
				int subItemCount = UIS.GetSubItemCount(_item, 0);
				subItemCount++;
				UIS.SetCanvasAbsoluteSize(_item, _item.parent.width - _item.parent.contentMargin * 2f, 26f * (float)subItemCount, 26f, 0f);
				PrefabS.RemoveComponentsByEntityIndex(_item.entityIndex);
				TouchAreaS.RemoveComponentsByTransformComponent(_item.TC);
				for (int i = 0; i < _item.TC.childs.Count; i++)
				{
					TouchAreaS.RemoveComponentsByTransformComponent(_item.TC.childs[i]);
				}
				TextS.RemoveComponent(_item.textC);
				SpriteS.RemoveSpritesFromTransformComponent(_item.TC);
				if (_item.identifier == 0)
				{
					GELibraryCategoryA.DrawItem(_item, _item.parent.canvasCamera);
				}
				else
				{
					DrawItem(_item, _item.parent.canvasCamera);
				}
				if ((!_item.parent.expanded && _item.parent.expandable) || !_item.parent.active)
				{
					UIS.SetActivityOfChildComponents(_item.parent, false);
				}
			}
			UIS.ResetCursor(_item);
			UIS.PlaceCanvasContent(_item);
			_item = _item.parent;
		}
	}

	public static bool IsItemChild(EIC _child, EIC _parent)
	{
		bool flag = false;
		if ((_child.itemType == 2 || _child.itemType == 1) && _parent.subItems != null)
		{
			for (int i = 0; i < _parent.subItems.Count; i++)
			{
				if (_parent.subItems[i] == _child)
				{
					return true;
				}
				flag = IsItemChild(_child, _parent.subItems[i]);
				if (flag)
				{
					return true;
				}
			}
		}
		return flag;
	}

	public static void SetItemIntents(UIC _item)
	{
		_item.intent = _item.parent.intent + 1f;
		for (int i = 0; i < _item.canvasComponents.Count; i++)
		{
			for (int j = 0; j < _item.canvasComponents[i].Count; j++)
			{
				SetItemIntents(_item.canvasComponents[i][j]);
			}
		}
		if (_item.canvasComponents.Count == 0)
		{
			UpdateCanvasSizes(_item);
		}
	}
}
