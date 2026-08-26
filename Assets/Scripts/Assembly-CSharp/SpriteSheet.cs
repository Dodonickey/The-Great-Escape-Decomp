using UnityEngine;

public class SpriteSheet
{
	public int m_index;

	public GenericArray<SpriteC> m_components;

	public Camera m_camera;

	public GameObject m_gameObject;

	public Mesh m_mesh;

	public MeshFilter m_meshFilter;

	public MeshRenderer m_meshRenderer;

	public Material m_material;

	public int m_textureWidth;

	public int m_textureHeight;

	public float m_globalSpriteScale;

	public Vector3[] m_vertices;

	public Vector3[] m_normals;

	public Vector2[] m_UVs;

	public Color[] m_colors;

	public int[] m_triIndices;

	public bool m_vertsChanged;

	public bool m_uvsChanged;

	public bool m_colorsChanged;

	public bool m_vertCountChanged;

	public bool m_sortMesh;

	public int m_vertDataCount;

	public Color m_defaultColor = new Color(0.5f, 0.5f, 0.5f, 1f);

	public SpriteSheet(int _maxComponentCount, Camera _camera, Texture _texture, Shader _shader, float _globalSpriteScale)
	{
		CreateSpriteSheet(_maxComponentCount, _camera, new Material(_shader)
		{
			mainTexture = _texture
		}, _globalSpriteScale);
	}

	public SpriteSheet(int _maxComponentCount, Camera _camera, Material _material, float _globalSpriteScale)
	{
		CreateSpriteSheet(_maxComponentCount, _camera, _material, _globalSpriteScale);
	}

	public void CreateSpriteSheet(int _maxComponentCount, Camera _camera, Material _material, float _globalSpriteScale)
	{
		m_camera = _camera;
		m_gameObject = new GameObject("SpriteSheet");
		m_gameObject.layer = m_camera.gameObject.layer;
		m_meshFilter = m_gameObject.AddComponent<MeshFilter>() as MeshFilter;
		m_meshRenderer = m_gameObject.AddComponent<MeshRenderer>() as MeshRenderer;
		m_material = _material;
		m_textureWidth = 0;
		m_textureHeight = 0;
		if (_material.mainTexture != null)
		{
			m_textureWidth = _material.mainTexture.width;
			m_textureHeight = _material.mainTexture.height;
		}
		m_mesh = m_meshFilter.mesh;
		m_meshRenderer.bounds.SetMinMax(new Vector3(float.MinValue, float.MinValue, float.MinValue), new Vector3(float.MaxValue, float.MaxValue, float.MaxValue));
		m_meshRenderer.GetComponent<Renderer>().material = m_material;
		m_globalSpriteScale = _globalSpriteScale;
		m_gameObject.transform.position = Vector3.zero;
		m_components = new GenericArray<SpriteC>(_maxComponentCount);
		for (int i = 0; i < _maxComponentCount; i++)
		{
			m_components.m_array[i] = new SpriteC();
			m_components.m_array[i].componentType = ComponentType.Sprite;
			m_components.m_array[i].index = i;
			m_components.m_array[i].vertDataIndex = i;
		}
		m_vertices = new Vector3[_maxComponentCount * 4];
		for (int j = 0; j < m_vertices.Length; j++)
		{
			m_vertices[j] = SpriteS.m_outOfScreen;
		}
		m_normals = new Vector3[_maxComponentCount * 4];
		m_UVs = new Vector2[_maxComponentCount * 4];
		m_colors = new Color[_maxComponentCount * 4];
		m_triIndices = new int[_maxComponentCount * 6];
		for (int k = 0; k < _maxComponentCount; k++)
		{
			m_triIndices[k * 6 + 5] = k * 4;
			m_triIndices[k * 6 + 4] = k * 4 + 1;
			m_triIndices[k * 6 + 3] = k * 4 + 3;
			m_triIndices[k * 6 + 2] = k * 4 + 3;
			m_triIndices[k * 6 + 1] = k * 4 + 1;
			m_triIndices[k * 6] = k * 4 + 2;
		}
		m_vertsChanged = true;
		m_colorsChanged = true;
		m_uvsChanged = true;
		m_vertCountChanged = false;
		m_mesh.vertices = m_vertices;
		m_mesh.uv = m_UVs;
		m_mesh.colors = m_colors;
		m_mesh.triangles = m_triIndices;
		m_mesh.normals = m_normals;
		m_mesh.bounds = new Bounds(Vector3.zero, new Vector3(99999f, 99999f, 99999f));
	}
}
