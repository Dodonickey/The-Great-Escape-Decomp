using System.Collections.Generic;
using UnityEngine;

public class EditorState : BasicState
{
	public static EditorScene p_parent;

	public static UIC m_mainCanvas;

	public static UIC m_menuArea;

	public static UIC m_outliner;

	public static UIC m_actionArea;

	public static List<EIC> m_selection;

	public static Vector3[] m_selectionOffsets;

	public static bool m_isSelectionLocked;

	public static GETransformGizmoC m_gizmo;

	public static bool m_drawMode;

	public static bool m_voxelDrawMode;

	public static TransformC m_drawHighlightTC;

	private static List<EIC> secondaryFillList = new List<EIC>();

	public EditorState()
	{
		m_selection = new List<EIC>();
	}

	public override void Enter(IStatedObject _parent)
	{
		p_parent = _parent as EditorScene;
		GEState.editorMode = true;
		m_drawMode = false;
		m_voxelDrawMode = false;
		m_isSelectionLocked = false;
		m_selection.Clear();
		CameraS.m_currentCameraPosition = GEState.editorCameraStartPosition;
		CameraS.m_currentCameraRotation = Vector3.zero;
		ResourceManager.LoadResourceGroup("EditorUI");
		if (GEState.editorUISheet == null)
		{
			GEState.editorUISheet = SpriteS.AddSpriteSheet(20, Main.uiCamera, ResourceManager.GetTexture("EditorUI"), ResourceManager.GetShader("EditorUIShader"), 1f);
		}
		if (GEState.outlinerIconSheet == null)
		{
			GEState.outlinerIconSheet = SpriteS.AddSpriteSheet(400, Main.uiCamera, ResourceManager.GetTexture("EditorItemIcons"), ResourceManager.GetShader("EditorUIShader"), 1f);
		}
		GEState.drawTC = EntityManager.AddEntityWithTC();
		GEState.connectionTC = EntityManager.AddEntityWithTC();
		GEPlugin[] plugins = GEState.plugins;
		foreach (GEPlugin gEPlugin in plugins)
		{
			gEPlugin.Enter(_parent);
		}
		CreateEditorUI();
		GELevel gELevel = null;
		if (LevelManager.m_currentLevel == null)
		{
			gELevel = LevelManager.CreateNewLevel() as GELevel;
			return;
		}
		if (LevelManager.m_currentChapterIndex != 0 && LevelManager.m_currentLevelIndex != 0)
		{
			LevelManager.ChangeLevel(LevelManager.m_currentChapterIndex, LevelManager.m_currentLevelIndex, true);
		}
		else
		{
			LevelManager.ChangeLevel(LevelManager.m_currentLevel.name, true);
		}
		ResetOutliner();
	}

	public override void Exit()
	{
		if (m_drawMode)
		{
			m_isSelectionLocked = false;
			m_drawMode = false;
			SetHighlight(false, m_selection[0]);
		}
		RemoveEditorUI();
		EntityManager.RemoveEntity(GEState.drawTC.entityIndex);
		GEState.drawTC = null;
		EntityManager.RemoveEntity(GEState.connectionTC.entityIndex);
		GEState.connectionTC = null;
		m_selection.Clear();
		UpdateSelection();
		GEState.editorMode = false;
		GEPlugin[] plugins = GEState.plugins;
		foreach (GEPlugin gEPlugin in plugins)
		{
			gEPlugin.Exit();
		}
		if (GEState.editorUISheet != null)
		{
			SpriteS.RemoveSpriteSheet(GEState.editorUISheet);
			GEState.editorUISheet = null;
		}
		if (GEState.outlinerIconSheet != null)
		{
			SpriteS.RemoveSpriteSheet(GEState.outlinerIconSheet);
			GEState.outlinerIconSheet = null;
		}
		ResourceManager.UnloadResourceGroup("EditorUI");
	}

