using System.Collections.Generic;
using UnityEngine;
using VoxelTerrain;

public class VTerrain : MonoBehaviour
{
	public enum OBJ
	{
		SPHERE = 0,
		CYLINDER = 1,
		CUBE = 2,
		RANDOM = 3
	}

	public enum SFX
	{
		ADD = 0,
		SUB = 1,
		DILATION = 2,
		EROSION = 3,
		PAINT = 4
	}

	private static VTerrain _instance;

	public int width = 100;

	public int height = 100;

	public int depth = 100;

	private List<VCube> _cubes;

	private List<VCube> _reBuild;

	private List<VCube> _reBuildCollider;

	private int _reBuildColliderCount;

	private static int RES = 25;

	public byte[,,] _map;

	public Color[,,] _colors;

	public static VTerrain Instance
	{
		get
		{
			return _instance;
		}
	}

	public void Start()
	{
		if (_instance == null)
		{
			_instance = this;
		}
		_cubes = new List<VCube>();
		_reBuild = new List<VCube>();
		_reBuildCollider = new List<VCube>();
		_reBuildColliderCount = 0;
		_map = new byte[width + 1, height + 1, depth + 1];
		_colors = new Color[width + 1, height + 1, depth + 1];
		Debug.Log(width / RES);
		for (int i = 0; i < width / RES; i++)
		{
			for (int j = 0; j < height / RES; j++)
			{
				for (int k = 0; k < depth / RES; k++)
				{
					Bounds bounds = default(Bounds);
					bounds.min = new Vector3(i, j, k) * RES;
					bounds.max = bounds.min + new Vector3(RES, RES, RES);
					VCube item = new VCube(bounds, this);
					_cubes.Add(item);
				}
			}
		}
		ResetMap();
	}

	public void Update()
	{
		if (_reBuild.Count != 0)
		{
			_reBuild[0].ReBuild();
			_reBuild.RemoveAt(0);
		}
		if (_reBuildColliderCount != 0)
		{
			_reBuildColliderCount--;
			_reBuildCollider[0].ReBuildCollider();
			_reBuildCollider.RemoveAt(0);
			Debug.Log("lol");
		}
	}

	public void Alteration(Vector3 position, Vector3 scale, OBJ obj, SFX sfx, Color color)
	{
		Matrix4x4 worldToLocalMatrix = base.transform.worldToLocalMatrix;
		position = worldToLocalMatrix.MultiplyPoint(position);
		scale = worldToLocalMatrix.MultiplyVector(scale);
		Bounds bounds = new Bounds(position, scale);
		int num = (int)scale.x / 2 + 1;
		int num2 = (int)scale.y / 2 + 1;
		int num3 = (int)scale.z / 2 + 1;
		int num4 = Mathf.Max((int)position.x - num, 1);
		int num5 = Mathf.Max((int)position.y - num2, 1);
		int num6 = Mathf.Max((int)position.z - num3, 1);
		int num7 = Mathf.Min((int)position.x + num + 2, width - 2);
		int num8 = Mathf.Min((int)position.y + num2 + 2, height - 2);
		int num9 = Mathf.Min((int)position.z + num3 + 2, depth - 2);
		if (sfx == SFX.SUB || sfx == SFX.EROSION)
		{
			for (int i = num4; i < num7; i++)
			{
				for (int j = num5; j < num8; j++)
				{
					for (int k = num6; k < num9; k++)
					{
						_map[i, j, k] = (byte)(255 - _map[i, j, k]);
					}
				}
			}
		}
		switch (sfx)
		{
		case SFX.ADD:
		case SFX.SUB:
			switch (obj)
			{
			case OBJ.CUBE:
				AddCube(bounds, num4, num5, num6, num7, num8, num9, color);
				break;
			case OBJ.SPHERE:
				AddSphere(bounds, num4, num5, num6, num7, num8, num9, color);
				break;
			case OBJ.CYLINDER:
				AddCylinder(bounds, num4, num5, num6, num7, num8, num9, color);
				break;
			case OBJ.RANDOM:
				AddRandom(bounds, num4, num5, num6, num7, num8, num9, color);
				break;
			}
			break;
		case SFX.DILATION:
		case SFX.EROSION:
			switch (obj)
			{
			case OBJ.CUBE:
				DilationCube(bounds, num4, num5, num6, num7, num8, num9, color);
				break;
			case OBJ.SPHERE:
				DilationSphere(bounds, num4, num5, num6, num7, num8, num9, color);
				break;
			case OBJ.RANDOM:
				DilationRandom(bounds, num4, num5, num6, num7, num8, num9, color);
				break;
			}
			break;
		case SFX.PAINT:
			switch (obj)
			{
			case OBJ.CUBE:
				PaintCube(bounds, num4, num5, num6, num7, num8, num9, color);
				break;
			case OBJ.SPHERE:
				PaintSphere(bounds, num4, num5, num6, num7, num8, num9, color);
				break;
			case OBJ.CYLINDER:
				PaintCylinder(bounds, num4, num5, num6, num7, num8, num9, color);
				break;
			case OBJ.RANDOM:
				PaintRandom(bounds, num4, num5, num6, num7, num8, num9, color);
				break;
			}
			break;
		}
		if (sfx == SFX.SUB || sfx == SFX.EROSION)
		{
			for (int l = num4; l < num7; l++)
			{
				for (int m = num5; m < num8; m++)
				{
					for (int n = num6; n < num9; n++)
					{
						_map[l, m, n] = (byte)(255 - _map[l, m, n]);
					}
				}
			}
		}
		bounds.SetMinMax(new Vector3(num4, num5, num6), new Vector3(num7, num8, num9));
		ReBuild(bounds);
	}

