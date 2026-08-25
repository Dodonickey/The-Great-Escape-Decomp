using System.Collections.Generic;
using UnityEngine;

public class GEEditorCanvasA
{
	public static int MOVE = 0;

	public static int PINCH = 1;

	public static int SELECTION_ADD = 2;

	public static int SELECTION_SUB = 3;

	public static int DRAW_ADD = 4;

	public static int DRAW_SUB = 5;

	public static int SCULPT_ADD = 6;

	public static int SCULPT_SUB = 7;

	public static int m_sculptSize = 20;

	public static SculptDepth m_sculptDepth = SculptDepth.All;

	private static int m_tool;

	private static bool m_toolActive;

	private static List<Vector2> m_touchPoints;

	private static TransformC m_toolTC;

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
			LevelManager.m_currentLevel.name + ":EditorCanvas",
			"EditorCanvas"
		};
		Entity entity = EntityManager.AddEntity(tags);
		TransformC transformComponent = TransformS.AddComponent(entity);
		TouchAreaC touchAreaC = TouchAreaS.AddComponent(transformComponent, "EditorCanvas", Screen.width, Screen.height, false, Main.uiCamera, null);
		TouchAreaS.AddTouchEventListener(touchAreaC, HandleEditorCanvasTouches);
		TouchAreaS.SetOrder(touchAreaC, -100);
		return entity;
	}

	public static void HandleEditorCanvasTouches(TouchAreaC _c, int _i, bool _consumed)
	{
		if (m_toolActive && m_tool == MOVE && _c.touchStartedInside.Count == 2)
		{
			Vector2 vector = _c.touchPos[0];
			Vector2 vector2 = _c.touchPos[1];
			float magnitude = (vector - vector2).magnitude;
			float num = magnitude - m_lastPinchDistance;
			m_lastPinchDistance = magnitude;
			m_tool = PINCH;
		}
		else if (m_toolActive && m_tool == PINCH && _c.touchStartedInside.Count == 2)
		{
			Vector2 vector3 = _c.touchPos[0];
			Vector2 vector4 = _c.touchPos[1];
			float magnitude2 = (vector3 - vector4).magnitude;
			float num2 = magnitude2 - m_lastPinchDistance;
			m_lastPinchDistance = magnitude2;
			CameraS.m_currentCameraPosition += Vector3.forward * num2;
			Vector3 currentCameraPosition = CameraS.m_currentCameraPosition;
			currentCameraPosition.z = Mathf.Max(-2500f, Mathf.Min(-100f, CameraS.m_currentCameraPosition.z));
			CameraS.m_currentCameraPosition = currentCameraPosition;
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
			if (m_toolActive)
			{
				return;
			}
			if (EditorState.m_drawMode)
			{
				if (GEState.m_addDown)
				{
					if (!EditorState.m_voxelDrawMode)
					{
						if (m_strokeTC == null)
						{
							float z = EditorState.m_selection[0].data.position.z;
							Vector3 vector8 = Main.camera.ScreenToWorldPoint((Vector3)_c.touchPos[_i] + new Vector3(0f, 0f, Main.camera.transform.position.z - z)) - Main.camera.transform.position * 2f;
							m_strokeArray.Clear();
							m_strokeArray.Add(-vector8);
							m_strokeTC = EntityManager.AddEntityWithTC();
						}
						m_tool = DRAW_ADD;
						m_toolActive = true;
					}
					else
					{
						m_tool = SCULPT_ADD;
						m_toolActive = true;
					}
				}
				else if (GEState.m_subDown)
				{
					if (!EditorState.m_voxelDrawMode)
					{
						if (m_strokeTC == null)
						{
							float z2 = EditorState.m_selection[0].data.position.z;
							Vector3 vector9 = Main.camera.ScreenToWorldPoint((Vector3)_c.touchPos[_i] + new Vector3(0f, 0f, Main.camera.transform.position.z - z2)) - Main.camera.transform.position * 2f;
							m_strokeArray.Clear();
							m_strokeArray.Add(-vector9);
							m_strokeTC = EntityManager.AddEntityWithTC();
						}
						m_tool = DRAW_SUB;
						m_toolActive = true;
					}
					else
					{
						m_tool = SCULPT_SUB;
						m_toolActive = true;
					}
				}
				else
				{
					m_tool = MOVE;
					m_toolActive = true;
				}
			}
			else if (!GEState.m_addDown && !GEState.m_subDown)
			{
				if (!EditorState.m_isSelectionLocked)
				{
					EditorState.m_selection.Clear();
					EditorState.UpdateSelection();
				}
			}
			else if (GEState.m_addDown)
			{
				m_tool = SELECTION_ADD;
				m_toolActive = true;
			}
			else if (GEState.m_subDown)
			{
				m_tool = SELECTION_SUB;
				m_toolActive = true;
			}
			if (m_toolActive)
			{
				EntityManager.SetActivityOfEntitiesWithTag(LevelManager.m_currentLevel.name + ":EditorItem", false, false);
				EntityManager.SetActivityOfEntitiesWithTag(LevelManager.m_currentLevel.name + ":EditorAnchor", false, false);
				EntityManager.SetActivityOfEntitiesWithTag(LevelManager.m_currentLevel.name + ":EditorHandle", false, false);
			}
		}
		else if (_c.touchEvent[_i] == TouchEvent.DragStart)
		{
			if (!GEState.m_addDown && !GEState.m_subDown)
			{
				m_tool = MOVE;
				m_toolActive = true;
			}
		}
		else if (_c.touchEvent[_i] == TouchEvent.Down || _c.touchEvent[_i] == TouchEvent.Drag)
		{
			if (!m_toolActive)
			{
				return;
			}
			if (m_tool == SCULPT_ADD || m_tool == SCULPT_SUB)
			{
				Vector3 zero = Vector3.zero;
				Vector3 scale = new Vector3(m_sculptSize, m_sculptSize, 100f);
				if (m_sculptDepth == SculptDepth.All)
				{
					zero.z = 50f;
					scale.z = 100f;
				}
				else if (m_sculptDepth == SculptDepth.Back)
				{
					zero.z = 80f;
					scale.z = 20f;
				}
				else if (m_sculptDepth == SculptDepth.Middle)
				{
					zero.z = 50f;
					scale.z = 20f;
				}
				else if (m_sculptDepth == SculptDepth.Front)
				{
					zero.z = 20f;
					scale.z = 20f;
				}
				Vector3 touchWorldPos2 = TouchAreaS.GetTouchWorldPos(Main.camera, _c.touchPos[_i], 0f - zero.z);
				zero.x = touchWorldPos2.x;
				zero.y = touchWorldPos2.y;
				if (m_tool == SCULPT_ADD)
				{
					GEVoxelShapeS.Alteration(EditorState.m_selection[0].gameComponents[0] as GEVoxelShapeC, zero, scale, VoxelPaintShape.CYLINDER, VoxelPaintEffect.ADD, Color.white);
				}
				else if (m_tool == SCULPT_SUB)
				{
					GEVoxelShapeS.Alteration(EditorState.m_selection[0].gameComponents[0] as GEVoxelShapeC, zero, scale, VoxelPaintShape.CYLINDER, VoxelPaintEffect.SUB, Color.white);
				}
			}
			else if (m_tool == DRAW_ADD || m_tool == DRAW_SUB)
			{
				if (_c.touchEvent[_i] == TouchEvent.Drag && (m_strokeArray[m_strokeArray.Count - 1] - (Vector2)touchWorldPos).sqrMagnitude > m_strokeResolution)
				{
					float z3 = EditorState.m_selection[0].data.position.z;
					Vector3 vector10 = Main.camera.ScreenToWorldPoint((Vector3)_c.touchPos[_i] + new Vector3(0f, 0f, Main.camera.transform.position.z - z3)) - Main.camera.transform.position * 2f;
					m_strokeArray.Add(-vector10);
					if (m_strokePC != null)
					{
						PrefabS.RemoveComponent(m_strokePC);
					}
					m_strokePC = PrefabS.CreatePathPrefabComponentFromVectorArray(m_strokeTC, Vector3.forward * z3, m_strokeArray.ToArray(), 6f, Color.white, ResourceManager.GetMaterial("Line6"), Main.camera, Position.Center, true);
				}
			}
			else if (m_tool == SELECTION_ADD || m_tool == SELECTION_SUB)
			{
				if (m_toolTC != null)
				{
					EntityManager.RemoveEntityByTransformComponent(m_toolTC, false);
				}
				m_toolTC = EntityManager.AddEntityWithTC("EditorTool");
				Vector2[] rect = DebugDraw.GetRect(Mathf.Abs(vector7.x), Mathf.Abs(vector7.y), vector7 * -0.5f + vector5 - new Vector2((float)Screen.width * 0.5f, (float)Screen.height * 0.5f), false);
				PrefabS.CreatePathPrefabComponentFromVectorArray(m_toolTC, Vector3.zero, rect, 8f, Color.white, ResourceManager.GetMaterial("Line8"), Main.uiCamera, Position.Center, true);
			}
			else if (m_tool == MOVE)
			{
				Vector2 vector11 = -InputManager.m_touches[_c.touchIndex[_i]].deltaPosition * (1f / Main.m_gameCameraDistanceMultipler);
				CameraS.m_currentCameraPosition += new Vector3(vector11.x, vector11.y, 0f);
			}
		}
		else if (_c.touchEvent[_i] == TouchEvent.Release && _c.touchWasDragged[_i])
		{
			if (m_toolActive)
			{
				if (m_tool == DRAW_ADD || m_tool == DRAW_SUB)
				{
					Polygon polygon = new Polygon();
					polygon.AddContour(new VertexList(m_strokeArray.ToArray()), false);
					EditorState.HandleDrawableEditorItem(EditorState.m_selection[0], polygon, m_tool);
				}
				else if (m_tool == SELECTION_ADD || m_tool == SELECTION_SUB)
				{
					if (m_toolTC != null)
					{
						EntityManager.RemoveEntityByTransformComponent(m_toolTC, false);
						m_toolTC = null;
					}
					Vector2[] rect2 = DebugDraw.GetRect(Mathf.Abs(vector7.x), Mathf.Abs(vector7.y), vector7 * -0.5f + vector5 - new Vector2((float)Screen.width * 0.5f, (float)Screen.height * 0.5f), false);
					int aliveCount = GES.m_editorItemComponents.m_aliveCount;
					for (int i = 0; i < aliveCount; i++)
					{
						EIC eIC = GES.m_editorItemComponents.m_array[GES.m_editorItemComponents.m_aliveIndices[i]];
						float num3 = 999999f;
						float num4 = -999999f;
						float num5 = 999999f;
						float num6 = -999999f;
						for (int j = 0; j < rect2.Length; j++)
						{
							num3 = Mathf.Min(rect2[j].x, num3);
							num4 = Mathf.Max(rect2[j].x, num4);
							num5 = Mathf.Min(rect2[j].y, num5);
							num6 = Mathf.Max(rect2[j].y, num6);
						}
						if (eIC.itemType != 1 && eIC.itemType != 2)
						{
							continue;
						}
						Vector3 position = eIC.uiTC.transform.position;
						if (!(position.x > num3) || !(position.x < num4) || !(position.y > num5) || !(position.y < num6))
						{
							continue;
						}
						int num7 = -1;
						for (int k = 0; k < EditorState.m_selection.Count; k++)
						{
							if (EditorState.m_selection[k] == eIC)
							{
								num7 = k;
								break;
							}
						}
						if (num7 > -1 && m_tool == SELECTION_SUB)
						{
							EditorState.m_selection.RemoveAt(num7);
						}
						else
						{
							if (num7 != -1 || m_tool != SELECTION_ADD)
							{
								continue;
							}
							int count = EditorState.m_selection.Count;
							for (int num8 = count - 1; num8 > -1; num8--)
							{
								for (EIC container = EditorState.m_selection[num8].container; container != null; container = container.container)
								{
									if (container == eIC)
									{
										EditorState.m_selection.RemoveAt(num8);
										break;
									}
								}
							}
							bool flag = false;
							for (EIC container2 = eIC.container; container2 != null; container2 = container2.container)
							{
								if (EditorState.m_selection.Contains(container2))
								{
									flag = true;
									break;
								}
							}
							if (!flag)
							{
								EditorState.m_selection.Add(eIC);
							}
						}
					}
					EditorState.UpdateSelection();
				}
			}
			if (m_toolActive && m_tool != DRAW_ADD && m_tool != DRAW_SUB)
			{
				EntityManager.SetActivityOfEntitiesWithTag(LevelManager.m_currentLevel.name + ":EditorItem", true, false);
				EntityManager.SetActivityOfEntitiesWithTag(LevelManager.m_currentLevel.name + ":EditorAnchor", true, false);
				EntityManager.SetActivityOfEntitiesWithTag(LevelManager.m_currentLevel.name + ":EditorHandle", true, false);
			}
			if (m_strokeTC != null)
			{
				EntityManager.RemoveEntity(m_strokeTC.entityIndex);
				m_strokeTC = null;
				m_strokePC = null;
			}
			m_tool = MOVE;
			m_toolActive = false;
		}
		else if (_c.touchEvent[_i] == TouchEvent.Release)
		{
			if (m_toolActive)
			{
				EntityManager.SetActivityOfEntitiesWithTag(LevelManager.m_currentLevel.name + ":EditorItem", true, false);
				EntityManager.SetActivityOfEntitiesWithTag(LevelManager.m_currentLevel.name + ":EditorAnchor", true, false);
				EntityManager.SetActivityOfEntitiesWithTag(LevelManager.m_currentLevel.name + ":EditorHandle", true, false);
			}
			if (m_strokeTC != null)
			{
				EntityManager.RemoveEntity(m_strokeTC.entityIndex);
				m_strokeTC = null;
				m_strokePC = null;
			}
			m_tool = MOVE;
			m_toolActive = false;
		}
	}
}
