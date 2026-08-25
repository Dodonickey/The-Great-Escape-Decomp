using System.Collections.Generic;
using UnityEngine;

public static class UIS
{
	private static GenericArray<UIC> m_components;
#if UNITY_IOS//Using this for now will probably remove once a cross platform solution is made
	private static iPhoneKeyboard m_keyboard;
#endif
	private static string m_typed;

	private static Vector3 m_origialTextFieldLocalPosition;

	public static void Initialize(int _maxComponentCount)
	{
		m_components = new GenericArray<UIC>(_maxComponentCount);
		for (int i = 0; i < _maxComponentCount; i++)
		{
			UIC uIC = new UIC();
			m_components.m_array[i] = uIC;
			uIC.entityIndex = -1;
			uIC.index = i;
			uIC.componentType = ComponentType.UI;
			uIC.TC = null;
			uIC.contentTC = null;
			uIC.cameraTC = null;
			uIC.TAC = null;
			uIC.EC = null;
			uIC.backgroundPCs = new List<PrefabC>();
			uIC.foregroundPCs = new List<PrefabC>();
			uIC.outlinePCs = new List<PrefabC>();
			uIC.textPCs = new List<PrefabC>();
			uIC.controlledUICs = new List<UIC>();
			uIC.controlledUICDirs = new List<bool>();
			uIC.uiComponentType = UIComponentType.None;
			uIC.enabled = true;
			uIC.label = string.Empty;
			uIC.draggable = false;
			uIC.dragOffset = Vector2.zero;
			uIC.isDragged = false;
			uIC.limitedDrag = false;
			uIC.minX = (float)Screen.width * -0.5f;
			uIC.maxX = (float)Screen.width * 0.5f;
			uIC.minY = (float)Screen.height * -0.5f;
			uIC.maxY = (float)Screen.height * 0.5f;
			uIC.width = 100f;
			uIC.height = 70f;
			uIC.radius = 70f;
			uIC.maxValX = 1f;
			uIC.minValX = 0f;
			uIC.currentValX = 0f;
			uIC.maxValY = 1f;
			uIC.minValY = 0f;
			uIC.currentValY = 0f;
			uIC.snap = false;
			uIC.snapPoints = 0;
			uIC.snapPointDistanceX = 0f;
			uIC.snapPointDistanceY = 0f;
			uIC.currentSnapIndexX = 0;
			uIC.currentSnapIndexY = 0;
			uIC.separateRenderSpace = false;
			uIC.canvasCamera = null;
			uIC.canvasEventC = null;
			uIC.canvasWidth = m_components.m_array[i].width;
			uIC.canvasHeight = m_components.m_array[i].height;
			uIC.scrollX = 0f;
			uIC.scrollY = 0f;
			uIC.scrollInertiaX = 0f;
			uIC.scrollInertiaY = 0f;
			uIC.contentSpacing = 0f;
			uIC.contentMargin = 0f;
			uIC.startContentX = 0f;
			uIC.startContentY = 0f;
			uIC.currentContentX = 0f;
			uIC.currentContentY = 0f;
			uIC.nextContentX = 0f;
			uIC.nextContentY = 0f;
			uIC.contentHAlign = Align.Left;
			uIC.contentVAlign = Align.Top;
			uIC.currentPage = 0;
			uIC.expandable = false;
			uIC.expanded = false;
			uIC.intent = 0f;
			uIC.canvasComponents = new List<List<UIC>>();
			uIC.isChecked = false;
			uIC.isSelected = false;
			uIC.radioButtonGroup = 0;
			uIC.radioButtonValue = 0;
			uIC.textC = null;
			uIC.isEditing = false;
			uIC.isInt = false;
			uIC.currentVal = 0f;
			uIC.minVal = 0f;
			uIC.maxVal = 0f;
		}
	}

	public static UIC AddComponent(TransformC _tc, UIComponentType _uiComponentType)
	{
		int num = m_components.AddItem();
		UIC uIC = m_components.m_array[num];
		uIC.entityIndex = _tc.entityIndex;
		uIC.active = true;
		uIC.TC = _tc;
		uIC.uiComponentType = _uiComponentType;
		EntityManager.m_entities.m_array[uIC.entityIndex].components.Add(uIC);
		return uIC;
	}

