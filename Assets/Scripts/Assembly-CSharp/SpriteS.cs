using System;
using System.Collections.Generic;
using UnityEngine;

public static class SpriteS
{
	private static GenericArray<SpriteSheet> m_sheets;

	private static SpriteC[] p_sprites;

	private static Vector3[] p_vertices;

	private static int[] p_aliveIndices;

	public static Vector3 m_outOfScreen = new Vector3(99999f, 99999f, -99999f);

	public static Color m_defaultColor = new Color(0.5f, 0.5f, 0.5f, 1f);

	private static int debug_rotationUpdates;

	private static int debug_scaleUpdates;

	private static int debug_positionUpdates;

	public static void Initialize()
	{
		m_sheets = new GenericArray<SpriteSheet>(50);
	}

	public static void ResetComponent(SpriteC _c)
	{
		_c.active = false;
		_c.visible = true;
		_c.update = true;
		_c.offset = Vector3.zero;
		_c.offsetRight = Vector3.right;
		_c.offsetUp = Vector3.up;
		_c.align = Vector3.zero;
		_c.relRight = Vector3.right;
		_c.relUp = Vector3.up;
		_c.scaledRelRight = _c.relRight;
		_c.scaledRelUp = _c.relUp;
		_c.sortValue = 0f;
		_c.wScale = 1f;
		_c.hScale = 1f;
		_c.dimensionScale = 1f;
		_c.wDimension = 0f;
		_c.hDimension = 0f;
		_c.color = m_defaultColor;
		_c.isVisible = true;
	}

	public static SpriteSheet AddSpriteSheet(int _maxComponentCount, Camera _camera, Material _material, float _globalSpriteScale)
	{
		SpriteSheet spriteSheet = new SpriteSheet(_maxComponentCount, _camera, _material, _globalSpriteScale);
		int num = m_sheets.AddItem(spriteSheet);
		m_sheets.m_array[num].m_index = num;
		return spriteSheet;
	}

	public static SpriteSheet AddSpriteSheet(int _maxComponentCount, Camera _camera, Texture _texture, Shader _shader, float _globalSpriteScale)
	{
		SpriteSheet spriteSheet = new SpriteSheet(_maxComponentCount, _camera, _texture, _shader, _globalSpriteScale);
		int num = m_sheets.AddItem(spriteSheet);
		m_sheets.m_array[num].m_index = num;
		return spriteSheet;
	}

	public static void RemoveSpriteSheet(SpriteSheet _sheet)
	{
		RemoveAllComponentsFromSheet(_sheet);
		UnityEngine.Object.Destroy(_sheet.m_gameObject);
		_sheet.m_gameObject = null;
		m_sheets.RemoveItem(_sheet.m_index);
		m_sheets.m_array[_sheet.m_index] = null;
	}

	public static TiledSpriteSheet AddTiledSpriteSheet(float _startX, float _startY, float _tileSize, int _width, int _height, int _maxComponentCount, Camera _camera, Texture _texture, Shader _shader, float _globalSpriteScale)
	{
		TiledSpriteSheet tiledSpriteSheet = new TiledSpriteSheet();
		tiledSpriteSheet.m_startX = _startX;
		tiledSpriteSheet.m_startY = _startY;
		tiledSpriteSheet.m_tileSize = _tileSize;
		tiledSpriteSheet.m_width = _width;
		tiledSpriteSheet.m_height = _height;
		tiledSpriteSheet.m_textureWidth = _texture.width;
		tiledSpriteSheet.m_textureHeight = _texture.height;
		tiledSpriteSheet.m_sheets = new SpriteSheet[_width * _height];
		Material material = new Material(_shader);
		material.mainTexture = _texture;
		for (int i = 0; i < _width * _height; i++)
		{
			SpriteSheet item = new SpriteSheet(_maxComponentCount, _camera, material, _globalSpriteScale);
			int num = m_sheets.AddItem(item);
			m_sheets.m_array[num].m_index = num;
			m_sheets.m_array[num].m_gameObject.name = "SpriteSheet (tile:" + i + ")";
			int num2 = Mathf.FloorToInt((float)i / (float)_width);
			int num3 = i - num2 * _width;
			m_sheets.m_array[num].m_mesh.bounds = new Bounds(new Vector3(_startX + _tileSize * (float)num3 + _tileSize * 0.5f, _startY + _tileSize * (float)num2 + _tileSize * 0.5f, 0f), new Vector3(_tileSize, _tileSize, _tileSize));
			tiledSpriteSheet.m_sheets[i] = m_sheets.m_array[num];
		}
		return tiledSpriteSheet;
	}

