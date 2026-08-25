using UnityEngine;

public static class IconA
{
	public static UIC Assemble(Camera _camera, string _identifier, TouchEventDelegate _touchEventHandler, TransformC _parent, Vector3 _pos, float _width, bool _drawLabel, string[] _tags, bool _consumeTouches)
	{
		TransformC transformC = EntityManager.AddEntityWithTC(_tags);
		if (_parent != null)
		{
			TransformS.ParentComponent(transformC, _parent);
		}
		TransformS.SetPosition(transformC, _pos);
		UIC uIC = UIS.AddComponent(transformC, UIComponentType.Button);
		uIC.width = _width;
		uIC.height = _width;
		if (_drawLabel)
		{
			uIC.height += 15f;
		}
		uIC.label = _identifier;
		uIC.contentTC = TransformS.AddComponent(EntityManager.m_entities.m_array[transformC.entityIndex]);
		TransformS.ParentComponent(uIC.contentTC, uIC.TC);
		TransformS.SetPosition(uIC.contentTC, Vector3.forward * -15f);
		uIC.TAC = TouchAreaS.AddComponent(transformC, _identifier, _width, _width, _consumeTouches, _camera, uIC);
		if (_touchEventHandler != null)
		{
			TouchAreaS.AddTouchEventListener(uIC.TAC, _touchEventHandler);
		}
		else
		{
			TouchAreaS.AddTouchEventListener(uIC.TAC, HandleTouches);
		}
		if (_drawLabel)
		{
			uIC.textC = TextS.AddSingleLineComponent(uIC.contentTC, _identifier, 0.25f, Align.Center, Align.Top);
			TransformS.Move(uIC.textC.contentTC, Vector3.up * (0f - _width) * 0.5f);
			SpriteS.SetColorByTransformComponent(uIC.textC.contentTC, DebugDraw.GetColor(0f, 0f, 0f), false, false);
		}
		Vector2[] roundedRect = DebugDraw.GetRoundedRect(_width, _width, 15f, 5, Vector2.zero);
		PrefabS.CreateFlatPrefabComponentsFromPolygon(transformC, Vector3.forward * 10f, DebugDraw.Vector2ArrayToPolygon(roundedRect), DebugDraw.GetColor(128f, 128f, 128f), ResourceManager.GetMaterial("Solid"), _camera);
		return uIC;
	}

	public static void HandleTouches(TouchAreaC _c, int _i, bool _consumed)
	{
		UIC c = _c.customComponent as UIC;
		TouchEvent touchEvent = _c.touchEvent[_i];
		Vector2 vector = _c.touchPos[_i];
		if ((touchEvent == TouchEvent.Began || touchEvent == TouchEvent.RollIn) && _c.touchStartedInside[_i])
		{
			UIS.HighlightIcon(c);
		}
		else if ((touchEvent == TouchEvent.Release || touchEvent == TouchEvent.ReleaseOutside || touchEvent == TouchEvent.RollOut) && _c.touchStartedInside[_i])
		{
			UIS.NormalizeIcon(c);
		}
	}
}
