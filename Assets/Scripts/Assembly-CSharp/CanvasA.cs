using UnityEngine;

public static class CanvasA
{
	public static UIC Assemble(Camera _camera, int _identifier, string _label, TouchEventDelegate _touchEventHandler, string[] _tags)
	{
		TransformC tc = EntityManager.AddEntityWithTC(_tags);
		UIC uIC = UIS.AddComponent(tc, UIComponentType.Canvas);
		uIC.width = 100f;
		uIC.height = 100f;
		uIC.canvasHeight = 100f;
		uIC.canvasWidth = 100f;
		uIC.headerHeight = 20f;
		uIC.footerHeight = 15f;
		uIC.contentMargin = (float)Screen.height * 0.025f;
		uIC.contentSpacing = (float)Screen.height * 0.01f;
		uIC.contentHAlign = Align.Left;
		uIC.contentVAlign = Align.Top;
		uIC.identifier = _identifier;
		uIC.label = _label;
		uIC.canvasCamera = _camera;
		UIS.ResetCursor(uIC);
		uIC.contentTC = TransformS.AddComponent(EntityManager.m_entities.m_array[uIC.entityIndex]);
		TransformS.ParentComponent(uIC.contentTC, uIC.TC);
		uIC.cameraTC = TransformS.AddComponent(EntityManager.m_entities.m_array[uIC.entityIndex]);
		TransformS.ParentComponent(uIC.cameraTC, uIC.contentTC, Vector3.up * (uIC.footerHeight - uIC.headerHeight) * 0.5f);
		if (_touchEventHandler != null)
		{
			uIC.TAC = TouchAreaS.AddComponent(uIC.cameraTC, _label, uIC.width, uIC.height, true, _camera, uIC);
			TouchAreaS.AddTouchEventListener(uIC.TAC, _touchEventHandler);
		}
		return uIC;
	}

	public static void HandleTouches(TouchAreaC _c, int _i, bool _consumed)
	{
		if (_c.touchEvent[_i] == TouchEvent.Drag && _c.touchStartedInside[_i])
		{
			UIC uIC = _c.customComponent as UIC;
			int num = _c.touchIndex[_i];
			TLTouch tLTouch = InputManager.m_touches[num];
			uIC.scrollInertiaX = tLTouch.deltaPosition.x;
			uIC.scrollInertiaY = tLTouch.deltaPosition.y;
		}
	}
}
