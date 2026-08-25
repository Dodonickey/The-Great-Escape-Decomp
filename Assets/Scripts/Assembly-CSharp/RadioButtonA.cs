using UnityEngine;

public static class RadioButtonA
{
	public static UIC Assemble(Camera _camera, string _identifier, EventDelegate _eventHandler, TouchEventDelegate _customTouchEventHandler, bool _defaultDraw, Align _labelPlacement, float _scale, bool _isSelected, int _value, int _group, string[] _tags)
	{
		TransformC transformC = EntityManager.AddEntityWithTC(_tags);
		UIC uIC = UIS.AddComponent(transformC, UIComponentType.RadioButton);
		if (_eventHandler != null)
		{
			uIC.EC = EventS.AddComponent(uIC.entityIndex, _identifier, _eventHandler, 0f, false, false, false, false);
			EventS.AddProperty(uIC.EC, "identifier", _identifier);
			EventS.AddProperty(uIC.EC, "group", _group);
			EventS.AddProperty(uIC.EC, "value", _value);
		}
		uIC.radius = 20f * _scale;
		uIC.width = 40f * _scale;
		uIC.height = 40f * _scale;
		uIC.isSelected = _isSelected;
		uIC.radioButtonGroup = _group;
		uIC.radioButtonValue = _value;
		uIC.contentTC = TransformS.AddComponent(EntityManager.m_entities.m_array[transformC.entityIndex]);
		TransformS.ParentComponent(uIC.contentTC, uIC.TC);
		TransformS.SetPosition(uIC.contentTC, Vector3.forward * -25f);
		if (_defaultDraw)
		{
			TextS.SetStyle("body");
			Vector3 zero = Vector3.zero;
			switch (_labelPlacement)
			{
			case Align.Left:
				uIC.textC = TextS.AddSingleLineComponent(uIC.contentTC, _identifier, 1f * _scale, Align.Right, Align.Middle);
				TransformS.Move(uIC.textC.contentTC, Vector3.right * (0f - uIC.width) * 0.75f);
				zero.x = uIC.width * 0.167f + uIC.textC.textWidth;
				uIC.width += zero.x;
				TransformS.Move(uIC.contentTC, zero * 0.5f);
				break;
			case Align.Right:
				uIC.textC = TextS.AddSingleLineComponent(uIC.contentTC, _identifier, 1f * _scale, Align.Left, Align.Middle);
				TransformS.Move(uIC.textC.contentTC, Vector3.right * uIC.width * 0.75f);
				zero.x = uIC.width * 0.167f + uIC.textC.textWidth;
				uIC.width += zero.x;
				TransformS.Move(uIC.contentTC, zero * -0.5f);
				break;
			case Align.Top:
				uIC.textC = TextS.AddSingleLineComponent(uIC.contentTC, _identifier, 1f * _scale, Align.Center, Align.Bottom);
				TransformS.Move(uIC.textC.contentTC, Vector3.up * uIC.width * 0.75f);
				zero.y = uIC.height * 0.167f + uIC.textC.textHeight;
				uIC.height += zero.y;
				TransformS.Move(uIC.contentTC, zero * -0.5f);
				break;
			case Align.Bottom:
				uIC.textC = TextS.AddSingleLineComponent(uIC.contentTC, _identifier, 1f * _scale, Align.Center, Align.Top);
				TransformS.Move(uIC.textC.contentTC, Vector3.up * (0f - uIC.width) * 0.75f);
				zero.y = uIC.height * 0.167f + uIC.textC.textHeight;
				uIC.height += zero.y;
				TransformS.Move(uIC.contentTC, zero * 0.5f);
				break;
			}
			SpriteS.SetColorByTransformComponent(uIC.textC.contentTC, DebugDraw.GetColor(0f, 0f, 0f), false, false);
			SpriteS.ConvertSpritesToPrefabComponent(uIC.textC.contentTC, _camera, true);
			Vector2[] circle = DebugDraw.GetCircle(uIC.radius, 32, Vector2.zero, true);
			Vector2[] circle2 = DebugDraw.GetCircle(uIC.radius * 0.647f, 32, Vector2.zero, true);
			PrefabS.CreateFlatPrefabComponentsFromPolygon(uIC.contentTC, Vector3.forward * 10f, DebugDraw.Vector2ArrayToPolygon(circle), DebugDraw.GetColor(230f, 230f, 230f), ResourceManager.GetMaterial("Solid"), _camera);
			uIC.foregroundPCs.AddRange(PrefabS.CreateFlatPrefabComponentsFromPolygon(uIC.contentTC, Vector3.forward * -10f, DebugDraw.Vector2ArrayToPolygon(circle2), DebugDraw.GetColor(0f, 0f, 0f), ResourceManager.GetMaterial("Solid"), _camera));
		}
		uIC.TAC = TouchAreaS.AddComponent(transformC, _identifier, uIC.width, uIC.height, true, _camera, uIC);
		if (_customTouchEventHandler != null)
		{
			TouchAreaS.AddTouchEventListener(uIC.TAC, _customTouchEventHandler);
		}
		else
		{
			TouchAreaS.AddTouchEventListener(uIC.TAC, HandleTouches);
		}
		if (_isSelected)
		{
			UIS.SelectRadioButton(uIC);
		}
		else
		{
			UIS.UnselectRadioButton(uIC);
		}
		return uIC;
	}

	public static void HandleTouches(TouchAreaC _c, int _i, bool _consumed)
	{
		UIC uIC = _c.customComponent as UIC;
		TouchEvent touchEvent = _c.touchEvent[_i];
		if (!uIC.enabled || touchEvent != TouchEvent.Began)
		{
			return;
		}
		UIC selectedRadioButton = UIS.GetSelectedRadioButton(uIC.radioButtonGroup);
		if (selectedRadioButton != null)
		{
			for (int i = 0; i < selectedRadioButton.controlledUICs.Count; i++)
			{
				if (selectedRadioButton.controlledUICDirs[i])
				{
					UIS.Enable(selectedRadioButton.controlledUICs[i]);
				}
				else
				{
					UIS.Disable(selectedRadioButton.controlledUICs[i]);
				}
			}
		}
		UIS.UnselectAllRadioButtonsFromGroup(uIC.radioButtonGroup);
		UIS.SelectRadioButton(uIC);
		for (int j = 0; j < uIC.controlledUICs.Count; j++)
		{
			if (uIC.controlledUICDirs[j])
			{
				UIS.Disable(uIC.controlledUICs[j]);
			}
			else
			{
				UIS.Enable(uIC.controlledUICs[j]);
			}
		}
		if (uIC.EC != null)
		{
			EventS.Dispatch(uIC.EC, false);
		}
	}
}
