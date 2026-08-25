using System.Collections.Generic;
using UnityEngine;

public class GEGameCanvasA
{
	public static int MOVE = 0;

	public static int SLICE = 1;

	private static int m_tool;

	private static bool m_toolActive;

	private static float m_lastPinchDistance;

	private static List<Vector2> m_strokeArray = new List<Vector2>();

	private static float m_strokeResolution = 25f;

	private static TransformC m_strokeTC;

	private static PrefabC m_strokePC;

	private static LineRenderer m_strokeLR;

	public static Entity Assemble()
	{
		string[] tags = new string[3]
		{
			LevelManager.m_currentLevel.name,
			LevelManager.m_currentLevel.name + ":GameCanvas",
			"GameCanvas"
		};
		Entity entity = EntityManager.AddEntity(tags);
		TransformC transformComponent = TransformS.AddComponent(entity);
		TouchAreaC touchAreaC = TouchAreaS.AddComponent(transformComponent, "EditorCanvas", Screen.width, Screen.height, true, Main.uiCamera, null);
		TouchAreaS.AddTouchEventListener(touchAreaC, HandleEditorCanvasTouches);
		TouchAreaS.SetOrder(touchAreaC, -100);
		Debug.Log("lllll");
		return entity;
	}

	public static void HandleEditorCanvasTouches(TouchAreaC _c, int _i, bool _consumed)
	{
		if (m_toolActive && m_tool == SLICE && _c.touchStartedInside.Count == 2)
		{
			Vector2 vector = _c.touchPos[0];
			Vector2 vector2 = _c.touchPos[1];
			float magnitude = (vector - vector2).magnitude;
			float num = magnitude - m_lastPinchDistance;
			m_lastPinchDistance = magnitude;
			m_tool = MOVE;
		}
		else if (m_toolActive && m_tool == MOVE && _c.touchStartedInside.Count == 2)
		{
			Vector2 vector3 = _c.touchPos[0];
			Vector2 vector4 = _c.touchPos[1];
			float magnitude2 = (vector3 - vector4).magnitude;
			float num2 = magnitude2 - m_lastPinchDistance;
			m_lastPinchDistance = magnitude2;
			Main.camera.transform.Translate(Vector3.forward * num2);
			Vector3 position = Main.camera.transform.position;
			position.z = Mathf.Max(-2500f, Mathf.Min(-100f, Main.camera.transform.position.z));
			Main.camera.transform.position = position;
			Debug.Log("lol");
		}
		if (_consumed)
		{
			return;
		}
		Vector2 vector5 = _c.touchPos[_i];
		Vector2 vector6 = _c.touchStartPos[_i];
		Vector2 vector7 = vector5 - vector6;
		Vector3 touchWorldPos = TouchAreaS.GetTouchWorldPos(Main.camera, _c.touchPos[_i]);
		if (_c.touchEvent[_i] == TouchEvent.Began)
		{
			return;
		}
		if (_c.touchEvent[_i] == TouchEvent.DragStart)
		{
			m_tool = SLICE;
			m_toolActive = true;
		}
		else if (_c.touchEvent[_i] == TouchEvent.Down || _c.touchEvent[_i] == TouchEvent.Drag)
		{
			if (m_toolActive && m_tool == MOVE)
			{
				Vector2 vector8 = -InputManager.m_touches[_c.touchIndex[_i]].deltaPosition * (1f / Main.m_gameCameraDistanceMultipler);
				Main.camera.transform.Translate(new Vector3(vector8.x, vector8.y, 0f));
			}
		}
		else if (_c.touchEvent[_i] == TouchEvent.Release)
		{
			m_tool = SLICE;
			m_toolActive = false;
		}
	}
}