	public static SpriteC AddComponent(TransformC _transformComponent, Frame _frame, SpriteSheet _sheet)
	{
		if (_sheet.m_components.m_aliveCount == 0)
		{
			PrefabS.RecursiveRendererVisibility(_sheet.m_gameObject.transform, true);
		}
		int num = _sheet.m_components.AddItem();
		SpriteC spriteC = _sheet.m_components.m_array[num];
		ResetComponent(spriteC);
		spriteC.entityIndex = _transformComponent.entityIndex;
		spriteC.p_TC = _transformComponent;
		spriteC.p_spriteSheet = _sheet;
		spriteC.active = true;
		SetFrame(_sheet, spriteC, _frame);
		EntityManager.m_entities.m_array[spriteC.entityIndex].components.Add(spriteC);
		return spriteC;
	}

	public static SpriteC AddComponent(TransformC _transformComponent, Frame _frame, TiledSpriteSheet _tiledSheet)
	{
		Vector3 position = _transformComponent.transform.position;
		int num = Mathf.FloorToInt((position.x - _tiledSheet.m_startX) / _tiledSheet.m_tileSize);
		int num2 = Mathf.FloorToInt((position.y - _tiledSheet.m_startY) / _tiledSheet.m_tileSize);
		int num3 = num2 * _tiledSheet.m_width + num;
		SpriteSheet sheet = _tiledSheet.m_sheets[num3];
		return AddComponent(_transformComponent, _frame, sheet);
	}

	public static void RemoveComponent(SpriteC _c)
	{
		SpriteSheet p_spriteSheet = _c.p_spriteSheet;
		int num = _c.vertDataIndex * 4;
		for (int i = 0; i < 4; i++)
		{
			p_spriteSheet.m_vertices[num + i] = m_outOfScreen;
		}
		p_spriteSheet.m_vertsChanged = true;
		_c.p_TC = null;
		_c.p_spriteSheet = null;
		_c.active = false;
		_c.sortValue = 0f;
		EntityManager.m_entities.m_array[_c.entityIndex].components.Remove(_c);
		p_spriteSheet.m_components.RemoveItem(_c.index);
		_c.entityIndex = -1;
		if (p_spriteSheet.m_components.m_aliveCount == 0)
		{
			PrefabS.RecursiveRendererVisibility(p_spriteSheet.m_gameObject.transform, false);
		}
	}

	public static void RemoveAllComponentsFromSheet(SpriteSheet _sheet)
	{
		while (_sheet.m_components.m_aliveCount > 0)
		{
			SpriteC spriteC = _sheet.m_components.m_array[_sheet.m_components.m_aliveIndices[0]];
			int num = spriteC.vertDataIndex * 4;
			for (int i = 0; i < 4; i++)
			{
				_sheet.m_vertices[num + i] = m_outOfScreen;
			}
			_sheet.m_vertsChanged = true;
			spriteC.p_TC = null;
			spriteC.p_spriteSheet = null;
			spriteC.active = false;
			spriteC.sortValue = 0f;
			EntityManager.m_entities.m_array[spriteC.entityIndex].components.Remove(spriteC);
			_sheet.m_components.RemoveItem(spriteC.index);
			spriteC.entityIndex = -1;
		}
	}