	public override void Execute()
	{
		if (Input.GetKey(KeyCode.A))
		{
			CameraS.m_currentCameraPosition += Vector3.forward * 100f;
			Vector3 currentCameraPosition = CameraS.m_currentCameraPosition;
			currentCameraPosition.z = Mathf.Max(-2500f, Mathf.Min(-100f, CameraS.m_currentCameraPosition.z));
			CameraS.m_currentCameraPosition = currentCameraPosition;
		}
		else if (Input.GetKey(KeyCode.Z))
		{
			CameraS.m_currentCameraPosition += Vector3.forward * -100f;
			Vector3 currentCameraPosition2 = CameraS.m_currentCameraPosition;
			currentCameraPosition2.z = Mathf.Max(-2500f, Mathf.Min(-100f, CameraS.m_currentCameraPosition.z));
			CameraS.m_currentCameraPosition = currentCameraPosition2;
		}
		if (Input.GetKey(KeyCode.Space))
		{
			ChipmunkS.Update(1f / 60f);
			for (int i = 0; i < m_selection.Count; i++)
			{
				for (int j = 0; j < m_selection[i].TC.childs.Count; j++)
				{
					TransformS.SetGlobalPositionWithoutChildren(m_selection[i].TC, m_selection[i].TC.childs[j].transform.position);
					TransformS.SetGlobalRotationWithoutChildren(m_selection[i].TC, m_selection[i].TC.childs[j].transform.eulerAngles);
				}
			}
		}
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			List<EIC> list = new List<EIC>();
			for (int k = 0; k < m_selection.Count; k++)
			{
				if (m_selection[k].container != null)
				{
					if (!list.Contains(m_selection[k].container))
					{
						list.Add(m_selection[k].container);
					}
				}
				else if (!list.Contains(m_selection[k]))
				{
					list.Add(m_selection[k]);
				}
			}
			m_selection = list;
			UpdateSelection();
		}
		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			if (m_gizmo != null)
			{
				EntityManager.RemoveEntitiesByTag("EditorHandle", true);
				EntityManager.RemoveEntity(m_gizmo.entityIndex);
				m_gizmo = GETransformGizmoA.Assemble(false);
			}
			GEState.m_addDown = true;
			EntityManager.SetActivityOfEntitiesWithTag(string.Concat(LevelManager.m_currentLevel, ":EditorItem"), false, false);
			EntityManager.SetActivityOfEntitiesWithTag(string.Concat(LevelManager.m_currentLevel, ":EditorAnchor"), false, false);
			EntityManager.SetActivityOfEntitiesWithTag(string.Concat(LevelManager.m_currentLevel, ":EditorHandle"), false, false);
			EntityManager.SetActivityOfEntitiesWithTag("ActionBar", false, false);
		}
		if (Input.GetKeyUp(KeyCode.LeftShift) && GEState.m_addDown)
		{
			if (m_gizmo != null)
			{
				EntityManager.RemoveEntitiesByTag("EditorHandle", true);
				EntityManager.RemoveEntity(m_gizmo.entityIndex);
				m_gizmo = GETransformGizmoA.Assemble(true);
			}
			GEState.m_addDown = false;
			EntityManager.SetActivityOfEntitiesWithTag(string.Concat(LevelManager.m_currentLevel, ":EditorItem"), true, false);
			EntityManager.SetActivityOfEntitiesWithTag(string.Concat(LevelManager.m_currentLevel, ":EditorAnchor"), true, false);
			EntityManager.SetActivityOfEntitiesWithTag(string.Concat(LevelManager.m_currentLevel, ":EditorHandle"), true, false);
			EntityManager.SetActivityOfEntitiesWithTag("ActionBar", true, false);
		}
		if (Input.GetKeyDown(KeyCode.LeftAlt))
		{
			if (m_gizmo != null)
			{
				EntityManager.RemoveEntitiesByTag("EditorHandle", true);
				EntityManager.RemoveEntity(m_gizmo.entityIndex);
				m_gizmo = GETransformGizmoA.Assemble(false);
			}
			GEState.m_subDown = true;
			EntityManager.SetActivityOfEntitiesWithTag(string.Concat(LevelManager.m_currentLevel, ":EditorItem"), false, false);
			EntityManager.SetActivityOfEntitiesWithTag(string.Concat(LevelManager.m_currentLevel, ":EditorAnchor"), false, false);
			EntityManager.SetActivityOfEntitiesWithTag(string.Concat(LevelManager.m_currentLevel, ":EditorHandle"), false, false);
			EntityManager.SetActivityOfEntitiesWithTag("ActionBar", false, false);
		}
		if (Input.GetKeyUp(KeyCode.LeftAlt) && GEState.m_subDown)
		{
			if (m_gizmo != null)
			{
				EntityManager.RemoveEntitiesByTag("EditorHandle", true);
				EntityManager.RemoveEntity(m_gizmo.entityIndex);
				m_gizmo = GETransformGizmoA.Assemble(true);
			}
			GEState.m_subDown = false;
			EntityManager.SetActivityOfEntitiesWithTag(string.Concat(LevelManager.m_currentLevel, ":EditorItem"), true, false);
			EntityManager.SetActivityOfEntitiesWithTag(string.Concat(LevelManager.m_currentLevel, ":EditorAnchor"), true, false);
			EntityManager.SetActivityOfEntitiesWithTag(string.Concat(LevelManager.m_currentLevel, ":EditorHandle"), true, false);
			EntityManager.SetActivityOfEntitiesWithTag("ActionBar", true, false);
		}
		if (Input.GetKeyDown(KeyCode.LeftControl))
		{
			GEState.m_specialDown = true;
		}
		if (Input.GetKeyUp(KeyCode.LeftControl) && GEState.m_specialDown)
		{
			GEState.m_specialDown = false;
		}
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			GEEditorCanvasA.m_sculptDepth = SculptDepth.Back;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			GEEditorCanvasA.m_sculptDepth = SculptDepth.Middle;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			GEEditorCanvasA.m_sculptDepth = SculptDepth.Front;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			GEEditorCanvasA.m_sculptDepth = SculptDepth.All;
		}
		else if (Input.GetKeyDown(KeyCode.Comma))
		{
			GEEditorCanvasA.m_sculptSize -= 5;
		}
		else if (Input.GetKeyDown(KeyCode.Period))
		{
			GEEditorCanvasA.m_sculptSize += 5;
		}
		GEEditorCanvasA.m_sculptSize = Mathf.Min(100, Mathf.Max(0, GEEditorCanvasA.m_sculptSize));
		GEPlugin[] plugins = GEState.plugins;
		foreach (GEPlugin gEPlugin in plugins)
		{
			gEPlugin.Execute();
		}
	}

	private static void CreateEditorUI()
	{
		string[] tags = new string[1] { "EditorCanvas" };
		m_mainCanvas = CanvasA.Assemble(Main.uiCamera, -1, "EditorCanvas", null, tags);
		UIS.SetCanvasRelativeSize(m_mainCanvas, 1f, 1f, 0f, 0f);
		UIS.SetCanvasRelativeMarginAndSpacing(m_mainCanvas, 0.02f, 0.01f);
		m_menuArea = GEMenuAreaA.Assemble(m_mainCanvas);
		m_outliner = GEOutlinerA.Assemble(m_mainCanvas, true);
		m_actionArea = GEActionAreaA.Assemble(m_mainCanvas);
		GEDrawButtonsAreaA.Assemble(m_mainCanvas, false);
	}

	public static void RemoveEditorUI()
	{
		EntityManager.RemoveEntitiesByTransformComponentHierarchy(m_menuArea.TC, false);
		EntityManager.RemoveEntitiesByTransformComponentHierarchy(m_actionArea.TC, false);
		EntityManager.RemoveEntitiesByTransformComponentHierarchy(m_outliner.TC, false);
		EntityManager.RemoveEntitiesByTransformComponentHierarchy(m_mainCanvas.TC, false);
		m_menuArea = null;
		m_outliner = null;
		m_actionArea = null;
		m_mainCanvas = null;
		EntityManager.RemoveEntitiesByTag("DrawArea", false);
	}

	public static void ResetOutliner()
	{
		if (GEOutlinerA.m_level != null)
		{
			UIS.RemoveContents(GEOutlinerA.m_level);
			GELevel gELevel = LevelManager.m_currentLevel as GELevel;
			AddItemToOutliner(GEOutlinerA.m_level, gELevel.items, 1);
		}
	}

	private static void AddItemToOutliner(UIC _canvas, List<EIC> _items, int _level)
	{
		for (int i = 0; i < _items.Count; i++)
		{
			EIC eIC = _items[i];
			UIC canvas = GELevelItemA.Assemble(_canvas, false, eIC, _level);
			AddItemToOutliner(canvas, eIC.subItems, _level + 1);
		}
		UIS.PlaceCanvasContent(_canvas);
	}

	public static void AddItemToOutliner(UIC _canvas, EIC _item, int _level)
	{
		UIC canvas = GELevelItemA.Assemble(_canvas, false, _item, _level);
		AddItemToOutliner(canvas, _item.subItems, _level + 1);
		GELevelItemA.UpdateCanvasSizes(_canvas);
	}

	public static void RemoveFromOutliner(UIC _parent, int _index)
	{
		int num = _parent.canvasComponents.Count - 1;
		for (int num2 = num; num2 > -1; num2--)
		{
			int num3 = _parent.canvasComponents[num2].Count - 1;
			for (int num4 = num3; num4 > -1; num4--)
			{
				UIC uIC = _parent.canvasComponents[num2][num4];
				RemoveFromOutliner(uIC, _index);
				if (uIC.identifier == _index)
				{
					_parent.canvasComponents[num2].RemoveAt(num4);
					GELevelItemA.UpdateCanvasSizes(_parent);
					EntityManager.RemoveEntitiesByTransformComponentHierarchy(uIC.TC, false);
					return;
				}
			}
		}
	}

	public static void SelectEditorItem(int _index)
	{
		EIC eIC = GES.m_editorItemComponents.m_array[_index];
		if (!eIC.active)
		{
			return;
		}
		if (GEState.m_addDown || GEState.m_subDown)
		{
			int num = -1;
			for (int i = 0; i < m_selection.Count; i++)
			{
				if (m_selection[i] == eIC)
				{
					num = i;
					break;
				}
			}
			if (num > -1)
			{
				m_selection.RemoveAt(num);
			}
			else
			{
				int count = m_selection.Count;
				for (int num2 = count - 1; num2 > -1; num2--)
				{
					for (EIC container = m_selection[num2].container; container != null; container = container.container)
					{
						if (container == eIC)
						{
							m_selection.RemoveAt(num2);
							break;
						}
					}
				}
				bool flag = false;
				for (EIC container2 = eIC.container; container2 != null; container2 = container2.container)
				{
					if (m_selection.Contains(container2))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					m_selection.Add(eIC);
				}
			}
		}
		else
		{
			m_selection.Clear();
			m_selection.Add(eIC);
		}
		UpdateSelection();
	}

	public static bool RemoveEditorItem(EIC _eic)
	{
		List<EIC> processed = new List<EIC>();
		return RemoveEditorItem(_eic, processed);
	}

	public static bool RemoveEditorItem(EIC _eic, List<EIC> _processed)
	{
		_processed.Add(_eic);
		bool flag = false;
		(LevelManager.m_currentLevel as GELevel).items.Remove(_eic);
		(LevelManager.m_currentLevel as GELevel).connections.Remove(_eic);
		if (_eic.subItems != null)
		{
			while (_eic.subItems.Count > 0)
			{
				int index = _eic.subItems.Count - 1;
				if (!_processed.Contains(_eic.subItems[index]))
				{
					flag = RemoveEditorItem(_eic.subItems[index], _processed);
				}
			}
		}
		if (_eic.gameComponents != null)
		{
			while (_eic.gameComponents.Count > 0)
			{
				int index2 = _eic.gameComponents.Count - 1;
				IComponent component = _eic.gameComponents[index2];
				if (component != null)
				{
					List<IComponent> list = new List<IComponent>();
					list.AddRange(EntityManager.GetComponentsByEntityIndex((ComponentType)112, component.entityIndex));
					list.AddRange(EntityManager.GetComponentsByEntityIndex((ComponentType)105, component.entityIndex));
					list.AddRange(EntityManager.GetComponentsByEntityIndex((ComponentType)104, component.entityIndex));
					for (int i = 0; i < list.Count; i++)
					{
						IControlledComponent controlledComponent = list[i] as IControlledComponent;
						GEConnectionLogic.RemoveConnectionsByAnchoredId(controlledComponent.id, ConnectionSlotType.Any);
					}
					if (component.componentType == (ComponentType)110)
					{
						flag = true;
					}
					if (_eic.gameComponents[index2].entityIndex != -1)
					{
						EntityManager.RemoveEntity(_eic.gameComponents[index2].entityIndex, true);
					}
				}
				_eic.gameComponents.RemoveAt(index2);
			}
		}
		if (_eic.data != null)
		{
			uint id = _eic.data.id;
			if (_eic.identifier != "RailPoint")
			{
				List<EIC> editorItemsWithUniqueId = GES.GetEditorItemsWithUniqueId(id);
				for (int j = 0; j < editorItemsWithUniqueId.Count; j++)
				{
					if (!_processed.Contains(editorItemsWithUniqueId[j]))
					{
						_processed.Add(editorItemsWithUniqueId[j]);
						bool flag2 = RemoveEditorItem(editorItemsWithUniqueId[j], _processed);
						if (!flag)
						{
							flag = flag2;
						}
					}
				}
			}
		}
		if (_eic.container != null)
		{
			_eic.container.subItems.Remove(_eic);
		}
		RemoveFromOutliner(m_outliner, _eic.index);
		EntityManager.RemoveEntity(_eic.entityIndex);
		return flag;
	}

	public static void ResetEditorItem(EIC _eic)
	{
		if (_eic.container != null && _eic.container.itemType == 0)
		{
			ClearEditorItem(_eic.container);
			FillEditorItemHierarchy(_eic.container);
		}
		else
		{
			ClearEditorItem(_eic);
			FillEditorItemHierarchy(_eic);
		}
		if (GEState.editorMode && GEOutlinerA.m_properties.expanded && m_outliner.expanded && m_selection.Count > 0 && _eic == m_selection[0])
		{
			UpdatePropertyBar(_eic);
		}
		if (_eic.identifier == "Rail")
		{
			GES.BuildRailAnchorHandles(_eic);
		}
		else if (_eic.identifier == "RailPoint")
		{
			GES.BuildRailAnchorHandles(_eic.container);
		}
	}

	public static void RemoveEditorItemFromContainer(EIC _eic)
	{
		if (_eic.TC.parent != null)
		{
			TransformS.UnparentComponent(_eic.TC);
		}
		if (_eic.container != null)
		{
			_eic.container.subItems.Remove(_eic);
			_eic.container = null;
		}
		GELevel gELevel = LevelManager.m_currentLevel as GELevel;
		if (gELevel.items.Contains(_eic))
		{
			gELevel.items.Remove(_eic);
		}
	}

	public static void AddEditorItemToContainer(EIC _eic, EIC _container, int index)
	{
		if (_container != null)
		{
			_container.subItems.Insert(Mathf.Min(index, _container.subItems.Count), _eic);
			_eic.container = _container;
			TransformS.ParentComponent(_eic.TC, _container.TC);
			_eic.TC.transform.position = _eic.data.position.ToVector3();
		}
		else
		{
			GELevel gELevel = LevelManager.m_currentLevel as GELevel;
			gELevel.items.Insert(Mathf.Min(index, gELevel.items.Count), _eic);
			_eic.container = null;
		}
	}

	public static void UpdateSelection()
	{
		if (m_gizmo != null)
		{
			EntityManager.RemoveEntitiesByTag("EditorHandle");
			EntityManager.RemoveEntity(m_gizmo.entityIndex);
			m_gizmo = null;
		}
		if (m_selection.Count > 0)
		{
			bool active = true;
			if (GEState.m_addDown || GEState.m_subDown)
			{
				active = false;
			}
			m_gizmo = GETransformGizmoA.Assemble(active);
			if (GEOutlinerA.m_properties.expanded)
			{
				UpdatePropertyBar(m_selection[0]);
			}
			if (m_selection.Count == 1)
			{
				if (m_selection[0].identifier == "Rail")
				{
					GES.BuildRailAnchorHandles(m_selection[0]);
				}
				else if (m_selection[0].identifier == "RailPoint")
				{
					ResetEditorItem(m_selection[0].container);
					GES.BuildRailAnchorHandles(m_selection[0].container);
				}
			}
		}
		else if (GEOutlinerA.m_properties != null)
		{
			UIS.RemoveContents(GEOutlinerA.m_properties);
		}
		List<IComponent> componentsByType = EntityManager.GetComponentsByType(ComponentType.Prefab);
		while (componentsByType.Count > 0)
		{
			int index = componentsByType.Count - 1;
			if ((componentsByType[index] as PrefabC).identifier == "ALISelection")
			{
				PrefabS.RemoveComponent(componentsByType[index] as PrefabC);
			}
			componentsByType.RemoveAt(index);
		}
		if (GEOutlinerA.m_level != null)
		{
			MarkSelectedOutlinerItems(GEOutlinerA.m_level);
		}
	}

	public static void MarkSelectedOutlinerItems(UIC _parent)
	{
		for (int i = 0; i < _parent.canvasComponents.Count; i++)
		{
			bool flag = false;
			for (int j = 0; j < _parent.canvasComponents[i].Count; j++)
			{
				UIC uIC = _parent.canvasComponents[i][j];
				MarkSelectedOutlinerItems(uIC);
				EIC item = GES.m_editorItemComponents.m_array[uIC.identifier];
				if (!m_selection.Contains(item))
				{
					continue;
				}
				Vector2[] rect = DebugDraw.GetRect(uIC.width, uIC.headerHeight, Vector2.zero);
				List<PrefabC> list = PrefabS.CreateFlatPrefabComponentsFromVectorArray(uIC.TC, Vector3.forward * -5f + Vector3.up * (uIC.height * 0.5f + uIC.headerHeight * -0.5f), rect, PrefabS.ColorToUInt(DebugDraw.GetColor(144f, 199f, 71f)), PrefabS.ColorToUInt(DebugDraw.GetColor(144f, 199f, 71f)), ResourceManager.GetMaterial("Solid"), uIC.parent.canvasCamera, "ALISelection");
				if (!uIC.active || (!uIC.parent.expanded && uIC.parent.expandable))
				{
					for (int k = 0; k < list.Count; k++)
					{
						PrefabS.SetVisibility(list[k], false, false);
					}
				}
				if (flag)
				{
					continue;
				}
				UIC parent = uIC.parent;
				while (parent != null && parent.identifier > 0)
				{
					list = PrefabS.CreateFlatPrefabComponentsFromVectorArray(parent.TC, Vector3.forward * -5f + Vector3.up * (parent.height * 0.5f + parent.headerHeight * -0.5f), rect, PrefabS.ColorToUInt(DebugDraw.GetColor(226f, 240f, 207f)), PrefabS.ColorToUInt(DebugDraw.GetColor(226f, 240f, 207f)), ResourceManager.GetMaterial("Solid"), parent.parent.canvasCamera, "ALISelection");
					if (!parent.active || (!parent.parent.expanded && parent.parent.expandable))
					{
						for (int l = 0; l < list.Count; l++)
						{
							PrefabS.SetVisibility(list[l], false, false);
						}
					}
					parent = parent.parent;
				}
				flag = true;
			}
		}
	}

	public static void SetHighlight(bool _highlight, EIC _item)
	{
		if (_highlight)
		{
			if (m_drawHighlightTC != null)
			{
				EntityManager.RemoveEntity(m_drawHighlightTC.entityIndex);
				m_drawHighlightTC = null;
			}
			if (_item.data.dataType == 7)
			{
				m_drawHighlightTC = EntityManager.AddEntityWithTC();
				TransformS.SetPosition(m_drawHighlightTC, _item.data.position.ToVector3());
				TransformS.SetRotation(m_drawHighlightTC, _item.data.rotation.ToVector3());
				PrefabS.CreatePathPrefabComponentFromPolygon(m_drawHighlightTC, Vector3.forward * -1f, (_item.data as ShapeData).polygon, 6f, Color.white, ResourceManager.GetMaterial("Line6"), Main.camera, Position.Inside, true);
			}
		}
		else if (m_drawHighlightTC != null)
		{
			EntityManager.RemoveEntity(m_drawHighlightTC.entityIndex);
			m_drawHighlightTC = null;
		}
	}

	public static void HandleDrawableEditorItem(EIC _eic, Polygon _drawPoly, int _tool)
	{
		if (_eic.data.dataType != 7)
		{
			return;
		}
		ShapeData shapeData = _eic.data as ShapeData;
		Polygon polygon = null;
		Polygon polygon2 = DebugDraw.TransformPolygon(_drawPoly, -_eic.data.position.ToVector3(), 0f);
		Polygon polygon3 = DebugDraw.TransformPolygon(shapeData.polygon, Vector2.zero, _eic.data.rotation.z);
		if (_tool == GEEditorCanvasA.DRAW_ADD)
		{
			polygon = polygon3.Clip(GpcOperation.Union, polygon2);
		}
		else if (_tool == GEEditorCanvasA.DRAW_SUB)
		{
			polygon = polygon3.Clip(GpcOperation.Difference, polygon2);
		}
		if (polygon.NofContours > 0)
		{
			if (_eic.identifier == "Block")
			{
				polygon = DebugDraw.TransformPolygon(polygon, Vector2.zero, 0f - _eic.data.rotation.z);
			}
			shapeData.polygon = polygon;
			ClearEditorItem(_eic);
			FillEditorItemHierarchy(_eic);
			SetHighlight(true, _eic);
		}
	}

	public static void ClearEditorItem(EIC _eic)
	{
		if (_eic.itemType == 1 || _eic.itemType == 2)
		{
			for (int i = 0; i < _eic.subItems.Count; i++)
			{
				ClearEditorItem(_eic.subItems[i]);
			}
			if (_eic.identifier == "Rope" || _eic.identifier == "Bar" || _eic.identifier == "Flexible Rope")
			{
				List<EIC> editorItemsWithUniqueId = GES.GetEditorItemsWithUniqueId(_eic.data.id);
				for (int j = 0; j < editorItemsWithUniqueId.Count; j++)
				{
					if (editorItemsWithUniqueId[j].gameComponents == null || editorItemsWithUniqueId[j] == _eic || editorItemsWithUniqueId[j].gameComponents.Count <= 0 || editorItemsWithUniqueId[j].itemType == 0)
					{
						continue;
					}
					while (editorItemsWithUniqueId[j].gameComponents.Count > 0)
					{
						int index = editorItemsWithUniqueId[j].gameComponents.Count - 1;
						if (editorItemsWithUniqueId[j].gameComponents[index] != null)
						{
							EntityManager.RemoveEntity(editorItemsWithUniqueId[j].gameComponents[index].entityIndex, true);
						}
						editorItemsWithUniqueId[j].gameComponents.RemoveAt(index);
					}
				}
			}
			while (_eic.gameComponents.Count > 0)
			{
				int index2 = _eic.gameComponents.Count - 1;
				if (_eic.gameComponents[index2] != null)
				{
					EntityManager.RemoveEntity(_eic.gameComponents[index2].entityIndex, true);
				}
				_eic.gameComponents.RemoveAt(index2);
			}
		}
		else if (_eic.itemType == 0)
		{
			for (int k = 0; k < _eic.subItems.Count; k++)
			{
				ClearEditorItem(_eic.subItems[k]);
			}
			while (_eic.gameComponents.Count > 0)
			{
				int index3 = _eic.gameComponents.Count - 1;
				EntityManager.RemoveEntity(_eic.gameComponents[index3].entityIndex, true);
				_eic.gameComponents.RemoveAt(index3);
			}
		}
	}

	public static void FillItemBar()
	{
		UIC library = GEOutlinerA.m_library;
		GEPlugin[] plugins = GEState.plugins;
		foreach (GEPlugin gEPlugin in plugins)
		{
			gEPlugin.FillItemBar(library);
		}
		UIC uIC = GELibraryCategoryA.Assemble(library, false, "Camera");
		GELibraryItemA.Assemble(uIC, "Target", 0);
		UIS.PlaceCanvasContent(uIC);
		UIC uIC2 = GELibraryCategoryA.Assemble(library, false, "Shapes");
		GELibraryItemA.Assemble(uIC2, "Voxel Shape", 15);
		GELibraryItemA.Assemble(uIC2, "Block", 2);
		GELibraryItemA.Assemble(uIC2, "Ground", 3);
		GELibraryItemA.Assemble(uIC2, "Background", 4);
		GELibraryItemA.Assemble(uIC2, "Landscape", 5);
		UIS.PlaceCanvasContent(uIC2);
		UIC uIC3 = GELibraryCategoryA.Assemble(library, false, "Constraints");
		GELibraryItemA.Assemble(uIC3, "Bolt", 6);
		GELibraryItemA.Assemble(uIC3, "Motor", 7);
		GELibraryItemA.Assemble(uIC3, "Rail", 8);
		GELibraryItemA.Assemble(uIC3, "Rail Motor", 7);
		GELibraryItemA.Assemble(uIC3, "Rope", 9);
		GELibraryItemA.Assemble(uIC3, "Flexible Rope", 10);
		GELibraryItemA.Assemble(uIC3, "Bar", 11);
		UIS.PlaceCanvasContent(uIC3);
		UIC uIC4 = GELibraryCategoryA.Assemble(library, false, "Triggers");
		GELibraryItemA.Assemble(uIC4, "Area", 10);
		GELibraryItemA.Assemble(uIC4, "Button", 10);
		GELibraryItemA.Assemble(uIC4, "Timer", 10);
		GELibraryItemA.Assemble(uIC4, "Event Dispatcher", 33);
		GELibraryItemA.Assemble(uIC4, "Event Listener", 34);
		UIS.PlaceCanvasContent(uIC4);
		UIC uIC5 = GELibraryCategoryA.Assemble(library, false, "Portals");
		GELibraryItemA.Assemble(uIC5, "Portal", 15);
		UIS.PlaceCanvasContent(uIC5);
		UIC uIC6 = GELibraryCategoryA.Assemble(library, false, "Math");
		GELibraryItemA.Assemble(uIC6, "Vector", 10);
		UIS.PlaceCanvasContent(uIC6);
		UIC uIC7 = GELibraryCategoryA.Assemble(library, false, "Controllers");
		GELibraryItemA.Assemble(uIC7, "Joystick", 32);
		GELibraryItemA.Assemble(uIC7, "DPad", 32);
		GELibraryItemA.Assemble(uIC7, "Accelerometer", 42);
		GELibraryItemA.Assemble(uIC7, "Sensor", 10);
		GELibraryItemA.Assemble(uIC7, "Draggable", 43);
		GELibraryItemA.Assemble(uIC7, "Flickable", 44);
		GELibraryItemA.Assemble(uIC7, "Slingable", 43);
		GELibraryItemA.Assemble(uIC7, "Sliceable", 45);
		GELibraryItemA.Assemble(uIC7, "Finger Controller Area", 10);
		GELibraryItemA.Assemble(uIC7, "Control Scheme", 10);
		UIS.PlaceCanvasContent(uIC7);
		UIC uIC8 = GELibraryCategoryA.Assemble(library, false, "Level Control");
		GELibraryItemA.Assemble(uIC8, "Change Level", 34);
		GELibraryItemA.Assemble(uIC8, "Next Level", 15);
		GELibraryItemA.Assemble(uIC8, "Reset All Levels", 35);
		GELibraryItemA.Assemble(uIC8, "Remove All And Reset Current", 35);
		GELibraryItemA.Assemble(uIC8, "Append Level", 36);
		GELibraryItemA.Assemble(uIC8, "Remove Level", 37);
		GELibraryItemA.Assemble(uIC8, "Enable By Tag", 37);
		GELibraryItemA.Assemble(uIC8, "Disable By Tag", 37);
		UIS.PlaceCanvasContent(uIC8);
		UIC uIC9 = GELibraryCategoryA.Assemble(library, false, "Physics Affectors");
		GELibraryItemA.Assemble(uIC9, "Apply Impulse", 34);
		GELibraryItemA.Assemble(uIC9, "Apply Velocity", 34);
		GELibraryItemA.Assemble(uIC9, "Apply Angular Velocity", 34);
		UIS.PlaceCanvasContent(uIC9);
		UIS.PlaceCanvasContent(library);
	}

	public static List<EIC> CreateNewEditorItem(EIC _container, string _identifier, Vector3 _pos, Vector3 _rot, Vector3 _sca)
	{
		return CreateNewEditorItem(_container, _identifier, _pos, _rot, _sca, false);
	}

	public static List<EIC> CreateNewEditorItem(EIC _container, string _identifier, Vector3 _pos, Vector3 _rot, Vector3 _sca, bool _isRedo)
	{
		GELevel gELevel = LevelManager.m_currentLevel as GELevel;
		List<EIC> list = new List<EIC>();
		bool flag = false;
		GEPlugin[] plugins = GEState.plugins;
		foreach (GEPlugin gEPlugin in plugins)
		{
			flag = gEPlugin.CreateNewEditorItem(gELevel, list, _container, _identifier, _pos, _rot, _sca);
			if (flag)
			{
				break;
			}
		}
		if (!flag)
		{
			switch (_identifier)
			{
			case "Apply Impulse":
			case "Apply Force":
			case "Apply Velocity":
			case "Apply Angular Velocity":
				list.Add(GEPhysicsAffectorA.CreateNewEditorItem(_container, _identifier, _pos, _rot, _sca));
				break;
			case "Voxel Shape":
				list.Add(GEVoxelShapeA.CreateNewEditorItem(_container, _identifier, _pos, _rot, _sca));
				break;
			case "Ground":
			case "Background":
			case "Landscape":
				list.Add(GEShapeA.CreateNewEditorItem(_container, _identifier, _pos, _rot, _sca));
				break;
			case "Block":
				list.Add(GEBlockA.CreateNewEditorItem(_container, _identifier, _pos, _rot, _sca));
				break;
			case "Target":
				list.Add(GECameraTargetA.CreateNewEditorItem(_container, _identifier, _pos, _rot, _sca));
				break;
			case "Sensor":
				list.Add(GESensorControllerA.CreateNewEditorItem(_container, _identifier, _pos, _rot, _sca));
				break;
			case "Area":
				list.Add(GEAreaTriggerA.CreateNewEditorItem(_container, _identifier, _pos, _rot, _sca));
				break;
			case "Change Level":
			case "Next Level":
			case "Reset All Levels":
			case "Remove All And Reset Current":
			case "Append Level":
			case "Remove Level":
			case "Enable By Tag":
			case "Disable By Tag":
				list.Add(GELevelControlA.CreateNewEditorItem(_container, _identifier, _pos, _rot, _sca));
				break;
			case "Bolt":
				list.AddRange(GEBoltA.CreateNewEditorItem(_container, _identifier, _pos, _rot, _sca));
				break;
			case "Motor":
				list.Add(GERotaryMotorA.CreateNewEditorItem(_container, _identifier, _pos, _rot, _sca));
				break;
			case "Rail Motor":
				list.Add(GERailMotorA.CreateNewEditorItem(_container, _identifier, _pos, _rot, _sca));
				break;
			case "Rail":
				list.AddRange(GERailA.CreateNewEditorItem(_container, _identifier, _pos, _rot, _sca));
				break;
			case "Rope":
			case "Flexible Rope":
			case "Bar":
				list.AddRange(GERopeA.CreateNewEditorItem(_container, _identifier, _pos, _rot, _sca));
				break;
			case "Portal":
				list.AddRange(GEPortalA.CreateNewEditorItem(_container, _identifier, _pos, _rot, _sca));
				break;
			case "Draggable":
				list.Add(GEDraggableControllerA.CreateNewEditorItem(_container, _identifier, _pos, _rot, _sca));
				break;
			case "Flickable":
				list.Add(GEFlickableControllerA.CreateNewEditorItem(_container, _identifier, _pos, _rot, _sca));
				break;
			case "Slingable":
				list.Add(GESlingableControllerA.CreateNewEditorItem(_container, _identifier, _pos, _rot, _sca));
				break;
			case "Sliceable":
				list.Add(GESliceableControllerA.CreateNewEditorItem(_container, _identifier, _pos, _rot, _sca));
				break;
			case "Control Scheme":
				list.Add(GEControlSchemeA.CreateNewEditorItem(_container, _identifier, _pos, _rot, _sca));
				break;
			case "Joystick":
				list.Add(GEJoystickControllerA.CreateNewEditorItem(_container, _identifier, _pos, _rot, _sca));
				break;
			case "DPad":
				list.Add(GEDPadControllerA.CreateNewEditorItem(_container, _identifier, _pos, _rot, _sca));
				break;
			case "Accelerometer":
				list.Add(GETiltControllerA.CreateNewEditorItem(_container, _identifier, _pos, _rot, _sca));
				break;
			case "Event Listener":
				list.Add(GEEventListenerA.CreateNewEditorItem(_container, _identifier, _pos, _rot, _sca));
				break;
			case "Event Dispatcher":
				list.Add(GEEventDispatcherA.CreateNewEditorItem(_container, _identifier, _pos, _rot, _sca));
				break;
			case "Finger Controller Area":
				list.Add(GEFingerControllerAreaA.CreateNewEditorItem(_container, _identifier, _pos, _rot, _sca));
				break;
			case "Button":
				list.Add(GEButtonTriggerA.CreateNewEditorItem(_container, _identifier, _pos, _rot, _sca));
				break;
			case "Timer":
				list.Add(GETimerTriggerA.CreateNewEditorItem(_container, _identifier, _pos, _rot, _sca));
				break;
			case "Vector":
			case "String":
			case "Boolean":
				list.Add(GEMathA.CreateNewEditorItem(_container, _identifier, _pos, _rot, _sca));
				break;
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			if (_container == null)
			{
				gELevel.items.Add(list[j]);
			}
		}
		return list;
	}

	public static EIC CreateLoadedEditorItem(EIC _container, EIC _loadedItem)
	{
        if (_loadedItem.data.id > GES.m_uniqueId)
		{
			GES.m_uniqueId = _loadedItem.data.id;
		}
		if (!GEState.editorMode)
		{
			int num = Mathf.Max(LevelManager.m_levels.Count - 1, 0);
			_loadedItem.data.id = _loadedItem.data.id + (uint)(10000 * num);
		}
		_loadedItem.data.name = _loadedItem.identifier + _loadedItem.data.id;
		GELevel level = LevelManager.m_currentLevel as GELevel;
		EIC eIC = null;
		GEPlugin[] plugins = GEState.plugins;
		foreach (GEPlugin gEPlugin in plugins)
		{
			eIC = gEPlugin.CreateLoadedEditorItem(level, _container, _loadedItem);
			if (eIC != null)
			{
				break;
			}
		}
		if (eIC == null)
		{
			switch (_loadedItem.identifier)
			{
			case "Apply Impulse":
			case "Apply Force":
			case "Apply Velocity":
			case "Apply Angular Velocity":
				eIC = GEPhysicsAffectorA.CreateLoadedEditorItem(_container, _loadedItem);
				break;
			case "Voxel Shape":
				eIC = GEVoxelShapeA.CreateLoadedEditorItem(_container, _loadedItem);
				break;
			case "EditorConnection":
				eIC = GEConnectionA.CreateLoadedEditorItem(_container, _loadedItem);
				break;
			case "Ground":
			case "Background":
			case "Landscape":
				eIC = GEShapeA.CreateLoadedEditorItem(_container, _loadedItem);
				break;
			case "Block":
				eIC = GEBlockA.CreateLoadedEditorItem(_container, _loadedItem);
				break;
			case "Target":
				eIC = GECameraTargetA.CreateLoadedEditorItem(_container, _loadedItem);
				break;
			case "Sensor":
				eIC = GESensorControllerA.CreateLoadedEditorItem(_container, _loadedItem);
				break;
			case "Area":
				eIC = GEAreaTriggerA.CreateLoadedEditorItem(_container, _loadedItem);
				break;
			case "Change Level":
			case "Next Level":
			case "Reset All Levels":
			case "Remove All And Reset Current":
			case "Append Level":
			case "Remove Level":
			case "Enable By Tag":
			case "Disable By Tag":
				eIC = GELevelControlA.CreateLoadedEditorItem(_container, _loadedItem);
				break;
			case "Bolt":
				eIC = GEBoltA.CreateLoadedEditorItem(_container, _loadedItem);
				break;
			case "Motor":
				eIC = GERotaryMotorA.CreateLoadedEditorItem(_container, _loadedItem);
				break;
			case "Rail Motor":
				eIC = GERailMotorA.CreateLoadedEditorItem(_container, _loadedItem);
				break;
			case "Rail":
				eIC = GERailA.CreateLoadedEditorItem(_container, _loadedItem);
				break;
			case "RailPoint":
				eIC = GERailA.CreateLoadedRailPointEditorItem(_container, _loadedItem);
				break;
			case "Rope":
			case "Flexible Rope":
			case "Bar":
				eIC = GERopeA.CreateLoadedEditorItem(_container, _loadedItem);
				break;
			case "Portal":
				eIC = GEPortalA.CreateLoadedEditorItem(_container, _loadedItem);
				break;
			case "Draggable":
				eIC = GEDraggableControllerA.CreateLoadedEditorItem(_container, _loadedItem);
				break;
			case "Flickable":
				eIC = GEFlickableControllerA.CreateLoadedEditorItem(_container, _loadedItem);
				break;
			case "Slingable":
				eIC = GESlingableControllerA.CreateLoadedEditorItem(_container, _loadedItem);
				break;
			case "Sliceable":
				eIC = GESliceableControllerA.CreateLoadedEditorItem(_container, _loadedItem);
				break;
			case "Control Scheme":
				eIC = GEControlSchemeA.CreateLoadedEditorItem(_container, _loadedItem);
				break;
			case "Joystick":
				eIC = GEJoystickControllerA.CreateLoadedEditorItem(_container, _loadedItem);
				break;
			case "DPad":
				eIC = GEDPadControllerA.CreateLoadedEditorItem(_container, _loadedItem);
				break;
			case "Accelerometer":
				eIC = GETiltControllerA.CreateLoadedEditorItem(_container, _loadedItem);
				break;
			case "Event Listener":
				eIC = GEEventListenerA.CreateLoadedEditorItem(_container, _loadedItem);
				break;
			case "Event Dispatcher":
				eIC = GEEventDispatcherA.CreateLoadedEditorItem(_container, _loadedItem);
				break;
			case "Finger Controller Area":
				eIC = GEFingerControllerAreaA.CreateLoadedEditorItem(_container, _loadedItem);
				break;
			case "Button":
				eIC = GEButtonTriggerA.CreateLoadedEditorItem(_container, _loadedItem);
				break;
			case "Timer":
				eIC = GETimerTriggerA.CreateLoadedEditorItem(_container, _loadedItem);
				break;
			case "Vector":
			case "String":
			case "Boolean":
				eIC = GEMathA.CreateLoadedEditorItem(_container, _loadedItem);
				break;
			}
		}
		if (eIC != null)
		{
			eIC.isDrawable = _loadedItem.isDrawable;
			eIC.isRealtimeMovable = _loadedItem.isRealtimeMovable;
			eIC.isRotateable = _loadedItem.isRotateable;
			eIC.isScaleable = _loadedItem.isScaleable;
			eIC.isScaleUnified = _loadedItem.isScaleUnified;
			eIC.connectionMode = _loadedItem.connectionMode;
			eIC.horizontalAnchor = _loadedItem.horizontalAnchor;
			eIC.horizontalIsAbsolute = _loadedItem.horizontalIsAbsolute;
			eIC.verticalAnchor = _loadedItem.verticalAnchor;
			eIC.verticalIsAbsolute = _loadedItem.verticalIsAbsolute;
			eIC.referenceWidth = _loadedItem.referenceWidth;
			eIC.referenceHeight = _loadedItem.referenceHeight;
		}
		return eIC;
	}

	public static void FillEditorItemHierarchy(EIC _eic)
	{
		if (_eic == null || (_eic.itemType != 1 && _eic.itemType != 2 && _eic.itemType != 0))
		{
			return;
		}
		bool flag = false;
		GEPlugin[] plugins = GEState.plugins;
		foreach (GEPlugin gEPlugin in plugins)
		{
			flag = gEPlugin.FillEditorItem(_eic);
			if (flag)
			{
				break;
			}
		}
		if (!flag)
		{
			switch (_eic.identifier)
			{
			case "Apply Impulse":
			case "Apply Force":
			case "Apply Velocity":
			case "Apply Angular Velocity":
				GEPhysicsAffectorA.FillEditorItem(_eic);
				break;
			case "Voxel Shape":
				GEVoxelShapeA.FillEditorItem(_eic);
				break;
			case "EditorConnection":
				GEConnectionA.FillEditorItem(_eic);
				break;
			case "Ground":
			case "Background":
			case "Landscape":
				GEShapeA.FillEditorItem(_eic);
				break;
			case "Block":
				GEBlockA.FillEditorItem(_eic);
				break;
			case "Target":
				GECameraTargetA.FillEditorItem(_eic);
				break;
			case "Sensor":
				GESensorControllerA.FillEditorItem(_eic);
				break;
			case "Area":
				GEAreaTriggerA.FillEditorItem(_eic);
				break;
			case "Change Level":
			case "Next Level":
			case "Reset All Levels":
			case "Remove All And Reset Current":
			case "Append Level":
			case "Remove Level":
			case "Enable By Tag":
			case "Disable By Tag":
				GELevelControlA.FillEditorItem(_eic);
				break;
			case "Bolt":
				GEBoltA.FillEditorItem(_eic);
				break;
			case "Motor":
				GERotaryMotorA.FillEditorItem(_eic);
				break;
			case "Rail Motor":
				GERailMotorA.FillEditorItem(_eic);
				break;
			case "Rail":
				GERailA.FillEditorItem(_eic);
				break;
			case "Rope":
			case "Flexible Rope":
			case "Bar":
				GERopeA.FillEditorItem(_eic);
				break;
			case "Portal":
				GEPortalA.FillEditorItem(_eic);
				break;
			case "Draggable":
				GEDraggableControllerA.FillEditorItem(_eic);
				break;
			case "Flickable":
				GEFlickableControllerA.FillEditorItem(_eic);
				break;
			case "Slingable":
				GESlingableControllerA.FillEditorItem(_eic);
				break;
			case "Sliceable":
				GESliceableControllerA.FillEditorItem(_eic);
				break;
			case "Control Scheme":
				GEControlSchemeA.FillEditorItem(_eic);
				break;
			case "Joystick":
				GEJoystickControllerA.FillEditorItem(_eic);
				break;
			case "DPad":
				GEDPadControllerA.FillEditorItem(_eic);
				break;
			case "Accelerometer":
				GETiltControllerA.FillEditorItem(_eic);
				break;
			case "Event Listener":
				GEEventListenerA.FillEditorItem(_eic);
				break;
			case "Event Dispatcher":
				GEEventDispatcherA.FillEditorItem(_eic);
				break;
			case "Finger Controller Area":
				GEFingerControllerAreaA.FillEditorItem(_eic);
				break;
			case "Button":
				GEButtonTriggerA.FillEditorItem(_eic);
				break;
			case "Timer":
				GETimerTriggerA.FillEditorItem(_eic);
				break;
			case "Vector":
			case "String":
			case "Boolean":
				GEMathA.FillEditorItem(_eic);
				break;
			}
		}
		for (int j = 0; j < _eic.subItems.Count; j++)
		{
			FillEditorItemHierarchy(_eic.subItems[j]);
		}
		GES.SetContainerPosition(_eic, false);
	}

	public static void UpdatePropertyBar(EIC _eic)
	{
		UIS.RemoveContents(GEOutlinerA.m_properties);
		UIS.SetCanvasAbsoluteMarginAndSpacing(GEOutlinerA.m_properties, 10f, 5f);
		if (m_selection.Count == 1)
		{
			if (_eic.camera == Main.uiCamera)
			{
				PopulatePropertyBar(_eic, GEOutlinerA.m_properties);
			}
			bool flag = false;
			GEPlugin[] plugins = GEState.plugins;
			foreach (GEPlugin gEPlugin in plugins)
			{
				flag = gEPlugin.UpdatePropertyBar(_eic, GEOutlinerA.m_properties);
				if (flag)
				{
					break;
				}
			}
			if (!flag)
			{
				switch (_eic.identifier)
				{
				case "Apply Impulse":
				case "Apply Force":
				case "Apply Velocity":
				case "Apply Angular Velocity":
					GESensorControllerA.PopulatePropertyBar(_eic, GEOutlinerA.m_properties);
					break;
				case "Voxel Shape":
					GEVoxelShapeA.PopulatePropertyBar(_eic, GEOutlinerA.m_properties);
					break;
				case "Ground":
				case "Background":
				case "Landscape":
					GEShapeA.PopulatePropertyBar(_eic, GEOutlinerA.m_properties);
					break;
				case "Block":
					GEBlockA.PopulatePropertyBar(_eic, GEOutlinerA.m_properties);
					break;
				case "Target":
					GECameraTargetA.PopulatePropertyBar(_eic, GEOutlinerA.m_properties);
					break;
				case "Sensor":
					GESensorControllerA.PopulatePropertyBar(_eic, GEOutlinerA.m_properties);
					break;
				case "Area":
					GEAreaTriggerA.PopulatePropertyBar(_eic, GEOutlinerA.m_properties);
					break;
				case "Change Level":
				case "Next Level":
				case "Reset All Levels":
				case "Remove All And Reset Current":
				case "Append Level":
				case "Remove Level":
				case "Enable By Tag":
				case "Disable By Tag":
					GELevelControlA.PopulatePropertyBar(_eic, GEOutlinerA.m_properties);
					break;
				case "Bolt":
					GEBoltA.PopulatePropertyBar(_eic, GEOutlinerA.m_properties);
					break;
				case "Motor":
					GERotaryMotorA.PopulatePropertyBar(_eic, GEOutlinerA.m_properties);
					break;
				case "Rail Motor":
					GERailMotorA.PopulatePropertyBar(_eic, GEOutlinerA.m_properties);
					break;
				case "Rail":
					GERailA.PopulatePropertyBar(_eic, GEOutlinerA.m_properties);
					break;
				case "RailPoint":
					GERailA.PopulatePointPropertyBar(_eic, GEOutlinerA.m_properties);
					break;
				case "Rope":
				case "Flexible Rope":
				case "Bar":
					GERopeA.PopulatePropertyBar(_eic, GEOutlinerA.m_properties);
					break;
				case "Flickable":
					GEFlickableControllerA.PopulatePropertyBar(_eic, GEOutlinerA.m_properties);
					break;
				case "Slingable":
					GESlingableControllerA.PopulatePropertyBar(_eic, GEOutlinerA.m_properties);
					break;
				case "DPad":
					GEDPadControllerA.PopulatePropertyBar(_eic, GEOutlinerA.m_properties);
					break;
				case "Event Listener":
					GEEventListenerA.PopulatePropertyBar(_eic, GEOutlinerA.m_properties);
					break;
				case "Event Dispatcher":
					GEEventDispatcherA.PopulatePropertyBar(_eic, GEOutlinerA.m_properties);
					break;
				case "Finger Controller Area":
					GEFingerControllerAreaA.PopulatePropertyBar(_eic, GEOutlinerA.m_properties);
					break;
				case "Button":
					GEButtonTriggerA.PopulatePropertyBar(_eic, GEOutlinerA.m_properties);
					break;
				case "Timer":
					GETimerTriggerA.PopulatePropertyBar(_eic, GEOutlinerA.m_properties);
					break;
				}
			}
		}
		UIS.PlaceCanvasContent(GEOutlinerA.m_properties);
	}

	public static void PopulatePropertyBar(EIC _eiC, UIC _propertyBar)
	{
		string[] tags = new string[1] { "propertyBar" };
		TriggerData triggerData = _eiC.data as TriggerData;
		Camera canvasCamera = _propertyBar.canvasCamera;
		UIC component = RadioButtonA.Assemble(canvasCamera, "Center", HandlePropertyChange, null, true, Align.Bottom, 1f, _eiC.horizontalAnchor == 0, 0, 101, tags);
		UIC component2 = RadioButtonA.Assemble(canvasCamera, "Left", HandlePropertyChange, null, true, Align.Bottom, 1f, _eiC.horizontalAnchor == 1, 1, 101, tags);
		UIC component3 = RadioButtonA.Assemble(canvasCamera, "Right", HandlePropertyChange, null, true, Align.Bottom, 1f, _eiC.horizontalAnchor == 2, 2, 101, tags);
		UIC component4 = CheckBoxA.Assemble(canvasCamera, "HAbsolute", HandlePropertyChange, null, true, Align.Right, 1f, _eiC.horizontalIsAbsolute, tags);
		UIC component5 = RadioButtonA.Assemble(canvasCamera, "Middle", HandlePropertyChange, null, true, Align.Bottom, 1f, _eiC.verticalAnchor == 0, 0, 102, tags);
		UIC component6 = RadioButtonA.Assemble(canvasCamera, "Top", HandlePropertyChange, null, true, Align.Bottom, 1f, _eiC.verticalAnchor == 1, 1, 102, tags);
		UIC component7 = RadioButtonA.Assemble(canvasCamera, "Bottom", HandlePropertyChange, null, true, Align.Bottom, 1f, _eiC.verticalAnchor == 2, 2, 102, tags);
		UIC component8 = CheckBoxA.Assemble(canvasCamera, "VAbsolute", HandlePropertyChange, null, true, Align.Right, 1f, _eiC.verticalIsAbsolute, tags);
		UIS.AddToCanvasGrid(LabelA.Assemble(canvasCamera, "Horizontal Anchor", tags), _propertyBar, true);
		UIS.AddToCanvasGrid(component2, _propertyBar, true);
		UIS.AddToCanvasGrid(component, _propertyBar, false);
		UIS.AddToCanvasGrid(component3, _propertyBar, false);
		UIS.AddToCanvasGrid(component4, _propertyBar, true);
		UIS.MoveCursor(_propertyBar, 0f, -15f);
		UIS.AddToCanvasGrid(LabelA.Assemble(canvasCamera, "Vertical Anchor", tags), _propertyBar, true);
		UIS.AddToCanvasGrid(component6, _propertyBar, true);
		UIS.AddToCanvasGrid(component5, _propertyBar, false);
		UIS.AddToCanvasGrid(component7, _propertyBar, false);
		UIS.AddToCanvasGrid(component8, _propertyBar, true);
		UIS.MoveCursor(_propertyBar, 0f, -15f);
	}

	public static void HandlePropertyChange(EventC _c)
	{
		EIC eIC = m_selection[0];
		switch (_c.identifier)
		{
		case "HAbsolute":
			eIC.horizontalIsAbsolute = (bool)_c.properties["checked"];
			break;
		case "VAbsolute":
			eIC.verticalIsAbsolute = (bool)_c.properties["checked"];
			break;
		case "Center":
		case "Left":
		case "Right":
			eIC.horizontalAnchor = UIS.GetValueFromRadioButtonGroup(101);
			break;
		case "Middle":
		case "Top":
		case "Bottom":
			eIC.verticalAnchor = UIS.GetValueFromRadioButtonGroup(102);
			break;
		}
	}
}
