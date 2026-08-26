using System.Collections.Generic;
using UnityEngine;

public static class PrefabS
{
	private static GenericArray<PrefabC> m_components;

	public static GameObject m_emptyGameObject;

	public static void Initialize(int _maxComponentCount)
	{
		m_components = new GenericArray<PrefabC>(_maxComponentCount);
		for (int i = 0; i < _maxComponentCount; i++)
		{
			m_components.m_array[i] = new PrefabC();
			m_components.m_array[i].entityIndex = -1;
			m_components.m_array[i].index = i;
			m_components.m_array[i].componentType = ComponentType.Prefab;
		}
		m_emptyGameObject = new GameObject("PrefabSystem: InstantiateHelper");
		MeshFilter meshFilter = m_emptyGameObject.AddComponent<MeshFilter>() as MeshFilter;
		meshFilter.mesh = new Mesh();
		m_emptyGameObject.AddComponent<MeshRenderer>();
	}

	public static PrefabC AddComponent(TransformC _parentTC, Vector3 _offset)
	{
		return AddComponent(_parentTC, _offset, string.Empty);
	}

	public static PrefabC AddComponent(TransformC _parentTC, Vector3 _offset, string _identifier)
	{
		int num = m_components.AddItem();
		PrefabC prefabC = m_components.m_array[num];
		prefabC.entityIndex = _parentTC.entityIndex;
		prefabC.active = true;
		prefabC.p_gameObject = Object.Instantiate(m_emptyGameObject) as GameObject;
		prefabC.p_renderer = prefabC.p_gameObject.GetComponent<Renderer>();
		prefabC.p_texture = prefabC.p_gameObject.GetComponent<Renderer>().material.mainTexture;
		prefabC.p_mesh = (prefabC.p_gameObject.GetComponent("MeshFilter") as MeshFilter).mesh;
		prefabC.p_gameObject.transform.parent = _parentTC.transform;
		prefabC.p_gameObject.transform.localPosition = _offset;
		prefabC.p_gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
		prefabC.p_parentTC = _parentTC;
		prefabC.identifier = _identifier;
		prefabC.isVisible = true;
		EntityManager.m_entities.m_array[prefabC.entityIndex].components.Add(prefabC);
		return prefabC;
	}

	public static PrefabC AddComponent(TransformC _parentTC, Vector3 _offset, GameObject _gameObject)
	{
		return AddComponent(_parentTC, _offset, _gameObject, string.Empty);
	}

	public static PrefabC AddComponent(TransformC _parentTC, Vector3 _offset, GameObject _gameObject, string _identifier)
	{
		int num = m_components.AddItem();
		PrefabC prefabC = m_components.m_array[num];
		prefabC.entityIndex = _parentTC.entityIndex;
		prefabC.active = true;
		prefabC.p_gameObject = Object.Instantiate(_gameObject) as GameObject;
		prefabC.p_renderer = prefabC.p_gameObject.gameObject.GetComponent<Renderer>();
		if (prefabC.p_renderer != null)
		{
			prefabC.p_texture = prefabC.p_renderer.material.mainTexture;
			MeshFilter meshFilter = prefabC.p_gameObject.GetComponent("MeshFilter") as MeshFilter;
			if (meshFilter != null)
			{
				prefabC.p_mesh = meshFilter.mesh;
			}
		}
		prefabC.p_gameObject.transform.parent = _parentTC.transform;
		prefabC.p_gameObject.transform.localPosition = _offset;
		prefabC.p_parentTC = _parentTC;
		prefabC.identifier = _identifier;
		prefabC.isVisible = true;
		EntityManager.m_entities.m_array[prefabC.entityIndex].components.Add(prefabC);
		return prefabC;
	}

	public static void RemoveComponent(PrefabC _c)
	{
		if (_c.p_renderer != null)
		{
			Object.Destroy(_c.p_renderer.material);
		}
		if (_c.p_mesh != null)
		{
			Object.Destroy(_c.p_mesh);
		}
		if (_c.p_gameObject != null)
		{
			Object.Destroy(_c.p_gameObject);
		}
		_c.p_renderer = null;
		_c.p_texture = null;
		_c.p_mesh = null;
		_c.p_gameObject = null;
		_c.p_parentTC = null;
		_c.identifier = string.Empty;
		_c.isVisible = false;
		m_components.RemoveItem(_c.index);
		EntityManager.m_entities.m_array[_c.entityIndex].components.Remove(_c);
		_c.entityIndex = -1;
	}