	public static void RemoveSpritesFromTransformComponent(TransformC _tc)
	{
		Entity entity = EntityManager.m_entities.m_array[_tc.entityIndex];
		List<IComponent> componentsByEntityIndex = EntityManager.GetComponentsByEntityIndex(ComponentType.Sprite, entity.index);
		while (componentsByEntityIndex.Count > 0)
		{
			RemoveComponent(componentsByEntityIndex[0] as SpriteC);
			componentsByEntityIndex.RemoveAt(0);
		}
	}

	public static void SetSpriteSheetCamera(SpriteSheet _sheet, Camera _camera)
	{
		_sheet.m_camera = _camera;
		_sheet.m_gameObject.layer = _sheet.m_camera.gameObject.layer;
	}

	public static void SetDimensions(SpriteC _c, float _width, float _height)
	{
		_c.wDimension = _width;
		_c.hDimension = _height;
		_c.width = _width * _c.p_spriteSheet.m_globalSpriteScale;
		_c.height = _height * _c.p_spriteSheet.m_globalSpriteScale;
		_c.update = true;
	}

	public static void SetDimensionScale(SpriteC _c, float _scale)
	{
		_c.dimensionScale = _scale;
		_c.update = true;
	}

	public static void SetVisibility(SpriteC _s, bool _visible)
	{
		_s.visible = _visible;
		_s.isVisible = _visible;
		_s.update = true;
		_s.p_spriteSheet.m_vertsChanged = true;
	}

	public static void SetVisibility(SpriteC _s, bool _visible, bool _markVisibility)
	{
		SetVisibility(_s.p_spriteSheet.m_index, _s.index, _visible, _markVisibility);
	}

	public static void SetVisibility(int _sheetIndex, int _spriteIndex, bool _visible)
	{
		SetVisibility(_sheetIndex, _spriteIndex, _visible, true);
	}

	public static void SetVisibility(int _sheetIndex, int _spriteIndex, bool _visible, bool _markVisibility)
	{
		SpriteC spriteC = m_sheets.m_array[_sheetIndex].m_components.m_array[_spriteIndex];
		spriteC.visible = _visible;
		if (_markVisibility)
		{
			spriteC.isVisible = _visible;
		}
		spriteC.update = true;
		m_sheets.m_array[_sheetIndex].m_vertsChanged = true;
	}

	public static void SetVisibilityByTransformComponent(TransformC _tc, bool _visible, bool _affectChildren, bool _affectWholeHierarchy)
	{
		if (_affectWholeHierarchy)
		{
			_tc = TransformS.GetRootTransformComponent(_tc);
		}
		if (_affectChildren || _affectWholeHierarchy)
		{
			for (int i = 0; i < _tc.childs.Count; i++)
			{
				SetVisibilityByTransformComponent(_tc.childs[i], _visible, true, false);
			}
		}
		for (int j = 0; j < m_sheets.m_aliveCount; j++)
		{
			SpriteSheet spriteSheet = m_sheets.m_array[m_sheets.m_aliveIndices[j]];
			p_sprites = spriteSheet.m_components.m_array;
			p_aliveIndices = spriteSheet.m_components.m_aliveIndices;
			int aliveCount = spriteSheet.m_components.m_aliveCount;
			for (int k = 0; k < aliveCount; k++)
			{
				SpriteC spriteC = p_sprites[p_aliveIndices[k]];
				if (spriteC.p_TC == _tc)
				{
					SetVisibility(spriteC, _visible);
				}
			}
		}
	}

	public static void SetAlignment(int _sheetIndex, int _spriteIndex, Align _horizontal, Align _vertical)
	{
		float horizontal = 0f;
		switch (_horizontal)
		{
		case Align.Left:
			horizontal = -0.5f;
			break;
		case Align.Right:
			horizontal = 0.5f;
			break;
		}
		float vertical = 0f;
		switch (_horizontal)
		{
		case Align.Top:
			vertical = 0.5f;
			break;
		case Align.Bottom:
			vertical = -0.5f;
			break;
		}
		SetAlignment(_sheetIndex, _spriteIndex, horizontal, vertical);
	}