	private void AddCube(Bounds bounds, int bX, int bY, int bZ, int eX, int eY, int eZ, Color color)
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

	private void AddSphere(Bounds bounds, int bX, int bY, int bZ, int eX, int eY, int eZ, Color color)
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

	private void AddCylinder(Bounds bounds, int bX, int bY, int bZ, int eX, int eY, int eZ, Color color)
	{
		Vector3 center = bounds.center;
		float num = Mathf.Min(Mathf.Min(bounds.size.x, bounds.size.y), bounds.size.z) / 2f;
		for (int i = bX; i < eX; i++)
		{
			for (int j = bY; j < eY; j++)
			{
				for (int k = bZ; k < eZ; k++)
				{
					float num2 = (Vector3.Distance(new Vector3(i, j, 20f), center) - num) * 255f;
					byte b = (byte)((num2 > 255f) ? 255f : ((!(num2 < 0f)) ? num2 : 0f));
					if (b < _map[i, j, k])
					{
						_map[i, j, k] = b;
					}
				}
			}
		}
	}

	private void AddRandom(Bounds bounds, int bX, int bY, int bZ, int eX, int eY, int eZ, Color color)
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

	private void DilationCube(Bounds bounds, int bX, int bY, int bZ, int eX, int eY, int eZ, Color color)
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

	private void DilationSphere(Bounds bounds, int bX, int bY, int bZ, int eX, int eY, int eZ, Color color)
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

	private void DilationRandom(Bounds bounds, int bX, int bY, int bZ, int eX, int eY, int eZ, Color color)
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

	private void PaintCube(Bounds bounds, int bX, int bY, int bZ, int eX, int eY, int eZ, Color color)
	{
	}

	private void PaintSphere(Bounds bounds, int bX, int bY, int bZ, int eX, int eY, int eZ, Color color)
	{
	}

	private void PaintCylinder(Bounds bounds, int bX, int bY, int bZ, int eX, int eY, int eZ, Color color)
	{
	}

	private void PaintRandom(Bounds bounds, int bX, int bY, int bZ, int eX, int eY, int eZ, Color color)
	{
	}

	private void OnDrawGizmosSelected()
	{
		Matrix4x4 localToWorldMatrix = base.transform.localToWorldMatrix;
		Gizmos.color = new Color(0.8f, 0.8f, 0.2f, 0.3f);
		for (int i = RES; i < width; i += RES)
		{
			Vector3 vector = localToWorldMatrix.MultiplyPoint(new Vector3(i, 0f, 0f));
			Vector3 to = localToWorldMatrix.MultiplyPoint(new Vector3(i, 0f, depth));
			Gizmos.DrawLine(vector, to);
		}
		for (float num = RES; num < (float)depth; num += (float)RES)
		{
			Vector3 vector2 = localToWorldMatrix.MultiplyPoint(new Vector3(0f, 0f, num));
			Vector3 to2 = localToWorldMatrix.MultiplyPoint(new Vector3(width, 0f, num));
			Gizmos.DrawLine(vector2, to2);
		}
		Vector3 vector3 = new Vector3(width, height, depth);
		Vector3 center = vector3 / 2f;
		Gizmos.color = new Color(0.8f, 0.4f, 0.3f, 0.6f);
		Display.GizmosDrawCube(center, vector3, localToWorldMatrix);
	}

	private void ResetMap()
	{
		for (int i = 0; i < width + 1; i++)
		{
			for (int j = 0; j < height + 1; j++)
			{
				for (int k = 0; k < depth + 1; k++)
				{
					_map[i, j, k] = byte.MaxValue;
				}
			}
		}
		for (int l = 10; l < width + 1; l += 10)
		{
			for (int m = 10; m < height + 1; m += 10)
			{
				for (int n = 10; n < depth + 1; n += 10)
				{
					float value = Random.value;
					for (int num = l - 10; num < l; num++)
					{
						for (int num2 = m - 10; num2 < m; num2++)
						{
							for (int num3 = n - 10; num3 < n; num3++)
							{
								_colors[num, num2, num3] = new Color(value, 0f, 0f, 1f);
							}
						}
					}
				}
			}
		}
		ReBuild();
	}

	public void ReBuild(VCube cube)
	{
		if (!_reBuild.Contains(cube))
		{
			_reBuild.Add(cube);
		}
		if (!_reBuildCollider.Contains(cube))
		{
			_reBuildCollider.Add(cube);
		}
	}

	public void ReBuild(Vector3 point)
	{
		foreach (VCube cube in _cubes)
		{
			if (cube.bounds.Contains(point))
			{
				ReBuild(cube);
				break;
			}
		}
	}

	public void ReBuild(Bounds bounds)
	{
		foreach (VCube cube in _cubes)
		{
			if (bounds.Intersects(cube.bounds))
			{
				ReBuild(cube);
			}
		}
	}

	public void ReBuild()
	{
		foreach (VCube cube in _cubes)
		{
			ReBuild(cube);
		}
	}

	public void ReBuildCollider()
	{
		_reBuildColliderCount = _reBuildCollider.Count;
	}

	public GameObject AddObject()
	{
		GameObject gameObject = new GameObject("VCube");
		gameObject.transform.parent = base.transform;
		gameObject.AddComponent<MeshRenderer>().materials = base.renderer.materials;
		gameObject.AddComponent<MeshFilter>();
		gameObject.AddComponent<MeshCollider>();
		return gameObject;
	}
}