	public static void RemoveComponent(IComponent _c)
	{
		UIC uIC = _c as UIC;
		uIC.active = false;
		if (uIC.separateRenderSpace)
		{
			Object.Destroy(uIC.canvasCamera.gameObject);
		}
		uIC.canvasCamera = null;
		uIC.TC = null;
		uIC.contentTC = null;
		uIC.cameraTC = null;
		uIC.TAC = null;
		uIC.EC = null;
		uIC.parent = null;
		uIC.backgroundPCs = new List<PrefabC>();
		uIC.foregroundPCs = new List<PrefabC>();
		uIC.outlinePCs = new List<PrefabC>();
		uIC.textPCs = new List<PrefabC>();
		uIC.controlledUICs = new List<UIC>();
		uIC.controlledUICDirs = new List<bool>();
		uIC.uiComponentType = UIComponentType.None;
		uIC.enabled = true;
		uIC.label = string.Empty;
		uIC.draggable = false;
		uIC.dragOffset = Vector2.zero;
		uIC.isDragged = false;
		uIC.limitedDrag = false;
		uIC.minX = (float)Screen.width * -0.5f;
		uIC.maxX = (float)Screen.width * 0.5f;
		uIC.minY = (float)Screen.height * -0.5f;
		uIC.maxY = (float)Screen.height * 0.5f;
		uIC.width = 100f;
		uIC.height = 70f;
		uIC.radius = 70f;
		uIC.maxValX = 1f;
		uIC.minValX = 0f;
		uIC.currentValX = 0f;
		uIC.maxValY = 1f;
		uIC.minValY = 0f;
		uIC.currentValY = 0f;
		uIC.snap = false;
		uIC.snapPoints = 0;
		uIC.snapPointDistanceX = 0f;
		uIC.snapPointDistanceY = 0f;
		uIC.currentSnapIndexX = 0;
		uIC.currentSnapIndexY = 0;
		uIC.separateRenderSpace = false;
		uIC.canvasCamera = null;
		uIC.canvasEventC = null;
		uIC.canvasWidth = uIC.width;
		uIC.canvasHeight = uIC.height;
		uIC.scrollX = 0f;
		uIC.scrollY = 0f;
		uIC.scrollInertiaX = 0f;
		uIC.scrollInertiaY = 0f;
		uIC.contentSpacing = 0f;
		uIC.contentMargin = 0f;
		uIC.startContentX = 0f;
		uIC.startContentY = 0f;
		uIC.currentContentX = 0f;
		uIC.currentContentY = 0f;
		uIC.nextContentX = 0f;
		uIC.nextContentY = 0f;
		uIC.contentHAlign = Align.Left;
		uIC.contentVAlign = Align.Top;
		uIC.currentPage = 0;
		uIC.expandable = false;
		uIC.expanded = false;
		uIC.intent = 0f;
		uIC.canvasComponents = new List<List<UIC>>();
		uIC.isChecked = false;
		uIC.isSelected = false;
		uIC.radioButtonGroup = 0;
		uIC.radioButtonValue = 0;
		uIC.textC = null;
		uIC.isEditing = false;
		uIC.isInt = false;
		uIC.currentVal = 0f;
		uIC.minVal = 0f;
		uIC.maxVal = 0f;
		m_components.RemoveItem(uIC.index);
		EntityManager.m_entities.m_array[uIC.entityIndex].components.Remove(uIC);
		uIC.entityIndex = -1;
	}

	public static void ResetCursor(UIC _canvas)
	{
		if (_canvas.contentHAlign == Align.Left)
		{
			_canvas.startContentX = _canvas.width * -0.5f + _canvas.contentMargin;
		}
		if (_canvas.contentVAlign == Align.Top)
		{
			_canvas.startContentY = _canvas.height * 0.5f - _canvas.contentMargin - _canvas.headerHeight;
		}
		_canvas.currentContentX = _canvas.startContentX;
		_canvas.currentContentY = _canvas.startContentY;
		_canvas.nextContentX = _canvas.startContentX;
		_canvas.nextContentY = _canvas.startContentY;
	}

	public static void MoveCursor(UIC _canvas, float _x, float _y)
	{
		_canvas.currentContentX += _x;
		_canvas.currentContentY += _y;
		_canvas.nextContentX += _x;
		_canvas.nextContentY += _y;
	}

	public static void SetCanvasAlign(UIC _canvas, Align _hAlign, Align _vAlign)
	{
		if (_hAlign == Align.Left)
		{
			_canvas.startContentX = _canvas.width * -0.5f + _canvas.contentMargin;
		}
		if (_vAlign == Align.Top)
		{
			_canvas.startContentY = _canvas.height * 0.5f - _canvas.contentMargin - _canvas.headerHeight;
		}
		_canvas.contentHAlign = _hAlign;
		_canvas.contentVAlign = _vAlign;
		_canvas.currentContentX = _canvas.startContentX;
		_canvas.nextContentX = _canvas.startContentX;
		_canvas.currentContentY = _canvas.startContentY;
		_canvas.nextContentY = _canvas.startContentY;
	}

	public static void SetCanvasExpandable(UIC _canvas, bool _expandable, bool _expanded)
	{
		_canvas.expandable = _expandable;
		_canvas.expanded = _expanded;
	}

	public static void AddToCanvas(UIC _component, UIC _canvas, Vector3 _position)
	{
		_component.parent = _canvas;
		TransformS.ParentComponent(_component.TC, _canvas.contentTC, _position);
		if (_component.TAC != null && _canvas.canvasCamera != null)
		{
			_component.TAC.camera = _canvas.canvasCamera;
		}
	}

	public static void AddToCanvasGrid(UIC _component, UIC _canvas, bool _toNewRow)
	{
		if (_toNewRow)
		{
			AddToCanvasGrid(_component, _canvas, _canvas.canvasComponents.Count, 0);
		}
		else if (_canvas.canvasComponents.Count > 0)
		{
			AddToCanvasGrid(_component, _canvas, _canvas.canvasComponents.Count - 1, _canvas.canvasComponents[_canvas.canvasComponents.Count - 1].Count);
		}
		else
		{
			AddToCanvasGrid(_component, _canvas, 0, 0);
		}
	}