	public static void RemoveComponentsByEntityIndex(int _index)
	{
		List<IComponent> componentsByEntityIndex = EntityManager.GetComponentsByEntityIndex(ComponentType.Prefab, _index);
		while (componentsByEntityIndex.Count > 0)
		{
			int index = componentsByEntityIndex.Count - 1;
			RemoveComponent(componentsByEntityIndex[index] as PrefabC);
			componentsByEntityIndex.RemoveAt(index);
		}
	}

	public static void Update()
	{
	}

	public static void SetVertexColors(PrefabC _c, Color _color)
	{
		MeshFilter meshFilter = _c.p_gameObject.GetComponent("MeshFilter") as MeshFilter;
		Mesh mesh = meshFilter.mesh;
		SetVertexColors(mesh, _color);
	}

	public static void SetVertexColors(GameObject _gameObject, Color _color)
	{
		MeshFilter meshFilter = _gameObject.GetComponent("MeshFilter") as MeshFilter;
		Mesh mesh = meshFilter.mesh;
		SetVertexColors(mesh, _color);
	}

	public static void SetVertexColors(Mesh _mesh, Color _color)
	{
		Color[] array = new Color[_mesh.colors.Length];
		for (int i = 0; i < _mesh.colors.Length; i++)
		{
			array[i] = _color;
		}
		_mesh.colors = array;
	}

	public static void SetVisibilityByTransformComponent(TransformC _tc, bool _visible)
	{
		int aliveCount = m_components.m_aliveCount;
		for (int i = 0; i < aliveCount; i++)
		{
			PrefabC prefabC = m_components.m_array[m_components.m_aliveIndices[i]];
			if (prefabC.p_parentTC == _tc)
			{
				SetVisibility(prefabC, _visible, true);
			}
		}
	}

	public static void SetVisibility(PrefabC _c, bool _visible)
	{
		SetVisibility(_c, _visible, true);
	}

	public static void SetVisibility(PrefabC _c, bool _visible, bool _markVisibility)
	{
		RecursiveRendererVisibility(_c.p_gameObject.transform, _visible);
		if (_markVisibility)
		{
			_c.isVisible = _visible;
		}
	}

	public static void RecursiveRendererVisibility(Transform _t, bool _visible)
	{
		_t.gameObject.active = _visible;
		MeshRenderer meshRenderer = _t.gameObject.GetComponent("MeshRenderer") as MeshRenderer;
		if (meshRenderer != null)
		{
			meshRenderer.enabled = _visible;
		}
		for (int i = 0; i < _t.childCount; i++)
		{
			RecursiveRendererVisibility(_t.GetChild(i), _visible);
		}
	}

	public static void ColorizeByTransformComponent(TransformC _tc, Color _color, bool _affectChildren, bool _affectWholeHierarchy)
	{
		if (_affectWholeHierarchy)
		{
			_tc = TransformS.GetRootTransformComponent(_tc);
		}
		if (_affectChildren || _affectWholeHierarchy)
		{
			for (int i = 0; i < _tc.childs.Count; i++)
			{
				ColorizeByTransformComponent(_tc.childs[i], _color, true, false);
			}
		}
		int aliveCount = m_components.m_aliveCount;
		for (int j = 0; j < aliveCount; j++)
		{
			PrefabC prefabC = m_components.m_array[m_components.m_aliveIndices[j]];
			if (prefabC.p_parentTC == _tc && prefabC.p_mesh != null)
			{
				SetVertexColors(prefabC.p_mesh, _color);
			}
		}
	}

	public static Color GetShaderColor(PrefabC _c)
	{
		return _c.p_renderer.material.GetColor("_Color");
	}

	public static void SetShaderColor(PrefabC _c, Color _color)
	{
		_c.p_renderer.material.SetColor("_Color", _color);
	}

	public static List<PrefabC> CreatePathPrefabComponentFromPolygon(TransformC _tc, Vector3 _offset, Polygon _polygon, float _width, Color _color, Material _material, Camera _camera, Position _align, bool _closed)
	{
		List<PrefabC> list = new List<PrefabC>();
		for (int i = 0; i < _polygon.NofContours; i++)
		{
			GraphicsPath graphicsPath = _polygon.Contour[i].ToGraphicsPath();
			Vector2[] pathPoints = graphicsPath.PathPoints;
			list.Add(CreatePathPrefabComponentFromVectorArray(_tc, _offset, pathPoints, _width, _color, _material, _camera, _align, _closed));
		}
		return list;
	}

