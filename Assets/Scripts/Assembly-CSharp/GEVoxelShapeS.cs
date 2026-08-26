using System.Collections.Generic;
using UnityEngine;
using VoxelTerrain;

public static class GEVoxelShapeS
{
	private static GenericArray<GEVoxelShapeC> m_components;

	public static void Initialize(int _maxComponentCount)
	{
		m_components = new GenericArray<GEVoxelShapeC>(_maxComponentCount);
		for (int i = 0; i < _maxComponentCount; i++)
		{
			m_components.m_array[i] = new GEVoxelShapeC();
			m_components.m_array[i].entityIndex = -1;
			m_components.m_array[i].index = i;
			m_components.m_array[i].componentType = (ComponentType)115;
		}
	}

	public static GEVoxelShapeC AddComponent(TransformC _TC, Vector3 _offset, VoxelData _data)
	{
		int num = m_components.AddItem();
		GEVoxelShapeC gEVoxelShapeC = m_components.m_array[num];
		gEVoxelShapeC.entityIndex = _TC.entityIndex;
		gEVoxelShapeC.active = true;
		gEVoxelShapeC.TC = _TC;
		gEVoxelShapeC.width = _data.width;
		gEVoxelShapeC.height = _data.height;
		gEVoxelShapeC.depth = _data.depth;
		gEVoxelShapeC.RES = _data.tileSize;
		gEVoxelShapeC.GOScale = _data.voxelScale;
		gEVoxelShapeC.enableFreeSculpting = _data.freeSculpt;
		gEVoxelShapeC.isPhysical = _data.isPhysical;
		gEVoxelShapeC.ISO = _data.iso;
		if (_data.map == null)
		{
			gEVoxelShapeC.map = new byte[gEVoxelShapeC.width + 1, gEVoxelShapeC.height + 1, gEVoxelShapeC.depth + 1];
		}
		else
		{
			gEVoxelShapeC.map = _data.map;
		}
		gEVoxelShapeC.GO = new GameObject("VoxelTerrainGameObject");
		gEVoxelShapeC.GO.AddComponent<MeshRenderer>();
		gEVoxelShapeC.GO.GetComponent<Renderer>().material = ResourceManager.GetMaterial(_data.groundSettings.fillMaterialResourceIdentifier);
		gEVoxelShapeC.GO.GetComponent<Renderer>().material.shader = ResourceManager.GetShader("VoxelShader");
		gEVoxelShapeC.GO.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(10f, 10f));
		gEVoxelShapeC.cubes = new List<GEVCube>();
		gEVoxelShapeC.reBuild = new List<GEVCube>();
		gEVoxelShapeC.reBuildCollider = new List<GEVCube>();
		gEVoxelShapeC.reBuildColliderCount = 0;
		for (int i = 0; i < gEVoxelShapeC.width / gEVoxelShapeC.RES; i++)
		{
			for (int j = 0; j < gEVoxelShapeC.height / gEVoxelShapeC.RES; j++)
			{
				for (int k = 0; k < gEVoxelShapeC.depth / gEVoxelShapeC.depth; k++)
				{
					Bounds bounds = default(Bounds);
					bounds.min = new Vector3(i, j, k) * gEVoxelShapeC.RES;
					bounds.max = bounds.min + new Vector3(gEVoxelShapeC.RES, gEVoxelShapeC.RES, gEVoxelShapeC.depth);
					GEVCube item = new GEVCube(gEVoxelShapeC, bounds);
					gEVoxelShapeC.cubes.Add(item);
				}
			}
		}
		gEVoxelShapeC.GO.transform.parent = _TC.transform;
		gEVoxelShapeC.GO.transform.localScale = Vector3.one * gEVoxelShapeC.GOScale;
		gEVoxelShapeC.GO.transform.localPosition = new Vector3((float)gEVoxelShapeC.width * (gEVoxelShapeC.GOScale * -0.5f), (float)gEVoxelShapeC.height * (gEVoxelShapeC.GOScale * -0.5f), 0f);
		if (_data.map == null)
		{
			ResetMap(gEVoxelShapeC);
		}
		else
		{
			ReBuild(gEVoxelShapeC);
			ReBuildCollider(gEVoxelShapeC);
		}
		EntityManager.m_entities.m_array[gEVoxelShapeC.entityIndex].components.Add(gEVoxelShapeC);
		return gEVoxelShapeC;
	}

	public static void RemoveComponent(GEVoxelShapeC _c)
	{
		_c.cubes = null;
		_c.reBuild = null;
		_c.reBuildCollider = null;
		_c.reBuildColliderCount = 0;
		_c.map = null;
		_c.colors = null;
		Object.Destroy(_c.GO);
		_c.GO = null;
		m_components.RemoveItem(_c.index);
		EntityManager.m_entities.m_array[_c.entityIndex].components.Remove(_c);
		_c.entityIndex = -1;
	}

	public static void ResetMap(GEVoxelShapeC _c)
	{
		int width = _c.width;
		int height = _c.height;
		int depth = _c.depth;
		byte[,,] map = _c.map;
		Color[,,] colors = _c.colors;
		for (int i = 0; i < width + 1; i++)
		{
			for (int j = 0; j < height + 1; j++)
			{
				for (int k = 0; k < depth + 1; k++)
				{
					map[i, j, k] = byte.MaxValue;
					if (colors != null)
					{
						colors[i, j, k] = Color.white;
					}
				}
			}
		}
		ReBuild(_c);
	}

	public static void ReBuild(GEVoxelShapeC _c, GEVCube _cube)
	{
		if (!_c.reBuild.Contains(_cube))
		{
			_c.reBuild.Add(_cube);
		}
		if (!_c.reBuildCollider.Contains(_cube))
		{
			_c.reBuildCollider.Add(_cube);
		}
	}

	public static void ReBuild(GEVoxelShapeC _c, Vector3 _point)
	{
		foreach (GEVCube cube in _c.cubes)
		{
			if (cube.bounds.Contains(_point))
			{
				ReBuild(_c, cube);
				break;
			}
		}
	}

	public static void ReBuild(GEVoxelShapeC _c, Bounds _bounds)
	{
		foreach (GEVCube cube in _c.cubes)
		{
			if (_bounds.Intersects(cube.bounds))
			{
				ReBuild(_c, cube);
			}
		}
	}

	public static void ReBuild(GEVoxelShapeC _c)
	{
		foreach (GEVCube cube in _c.cubes)
		{
			ReBuild(_c, cube);
		}
	}

	public static void ReBuildCollider(GEVoxelShapeC _c)
	{
		_c.reBuildColliderCount = _c.reBuildCollider.Count;
	}

	public static GameObject AddObject(GEVoxelShapeC _c)
	{
		GameObject gameObject = new GameObject("VCube");
		gameObject.transform.parent = _c.GO.transform;
		gameObject.AddComponent<MeshRenderer>().materials = _c.GO.GetComponent<Renderer>().materials;
		gameObject.AddComponent<MeshFilter>();
		if (_c.enableFreeSculpting)
		{
			gameObject.AddComponent<MeshCollider>();
		}
		return gameObject;
	}

	public static void Alteration(GEVoxelShapeC _c, Vector3 position, Vector3 scale, VoxelPaintShape obj, VoxelPaintEffect sfx, Color color)
	{
		Matrix4x4 worldToLocalMatrix = _c.GO.transform.worldToLocalMatrix;
		position = worldToLocalMatrix.MultiplyPoint(position);
		scale = worldToLocalMatrix.MultiplyVector(scale);
		Bounds bounds = new Bounds(position, scale);
		float num = scale.x / 2f;
		float num2 = scale.y / 2f;
		float num3 = scale.z / 2f;
		int num4 = Mathf.Max(Mathf.RoundToInt(position.x - num), 1);
		int num5 = Mathf.Max(Mathf.RoundToInt(position.y - num2), 1);
		int num6 = Mathf.Max(Mathf.RoundToInt(position.z - num3), 1);
		int num7 = Mathf.Min(Mathf.RoundToInt(position.x + num + 1f), _c.width);
		int num8 = Mathf.Min(Mathf.RoundToInt(position.y + num2 + 1f), _c.height);
		int num9 = Mathf.Min(Mathf.RoundToInt(position.z + num3 + 1f), _c.depth + 1);
		if (sfx == VoxelPaintEffect.SUB || sfx == VoxelPaintEffect.EROSION)
		{
			byte[,,] map = _c.map;
			for (int i = num4; i < num7; i++)
			{
				for (int j = num5; j < num8; j++)
				{
					for (int k = num6; k < num9; k++)
					{
						map[i, j, k] = (byte)(255 - map[i, j, k]);
					}
				}
			}
		}
		switch (sfx)
		{
		case VoxelPaintEffect.ADD:
		case VoxelPaintEffect.SUB:
			switch (obj)
			{
			case VoxelPaintShape.CUBE:
				AddCube(_c.map, bounds, num4, num5, num6, num7, num8, num9, color);
				break;
			case VoxelPaintShape.SPHERE:
				AddSphere(_c.map, bounds, num4, num5, num6, num7, num8, num9, color);
				break;
			case VoxelPaintShape.CYLINDER:
				AddCylinder(_c.map, bounds, num4, num5, num6, num7, num8, num9, color);
				break;
			case VoxelPaintShape.RANDOM:
				AddRandom(_c.map, bounds, num4, num5, num6, num7, num8, num9, color);
				break;
			}
			break;
		case VoxelPaintEffect.DILATION:
		case VoxelPaintEffect.EROSION:
			switch (obj)
			{
			case VoxelPaintShape.CUBE:
				DilationCube(_c.map, bounds, num4, num5, num6, num7, num8, num9, color);
				break;
			case VoxelPaintShape.SPHERE:
				DilationSphere(_c.map, bounds, num4, num5, num6, num7, num8, num9, color);
				break;
			case VoxelPaintShape.RANDOM:
				DilationRandom(_c.map, bounds, num4, num5, num6, num7, num8, num9, color);
				break;
			}
			break;
		case VoxelPaintEffect.PAINT:
			switch (obj)
			{
			case VoxelPaintShape.CUBE:
				PaintCube(_c.colors, bounds, num4, num5, num6, num7, num8, num9, color);
				break;
			case VoxelPaintShape.SPHERE:
				PaintSphere(_c.colors, bounds, num4, num5, num6, num7, num8, num9, color);
				break;
			case VoxelPaintShape.CYLINDER:
				PaintCylinder(_c.colors, bounds, num4, num5, num6, num7, num8, num9, color);
				break;
			case VoxelPaintShape.RANDOM:
				PaintRandom(_c.colors, bounds, num4, num5, num6, num7, num8, num9, color);
				break;
			}
			break;
		}
		if (sfx == VoxelPaintEffect.SUB || sfx == VoxelPaintEffect.EROSION)
		{
			byte[,,] map2 = _c.map;
			for (int l = num4; l < num7; l++)
			{
				for (int m = num5; m < num8; m++)
				{
					for (int n = num6; n < num9; n++)
					{
						map2[l, m, n] = (byte)(255 - map2[l, m, n]);
					}
				}
			}
		}
		bounds.SetMinMax(new Vector3(num4, num5, num6), new Vector3(num7, num8, num9));
		ReBuild(_c, bounds);
		if (_c.enableFreeSculpting)
		{
			ReBuildCollider(_c);
		}
	}

	private static void AddCube(byte[,,] _map, Bounds bounds, int bX, int bY, int bZ, int eX, int eY, int eZ, Color color)
	{
		for (int i = bX; i < eX; i++)
		{
			for (int j = bY; j < eY; j++)
			{
				for (int k = bZ; k < eZ; k++)
				{
					_map[i, j, k] = 0;
				}
			}
		}
	}

	private static void AddSphere(byte[,,] _map, Bounds bounds, int bX, int bY, int bZ, int eX, int eY, int eZ, Color color)
	{
		Vector3 center = bounds.center;
		float num = Mathf.Min(Mathf.Min(bounds.size.x, bounds.size.y), bounds.size.z) / 2f;
		for (int i = bX; i < eX; i++)
		{
			for (int j = bY; j < eY; j++)
			{
				for (int k = bZ; k < eZ; k++)
				{
					float num2 = (Vector3.Distance(new Vector3(i, j, k), center) - num) * 255f;
					byte b = (byte)((num2 > 255f) ? 255f : ((!(num2 < 0f)) ? num2 : 0f));
					if (b < _map[i, j, k])
					{
						_map[i, j, k] = b;
					}
				}
			}
		}
	}

	private static void AddCylinder(byte[,,] _map, Bounds bounds, int bX, int bY, int bZ, int eX, int eY, int eZ, Color color)
	{
		Vector3 center = bounds.center;
		float num = Mathf.Min(bounds.size.x, bounds.size.y) / 2f;
		for (int i = bX; i < eX; i++)
		{
			for (int j = bY; j < eY; j++)
			{
				for (int k = bZ; k < eZ; k++)
				{
					float num2 = (Vector3.Distance(new Vector3(i, j, center.z), center) - num) * 255f;
					byte b = (byte)((num2 > 255f) ? 255f : ((!(num2 < 0f)) ? num2 : 0f));
					if (b < _map[i, j, k])
					{
						_map[i, j, k] = b;
					}
				}
			}
		}
	}

	private static void AddRandom(byte[,,] _map, Bounds bounds, int bX, int bY, int bZ, int eX, int eY, int eZ, Color color)
	{
		Vector3 center = bounds.center;
		float num = Mathf.Min(Mathf.Min(bounds.size.x, bounds.size.y), bounds.size.z) / 2f;
		for (int i = bX; i < eX; i++)
		{
			for (int j = bY; j < eY; j++)
			{
				for (int k = bZ; k < eZ; k++)
				{
					float num2 = (Vector3.Distance(new Vector3(i, j, k), center) - num) * 0.5f;
					num2 = (num2 + Random.value) * 255f;
					byte b = (byte)((num2 > 255f) ? 255f : ((!(num2 < 0f)) ? num2 : 0f));
					if (b < _map[i, j, k])
					{
						_map[i, j, k] = b;
					}
				}
			}
		}
	}

	private static void DilationCube(byte[,,] _map, Bounds bounds, int bX, int bY, int bZ, int eX, int eY, int eZ, Color color)
	{
		for (int i = bX; i < eX - 1; i++)
		{
			for (int j = bY; j < eY - 1; j++)
			{
				for (int k = bZ; k < eZ - 1; k++)
				{
					byte b = _map[i, j, k];
					for (int l = i; l < i + 1; l++)
					{
						for (int m = j; m < j + 1; m++)
						{
							for (int n = k; n < k + 1; n++)
							{
								if (b > _map[l, m, n])
								{
									b = _map[l, m, n];
								}
								UnityEngine.Debug.Log("min" + b + "map" + _map[l, m, n]);
								UnityEngine.Debug.Break();
							}
						}
					}
					_map[i, j, k] = b;
				}
			}
		}
	}

	private static void DilationSphere(byte[,,] _map, Bounds bounds, int bX, int bY, int bZ, int eX, int eY, int eZ, Color color)
	{
		Vector3 center = bounds.center;
		float num = Mathf.Min(Mathf.Min(bounds.size.x, bounds.size.y), bounds.size.z) / 2f;
		for (int i = bX + 1; i < eX - 1; i++)
		{
			for (int j = bY + 1; j < eY - 1; j++)
			{
				for (int k = bZ + 1; k < eZ - 1; k++)
				{
					if (!(Vector3.Distance(new Vector3(i, j, k), center) - num > 0.5f))
					{
						byte b = _map[i, j, k];
						if (b > _map[i - 1, j, k])
						{
							b = _map[i - 1, j, k];
						}
						if (b > _map[i + 1, j, k])
						{
							b = _map[i + 1, j, k];
						}
						if (b > _map[i, j - 1, k])
						{
							b = _map[i, j - 1, k];
						}
						if (b > _map[i, j + 1, k])
						{
							b = _map[i, j + 1, k];
						}
						if (b > _map[i, j, k - 1])
						{
							b = _map[i, j, k - 1];
						}
						if (b > _map[i, j, k + 1])
						{
							b = _map[i, j, k + 1];
						}
						_map[i, j, k] = b;
					}
				}
			}
		}
	}

	private static void DilationRandom(byte[,,] _map, Bounds bounds, int bX, int bY, int bZ, int eX, int eY, int eZ, Color color)
	{
		for (int i = bX + 1; i < eX - 1; i++)
		{
			for (int j = bY + 1; j < eY - 1; j++)
			{
				for (int k = bZ + 1; k < eZ - 1; k++)
				{
					if (!(Random.value > 0.5f))
					{
						byte b = _map[i, j, k];
						if (b > _map[i - 1, j, k])
						{
							b = _map[i - 1, j, k];
						}
						if (b > _map[i + 1, j, k])
						{
							b = _map[i + 1, j, k];
						}
						if (b > _map[i, j - 1, k])
						{
							b = _map[i, j - 1, k];
						}
						if (b > _map[i, j + 1, k])
						{
							b = _map[i, j + 1, k];
						}
						if (b > _map[i, j, k - 1])
						{
							b = _map[i, j, k - 1];
						}
						if (b > _map[i, j, k + 1])
						{
							b = _map[i, j, k + 1];
						}
						_map[i, j, k] = b;
					}
				}
			}
		}
	}

	private static void PaintCube(Color[,,] _colors, Bounds bounds, int bX, int bY, int bZ, int eX, int eY, int eZ, Color color)
	{
	}

	private static void PaintSphere(Color[,,] _colors, Bounds bounds, int bX, int bY, int bZ, int eX, int eY, int eZ, Color color)
	{
	}

	private static void PaintCylinder(Color[,,] _colors, Bounds bounds, int bX, int bY, int bZ, int eX, int eY, int eZ, Color color)
	{
	}

	private static void PaintRandom(Color[,,] _colors, Bounds bounds, int bX, int bY, int bZ, int eX, int eY, int eZ, Color color)
	{
	}

	public static int[][] GetEdgeLoop(GEVoxelShapeC _c, Mesh _mesh, float _z)
	{
		_z /= _c.GOScale;
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		List<int[]> list = new List<int[]>();
		List<int> list2 = new List<int>();
		List<byte> list3 = new List<byte>();
		Vector3[] vertices = _mesh.vertices;
		int[] triangles = _mesh.triangles;
		for (int i = 0; i < triangles.Length / 3; i++)
		{
			Vector3 vector = vertices[triangles[i * 3]];
			Vector3 vector2 = vertices[triangles[i * 3 + 1]];
			Vector3 vector3 = vertices[triangles[i * 3 + 2]];
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			byte b = 0;
			float num = 0f;
			if (vector.z == _z)
			{
				flag = true;
				b++;
			}
			else
			{
				num = vector.z - _z;
			}
			if (vector2.z == _z)
			{
				flag2 = true;
				b++;
			}
			else
			{
				num = vector2.z - _z;
			}
			if (vector3.z == _z)
			{
				flag3 = true;
				b++;
			}
			else
			{
				num = vector3.z - _z;
			}
			if (b > 1 && num > 0f)
			{
				if (flag)
				{
					list2.Add(triangles[i * 3]);
				}
				if (flag2)
				{
					list2.Add(triangles[i * 3 + 1]);
				}
				if (flag3)
				{
					list2.Add(triangles[i * 3 + 2]);
				}
				list3.Add(b);
			}
		}
		List<int> list4 = new List<int>();
		int num2 = 0;
		if (list2.Count > 0)
		{
			int num3 = list2[0];
			list4.Add(num3);
			do
			{
				int num4 = 0;
				int num5 = 0;
				bool flag4 = false;
				int num6 = 0;
				int num7 = 1;
				int num8 = list3.Count - 1;
				bool flag5 = false;
				while (list3.Count != 0 && num6 <= list3.Count - 1)
				{
					int num9 = list3[num6];
					bool flag6 = false;
					if (num9 != 1 && num9 == 2)
					{
						if (list2[num4] == num3)
						{
							num3 = list2[num4 + 1];
							list4.Add(list2[num4 + 1]);
							if (!list4.Contains(list2[num4 + 1]))
							{
								flag5 = true;
							}
							flag6 = true;
						}
						else if (list2[num4 + 1] == num3)
						{
							num3 = list2[num4];
							list4.Add(list2[num4]);
							if (!list4.Contains(list2[num4]))
							{
								flag5 = true;
							}
							flag6 = true;
						}
					}
					if (!flag6)
					{
						num4 = ((num7 != 1) ? (num4 - num9) : (num4 + num9));
					}
					else
					{
						list2.RemoveRange(num4, num9);
						list3.RemoveAt(num6);
						if (num7 == -1)
						{
							num4 -= num9;
						}
						if (num7 == 1)
						{
							num6--;
						}
						num8 = list3.Count - 1;
						num5 = num4;
						flag4 = true;
					}
					num6 += num7;
					if (num7 == 1 && num6 >= num8)
					{
						num7 *= -1;
					}
					if (flag5 || num6 <= 0 || num2 >= 50000)
					{
						break;
					}
				}
				if (list3.Count == 0 || flag5 || !flag4)
				{
					if (list4.Count <= 0)
					{
						break;
					}
					bool flag7 = false;
					for (int j = 0; j < list.Count; j++)
					{
						if (list[j][0] == list4[list4.Count - 1])
						{
							int[] array = new int[list4.Count - 1 + list[j].Length];
							list4.CopyTo(0, array, 0, list4.Count - 1);
							list[j].CopyTo(array, list4.Count - 1);
							list[j] = array;
							flag7 = true;
							break;
						}
					}
					if (!flag7)
					{
						list.Add(list4.ToArray());
					}
					if (list3.Count == 0)
					{
						break;
					}
					list4 = new List<int>();
					num3 = list2[0];
					list4.Add(num3);
				}
				num2++;
			}
			while (list3.Count > 0 && num2 < 50000);
		}
		return list.ToArray();
	}

	public static void Update()
	{
		int aliveCount = m_components.m_aliveCount;
		for (int i = 0; i < aliveCount; i++)
		{
			GEVoxelShapeC gEVoxelShapeC = m_components.m_array[m_components.m_aliveIndices[i]];
			if (!gEVoxelShapeC.active)
			{
				continue;
			}
			while (gEVoxelShapeC.reBuild.Count != 0)
			{
				gEVoxelShapeC.reBuild[0].ReBuild();
				var o_682_4_639233471845008422 = gEVoxelShapeC.reBuild[0].m_mesh;
				gEVoxelShapeC.reBuild.RemoveAt(0);
				if (gEVoxelShapeC.reBuild.Count != 0 || !gEVoxelShapeC.isPhysical || GEState.editorMode)
				{
					continue;
				}
				List<List<List<Vector2>>> list = new List<List<List<Vector2>>>();
				for (int j = 0; j < gEVoxelShapeC.cubes.Count; j++)
				{
					list.Add(new List<List<Vector2>>());
					Vector3[] vertices = gEVoxelShapeC.cubes[j].m_mesh.vertices;
					int[][] edgeLoop = GetEdgeLoop(gEVoxelShapeC, gEVoxelShapeC.cubes[j].m_mesh, 50f);
					for (int k = 0; k < edgeLoop.Length; k++)
					{
						List<Vector2> list2 = new List<Vector2>();
						for (int l = 0; l < edgeLoop[k].Length; l++)
						{
							list2.Add(vertices[edgeLoop[k][l]] * gEVoxelShapeC.GOScale);
						}
						bool flag = false;
						for (int m = 0; m < list.Count; m++)
						{
							for (int n = 0; n < list[m].Count; n++)
							{
							}
						}
						if (!flag)
						{
							list[j].Add(list2);
						}
					}
				}
				bool removeShapesFromBody = true;
				for (int num = 0; num < list.Count; num++)
				{
					for (int num2 = 0; num2 < list[num].Count; num2++)
					{
						ChipmunkS.CreateSegmentShapesFromVectorArray(gEVoxelShapeC.CMC, list[num][num2].ToArray(), GEState.layer_all, 1f, 1f, 3f, -gEVoxelShapeC.GO.transform.localPosition, removeShapesFromBody);
						removeShapesFromBody = false;
					}
				}
			}
			if (Input.GetMouseButton(0))
			{
				continue;
			}
			bool flag2 = false;
			while (gEVoxelShapeC.reBuildColliderCount != 0)
			{
				gEVoxelShapeC.reBuildColliderCount--;
				gEVoxelShapeC.reBuildCollider[0].ReBuildCollider();
				gEVoxelShapeC.reBuildCollider.RemoveAt(0);
				flag2 = true;
			}
			if (!flag2)
			{
				continue;
			}
			GameObject gameObject = new GameObject();
			Transform transform = gameObject.transform;
			for (int num3 = 0; num3 < gEVoxelShapeC.cubes.Count; num3++)
			{
				GEVCube gEVCube = gEVoxelShapeC.cubes[num3];
				Vector3[] vertices2 = gEVCube.m_mesh.vertices;
				Vector3[] normals = gEVCube.m_mesh.normals;
				Color[] colors = gEVCube.m_mesh.colors;
				for (int num4 = 0; num4 < vertices2.Length; num4++)
				{
					RaycastHit hitInfo = default(RaycastHit);
					transform.transform.position = vertices2[num4] * 10f + gEVoxelShapeC.GO.transform.localPosition + gEVoxelShapeC.TC.transform.position + normals[num4] * 5f;
					transform.transform.rotation = Quaternion.LookRotation(normals[num4]);
					float num5 = 1f;
					for (int num6 = 0; num6 < 256; num6++)
					{
						Vector3 vector = Random.Range(-1f, 1f) * Vector3.right + Random.Range(-1f, 1f) * Vector3.up + Random.value * Vector3.forward;
						if (Physics.Raycast(new Ray(transform.transform.position, transform.rotation * vector), out hitInfo, 50f))
						{
							num5 -= hitInfo.distance / 50f / 256f;
						}
					}
					float num7 = Vector3.Dot(Vector3.up, normals[num4]) * 0.25f + 0.875f;
					num5 *= num7;
					colors[num4] = new Color(num5, num5, num5, 1f);
				}
				gEVCube.m_mesh.colors = colors;
			}
			Object.Destroy(gameObject);
		}
	}
}