	public static void AddToCanvasGrid(UIC _component, UIC _canvas, int _row, int _col)
	{
		AddToCanvasGrid(_component, _canvas, _row, _col, false);
	}

	public static void AddToCanvasGrid(UIC _component, UIC _canvas, int _row, int _col, bool _insert)
	{
		_component.parent = _canvas;
		TransformS.ParentComponent(_component.TC, _canvas.contentTC, Vector3.zero);
		if (_canvas.canvasCamera != null && _component.TAC != null)
		{
			_component.TAC.camera = _canvas.canvasCamera;
		}
		if (_canvas.separateRenderSpace && _component.TAC != null)
		{
			int minX = Mathf.RoundToInt(_canvas.TC.transform.position.x - _canvas.width * 0.5f + (float)Screen.width * 0.5f);
			int maxX = Mathf.RoundToInt(_canvas.TC.transform.position.x + _canvas.width * 0.5f + (float)Screen.width * 0.5f);
			int minY = Mathf.RoundToInt(_canvas.TC.transform.position.y - _canvas.height * 0.5f + (float)Screen.height * 0.5f);
			int maxY = Mathf.RoundToInt(_canvas.TC.transform.position.y + _canvas.height * 0.5f + (float)Screen.height * 0.5f);
			TouchAreaS.SetClip(_component.TAC, minX, maxX, minY, maxY);
		}
		int index = _row;
		if (_row >= _canvas.canvasComponents.Count || _row < 0)
		{
			index = _canvas.canvasComponents.Count;
			_canvas.canvasComponents.Add(new List<UIC>());
		}
		else if (_insert)
		{
			_canvas.canvasComponents.Insert(index, new List<UIC>());
		}
		if (_col >= _canvas.canvasComponents[index].Count || _col < 0)
		{
			_canvas.canvasComponents[index].Add(_component);
		}
		else
		{
			_canvas.canvasComponents[index].Insert(_col, _component);
		}
	}

	public static void RemoveFromCanvasGrid(UIC _component)
	{
		for (int i = 0; i < _component.parent.canvasComponents.Count; i++)
		{
			for (int j = 0; j < _component.parent.canvasComponents[i].Count; j++)
			{
				if (_component.parent.canvasComponents[i][j] == _component)
				{
					_component.parent.canvasComponents[i].RemoveAt(j);
					if (_component.parent.canvasComponents[i].Count == 0)
					{
						_component.parent.canvasComponents.RemoveAt(i);
					}
					_component.parent = null;
					TransformS.UnparentComponent(_component.TC);
					return;
				}
			}
		}
	}

	public static int GetRowIndex(UIC _component)
	{
		for (int i = 0; i < _component.parent.canvasComponents.Count; i++)
		{
			for (int j = 0; j < _component.parent.canvasComponents[i].Count; j++)
			{
				if (_component.parent.canvasComponents[i][j] == _component)
				{
					return i;
				}
			}
		}
		return -1;
	}

	public static int GetColIndex(UIC _component)
	{
		for (int i = 0; i < _component.parent.canvasComponents.Count; i++)
		{
			for (int j = 0; j < _component.parent.canvasComponents[i].Count; j++)
			{
				if (_component.parent.canvasComponents[i][j] == _component)
				{
					return j;
				}
			}
		}
		return -1;
	}

	public static int GetColIndex(UIC _component, int _row)
	{
		for (int i = 0; i < _component.parent.canvasComponents[_row].Count; i++)
		{
			if (_component.parent.canvasComponents[_row][i] == _component)
			{
				return i;
			}
		}
		return -1;
	}

