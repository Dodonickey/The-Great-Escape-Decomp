using UnityEngine;

namespace VoxelTerrain
{
	public class VCube
	{
		private GameObject _object;

		private Mesh _mesh;

		private Bounds _bounds;

		private int _minX;

		private int _minY;

		private int _minZ;

		private int _maxX;

		private int _maxY;

		private int _maxZ;

		private VTerrain _terrain;

		public Bounds bounds
		{
			get
			{
				return _bounds;
			}
		}

		public VCube(Bounds bounds, VTerrain terrain)
		{
			_minX = (int)bounds.min.x;
			_minY = (int)bounds.min.y;
			_minZ = (int)bounds.min.z;
			_maxX = (int)bounds.max.x;
			_maxY = (int)bounds.max.y;
			_maxZ = (int)bounds.max.z;
			_bounds = bounds;
			_terrain = terrain;
			_object = terrain.AddObject();
			_mesh = _object.GetComponent<MeshFilter>().mesh;
			_mesh.bounds = _bounds;
		}

		public void ReBuild()
		{
			int width = _terrain.width;
			int height = _terrain.height;
			int depth = _terrain.depth;
			_mesh.Clear();
			Vector3[] verticesOut;
			int[] trianglesOut;
			VRender.MarchingCubesRender(_terrain._map, _minX, _minY, _minZ, _maxX, _maxY, _maxZ, out verticesOut, out trianglesOut);
			Vector2[] array = new Vector2[verticesOut.Length];
			Vector2[] array2 = new Vector2[verticesOut.Length];
			Color[] array3 = new Color[verticesOut.Length];
			for (int i = 1; i < verticesOut.Length; i++)
			{
				Vector3 vector = verticesOut[i];
				array[i] = new Vector2(vector.x / (float)width, vector.z / (float)depth);
				array2[i] = new Vector2(vector.x / (float)width, vector.y / (float)height);
				array3[i] = _terrain._colors[(int)vector.x, (int)vector.y, (int)vector.z];
			}
			_mesh.vertices = verticesOut;
			_mesh.triangles = trianglesOut;
			_mesh.uv = array;
			_mesh.uv2 = array2;
			_mesh.colors = array3;
			_mesh.RecalculateNormals();
		}

		public void ReBuildCollider()
		{
			_object.GetComponent<MeshCollider>().sharedMesh = null;
			_object.GetComponent<MeshCollider>().sharedMesh = _mesh;
		}
	}
}