	public static void SetAlignment(int _sheetIndex, int _spriteIndex, float _horizontal, float _vertical)
	{
		SpriteC spriteC = m_sheets.m_array[_sheetIndex].m_components.m_array[_spriteIndex];
		spriteC.align.x = (0f - _horizontal) * spriteC.width;
		spriteC.align.y = _vertical * spriteC.height;
		spriteC.update = true;
		m_sheets.m_array[_sheetIndex].m_vertsChanged = true;
	}

	public static void SetOffset(SpriteC _s, Vector3 _pos, float _rot)
	{
		_s.offset = _pos;
		float num = Mathf.Cos(_rot * ((float)Math.PI / 180f));
		float num2 = Mathf.Sin(_rot * ((float)Math.PI / 180f));
		_s.offsetRight = new Vector3(num, num2, 0f);
		_s.offsetUp = new Vector3(0f - num2, num, 0f);
		_s.p_TC.updatePosition = true;
		_s.p_TC.updateRotation = true;
		_s.p_spriteSheet.m_vertsChanged = true;
	}

	public static void SetColor(SpriteC _sprite, Color _color)
	{
		int num = _sprite.vertDataIndex * 4;
		Color[] colors = _sprite.p_spriteSheet.m_colors;
		colors[num] = _color;
		colors[num + 1] = _color;
		colors[num + 2] = _color;
		colors[num + 3] = _color;
		_sprite.color = _color;
		_sprite.p_spriteSheet.m_colorsChanged = true;
	}

	public static void SetColorByTransformComponent(TransformC _tc, Color _color, bool _affectChildren, bool _affectWholeHierarchy)
	{
		if (_affectWholeHierarchy)
		{
			_tc = TransformS.GetRootTransformComponent(_tc);
		}
		if (_affectChildren || _affectWholeHierarchy)
		{
			for (int i = 0; i < _tc.childs.Count; i++)
			{
				SetColorByTransformComponent(_tc.childs[i], _color, true, false);
			}
		}
		for (int j = 0; j < m_sheets.m_aliveCount; j++)
		{
			SpriteSheet spriteSheet = m_sheets.m_array[m_sheets.m_aliveIndices[j]];
			p_sprites = spriteSheet.m_components.m_array;
			p_aliveIndices = spriteSheet.m_components.m_aliveIndices;
			int aliveCount = spriteSheet.m_components.m_aliveCount;
			for (int k = 0; k < aliveCount; k++)
			{
				SpriteC spriteC = p_sprites[p_aliveIndices[k]];
				if (spriteC.p_TC == _tc)
				{
					SetColor(spriteC, _color);
				}
			}
		}
	}

	public static void SetAlphaByTransformComponent(TransformC _tc, float _alpha, bool _affectChildren, bool _affectWholeHierarchy)
	{
		if (_affectWholeHierarchy)
		{
			_tc = TransformS.GetRootTransformComponent(_tc);
		}
		if (_affectChildren || _affectWholeHierarchy)
		{
			for (int i = 0; i < _tc.childs.Count; i++)
			{
				SetAlphaByTransformComponent(_tc.childs[i], _alpha, true, false);
			}
		}
		for (int j = 0; j < m_sheets.m_aliveCount; j++)
		{
			SpriteSheet spriteSheet = m_sheets.m_array[m_sheets.m_aliveIndices[j]];
			p_sprites = spriteSheet.m_components.m_array;
			p_aliveIndices = spriteSheet.m_components.m_aliveIndices;
			int aliveCount = spriteSheet.m_components.m_aliveCount;
			for (int k = 0; k < aliveCount; k++)
			{
				SpriteC spriteC = p_sprites[p_aliveIndices[k]];
				Color color = spriteC.color;
				color.a = _alpha;
				if (spriteC.p_TC == _tc)
				{
					SetColor(spriteC, color);
				}
			}
		}
	}

	public static void SetFlip(SpriteC _c, bool _x, bool _y)
	{
		_c.frame.flipX = _x;
		_c.frame.flipY = _y;
		SetFrame(_c.p_spriteSheet, _c, _c.frame);
	}