	public static void PlaceCanvasContent(UIC _canvas)
	{
		float num = 0f;
		for (int i = 0; i < _canvas.canvasComponents.Count; i++)
		{
			float num2 = 0f;
			for (int j = 0; j < _canvas.canvasComponents[i].Count; j++)
			{
				UIC uIC = _canvas.canvasComponents[i][j];
				if (uIC.active && uIC.height > num2)
				{
					num2 = uIC.height;
				}
			}
			if (num2 > 0f)
			{
				num = ((i >= _canvas.canvasComponents.Count - 1) ? (num + num2) : (num + (num2 + _canvas.contentSpacing)));
			}
		}
		if (_canvas.contentVAlign == Align.Top)
		{
			_canvas.currentContentY = _canvas.startContentY;
		}
		else if (_canvas.contentVAlign == Align.Middle)
		{
			_canvas.currentContentY = num * 0.5f - _canvas.headerHeight * 0.5f + _canvas.footerHeight * 0.5f;
		}
		else if (_canvas.contentVAlign == Align.Bottom)
		{
			_canvas.currentContentY = (0f - _canvas.canvasHeight) * 0.5f + _canvas.footerHeight + _canvas.contentMargin + num;
		}
		for (int k = 0; k < _canvas.canvasComponents.Count; k++)
		{
			float num3 = 0f;
			float num4 = 0f;
			for (int l = 0; l < _canvas.canvasComponents[k].Count; l++)
			{
				UIC uIC2 = _canvas.canvasComponents[k][l];
				if (uIC2.active)
				{
					num3 = ((l >= _canvas.canvasComponents[k].Count - 1) ? (num3 + uIC2.width) : (num3 + (uIC2.width + _canvas.contentSpacing)));
					if (uIC2.height > num4)
					{
						num4 = uIC2.height;
					}
				}
			}
			if (_canvas.contentHAlign == Align.Left)
			{
				_canvas.currentContentX = _canvas.startContentX;
			}
			else if (_canvas.contentHAlign == Align.Right)
			{
				_canvas.currentContentX = _canvas.canvasWidth * 0.5f - _canvas.contentMargin - num3;
			}
			else if (_canvas.contentHAlign == Align.Center)
			{
				_canvas.currentContentX = num3 * -0.5f;
			}
			for (int m = 0; m < _canvas.canvasComponents[k].Count; m++)
			{
				UIC uIC3 = _canvas.canvasComponents[k][m];
				if (uIC3.active)
				{
					TransformS.SetPosition(uIC3.TC, new Vector3(_canvas.currentContentX + uIC3.width * 0.5f, _canvas.currentContentY - uIC3.height * 0.5f, -10f));
					_canvas.currentContentX += uIC3.width + _canvas.contentSpacing;
				}
			}
			_canvas.currentContentY -= num4 + _canvas.contentSpacing;
		}
		if (0f - _canvas.currentContentY + _canvas.height * 0.5f + _canvas.footerHeight > _canvas.canvasHeight)
		{
			_canvas.canvasHeight = 0f - _canvas.currentContentY + _canvas.height * 0.5f + _canvas.footerHeight;
		}
	}

	public static void ResetClipsForTouchAreasInSeparateRenderSpaces(UIC _parent)
	{
		for (int i = 0; i < _parent.canvasComponents.Count; i++)
		{
			for (int j = 0; j < _parent.canvasComponents[i].Count; j++)
			{
				UIC uIC = _parent.canvasComponents[i][j];
				if (_parent.separateRenderSpace && uIC.TAC != null)
				{
					int minX = Mathf.RoundToInt(_parent.TC.transform.position.x - _parent.width * 0.5f + (float)Screen.width * 0.5f);
					int maxX = Mathf.RoundToInt(_parent.TC.transform.position.x + _parent.width * 0.5f + (float)Screen.width * 0.5f);
					int minY = Mathf.RoundToInt(_parent.TC.transform.position.y - _parent.height * 0.5f + (float)Screen.height * 0.5f);
					int maxY = Mathf.RoundToInt(_parent.TC.transform.position.y + _parent.height * 0.5f + (float)Screen.height * 0.5f);
					TouchAreaS.SetClip(uIC.TAC, minX, maxX, minY, maxY);
				}
				ResetClipsForTouchAreasInSeparateRenderSpaces(uIC);
			}
		}
	}

	public static UIC GetUIComponentByLabel(string _label)
	{
		int aliveCount = m_components.m_aliveCount;
		for (int i = 0; i < aliveCount; i++)
		{
			UIC uIC = m_components.m_array[m_components.m_aliveIndices[i]];
			if (uIC.label == _label)
			{
				return uIC;
			}
		}
		return null;
	}

	public static UIC GetUIComponentByIdentifier(int _identifier)
	{
		int aliveCount = m_components.m_aliveCount;
		for (int i = 0; i < aliveCount; i++)
		{
			UIC uIC = m_components.m_array[m_components.m_aliveIndices[i]];
			if (uIC.identifier == _identifier)
			{
				return uIC;
			}
		}
		return null;
	}

	public static UIC GetUIComponentByIndex(int _index)
	{
		return m_components.m_array[_index];
	}

	public static void StartTextFieldEditing(UIC _c)
	{
		_c.isEditing = true;
		m_typed = _c.textC.text;
		#if UNITY_IOS
		m_keyboard = iPhoneKeyboard.Open(m_typed, iPhoneKeyboardType.ASCIICapable, false, false, false, false, string.Empty);
#endif
	}

	public static void StopTextFieldEditing(UIC _c)
	{
		_c.isEditing = false;
		if (_c.EC != null)
		{
			_c.EC.properties["value"] = _c.textC.text;
			EventS.Dispatch(_c.EC, false);
		}
	}

	public static void StopNumericFieldEditing(UIC _c)
	{
		_c.isEditing = false;
		float result = 0f;
		string text = _c.textC.text;
		text = text.Replace(",", ".");
		if (float.TryParse(text, out result))
		{
			_c.currentVal = result;
			if (_c.isInt)
			{
				_c.currentVal = Mathf.Round(_c.currentVal);
			}
			_c.currentVal = Mathf.Max(Mathf.Min(_c.currentVal, _c.maxVal), _c.minVal);
		}
		while (_c.textPCs.Count > 0)
		{
			int index = _c.textPCs.Count - 1;
			PrefabS.RemoveComponent(_c.textPCs[index]);
			_c.textPCs.RemoveAt(index);
		}
		TextS.ChangeText(_c.textC, _c.currentVal.ToString());
		SpriteS.SetColorByTransformComponent(_c.textC.contentTC, Color.black, false, false);
		_c.textPCs.AddRange(SpriteS.ConvertSpritesToPrefabComponent(_c.textC.contentTC, _c.canvasCamera, true));
		if (_c.EC != null)
		{
			_c.EC.properties["value"] = _c.currentVal;
			EventS.Dispatch(_c.EC, false);
		}
	}

