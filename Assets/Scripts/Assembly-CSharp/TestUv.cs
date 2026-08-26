using UnityEngine;

public class TestUv : MonoBehaviour
{
	public bool init = true;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnDrawGizmos()
	{
		if (init)
		{
			init = false;
			Mesh mesh = GetComponent<MeshFilter>().mesh;
			Vector3[] vertices = mesh.vertices;
			Vector2[] array = new Vector2[vertices.Length];
			Vector2[] array2 = new Vector2[vertices.Length];
			Vector2[] array3 = new Vector2[vertices.Length];
			Color[] array4 = new Color[vertices.Length];
			float num = mesh.bounds.max.x - mesh.bounds.min.x;
			float num2 = mesh.bounds.max.y - mesh.bounds.min.y;
			float num3 = mesh.bounds.max.z - mesh.bounds.min.z;
			for (int i = 0; i < vertices.Length; i++)
			{
				Vector3 vector = vertices[i];
				array[i] = new Vector2(vector.y, vector.z);
				array2[i] = new Vector2(vector.x, vector.z);
				array3[i] = new Vector2(vector.x, vector.y);
				array4[i] = new Color(vector.x / num, vector.y / num2, vector.z / num3);
			}
			mesh.uv = array2;
			mesh.uv2 = array;
			mesh.uv2 = array3;
			mesh.colors = array4;
		}
	}
}