	public static PrefabC CreatePathPrefabComponentFromVectorArray(TransformC _tc, Vector3 _offset, Vector2[] _points, float _width, Color _color, Material _material, Camera _camera, Position _align, bool _closed)
	{
		PrefabC prefabC = AddComponent(_tc, Vector3.zero);
		prefabC.p_gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
		prefabC.p_gameObject.layer = _camera.gameObject.layer;
		Object.Destroy(prefabC.p_renderer.material);
		prefabC.p_renderer.material = _material;
		Vector2[] array;
		if (_points[0] - _points[_points.Length - 1] == Vector2.zero || !_closed)
		{
			array = _points;
		}
		else
		{
			array = new Vector2[_points.Length + 1];
			_points.CopyTo(array, 0);
			array[array.Length - 1] = array[0];
		}
		Vector3[] array2 = new Vector3[array.Length * 2];
		Vector3[] array3 = new Vector3[array.Length * 2];
		Vector2[] array4 = new Vector2[array.Length * 2];
		Color[] array5 = new Color[array.Length * 2];
		int[] array6 = new int[array.Length * 6];
		int num = -1;
		for (int i = 0; i < array.Length; i++)
		{
			Vector2 vector = array[i];
			Vector2 vector2;
			Vector2 vector3;
			if (!_closed)
			{
				if (i == 0)
				{
					vector2 = array[array.Length - 1];
					vector3 = array[i + 1];
				}
				else if (i == array.Length - 1)
				{
					vector2 = array[i - 1];
					vector3 = array[0];
				}
				else
				{
					vector2 = array[i - 1];
					vector3 = array[i + 1];
				}
			}
			else if (i == 0)
			{
				vector2 = array[array.Length - 2];
				vector3 = array[i + 1];
			}
			else if (i == array.Length - 1)
			{
				vector2 = array[i - 1];
				vector3 = array[1];
			}
			else
			{
				vector2 = array[i - 1];
				vector3 = array[i + 1];
			}
			Vector2 normalized = (vector2 - vector).normalized;
			Vector2 normalized2 = (vector - vector3).normalized;
			float f = Mathf.Atan2(0f - normalized.y, normalized.x);
			float x = Mathf.Sin(f);
			float y = Mathf.Cos(f);
			Vector2 vector4 = new Vector2(x, y);
			float f2 = Mathf.Atan2(0f - normalized2.y, normalized2.x);
			float x2 = Mathf.Sin(f2);
			float y2 = Mathf.Cos(f2);
			Vector2 vector5 = new Vector2(x2, y2);
			Vector2 normalized3 = ((vector4 + vector5) * 0.5f).normalized;
			Vector3 vector6 = new Vector3(normalized3.x, normalized3.y, 0f);
			Vector3 vector7 = new Vector3(vector.x, vector.y, 0f);
			Vector3 vector8 = vector7;
			Vector3 vector9 = vector7;
			switch (_align)
			{
			case Position.Center:
				vector8 = vector7 + vector6 * _width * 0.5f;
				vector9 = vector7 - vector6 * _width * 0.5f;
				break;
			case Position.Inside:
				vector8 = vector7 + vector6 * _width;
				vector9 = vector7;
				break;
			case Position.Outside:
				vector8 = vector7;
				vector9 = vector7 - vector6 * _width;
				break;
			}
			array2[i * 2] = vector8 + _offset;
			array2[i * 2 + 1] = vector9 + _offset;
			if (num == -1)
			{
				array4[i * 2] = Vector2.zero;
				array4[i * 2 + 1] = Vector2.right;
			}
			else
			{
				array4[i * 2] = Vector2.up;
				array4[i * 2 + 1] = Vector2.one;
			}
			num *= -1;
			array3[i * 2] = Vector3.forward;
			array3[i * 2 + 1] = Vector3.forward;
			array5[i * 2] = _color;
			array5[i * 2 + 1] = _color;
			if (i < array.Length - 1)
			{
				array6[i * 6] = i * 2;
				array6[i * 6 + 1] = i * 2 + 1;
				array6[i * 6 + 2] = i * 2 + 2;
				array6[i * 6 + 3] = i * 2 + 2;
				array6[i * 6 + 4] = i * 2 + 1;
				array6[i * 6 + 5] = i * 2 + 3;
			}
		}
		prefabC.p_mesh.vertices = array2;
		prefabC.p_mesh.uv = array4;
		prefabC.p_mesh.colors = array5;
		prefabC.p_mesh.triangles = array6;
		prefabC.p_mesh.normals = array3;
		prefabC.p_mesh.RecalculateBounds();
		prefabC.p_mesh.RecalculateNormals();
		return prefabC;
	}