	public static void UnselectAllRadioButtonsFromGroup(int _group)
	{
		int aliveCount = m_components.m_aliveCount;
		for (int i = 0; i < aliveCount; i++)
		{
			UIC uIC = m_components.m_array[m_components.m_aliveIndices[i]];
			if (uIC.active && uIC.uiComponentType == UIComponentType.RadioButton && uIC.radioButtonGroup == _group)
			{
				UnselectRadioButton(uIC);
			}
		}
	}

	public static void SelectRadioButtonByValue(int _group, int _value)
	{
		int aliveCount = m_components.m_aliveCount;
		for (int i = 0; i < aliveCount; i++)
		{
			UIC uIC = m_components.m_array[m_components.m_aliveIndices[i]];
			if (uIC.active && uIC.uiComponentType == UIComponentType.RadioButton && uIC.radioButtonGroup == _group && uIC.radioButtonValue == _value)
			{
				UnselectAllRadioButtonsFromGroup(_group);
				SelectRadioButton(uIC);
				break;
			}
		}
	}

	public static int GetValueFromRadioButtonGroup(int _group)
	{
		int aliveCount = m_components.m_aliveCount;
		for (int i = 0; i < aliveCount; i++)
		{
			UIC uIC = m_components.m_array[m_components.m_aliveIndices[i]];
			if (uIC.uiComponentType == UIComponentType.RadioButton && uIC.radioButtonGroup == _group && uIC.isSelected)
			{
				return uIC.radioButtonValue;
			}
		}
		return -1;
	}

	public static UIC GetSelectedRadioButton(int _group)
	{
		int aliveCount = m_components.m_aliveCount;
		for (int i = 0; i < aliveCount; i++)
		{
			UIC uIC = m_components.m_array[m_components.m_aliveIndices[i]];
			if (uIC.uiComponentType == UIComponentType.RadioButton && uIC.radioButtonGroup == _group && uIC.isSelected)
			{
				return uIC;
			}
		}
		return null;
	}

	public static List<UIC> GetRadioButtonsFromGroup(int _group)
	{
		List<UIC> list = new List<UIC>();
		int aliveCount = m_components.m_aliveCount;
		for (int i = 0; i < aliveCount; i++)
		{
			UIC uIC = m_components.m_array[m_components.m_aliveIndices[i]];
			if (uIC.uiComponentType == UIComponentType.RadioButton && uIC.radioButtonGroup == _group)
			{
				list.Add(uIC);
			}
		}
		return list;
	}

	public static void UnselectRadioButton(UIC _c)
	{
		_c.isSelected = false;
		for (int i = 0; i < _c.foregroundPCs.Count; i++)
		{
			PrefabS.SetVisibility(_c.foregroundPCs[i], false);
		}
	}

	public static void SelectRadioButton(UIC _c)
	{
		_c.isSelected = true;
		for (int i = 0; i < _c.foregroundPCs.Count; i++)
		{
			PrefabS.SetVisibility(_c.foregroundPCs[i], true);
		}
	}

	public static void CheckBox(UIC _c)
	{
		_c.isChecked = true;
		for (int i = 0; i < _c.foregroundPCs.Count; i++)
		{
			PrefabS.SetVisibility(_c.foregroundPCs[i], true);
		}
	}

	public static void UncheckBox(UIC _c)
	{
		_c.isChecked = false;
		for (int i = 0; i < _c.foregroundPCs.Count; i++)
		{
			PrefabS.SetVisibility(_c.foregroundPCs[i], false);
		}
	}

	public static void HighlightButton(UIC _c)
	{
		for (int i = 0; i < _c.outlinePCs.Count; i++)
		{
			PrefabS.SetVertexColors(_c.outlinePCs[i], DebugDraw.GetColor(85f, 188f, 61f));
		}
		for (int j = 0; j < _c.backgroundPCs.Count; j++)
		{
			PrefabS.SetVertexColors(_c.backgroundPCs[j], DebugDraw.GetColor(197f, 238f, 36f));
		}
		if (_c.textC != null)
		{
			SpriteS.SetColorByTransformComponent(_c.textC.contentTC, DebugDraw.GetColor(58f, 115f, 56f), false, false);
		}
	}

