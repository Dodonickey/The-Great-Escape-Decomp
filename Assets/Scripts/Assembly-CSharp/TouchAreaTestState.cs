using UnityEngine;

public class TouchAreaTestState : BasicState
{
	protected EditorScene p_parent;

	public override void Enter(IStatedObject _parent)
	{
		p_parent = _parent as EditorScene;
		TouchAreaS.AddTouchEventListener(GEState.fullScreenTAC, HandleCanvas);
		TransformC transformComponent = EntityManager.AddEntityWithTC();
		TouchAreaC touchAreaComponent = TouchAreaS.AddComponent(transformComponent, "first", 100f, 100f, false, Main.uiCamera, null);
		TouchAreaS.AddTouchEventListener(touchAreaComponent, HandleCanvas);
		DebugDraw.CreateBox(Main.uiCamera, transformComponent, Vector2.zero, 100f, 100f, false);
		transformComponent = EntityManager.AddEntityWithTC();
		touchAreaComponent = TouchAreaS.AddComponent(transformComponent, "second", 200f, 50f, true, Main.uiCamera, null);
		TouchAreaS.AddTouchEventListener(touchAreaComponent, HandleCanvas);
		DebugDraw.CreateBox(Main.uiCamera, transformComponent, Vector2.zero, 200f, 50f, false);
	}

	public override void Execute()
	{
	}

	public override void Exit()
	{
	}

	protected virtual void HandleCanvas(TouchAreaC _c, int _i, bool _consumed)
	{
		if (_c.touchEvent[_i] == TouchEvent.Began)
		{
			Debug.Log(_c.identifier + " began | consumed: " + _consumed);
		}
		else if (_c.touchEvent[_i] == TouchEvent.RollIn)
		{
			Debug.Log(_c.identifier + " rolled in | consumed: " + _consumed);
		}
		else if (_c.touchEvent[_i] == TouchEvent.RollOut)
		{
			Debug.Log(_c.identifier + " rolled out | consumed: " + _consumed + " pos: " + _c.touchPos[_i]);
		}
		else if (_c.touchEvent[_i] == TouchEvent.Drag)
		{
			Debug.Log(_c.identifier + " drag | consumed: " + _consumed);
		}
		else if (_c.touchEvent[_i] == TouchEvent.Release)
		{
			Debug.Log(_c.identifier + " released | consumed: " + _consumed);
		}
		else if (_c.touchEvent[_i] == TouchEvent.ReleaseOutside)
		{
			Debug.Log(_c.identifier + " released outside | consumed: " + _consumed);
		}
	}
}
