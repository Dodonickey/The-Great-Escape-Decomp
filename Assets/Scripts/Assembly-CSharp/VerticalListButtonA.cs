using UnityEngine;

public static class VerticalListButtonA
{
	public static UIC Assemble(UIC _canvas, int _identifier, string _label, TouchEventDelegate _touchEventHandler, string[] _tags)
	{
		TransformC transformC = EntityManager.AddEntityWithTC(_tags);
		TransformS.SetPosition(transformC, Vector3.zero);
		UIC uIC = UIS.AddComponent(transformC, UIComponentType.ListButton);
		uIC.width = _canvas.width - _canvas.contentMargin * 2f;
		uIC.height = 20f;
		uIC.headerHeight = 0f;
		uIC.footerHeight = 0f;
		uIC.label = _label;
		uIC.identifier = _identifier;
		uIC.contentTC = TransformS.AddComponent(EntityManager.m_entities.m_array[transformC.entityIndex]);
		TransformS.ParentComponent(uIC.contentTC, uIC.TC);
		TransformS.SetPosition(uIC.contentTC, Vector3.forward * -15f);
		if (_touchEventHandler != null)
		{
			uIC.TAC = TouchAreaS.AddComponent(transformC, _label, uIC.width, uIC.height, true, _canvas.canvasCamera, uIC);
			TouchAreaS.AddTouchEventListener(uIC.TAC, _touchEventHandler);
		}
		UIS.AddToCanvasGrid(uIC, _canvas, true);
		uIC.textC = TextS.AddSingleLineComponent(uIC.contentTC, uIC.label, 0.3f, Align.Left, Align.Middle);
		SpriteS.SetColorByTransformComponent(uIC.textC.contentTC, DebugDraw.GetColor(50f, 50f, 50f), false, false);
		TransformS.Move(uIC.textC.contentTC, Vector3.right * uIC.width * -0.5f + Vector3.right * uIC.width * 0.1f);
		uIC.textPCs.AddRange(SpriteS.ConvertSpritesToPrefabComponent(uIC.textC.TC, _canvas.canvasCamera, true));
		return uIC;
	}

	public static void HandleTouches(TouchAreaC _c, int _i)
	{
		UIC c = _c.customComponent as UIC;
		TouchEvent touchEvent = _c.touchEvent[_i];
		Vector2 vector = _c.touchPos[_i];
		if (touchEvent == TouchEvent.Began || (touchEvent == TouchEvent.RollIn && _c.touchStartedInside[_i] && !_c.touchWasDragged[_i]))
		{
			UIS.HighlightButton(c);
		}
		else if (touchEvent == TouchEvent.Release || touchEvent == TouchEvent.ReleaseOutside || touchEvent == TouchEvent.RollOut || touchEvent == TouchEvent.DragStart)
		{
			UIS.NormalizeButton(c);
		}
	}
}