	public static void NormalizeButton(UIC _c)
	{
		for (int i = 0; i < _c.outlinePCs.Count; i++)
		{
			PrefabS.SetVertexColors(_c.outlinePCs[i], DebugDraw.GetColor(60f, 66f, 0f));
		}
		for (int j = 0; j < _c.backgroundPCs.Count; j++)
		{
			PrefabS.SetVertexColors(_c.backgroundPCs[j], DebugDraw.GetColor(95f, 108f, 8f));
		}
		if (_c.textC != null)
		{
			SpriteS.SetColorByTransformComponent(_c.textC.contentTC, DebugDraw.GetColor(181f, 218f, 32f), false, false);
		}
	}

	public static void HighlightIcon(UIC _c)
	{
	}

	public static void NormalizeIcon(UIC _c)
	{
	}

	public static void SetController(UIC _controller, UIC _c, bool _enabled, bool _invert)
	{
		_controller.controlledUICs.Add(_c);
		_controller.controlledUICDirs.Add(_invert);
		if (!_enabled)
		{
			Disable(_c);
		}
	}

	public static void SetController(UIC _controller, UIC _c, bool _enabled)
	{
		SetController(_controller, _c, _enabled, false);
	}

	public static void Enable(UIC _c)
	{
		_c.enabled = true;
		PrefabS.ColorizeByTransformComponent(_c.TC, Color.gray, true, false);
		SpriteS.SetColorByTransformComponent(_c.TC, Color.black, true, false);
	}

	public static void Disable(UIC _c)
	{
		_c.enabled = false;
		PrefabS.ColorizeByTransformComponent(_c.TC, new Color(0.5f, 0.5f, 0.5f, 0.353f), true, false);
		SpriteS.SetColorByTransformComponent(_c.TC, new Color(0f, 0f, 0f, 0.353f), true, false);
	}

	public static void SetAbsoluteSize(UIC _c, float _width, float _height)
	{
		_c.width = _width;
		_c.height = _height;
		_c.canvasWidth = _c.width;
		_c.canvasHeight = _c.height;
		if (_c.TAC != null)
		{
			_c.TAC.width = _c.width;
			_c.TAC.height = _c.height;
		}
	}

	public static void SetRelativeSize(UIC _c, float _width, float _height)
	{
		if (_c.parent == null)
		{
			_c.width = _width * (float)Screen.width;
			_c.height = _height * (float)Screen.height;
		}
		else
		{
			_c.width = _width * (_c.parent.width - _c.parent.contentMargin * 2f);
			_c.height = _height * (_c.parent.height - _c.parent.contentMargin * 2f);
		}
		_c.canvasWidth = _c.width;
		_c.canvasHeight = _c.height;
		if (_c.TAC != null)
		{
			_c.TAC.width = _c.width;
			_c.TAC.height = _c.height;
		}
	}

	public static void SetRelativeSize(UIC _c, float _height)
	{
		if (_c.parent == null)
		{
			_c.width = _height * (float)Screen.height;
			_c.height = _height * (float)Screen.height;
		}
		else
		{
			_c.width = _height * (_c.parent.height - _c.parent.contentMargin * 2f);
			_c.height = _height * (_c.parent.height - _c.parent.contentMargin * 2f);
		}
		_c.canvasWidth = _c.width;
		_c.canvasHeight = _c.height;
		if (_c.TAC != null)
		{
			_c.TAC.width = _c.width;
			_c.TAC.height = _c.height;
		}
	}

	public static void SetCanvasAbsoluteSize(UIC _c, float _width, float _height, float _headerHeight, float _footerHeight)
	{
		_c.width = _width;
		_c.height = _height;
		_c.canvasWidth = _c.width;
		_c.canvasHeight = _c.height;
		_c.headerHeight = _headerHeight;
		_c.footerHeight = _footerHeight;
		if (_c.TAC != null)
		{
			_c.TAC.width = _c.width;
			_c.TAC.height = _c.height - _c.headerHeight - _c.footerHeight;
		}
		TransformS.SetPosition(_c.cameraTC, Vector3.up * (_c.footerHeight - _c.headerHeight) * 0.5f);
	}

	public static void SetCanvasRelativeSize(UIC _c, float _width, float _height, float _headerHeight, float _footerHeight)
	{
		if (_c.parent == null)
		{
			_c.width = _width * (float)Screen.width;
			_c.height = _height * (float)Screen.height;
			_c.headerHeight = _headerHeight * (float)Screen.height;
			_c.footerHeight = _footerHeight * (float)Screen.height;
		}
		else
		{
			_c.width = _width * (_c.parent.width - _c.parent.contentMargin * 2f);
			_c.height = _height * (_c.parent.height - _c.parent.headerHeight - _c.parent.footerHeight - _c.parent.contentMargin * 2f);
			_c.headerHeight = _headerHeight * (_c.parent.height - _c.parent.headerHeight - _c.parent.footerHeight - _c.parent.contentMargin * 2f);
			_c.footerHeight = _footerHeight * (_c.parent.height - _c.parent.headerHeight - _c.parent.footerHeight - _c.parent.contentMargin * 2f);
		}
		_c.canvasWidth = _c.width;
		_c.canvasHeight = _c.height;
		if (_c.TAC != null)
		{
			_c.TAC.width = _c.width;
			_c.TAC.height = _c.height - _c.headerHeight - _c.footerHeight;
		}
		TransformS.SetPosition(_c.cameraTC, Vector3.up * (_c.footerHeight - _c.headerHeight) * 0.5f);
	}

