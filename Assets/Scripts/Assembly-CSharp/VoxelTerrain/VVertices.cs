using System.Collections.Generic;
using UnityEngine;

namespace VoxelTerrain
{
	public class VVertices
	{
		private List<Vector3> _vertices;

		private Dictionary<Vector3, int> _find;

		private int _index;

		public VVertices()
		{
			_vertices = new List<Vector3>();
			_find = new Dictionary<Vector3, int>();
			_index = 0;
		}

		public int GetIndex(Vector3 vertex)
		{
			int value;
			if (_find.TryGetValue(vertex, out value))
			{
				return value;
			}
			_vertices.Add(vertex);
			_find.Add(vertex, _index);
			return _index++;
		}

		public Vector3[] ToArray()
		{
			return _vertices.ToArray();
		}
	}
}