	public static void SetSortValue(TransformC _tc, float _sortValue)
	{
		for (int i = 0; i < m_sheets.m_aliveCount; i++)
		{
			SpriteSheet spriteSheet = m_sheets.m_array[m_sheets.m_aliveIndices[i]];
			p_sprites = spriteSheet.m_components.m_array;
			p_aliveIndices = spriteSheet.m_components.m_aliveIndices;
			int aliveCount = spriteSheet.m_components.m_aliveCount;
			for (int j = 0; j < aliveCount; j++)
			{
				SpriteC spriteC = p_sprites[p_aliveIndices[j]];
				if (spriteC.p_TC == _tc)
				{
					SetSortValue(spriteC, _sortValue);
				}
			}
		}
	}

	public static void SetSortValue(SpriteC _s, float _sortValue)
	{
		SetSortValue(_s, _sortValue, true);
	}

	public static void SetSortValue(SpriteC _s, float _sortValue, bool _sortMesh)
	{
		_s.sortValue = _sortValue;
		if (_sortMesh)
		{
			_s.p_spriteSheet.m_sortMesh = true;
		}
	}

	public static void SortMesh(SpriteSheet _sheet)
	{
		for (int i = 0; i < _sheet.m_components.m_arrayLength; i++)
		{
			_sheet.m_components.m_array[i].vertDataIndex = i;
		}
		for (int j = 0; j < _sheet.m_components.m_arrayLength; j++)
		{
			int num = _sheet.m_components.m_array[j].vertDataIndex * 4;
			for (int k = 0; k < 4; k++)
			{
				_sheet.m_vertices[num + k] = m_outOfScreen;
			}
		}
		int[] array = new int[_sheet.m_components.m_aliveCount];
		float[] array2 = new float[_sheet.m_components.m_aliveCount];
		for (int l = 0; l < _sheet.m_components.m_aliveCount; l++)
		{
			int num2 = (array[l] = _sheet.m_components.m_array[_sheet.m_components.m_aliveIndices[l]].vertDataIndex);
			array2[l] = _sheet.m_components.m_array[num2].sortValue;
		}
		array = ToolBox.sortTable(array, array2);
		for (int m = 0; m < _sheet.m_components.m_aliveCount; m++)
		{
			_sheet.m_components.m_array[array[m]].vertDataIndex = m;
			_sheet.m_components.m_array[array[m]].update = true;
		}
	}

	public static void SetFrame(SpriteSheet _sheet, SpriteC _sprite, Frame _frame)
	{
		_sprite.frame = _frame;
		if (_sprite.wDimension > 0f)
		{
			_sprite.width = _sprite.wDimension * _sheet.m_globalSpriteScale;
		}
		else
		{
			_sprite.wDimension = _frame.width;
			_sprite.width = _frame.width * _sheet.m_globalSpriteScale;
		}
		if (_sprite.hDimension > 0f)
		{
			_sprite.height = _sprite.hDimension * _sheet.m_globalSpriteScale;
		}
		else
		{
			_sprite.hDimension = _frame.height;
			_sprite.height = _frame.height * _sheet.m_globalSpriteScale;
		}
		if (_sprite.vertDataIndex > -1)
		{
			float num = _frame.x / (float)_sheet.m_textureWidth;
			float num2 = (_frame.x + _frame.width) / (float)_sheet.m_textureWidth;
			float num3 = 1f - _frame.y / (float)_sheet.m_textureHeight;
			float num4 = 1f - (_frame.y + _frame.height) / (float)_sheet.m_textureHeight;
			if (_sprite.frame.flipX)
			{
				float num5 = num;
				num = num2;
				num2 = num5;
			}
			if (_sprite.frame.flipY)
			{
				float num6 = num3;
				num3 = num4;
				num4 = num6;
			}
			int num7 = _sprite.vertDataIndex * 4;
			Vector2[] uVs = _sheet.m_UVs;
			uVs[num7 + 3].x = num2;
			uVs[num7 + 3].y = num3;
			uVs[num7 + 2].x = num2;
			uVs[num7 + 2].y = num4;
			uVs[num7 + 1].x = num;
			uVs[num7 + 1].y = num4;
			uVs[num7].x = num;
			uVs[num7].y = num3;
			_sheet.m_uvsChanged = true;
			Color[] colors = _sheet.m_colors;
			colors[num7] = _sprite.color;
			colors[num7 + 1] = _sprite.color;
			colors[num7 + 2] = _sprite.color;
			colors[num7 + 3] = _sprite.color;
			_sheet.m_colorsChanged = true;
		}
	}