	public static void SetRelativePosition(UIC _c, Vector2 _pos, int depthIndex)
	{
		Vector3 zero = Vector3.zero;
		zero.z = _c.TC.transform.position.z;
		TransformS.SetPosition(_position: (_c.parent != null) ? new Vector3(_pos.x * (_c.parent.width - _c.width - _c.parent.contentMargin * 2f) - _c.parent.width * 0.5f + _c.parent.contentMargin + _c.width * 0.5f, _pos.y * (_c.parent.height - _c.parent.headerHeight - _c.parent.footerHeight - _c.height - _c.parent.contentMargin * 2f) - _c.parent.height * 0.5f + _c.parent.contentMargin + _c.height * 0.5f + _c.parent.footerHeight, (_c.depthIndex + (float)depthIndex) * -100f) : new Vector3(_pos.x * ((float)Screen.width - _c.width) + _c.width * 0.5f - (float)Screen.width * 0.5f, _pos.y * ((float)Screen.height - _c.height) + _c.height * 0.5f - (float)Screen.height * 0.5f, depthIndex * -100), _c: _c.TC);
	}

	public static void SetAbsolutePosition(UIC _c, Vector2 _pos, int depthIndex)
	{
		Vector3 zero = Vector3.zero;
		TransformS.SetPosition(_position: (_c.parent != null) ? new Vector3(_pos.x, _pos.y + (_c.footerHeight - _c.headerHeight) * 0.5f, (_c.parent.depthIndex + (float)depthIndex) * -100f) : new Vector3(_pos.x, _pos.y + (_c.footerHeight - _c.headerHeight) * 0.5f, depthIndex * -100), _c: _c.TC);
	}

	public static void SetCanvasRelativeMarginAndSpacing(UIC _c, float _margin, float _spacing)
	{
		_c.contentMargin = _margin * _c.height;
		_c.contentSpacing = _spacing * _c.height;
		ResetCursor(_c);
	}

	public static void SetCanvasAbsoluteMarginAndSpacing(UIC _c, float _margin, float _spacing)
	{
		_c.contentMargin = _margin;
		_c.contentSpacing = _spacing;
		ResetCursor(_c);
	}

	public static void SetCanvasSeparateRenderSpace(UIC _c)
	{
		_c.separateRenderSpace = true;
		GameObject gameObject = new GameObject("Canvas Camera(" + _c.identifier + ")");
		Camera camera = gameObject.AddComponent("Camera") as Camera;
		camera.orthographic = true;
		camera.orthographicSize = (_c.canvasHeight - _c.footerHeight - _c.headerHeight) * 0.5f;
		camera.depth = 1f;
		camera.cullingMask = 512;
		camera.gameObject.layer = 9;
		camera.nearClipPlane = 1f;
		camera.farClipPlane = 500f;
		camera.clearFlags = CameraClearFlags.Depth;
		camera.pixelRect = new Rect((float)Screen.width * 0.5f + _c.TC.transform.position.x + _c.width * -0.5f, (float)Screen.height * 0.5f + _c.TC.transform.position.y + (_c.height - _c.headerHeight - _c.footerHeight) * -0.5f - (_c.headerHeight - _c.footerHeight) * 0.5f, _c.canvasWidth, _c.canvasHeight - _c.footerHeight - _c.headerHeight);
		_c.canvasCamera = camera;
		_c.canvasCamera.transform.parent = _c.cameraTC.transform;
		_c.canvasCamera.transform.localPosition = Vector3.forward * -250f + Vector3.up * (_c.footerHeight - _c.headerHeight) * 0.5f;
		_c.scrollX = Mathf.Max(Mathf.Min(_c.scrollX, _c.canvasWidth - _c.width), 0f);
		_c.scrollY = Mathf.Min(Mathf.Max(_c.scrollY, 0f - _c.canvasHeight + _c.height), 0f);
		TransformS.SetPosition(_position: new Vector3(_c.scrollX - _c.scrollInertiaX, _c.scrollY - _c.scrollInertiaY, 0f), _c: _c.cameraTC);
		if (_c.TAC != null)
		{
			_c.TAC.camera = camera;
		}
	}

	public static void RemoveContents(UIC _c)
	{
		_c.canvasWidth = _c.width;
		_c.canvasHeight = _c.height - _c.headerHeight - _c.footerHeight;
		while (_c.canvasComponents.Count > 0)
		{
			int index = _c.canvasComponents.Count - 1;
			while (_c.canvasComponents[index].Count > 0)
			{
				int index2 = _c.canvasComponents[index].Count - 1;
				EntityManager.RemoveEntitiesByTransformComponentHierarchy(_c.canvasComponents[index][index2].TC, false);
				_c.canvasComponents[index].RemoveAt(index2);
			}
			_c.canvasComponents.RemoveAt(index);
		}
	}

