using UnityEngine;

namespace VoxelTerrain
{
	public class GEVCube
	{
		public GameObject m_object;

		public Mesh m_mesh;

		private Bounds m_bounds;

		private int m_minX;

		private int m_minY;

		private int m_minZ;

		private int m_maxX;

		private int m_maxY;

		private int m_maxZ;

		private GEVoxelShapeC p_VC;

		public Bounds bounds
		{
			get
			{
				return m_bounds;
			}
		}

		public GEVCube(GEVoxelShapeC _c, Bounds bounds)
		{
			m_minX = (int)bounds.min.x;
			m_minY = (int)bounds.min.y;
			m_minZ = (int)bounds.min.z;
			m_maxX = (int)bounds.max.x;
			m_maxY = (int)bounds.max.y;
			m_maxZ = (int)bounds.max.z;
			m_bounds = bounds;
			p_VC = _c;
			m_object = GEVoxelShapeS.AddObject(p_VC);
			m_mesh = m_object.GetComponent<MeshFilter>().mesh;
			m_mesh.bounds = m_bounds;
		}

		public void ReBuild()
		{
			int width = p_VC.width;
			int height = p_VC.height;
			int depth = p_VC.depth;
			m_mesh.Clear();
			Vector3[] verticesOut;
			int[] trianglesOut;
			GEVRender.MarchingCubesRender(p_VC.map, m_minX, m_minY, m_minZ, m_maxX, m_maxY, m_maxZ, out verticesOut, out trianglesOut, p_VC.ISO);
			Vector2[] array = new Vector2[verticesOut.Length];
			Vector2[] array2 = new Vector2[verticesOut.Length];
			Color[] array3 = new Color[verticesOut.Length];
			for (int i = 1; i < verticesOut.Length; i++)
			{
				Vector3 vector = verticesOut[i];
				array[i] = new Vector2(vector.x / (float)width, vector.z / (float)depth);
				array2[i] = new Vector2(vector.x / (float)width, vector.y / (float)height);
				if (p_VC.colors != null)
				{
					array3[i] = p_VC.colors[(int)vector.x, (int)vector.y, (int)vector.z];
				}
				else
				{
					array3[i] = Color.white;
				}
			}
			m_mesh.vertices = verticesOut;
			m_mesh.triangles = trianglesOut;
			m_mesh.uv = array2;
			m_mesh.colors = array3;
			m_mesh.RecalculateNormals();
		}

		public void ReBuildCollider()
		{
			m_object.GetComponent<MeshCollider>().sharedMesh = null;
			m_object.GetComponent<MeshCollider>().sharedMesh = m_mesh;
		}
	}
}