	public static void ClearSheet(SpriteSheet _sheet)
	{
		for (int i = 0; i < _sheet.m_components.m_arrayLength; i++)
		{
			_sheet.m_components.m_array[i].active = false;
			_sheet.m_components.m_freeArray[i] = true;
			_sheet.m_components.m_freeIndices[i] = i;
			_sheet.m_components.m_array[i].p_TC = null;
			int num = _sheet.m_components.m_array[i].vertDataIndex * 4;
			for (int j = 0; j < 4; j++)
			{
				_sheet.m_vertices[num + j] = m_outOfScreen;
			}
		}
		_sheet.m_vertsChanged = true;
		_sheet.m_colorsChanged = true;
		_sheet.m_uvsChanged = true;
		_sheet.m_components.m_freeCount = _sheet.m_components.m_arrayLength;
		_sheet.m_components.m_aliveCount = 0;
		_sheet.m_components.m_lastReserved = 0;
	}

	public static void Update()
	{
		int aliveCount = m_sheets.m_aliveCount;
		for (int i = 0; i < aliveCount; i++)
		{
			SpriteSheet spriteSheet = m_sheets.m_array[m_sheets.m_aliveIndices[i]];
			p_sprites = spriteSheet.m_components.m_array;
			p_vertices = spriteSheet.m_vertices;
			p_aliveIndices = spriteSheet.m_components.m_aliveIndices;
			int aliveCount2 = spriteSheet.m_components.m_aliveCount;
			Camera camera = spriteSheet.m_camera;
			if (spriteSheet.m_sortMesh)
			{
				SortMesh(spriteSheet);
			}
			for (int j = 0; j < aliveCount2; j++)
			{
				SpriteC spriteC = p_sprites[p_aliveIndices[j]];
				if (spriteSheet.m_sortMesh)
				{
					SetFrame(spriteSheet, spriteC, spriteC.frame);
				}
				if (!spriteC.active && !spriteC.update)
				{
					continue;
				}
				if (spriteC.visible)
				{
					TransformC p_TC = spriteC.p_TC;
					Transform transform = spriteC.p_TC.transform;
					bool flag = false;
					if (p_TC.updatedScale || spriteC.update)
					{
						if (p_TC.forceScale)
						{
							spriteC.wScale = spriteC.width * spriteC.dimensionScale * 0.5f * p_TC.forcedScale.x;
							spriteC.hScale = spriteC.height * spriteC.dimensionScale * 0.5f * p_TC.forcedScale.y;
						}
						else
						{
							Vector3 lossyScale = transform.lossyScale;
							spriteC.wScale = spriteC.width * spriteC.dimensionScale * 0.5f * lossyScale.x;
							spriteC.hScale = spriteC.height * spriteC.dimensionScale * 0.5f * lossyScale.y;
						}
						flag = true;
						debug_scaleUpdates++;
					}
					if (p_TC.updatedRotation || flag)
					{
						if (p_TC.forceRotation)
						{
							spriteC.relRight = p_TC.forcedRotation * spriteC.offsetRight;
							spriteC.relUp = p_TC.forcedRotation * spriteC.offsetUp;
							spriteC.relOffset = p_TC.forcedRotation * spriteC.offset;
							spriteC.relOffset.Scale(transform.lossyScale);
						}
						else
						{
							Quaternion rotation = transform.rotation;
							spriteC.relRight = rotation * spriteC.offsetRight;
							spriteC.relUp = rotation * spriteC.offsetUp;
							spriteC.relOffset = rotation * spriteC.offset;
							spriteC.relOffset.Scale(transform.lossyScale);
						}
						flag = true;
						debug_rotationUpdates++;
					}
					if (p_TC.updatedPosition || flag)
					{
						if (flag)
						{
							spriteC.scaledRelRight = spriteC.relRight * spriteC.wScale;
							spriteC.scaledRelUp = spriteC.relUp * spriteC.hScale;
						}
						Vector3 vector = transform.position + spriteC.relOffset;
						int num = spriteC.vertDataIndex * 4;
						p_vertices[num] = vector + spriteC.scaledRelUp - spriteC.scaledRelRight;
						p_vertices[num + 1] = vector - spriteC.scaledRelUp - spriteC.scaledRelRight;
						p_vertices[num + 2] = vector - spriteC.scaledRelUp + spriteC.scaledRelRight;
						p_vertices[num + 3] = vector + spriteC.scaledRelUp + spriteC.scaledRelRight;
						spriteSheet.m_vertsChanged = true;
						spriteC.update = false;
						debug_positionUpdates++;
					}
				}
				else if (spriteC.update)
				{
					Vector3 position = spriteSheet.m_camera.transform.position;
					int num2 = spriteC.vertDataIndex * 4;
					p_vertices[num2] = position;
					p_vertices[num2 + 1] = position;
					p_vertices[num2 + 2] = position;
					p_vertices[num2 + 3] = position;
					spriteSheet.m_vertsChanged = true;
					spriteC.update = false;
				}
			}
			if (spriteSheet.m_vertsChanged)
			{
				spriteSheet.m_mesh.vertices = p_vertices;
				spriteSheet.m_vertsChanged = false;
			}
			if (spriteSheet.m_colorsChanged)
			{
				spriteSheet.m_mesh.colors = spriteSheet.m_colors;
				spriteSheet.m_colorsChanged = false;
			}
			if (spriteSheet.m_uvsChanged)
			{
				spriteSheet.m_mesh.uv = spriteSheet.m_UVs;
				spriteSheet.m_uvsChanged = false;
			}
			if (spriteSheet.m_sortMesh)
			{
				spriteSheet.m_sortMesh = false;
			}
			debug_scaleUpdates = 0;
			debug_rotationUpdates = 0;
			debug_positionUpdates = 0;
		}
	}