	public static PrefabC CreateLinePrefabComponentFromVectorArray(TransformC _tc, Vector3 _offset, Vector2[] _points, float _width, Color _color, Material _material, Camera _camera, Position _align)
	{
		PrefabC prefabC = AddComponent(_tc, Vector3.zero);
		prefabC.p_gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
		prefabC.p_gameObject.layer = _camera.gameObject.layer;
		Object.Destroy(prefabC.p_renderer.material);
		prefabC.p_renderer.material = _material;
		Vector3[] array = new Vector3[_points.Length * 2];
		Vector3[] array2 = new Vector3[_points.Length * 2];
		Vector2[] array3 = new Vector2[_points.Length * 2];
		Color[] array4 = new Color[_points.Length * 2];
		int[] array5 = new int[_points.Length * 6];
		int num = -1;
		for (int i = 0; i < _points.Length; i++)
		{
			Vector2 vector = _points[i];
			Vector2 vector2;
			Vector2 vector3;
			if (i == 0)
			{
				vector2 = _points[i + 1];
				vector3 = vector + vector - vector2;
			}
			else if (i == _points.Length - 1)
			{
				vector3 = _points[i - 1];
				vector2 = vector + vector - vector3;
			}
			else
			{
				vector3 = _points[i - 1];
				vector2 = _points[i + 1];
			}
			Vector2 normalized = (vector3 - vector).normalized;
			Vector2 normalized2 = (vector - vector2).normalized;
			float f = Mathf.Atan2(0f - normalized.y, normalized.x);
			float x = Mathf.Sin(f);
			float y = Mathf.Cos(f);
			Vector2 vector4 = new Vector2(x, y);
			float f2 = Mathf.Atan2(0f - normalized2.y, normalized2.x);
			float x2 = Mathf.Sin(f2);
			float y2 = Mathf.Cos(f2);
			Vector2 vector5 = new Vector2(x2, y2);
			Vector2 normalized3 = ((vector4 + vector5) * 0.5f).normalized;
			Vector3 vector6 = new Vector3(normalized3.x, normalized3.y, 0f);
			Vector3 vector7 = new Vector3(vector.x, vector.y, 0f);
			Vector3 vector8 = vector7;
			Vector3 vector9 = vector7;
			switch (_align)
			{
			case Position.Center:
				vector8 = vector7 + vector6 * _width * 0.5f;
				vector9 = vector7 - vector6 * _width * 0.5f;
				break;
			case Position.Inside:
				vector8 = vector7 + vector6 * _width;
				vector9 = vector7;
				break;
			case Position.Outside:
				vector8 = vector7;
				vector9 = vector7 - vector6 * _width;
				break;
			}
			array[i * 2] = vector8 + _offset;
			array[i * 2 + 1] = vector9 + _offset;
			if (num == -1)
			{
				array3[i * 2] = Vector2.zero;
				array3[i * 2 + 1] = Vector2.right;
			}
			else
			{
				array3[i * 2] = Vector2.up;
				array3[i * 2 + 1] = Vector2.one;
			}
			num *= -1;
			array2[i * 2] = Vector3.forward;
			array2[i * 2 + 1] = Vector3.forward;
			array4[i * 2] = _color;
			array4[i * 2 + 1] = _color;
			if (i < _points.Length - 1)
			{
				array5[i * 6] = i * 2;
				array5[i * 6 + 1] = i * 2 + 1;
				array5[i * 6 + 2] = i * 2 + 2;
				array5[i * 6 + 3] = i * 2 + 2;
				array5[i * 6 + 4] = i * 2 + 1;
				array5[i * 6 + 5] = i * 2 + 3;
			}
		}
		prefabC.p_mesh.vertices = array;
		prefabC.p_mesh.uv = array3;
		prefabC.p_mesh.colors = array4;
		prefabC.p_mesh.triangles = array5;
		prefabC.p_mesh.normals = array2;
		prefabC.p_mesh.RecalculateBounds();
		prefabC.p_mesh.RecalculateNormals();
		return prefabC;
	}

