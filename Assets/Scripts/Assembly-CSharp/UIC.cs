using System.Collections.Generic;
using UnityEngine;

public class UIC : IComponent
{
	private bool _active;

	private int _index;

	private int _entityIndex;

	private ComponentType _componentType;

	public TransformC TC;

	public TransformC contentTC;

	public TransformC cameraTC;

	public TouchAreaC TAC;

	public EventC EC;

	public UIC parent;

	public List<PrefabC> backgroundPCs;

	public List<PrefabC> foregroundPCs;

	public List<PrefabC> outlinePCs;

	public List<PrefabC> textPCs;

	public List<UIC> controlledUICs;

	public List<bool> controlledUICDirs;

	public UIComponentType uiComponentType;

	public float depthIndex;

	public SpriteSheet iconSheet;

	public int iconIndex;

	public bool enabled;

	public int identifier;

	public string label;

	public bool draggable;

	public Vector2 dragOffset;

	public bool isDragged;

	public bool limitedDrag;

	public float minX;

	public float maxX;

	public float minY;

	public float maxY;

	public float width;

	public float height;

	public float headerHeight;

	public float footerHeight;

	public float radius;

	public float maxValX;

	public float minValX;

	public float currentValX;

	public float maxValY;

	public float minValY;

	public float currentValY;

	public bool snap;

	public int snapPoints;

	public float snapPointDistanceX;

	public float snapPointDistanceY;

	public int currentSnapIndexX;

	public int currentSnapIndexY;

	public bool separateRenderSpace;

	public Camera canvasCamera;

	public EventC canvasEventC;

	public float canvasWidth;

	public float canvasHeight;

	public float scrollX;

	public float scrollY;

	public float scrollInertiaX;

	public float scrollInertiaY;

	public Align contentHAlign;

	public Align contentVAlign;

	public float contentMargin;

	public float contentSpacing;

	public float startContentX;

	public float startContentY;

	public float currentContentX;

	public float currentContentY;

	public float nextContentX;

	public float nextContentY;

	public int currentPage;

	public bool expandable;

	public bool expanded;

	public float intent;

	public List<List<UIC>> canvasComponents;

	public bool isChecked;

	public bool isSelected;

	public int radioButtonGroup;

	public int radioButtonValue;

	public TextC textC;

	public float currentVal;

	public float minVal;

	public float maxVal;

	public bool isEditing;

	public bool isInt;

	public bool active
	{
		get
		{
			return _active;
		}
		set
		{
			_active = value;
		}
	}

	public int index
	{
		get
		{
			return _index;
		}
		set
		{
			_index = value;
		}
	}

	public int entityIndex
	{
		get
		{
			return _entityIndex;
		}
		set
		{
			_entityIndex = value;
		}
	}

	public ComponentType componentType
	{
		get
		{
			return _componentType;
		}
		set
		{
			_componentType = value;
		}
	}
}
