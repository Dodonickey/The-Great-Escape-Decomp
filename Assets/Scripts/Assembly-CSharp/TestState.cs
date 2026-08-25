using UnityEngine;

public class TestState : BasicState
{
	public static EditorScene p_parent;

	private UIC m_testArea;

	private UIC m_mainCanvas;

	private int m_load;

	public static int MOVE;

	public static int SLICE = 1;

	private static int m_tool;

	private static bool m_toolActive;

	private static float m_lastPinchDistance;

	public override void Enter(IStatedObject _parent)
	{
		p_parent = _parent as EditorScene;
		string[] tags = new string[1] { "TestCanvas" };
		m_mainCanvas = CanvasA.Assemble(Main.uiCamera, -1, "TestCanvas", HandleCanvas, tags);
		UIS.SetCanvasRelativeSize(m_mainCanvas, 1f, 1f, 0f, 0f);
		UIS.SetCanvasRelativeMarginAndSpacing(m_mainCanvas, 0.02f, 0.01f);
		m_testArea = GETestAreaA.Assemble(m_mainCanvas);
		LevelManager.ResetAll(true);
		m_load = 0;
	}

	public override void Execute()
	{
		AbstractPhysics.Update(1f / 60f);
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			GEState.editorCameraStartPosition = Main.camera.transform.position;
			p_parent.StateMachine.ChangeState(new EditorState());
		}
	}

	public override void Exit()
	{
		EntityManager.RemoveEntitiesByTransformComponentHierarchy(m_testArea.TC, false);
		EntityManager.RemoveEntitiesByTransformComponentHierarchy(m_mainCanvas.TC, false);
		m_testArea = null;
		m_mainCanvas = null;
	}

	protected virtual void HandleUI(TouchAreaC _c, int _i, bool _consumed)
	{
		if (_c.touchEvent[_i] == TouchEvent.Release && _c.identifier == "Edit")
		{
			GEState.editorCameraStartPosition = Main.camera.transform.position;
			p_parent.StateMachine.ChangeState(new EditorState());
		}
	}

	protected virtual void HandleCanvas(TouchAreaC _c, int _i, bool _consumed)
	{
		TLTouch tLTouch = InputManager.m_touches[_c.touchIndex[_i]];
		Vector3 touchWorldPos = TouchAreaS.GetTouchWorldPos(Main.camera, tLTouch.position);
		Vector3 touchWorldPos2 = TouchAreaS.GetTouchWorldPos(Main.camera, tLTouch.position - tLTouch.deltaPosition);
		if (m_toolActive && m_tool == SLICE && _c.touchStartedInside.Count == 2)
		{
			m_tool = MOVE;
		}
		if (_consumed)
		{
			return;
		}
		Vector2 vector = _c.touchPos[_i];
		Vector2 vector2 = _c.touchStartPos[_i];
		if (_c.touchEvent[_i] == TouchEvent.Began)
		{
			m_tool = SLICE;
			m_toolActive = true;
		}
		else
		{
			if (_c.touchEvent[_i] == TouchEvent.DragStart)
			{
				return;
			}
			if (_c.touchEvent[_i] == TouchEvent.Down || _c.touchEvent[_i] == TouchEvent.Drag)
			{
				if (!m_toolActive)
				{
					return;
				}
				if (m_tool == MOVE)
				{
					Vector3 vector3 = -InputManager.m_touches[_c.touchIndex[_i]].deltaPosition * (1f / Main.m_gameCameraDistanceMultipler);
					CameraS.m_currentCameraPositionOffset += vector3;
					CameraS.m_offsetLocked = true;
				}
				else
				{
					if (m_tool != SLICE)
					{
						return;
					}
					int aliveCount = GES.m_constraintComponents.m_aliveCount;
					for (int i = 0; i < aliveCount; i++)
					{
						GEConstraintC gEConstraintC = GES.m_constraintComponents.m_array[GES.m_constraintComponents.m_aliveIndices[i]];
						if (!gEConstraintC.active || gEConstraintC.constraintType != ConstraintType.Rope || !gEConstraintC.isCuttable)
						{
							continue;
						}
						bool flag = false;
						for (int j = 1; j < gEConstraintC.ropeCMCs.Length; j++)
						{
							Vector3 position = gEConstraintC.ropeCMCs[j - 1].TC.transform.position;
							Vector3 position2 = gEConstraintC.ropeCMCs[j].TC.transform.position;
							Vector2 _pos = Vector2.zero;
							if (ToolBox.DoLinesIntersect(touchWorldPos, touchWorldPos2, position, position2, ref _pos))
							{
								GERopeA.AssembleSlicedRope(gEConstraintC, j, _pos, touchWorldPos - touchWorldPos2);
								GEConnectionLogic.DestroyControlledComponent(gEConstraintC);
								break;
							}
						}
					}
				}
			}
			else if (_c.touchEvent[_i] == TouchEvent.Release)
			{
				CameraS.m_offsetLocked = false;
				m_tool = SLICE;
				m_toolActive = false;
			}
		}
	}
}
