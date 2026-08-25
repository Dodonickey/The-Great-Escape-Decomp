using UnityEngine;

public static class NumericFieldA
{
	public static UIC Assemble(Camera _camera, string _identifier, EventDelegate _eventHandler, TouchEventDelegate _customTouchEventHandler, bool _defaultDraw, Align _align, float _width, float _scale, bool _isInt, float _min, float _max, float _default, string[] _tags)
	{
		TransformC transformC = EntityManager.AddEntityWithTC(_tags);
		UIC uIC = UIS.AddComponent(transformC, UIComponentType.NumericField);
		if (_eventHandler != null)
		{
			uIC.EC = EventS.AddComponent(uIC.entityIndex, _identifier, _eventHandler, 0f, false, false, false, false);
			EventS.AddProperty(uIC.EC, "identifier", _identifier);
			EventS.AddProperty(uIC.EC, "value", _default);
		}
		uIC.width = _width;
		uIC.height = 40f * _scale;
		uIC.label = _identifier;
		uIC.currentVal = _default;
		uIC.minVal = _min;
		uIC.maxVal = _max;
		uIC.isInt = _isInt;
		uIC.contentTC = TransformS.AddComponent(EntityManager.m_entities.m_array[transformC.entityIndex]);
		TransformS.ParentComponent(uIC.contentTC, uIC.TC);
		TransformS.SetPosition(uIC.contentTC, Vector3.forward * -25f);
		uIC.TAC = TouchAreaS.AddComponent(transformC, _identifier, uIC.width, uIC.height, false, _camera, uIC);
		if (_customTouchEventHandler != null)
		{
			TouchAreaS.AddTouchEventListener(uIC.TAC, _customTouchEventHandler);
		}
		else
		{
			TouchAreaS.AddTouchEventListener(uIC.TAC, HandleTouches);
		}
		if (_defaultDraw)
		{
			TextS.SetStyle("body");
			TextC textC = TextS.AddSingleLineComponent(uIC.contentTC, _identifier, 1f * _scale, Align.Left, Align.Top);
			SpriteS.SetColorByTransformComponent(textC.contentTC, DebugDraw.GetColor(0f, 0f, 0f), false, false);
			TransformS.Move(textC.contentTC, new Vector3(uIC.width * -0.5f, uIC.height * 0.5f, 0f));
			SpriteS.ConvertSpritesToPrefabComponent(textC.contentTC, _camera, true);
			TextS.RemoveComponent(textC);
			Vector2[] roundedRect = DebugDraw.GetRoundedRect(uIC.width, uIC.height, 5f * _scale, 5, Vector2.zero);
			PrefabS.CreateFlatPrefabComponentsFromPolygon(transformC, Vector3.forward * 10f, DebugDraw.Vector2ArrayToPolygon(roundedRect), DebugDraw.GetColor(230f, 230f, 230f), ResourceManager.GetMaterial("Solid"), _camera);
		}
		TextS.SetStyle("subheader");
		uIC.textC = TextS.AddSingleLineComponent(uIC.contentTC, _default.ToString(), 1f * _scale, _align, Align.Top);
		SpriteS.SetColorByTransformComponent(uIC.textC.contentTC, DebugDraw.GetColor(0f, 0f, 0f), false, false);
		if (_align == Align.Left)
		{
			TransformS.Move(uIC.textC.contentTC, new Vector3(uIC.width * -0.5f + 10f * _scale, 0f, 0f));
		}
		if (_align == Align.Right)
		{
			TransformS.Move(uIC.textC.contentTC, new Vector3(uIC.width * 0.5f - 10f * _scale, 0f, 0f));
		}
		uIC.textPCs.AddRange(SpriteS.ConvertSpritesToPrefabComponent(uIC.textC.contentTC, _camera, true));
		return uIC;
	}

	public static void HandleTouches(TouchAreaC _c, int _i, bool _consumed)
	{
		UIC uIC = _c.customComponent as UIC;
		TouchEvent touchEvent = _c.touchEvent[_i];
		Vector2 vector = _c.touchPos[_i];
		if (uIC.enabled && touchEvent == TouchEvent.Release && _c.touchStartedInside[_i])
		{
			if (!uIC.isEditing)
			{
				UIS.StartTextFieldEditing(uIC);
			}
			else
			{
				UIS.StopNumericFieldEditing(uIC);
			}
		}
	}
}