	public static uint ColorToUInt(Color _color)
	{
		return (uint)((Mathf.RoundToInt(_color.a * 255f) << 24) | (Mathf.RoundToInt(_color.r * 255f) << 16) | (Mathf.RoundToInt(_color.g * 255f) << 8) | Mathf.RoundToInt(_color.b * 255f));
	}

	public static Color UIntToColor(uint _uint)
	{
		return DebugDraw.GetColor((_uint >> 16) & 0xFF, (_uint >> 8) & 0xFF, _uint & 0xFF, (_uint >> 24) & 0xFF);
	}

	public static List<PrefabC> CreateFlatPrefabComponentsFromPolygon(TransformC _tc, Vector3 _offset, Polygon _polygon, Color _color, Material _material, Camera _camera)
	{
		uint num = ColorToUInt(_color);
		return CreateFlatPrefabComponentsFromPolygon(_tc, _offset, _polygon, num, num, _material, _camera, string.Empty);
	}

	public static List<PrefabC> CreateFlatPrefabComponentsFromVectorArray(TransformC _tc, Vector3 _offset, Vector2[] _points, uint _bottomColor, uint _topColor, Material _material, Camera _camera, string _identifier)
	{
		Polygon polygon = new Polygon();
		polygon.AddContour(new VertexList(_points), false);
		return CreateFlatPrefabComponentsFromPolygon(_tc, _offset, polygon, _bottomColor, _topColor, _material, _camera, _identifier);
	}

	public static List<PrefabC> CreateFlatPrefabComponentsFromPolygon(TransformC _tc, Vector3 _offset, Polygon _polygon, uint _bottomColor, uint _topColor, Material _material, Camera _camera, string _identifier)
	{
		List<PrefabC> list = new List<PrefabC>();
		if (_polygon.NofContours > 0)
		{
			Tristrip tristrip = _polygon.ToTristrip();
			float num = 99999f;
			float num2 = -99999f;
			if (_topColor != _bottomColor)
			{
				for (int i = 0; i < tristrip.NofStrips; i++)
				{
					VertexList vertexList = tristrip.Strip[i];
					for (int j = 0; j < vertexList.NofVertices; j++)
					{
						Vector2 vector = vertexList.Vertex[j];
						if (vector.y < num)
						{
							num = vector.y;
						}
						if (vector.y > num2)
						{
							num2 = vector.y;
						}
					}
				}
			}
			float num3 = num2 - num;
			for (int k = 0; k < tristrip.NofStrips; k++)
			{
				PrefabC prefabC = AddComponent(_tc, Vector3.zero);
				prefabC.p_gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
				prefabC.p_gameObject.layer = _camera.gameObject.layer;
				Object.Destroy(prefabC.p_renderer.material);
				prefabC.p_renderer.material = _material;
				prefabC.identifier = _identifier;
				VertexList vertexList2 = tristrip.Strip[k];
				Vector3[] array = new Vector3[vertexList2.NofVertices];
				Vector2[] array2 = new Vector2[vertexList2.NofVertices];
				Color[] array3 = new Color[vertexList2.NofVertices];
				int[] array4 = new int[(vertexList2.NofVertices - 2) * 3];
				int num4 = -1;
				for (int l = 0; l < vertexList2.NofVertices; l++)
				{
					Vector2 vector2 = vertexList2.Vertex[l];
					array[l] = new Vector3(vector2.x, vector2.y, 0f) + _offset;
					array2[l] = vector2;
					Color color = UIntToColor(_bottomColor);
					Color color2 = UIntToColor(_topColor);
					float num5 = (vector2.y - num) / num3;
					array3[l] = color2 * num5 + color * (1f - num5);
					if (l < vertexList2.NofVertices - 2)
					{
						if (num4 == -1)
						{
							array4[l * 3] = l;
							array4[l * 3 + 1] = l + 2;
							array4[l * 3 + 2] = l + 1;
							num4 *= -1;
						}
						else
						{
							array4[l * 3] = l;
							array4[l * 3 + 1] = l + 1;
							array4[l * 3 + 2] = l + 2;
							num4 *= -1;
						}
					}
				}
				prefabC.p_mesh.vertices = array;
				prefabC.p_mesh.triangles = array4;
				prefabC.p_mesh.uv = array2;
				prefabC.p_mesh.colors = array3;
				prefabC.p_mesh.RecalculateBounds();
				prefabC.p_mesh.RecalculateNormals();
				list.Add(prefabC);
			}
		}
		return list;
	}
}
