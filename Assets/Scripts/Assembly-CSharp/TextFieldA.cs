using UnityEngine;

public static class TextFieldA
{
	public static UIC Assemble(Camera _camera, int _identifier, string _label, EventDelegate _eventHandler, TouchEventDelegate _customTouchEventHandler, string[] _tags)
	{
		TransformC transformC = EntityManager.AddEntityWithTC(_tags);
		UIC uIC = UIS.AddComponent(transformC, UIComponentType.TextField);
		if (_eventHandler != null)
		{
			uIC.EC = EventS.AddComponent(uIC.entityIndex, _label, _eventHandler, 0f, false, false, false, false);
			EventS.AddProperty(uIC.EC, "identifier", _identifier);
			EventS.AddProperty(uIC.EC, "label", _label);
			EventS.AddProperty(uIC.EC, "value", string.Empty);
		}
		uIC.width = 100f;
		uIC.height = 100f;
		uIC.label = _label;
		uIC.identifier = _identifier;
		uIC.contentTC = TransformS.AddComponent(EntityManager.m_entities.m_array[transformC.entityIndex]);
		TransformS.ParentComponent(uIC.contentTC, uIC.TC);
		TransformS.SetPosition(uIC.contentTC, Vector3.forward * -25f);
		uIC.TAC = TouchAreaS.AddComponent(transformC, _label, uIC.width, uIC.height, false, _camera, uIC);
		if (_customTouchEventHandler != null)
		{
			TouchAreaS.AddTouchEventListener(uIC.TAC, _customTouchEventHandler);
		}
		else
		{
			TouchAreaS.AddTouchEventListener(uIC.TAC, HandleTouches);
		}
		return uIC;
	}

	public static void DrawTextField(UIC _textField, string _text)
	{
		Vector2[] roundedRect = DebugDraw.GetRoundedRect(_textField.width, _textField.height, 5f, 5, Vector2.zero);
		PrefabS.CreateFlatPrefabComponentsFromPolygon(_textField.TC, Vector3.forward * -10f, DebugDraw.Vector2ArrayToPolygon(roundedRect), DebugDraw.GetColor(250f, 250f, 250f), ResourceManager.GetMaterial("Solid"), _textField.parent.canvasCamera);
		TextS.SetStyle("subheader");
		_textField.textC = TextS.AddComponent(_textField.contentTC, _text, 1f, true, true, 0.5f, 0.5f, _textField.width, _textField.height, Align.Left, Align.Middle, 20f, 20f, 0f, 0f, 0f, 0f);
		SpriteS.SetColorByTransformComponent(_textField.textC.contentTC, DebugDraw.GetColor(0f, 0f, 0f), false, false);
		_textField.textPCs.AddRange(SpriteS.ConvertSpritesToPrefabComponent(_textField.textC.contentTC, _textField.parent.canvasCamera, true));
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
				UIS.StopTextFieldEditing(uIC);
			}
		}
	}
}
