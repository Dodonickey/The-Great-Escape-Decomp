using UnityEngine;

public class VBrush : MonoBehaviour
{
	private float _distance = 100f;

	private Vector3 _position = Vector3.zero;

	private float _size = 2f;

	private bool _paintEnable;

	private VTerrain.OBJ _obj;

	private VTerrain.SFX _sfx;

	private Color _color = new Color(1f, 1f, 1f, 1f);

	private Ray _ray;

	private RaycastHit _hit;

	private float _lastClick;

	private bool _rotationEnable;

	private void DrawGUI()
	{
		GUILayout.BeginArea(new Rect(2f, 2f, 100f, Screen.height - 4));
		GUILayout.BeginVertical();
		GUILayout.Label("Effect");
		if (GUILayout.RepeatButton("Addition"))
		{
			_sfx = VTerrain.SFX.ADD;
		}
		if (GUILayout.RepeatButton("Subtraction"))
		{
			_sfx = VTerrain.SFX.SUB;
		}
		if (GUILayout.RepeatButton("Dilation"))
		{
			_sfx = VTerrain.SFX.DILATION;
		}
		if (GUILayout.RepeatButton("Erosion"))
		{
			Debug.Log("erosion");
		}
		GUILayout.Label("Object");
		if (GUILayout.RepeatButton("Random"))
		{
			_obj = VTerrain.OBJ.RANDOM;
		}
		if (GUILayout.RepeatButton("Sphere"))
		{
			_obj = VTerrain.OBJ.SPHERE;
		}
		if (GUILayout.RepeatButton("Cylinder"))
		{
			_obj = VTerrain.OBJ.CYLINDER;
		}
		if (GUILayout.RepeatButton("Cube"))
		{
			_obj = VTerrain.OBJ.CUBE;
		}
		GUILayout.Label("Paint");
		if (GUILayout.RepeatButton("Paint"))
		{
			_sfx = VTerrain.SFX.PAINT;
		}
		GUILayout.Label("Texture");
		_color.r = GUILayout.HorizontalSlider(_color.r, 0f, 10f);
		GUILayout.Label("Brush size");
		_size = GUILayout.HorizontalSlider(_size, 0f, 10f);
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}

	private void OnGUI()
	{
		DrawGUI();
		if (Input.GetMouseButtonDown(0) && _lastClick < Time.time - 1f)
		{
			_paintEnable = true;
		}
		if (Input.GetMouseButtonUp(0))
		{
			_paintEnable = false;
		}
		if (Input.GetMouseButtonDown(1))
		{
			_rotationEnable = true;
		}
		if (Input.GetMouseButtonUp(1))
		{
			_rotationEnable = false;
		}
		if (_rotationEnable)
		{
			float num = 100f * Input.GetAxis("Mouse X");
			float num2 = -100f * Input.GetAxis("Mouse Y");
			Camera.main.transform.RotateAround(new Vector3(100f, 100f, 100f), Vector3.up, num * Time.deltaTime);
			Camera.main.transform.RotateAround(new Vector3(100f, 100f, 100f), Vector3.right, num2 * Time.deltaTime);
			return;
		}
		if (_paintEnable)
		{
			_ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(_ray, out _hit))
			{
				_distance = _hit.distance;
			}
			_position = _ray.GetPoint(_distance);
			_position.z = 20f;
			base.transform.localScale = new Vector3(_size, _size, _size);
			base.transform.position = _position;
		}
		else
		{
			VTerrain.Instance.ReBuildCollider();
		}
		VTerrain.Instance.Alteration(_position, new Vector3(_size, _size, _size), _obj, _sfx, _color);
	}
}