	public static void SetActivityOfChildComponents(UIC _item, bool _active)
	{
		for (int i = 0; i < _item.canvasComponents.Count; i++)
		{
			for (int j = 0; j < _item.canvasComponents[i].Count; j++)
			{
				EntityManager.SetActivityOfEntity(_item.canvasComponents[i][j].entityIndex, _active, true);
				if (_item.canvasComponents[i][j].expanded)
				{
					SetActivityOfChildComponents(_item.canvasComponents[i][j], _active);
				}
			}
		}
	}

	public static int GetSubItemCount(UIC _item, int _count)
	{
		for (int i = 0; i < _item.canvasComponents.Count; i++)
		{
			for (int j = 0; j < _item.canvasComponents[i].Count; j++)
			{
				UIC uIC = _item.canvasComponents[i][j];
				if (uIC.active)
				{
					_count++;
					if (uIC.expanded)
					{
						_count = GetSubItemCount(_item.canvasComponents[i][j], _count);
					}
				}
			}
		}
		return _count;
	}

	public static void Update()
	{
		int aliveCount = m_components.m_aliveCount;
		for (int i = 0; i < aliveCount; i++)
		{
			UIC uIC = m_components.m_array[m_components.m_aliveIndices[i]];
			if (!uIC.active)
			{
				continue;
			}
			switch (uIC.uiComponentType)
			{
			case UIComponentType.Canvas:
			{
				if (uIC.canvasWidth > uIC.width)
				{
					uIC.scrollX -= uIC.scrollInertiaX;
				}
				else
				{
					uIC.scrollInertiaX = 0f;
				}
				if (uIC.canvasHeight > uIC.height)
				{
					uIC.scrollY -= uIC.scrollInertiaY;
				}
				else
				{
					uIC.scrollInertiaY = 0f;
				}
				uIC.scrollX = Mathf.Max(Mathf.Min(uIC.scrollX, uIC.canvasWidth - uIC.width), 0f);
				uIC.scrollY = Mathf.Min(Mathf.Max(uIC.scrollY, 0f - uIC.canvasHeight + uIC.height), 0f);
				Vector3 vector = new Vector3(uIC.scrollX - uIC.scrollInertiaX, uIC.scrollY - uIC.scrollInertiaY, 0f);
				uIC.scrollInertiaX *= 0.85f;
				uIC.scrollInertiaY *= 0.85f;
				if (Mathf.Abs(uIC.scrollInertiaY) < 0.1f)
				{
					uIC.scrollInertiaY = 0f;
				}
				if (Mathf.Abs(uIC.scrollInertiaX) < 0.1f)
				{
					uIC.scrollInertiaX = 0f;
				}
				if (uIC.separateRenderSpace)
				{
					TransformS.SetPosition(uIC.cameraTC, vector);
					uIC.canvasCamera.pixelRect = new Rect((float)Screen.width * 0.5f + uIC.TC.transform.position.x + uIC.width * -0.5f, (float)Screen.height * 0.5f + uIC.TC.transform.position.y + (uIC.height - uIC.headerHeight - uIC.footerHeight) * -0.5f - (uIC.headerHeight - uIC.footerHeight) * 0.5f, uIC.width, uIC.height - uIC.footerHeight - uIC.headerHeight);
					uIC.canvasCamera.orthographicSize = (uIC.height - uIC.footerHeight - uIC.headerHeight) * 0.5f;
				}
				else
				{
					TransformS.SetPosition(uIC.contentTC, -vector);
				}
				break;
			}
			case UIComponentType.Slider:
				if (uIC.snap)
				{
					Vector3 zero = Vector3.zero;
					zero.x = (float)uIC.currentSnapIndexX * uIC.snapPointDistanceX + uIC.minX;
					zero.y = (float)uIC.currentSnapIndexY * uIC.snapPointDistanceY + uIC.minY;
					float num = uIC.maxX - uIC.minX;
					float num2 = uIC.maxY - uIC.minY;
					uIC.currentValX = (0f - (uIC.TC.transform.position.x + uIC.minX - zero.x)) / num * (uIC.maxValX - uIC.minValX) + uIC.minValX;
					uIC.currentValY = (0f - (uIC.TC.transform.position.y + uIC.minY - zero.y)) / num2 * (uIC.maxValY - uIC.minValY) + uIC.minValY;
					Vector3 localPosition = uIC.contentTC.transform.localPosition;
					Vector3 vector2 = zero - localPosition;
					TransformS.SetPosition(uIC.contentTC, localPosition + vector2 * 0.2f);
				}
				break;
			case UIComponentType.TextField:
			case UIComponentType.NumericField:
				if (!uIC.isEditing)
				{
					break;
				}
				#if UNITY_IOS
				if (m_keyboard.text != m_typed)
				{
					m_typed = m_keyboard.text;
					TextS.ChangeText(uIC.textC, m_typed);
					SpriteS.SetColorByTransformComponent(uIC.textC.contentTC, Color.black, false, false);
				}
				if (m_keyboard.done || !m_keyboard.active)
				{
					uIC.isEditing = false;
					if (uIC.uiComponentType == UIComponentType.TextField)
					{
						StopTextFieldEditing(uIC);
					}
					else
					{
						StopNumericFieldEditing(uIC);
					}
				}
#endif
				break;
			}
		}
	}
}