	public static List<PrefabC> ConvertSpritesToPrefabComponent(TransformC _tc, bool _removeSprites)
	{
		return ConvertSpritesToPrefabComponent(_tc, null, _removeSprites);
	}

	public static List<PrefabC> ConvertSpritesToPrefabComponent(TransformC _tc, Camera _camera, bool _removeSprites)
	{
		Update();
		List<PrefabC> list = new List<PrefabC>();
		List<IComponent> componentsByEntityIndex = EntityManager.GetComponentsByEntityIndex(ComponentType.Sprite, _tc.entityIndex);
		List<GameObject> list2 = new List<GameObject>();
		List<SpriteSheet> list3 = new List<SpriteSheet>();
		List<List<int>> list4 = new List<List<int>>();
		if (componentsByEntityIndex.Count > 0)
		{
			for (int i = 0; i < componentsByEntityIndex.Count; i++)
			{
				SpriteC spriteC = componentsByEntityIndex[i] as SpriteC;
				SpriteSheet p_spriteSheet = spriteC.p_spriteSheet;
				bool flag = false;
				int num = 0;
				if (list3.Count > 0)
				{
					for (int j = 0; j < list3.Count; j++)
					{
						if (p_spriteSheet == list3[j])
						{
							flag = true;
							num = j;
							list4[num].Add(i);
							break;
						}
					}
				}
				if (!flag || list3.Count == 0)
				{
					num = list3.Count;
					list3.Add(p_spriteSheet);
					list4.Add(new List<int>());
					list4[num].Add(i);
					list2.Add(new GameObject("ConvertedSprites"));
					if (_camera == null)
					{
						list2[num].layer = p_spriteSheet.m_camera.gameObject.layer;
					}
					else
					{
						list2[num].layer = _camera.gameObject.layer;
					}
					MeshFilter meshFilter = list2[num].AddComponent("MeshFilter") as MeshFilter;
					MeshRenderer meshRenderer = list2[num].AddComponent("MeshRenderer") as MeshRenderer;
					meshRenderer.renderer.material = p_spriteSheet.m_material;
				}
			}
			for (int k = 0; k < list3.Count; k++)
			{
				Vector3[] array = new Vector3[list4[k].Count * 4];
				Vector3[] normals = new Vector3[list4[k].Count * 4];
				Vector2[] array2 = new Vector2[list4[k].Count * 4];
				Color[] array3 = new Color[list4[k].Count * 4];
				int[] array4 = new int[list4[k].Count * 6];
				for (int l = 0; l < list4[k].Count; l++)
				{
					array4[l * 6 + 5] = l * 4;
					array4[l * 6 + 4] = l * 4 + 1;
					array4[l * 6 + 3] = l * 4 + 3;
					array4[l * 6 + 2] = l * 4 + 3;
					array4[l * 6 + 1] = l * 4 + 1;
					array4[l * 6] = l * 4 + 2;
				}
				int[] array5 = new int[list4[k].Count];
				float[] array6 = new float[list4[k].Count];
				for (int m = 0; m < list4[k].Count; m++)
				{
					array5[m] = list4[k][m];
					array6[m] = (componentsByEntityIndex[list4[k][m]] as SpriteC).sortValue;
				}
				int[] array7 = ToolBox.sortTable(array5, array6);
				for (int n = 0; n < list4[k].Count; n++)
				{
					SpriteC spriteC2 = componentsByEntityIndex[array7[n]] as SpriteC;
					int num2 = spriteC2.vertDataIndex * 4;
					int num3 = n * 4;
					Vector3[] vertices = list3[k].m_vertices;
					array[num3] = vertices[num2];
					array[num3 + 1] = vertices[num2 + 1];
					array[num3 + 2] = vertices[num2 + 2];
					array[num3 + 3] = vertices[num2 + 3];
					Vector2[] uVs = list3[k].m_UVs;
					array2[num3 + 3].x = uVs[num2 + 3].x;
					array2[num3 + 3].y = uVs[num2 + 3].y;
					array2[num3 + 2].x = uVs[num2 + 2].x;
					array2[num3 + 2].y = uVs[num2 + 2].y;
					array2[num3 + 1].x = uVs[num2 + 1].x;
					array2[num3 + 1].y = uVs[num2 + 1].y;
					array2[num3].x = uVs[num2].x;
					array2[num3].y = uVs[num2].y;
					Color[] colors = list3[k].m_colors;
					array3[num3] = colors[num2];
					array3[num3 + 1] = colors[num2 + 1];
					array3[num3 + 2] = colors[num2 + 2];
					array3[num3 + 3] = colors[num2 + 3];
					if (_removeSprites)
					{
						RemoveComponent(spriteC2);
					}
				}
				MeshFilter meshFilter2 = list2[k].gameObject.GetComponent("MeshFilter") as MeshFilter;
				meshFilter2.mesh.vertices = array;
				meshFilter2.mesh.uv = array2;
				meshFilter2.mesh.colors = array3;
				meshFilter2.mesh.triangles = array4;
				meshFilter2.mesh.normals = normals;
				meshFilter2.mesh.RecalculateBounds();
				list.Add(PrefabS.AddComponent(_tc, -_tc.transform.position, list2[k], "ConvertedSprites"));
				UnityEngine.Object.Destroy(list2[k]);
			}
		}
		return list;
	}

	public static GameObject CloneAsGameObject(SpriteSheet _sheet)
	{
		return UnityEngine.Object.Instantiate(_sheet.m_gameObject) as GameObject;
	}
}
